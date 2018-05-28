using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using YunkuEntSDK.UtilClass;
using YunkuEntSDK.Data;
using System.Threading;

namespace YunkuEntSDK.Net
{
    internal class UploadManager
    {
        private const string UrlUploadInit = "/upload_init";
        private const string UrlUploadReq = "/upload_req";
        private const string UrlUploadPart = "/upload_part";
        private const string UrlUploadAbort = "/upload_abort";
        private const string UrlUploadFinish = "/upload_finish";

        private int _blockSize; // 上传分块大小
        private string _session = ""; // 上传session

        private EntFileManager _engine;

        private string _fullpath;
        private string _opName;
        private int _opId;
        private bool _overwrite;
        private Stream _stream;
        private Data.FileInfo _fileinfo = new Data.FileInfo();

        public event CompletedEventHandler Completed;
        public event ProgressChangeEventHandler ProgresChanged;
        public delegate void CompletedEventHandler(object sender, CompletedEventArgs e);
        public delegate void ProgressChangeEventHandler(object sender, ProgressEventArgs e);


        public UploadManager(int blockSize, EntFileManager engine)
        {
            this._blockSize = blockSize;
            this._engine = engine;
        }

        public void SetOperator(int opId)
        {
            this._opId = opId;
        }

        public void SetOperator(String opName)
        {
            this._opName = opName;
        }

        public Data.FileInfo Upload(Stream stream, string fullpath, bool overwrite)
        {
            if (!stream.CanRead)
            {
                throw new Exception("stream can not read");
            }

            if (!stream.CanSeek)
            {
                throw new Exception("stream can not seek");
            }
            
            _stream = stream;
            _fullpath = fullpath;
            _fileinfo.Fullpath = _fullpath;
            _overwrite = overwrite;
            this.StartUpload(false);
            return _fileinfo;
        }

        public bool UploadAsync(Stream stream, string fullpath, bool overwrite)
        {
            _stream = stream;
            _fullpath = fullpath;
            _fileinfo.Fullpath = _fullpath;
            _overwrite = overwrite;
            WaitCallback callback = new WaitCallback(delegate(object state)
            {
                try
                {
                    this.StartUpload(true);
                    if (Completed != null)
                    {
                        Completed(this, new CompletedEventArgs() { Fullpath = _fileinfo.Fullpath, FileInfo = _fileinfo });
                    }
                }
                catch (Exception e)
                {
                    if (Completed != null)
                    {
                        Completed(this,
                            new CompletedEventArgs()
                            {
                                IsError = true,
                                ErrorMessage = e.Message,
                                Fullpath = _fullpath,
                                FileInfo = _fileinfo
                            });
                    }
                }
            });
            return ThreadPool.QueueUserWorkItem(callback);
            
        }

        /// <summary>
        /// 开始上传
        /// </summary>
        /// <param name="item"></param>
        private void StartUpload(bool async)
        {
            ReturnResult result;
            _fileinfo.Filename = Util.GetFileNameFromPath(_fileinfo.Fullpath);
            _fileinfo.Filehash = Util.CaculateFileHashCode(_stream);
            _fileinfo.Filesize = _stream.Length;

            for (int trys = 0; trys <3; trys++)
            {
                result = this._engine.CreateFile(this._fullpath, this._fileinfo.Filehash, this._fileinfo.Filesize, this._opId, this._opName, this._overwrite);
                bool shouldUpload = this.DecodeAddFileResult(result);
                if (!shouldUpload) //秒传
                {
                    return;
                }

                if (string.IsNullOrEmpty(_fileinfo.UploadServer))
                {
                    throw new YunkuException("fail to get upload server", result);
                }
                
                LogPrint.Print("upload server: " + _fileinfo.UploadServer);

                result = this.UploadInit();
                if (result == null)
                {
                    continue;
                }
                if (result.Code == (int)HttpStatusCode.Accepted) //秒传
                {
                    return;
                }

                long checkSize = this.UploadReq();
                if (checkSize < 0)
                {
                    continue;
                }
                if (checkSize == _fileinfo.Filesize) //完成上传
                {
                    break;
                }

                long buflen;
                long offset = checkSize;
                long rang_end;
                long crc32;
                string range;
                bool uploadPartErr = false;
                int code;

                _stream.Seek(offset, SeekOrigin.Begin);

                while (offset < _fileinfo.Filesize - 1)
                {
                    if (ProgresChanged != null)
                    {
                        ProgresChanged(this, new ProgressEventArgs()
                        {
                            Fullpath = _fullpath,
                            ProgressPercent = (int)(((float)offset / _fileinfo.Filesize) * 100)
                        });
                    }

                    buflen = offset + _blockSize > _fileinfo.Filesize ? _fileinfo.Filesize - offset : _blockSize;
                    byte[] buffer = new byte[buflen];
                    _stream.Read(buffer, 0, (int)buflen);
                    crc32 = CRC32.Compute(buffer);
                    rang_end = offset + buflen - 1;
                    range = offset + "-" + rang_end;

                    result = this.UploadPart(range, new MemoryStream(buffer), crc32);
                    code = result.Code;

                    if (code == (int)HttpStatusCode.OK)
                    {
                        offset += buflen;
                    }
                    else if (code == (int)HttpStatusCode.Accepted)
                    {
                        //uploadPartErr = true;
                        break;
                    }
                    else if (code >= (int)HttpStatusCode.InternalServerError)
                    {
                        uploadPartErr = true;
                        break;
                    }
                    else if (code == (int)HttpStatusCode.Unauthorized)
                    {
                        uploadPartErr = true;
                        break;
                    }
                    else if (code == (int)HttpStatusCode.Conflict)
                    {
                        var json = (IDictionary<string, object>)SimpleJson.DeserializeObject(result.Body);
                        offset = Convert.ToInt64(json["expect"]);
                        _stream.Seek(offset, SeekOrigin.Begin);
                    }
                    else
                    {
                        throw new YunkuException("fail to upload part", result);
                    }
                }
                if (!uploadPartErr)
                {
                    break;
                }
            }
            this.UploadFinish();
        }

        /// <summary>
        /// 解码creat_file返回结果，判断是否需要上传
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool DecodeAddFileResult(ReturnResult result)
        {
            if (result.Body == null || result.Code != (int)HttpStatusCode.OK)
            {
                throw new YunkuException("fail to get upload server", result);
            }

            var json = (IDictionary<string, object>)SimpleJson.DeserializeObject(result.Body);
            _fileinfo.Fullpath = SimpleJson.TryStringValue(json, "fullpath");
            _fileinfo.Filename = Util.GetFileNameFromPath(_fileinfo.Fullpath);
            _fileinfo.Hash = SimpleJson.TryStringValue(json, "hash");
            _fileinfo.UploadServer = SimpleJson.TryStringValue(json, "server");

            int state = SimpleJson.TryIntValue(json, "state");

            return state == 0;
        }

        /// <summary>
        /// 上传初始化
        /// </summary>
        private ReturnResult UploadInit()
        {
            string url = _fileinfo.UploadServer + UrlUploadInit + "?org_client_id=" + this._engine.GetClientId();

            var heads = new Dictionary<string, string>();
            heads.Add("x-gk-upload-pathhash", _fileinfo.Hash);
            heads.Add("x-gk-upload-filename", _fileinfo.Filename);
            heads.Add("x-gk-upload-filehash", _fileinfo.Filehash);
            heads.Add("x-gk-upload-filesize", _fileinfo.Filesize.ToString());

            ReturnResult result = this._engine.Call(url, RequestType.POST, heads, null, null, true);
            if (result.Code == (int)HttpStatusCode.OK)
            {
                var json = (IDictionary<string, object>)SimpleJson.DeserializeObject(result.Body);
                _session = SimpleJson.TryStringValue(json, "session");
                if (string.IsNullOrEmpty(_session))
                {
                    throw new YunkuException("fail to get session in upload_init", result);
                }
            }
            else if (result.Code >= (int)HttpStatusCode.InternalServerError)
            {
                return null;
            }
            else if (result.Code != (int)HttpStatusCode.Accepted)
            {
                throw new YunkuException("fail to init upload", result);
            }
            return result;
        }

        /// <summary>
        /// 断点续传检查，返回-1表示服务器异常
        /// </summary>
        private long UploadReq()
        {
            string url = _fileinfo.UploadServer + UrlUploadReq;

            var heads = new Dictionary<string, string>();
            heads.Add("x-gk-upload-session", _session);

            long checkSize = 0;
            ReturnResult result = this._engine.Call(url, RequestType.GET, heads, null, null, true);
            if (result.Code == (int)HttpStatusCode.OK)
            {
                long.TryParse(result.Body, out checkSize);
            } else if (result.Code >= (int)HttpStatusCode.InternalServerError)
            {
                checkSize = -1;
            } else
            {
                throw new YunkuException("fail to get upload req", result);
            }
            return checkSize;
        }

        /// <summary>
        /// 分块上传
        /// </summary>
        /// <param name="range"></param>
        /// <param name="data"></param>
        /// <param name="crc32"></param>
        private ReturnResult UploadPart(String range, Stream data, long crc32)
        {
            string url = _fileinfo.UploadServer + UrlUploadPart;

            var heads = new Dictionary<string, string>();
            //headParameter.Add("Connection", "keep-alive");
            heads.Add("x-gk-upload-session", _session);
            heads.Add("x-gk-upload-range", range);
            heads.Add("x-gk-upload-crc", crc32.ToString());
            
            return this._engine.Call(url, RequestType.PUT, heads, null, data, true);
        }

        /// <summary>
        /// 传输结束
        /// </summary>
        public void UploadFinish()
        {
            string url = _fileinfo.UploadServer + UrlUploadFinish;

            var heads = new Dictionary<string, string>();
            heads.Add("x-gk-upload-session", _session);
            ReturnResult result = null;

            int retry = 10;
            while(retry-- > 0)
            {
                result = this._engine.Call(url, RequestType.POST, heads, null, null, true);
                if (result.Code == (int)HttpStatusCode.Accepted)
                {
                    Thread.Sleep(2000);
                }
                else
                {
                    break;
                }
            }

            if (!result.IsOK())
            {
                throw new YunkuException("fail to call upload_finish", result);
            }
        }

        private void UploadAbort()
        {
            if (String.IsNullOrEmpty(_fileinfo.UploadServer) || String.IsNullOrEmpty(_session))
            {
                return;
            }
            string url = _fileinfo.UploadServer + UrlUploadAbort;
            var heads = new Dictionary<string, string>();
            heads.Add("x-gk-upload-session", _session);

            this._engine.Call(url, RequestType.POST, heads, null, null, true);
        }
    }
}