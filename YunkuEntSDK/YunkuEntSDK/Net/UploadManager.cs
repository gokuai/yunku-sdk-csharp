using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using YunkuEntSDK.UtilClass;
using YunkuEntSDK.Data;


namespace YunkuEntSDK.Net
{
    internal class UploadManager : HttpEngine
    {
        private const string UrlUploadInit = "/upload_init";
        private const string UrlUploadReq = "/upload_req";
        private const string UrlUploadPart = "/upload_part";
        private const string UrlUploadAbort = "/upload_abort";
        private const string UrlUploadFinish = "/upload_finish";

        private int _blockSize; // 上传分块大小
        private string _session = ""; // 上传session
        private string _apiUrl = "";

        private readonly string _orgClientId;
        private string _fullpath;
        private long _dateline;
        private string _opName;
        private int _opId;
        private bool _overWrite;
        private Stream _stream;
        private Data.FileInfo _fileinfo = new Data.FileInfo();

        public event CompletedEventHandler Completed;
        public event ProgressChangeEventHandler ProgresChanged;
        public delegate void CompletedEventHandler(object sender, CompletedEventArgs e);
        public delegate void ProgressChangeEventHandler(object sender, ProgressEventArgs e);

        public UploadManager(string apiUrl, Stream stream, string fullpath,
            string opName, int opId, string orgClientId, long dateline, string clientSecret, bool overwrite, int blockSize) : base(orgClientId, clientSecret)
        {
            if (!stream.CanRead)
            {
                throw new Exception("stream can not read");
            }

            if (!stream.CanSeek)
            {
                throw new Exception("stream can not seek");
            }

            _apiUrl = apiUrl;
            _stream = stream;
            _opName = opName;
            _opId = opId;
            _orgClientId = orgClientId;
            _dateline = dateline;
            _clientSecret = clientSecret;
            _overWrite = overwrite;
            _blockSize = blockSize;
            _fullpath = fullpath;
            _fileinfo.Fullpath = _fullpath;
        }

        public void UploadAsync(object i)
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
        }

        public Data.FileInfo Upload()
        {
            this.StartUpload(false);
            return _fileinfo;
        }

        /// <summary>
        /// 开始上传
        /// </summary>
        /// <param name="item"></param>
        private void StartUpload(bool async)
        {
            int code;
            ReturnResult result;
            _fileinfo.Filename = Util.GetFileNameFromPath(_fileinfo.Fullpath);
            _fileinfo.Filehash = Util.CaculateFileHashCode(_stream);
            _fileinfo.Filesize = _stream.Length;

            for (int trys = 0; trys <3; trys++)
            {
                result = this.AddFile();
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

                result = this.UploadInit(_fileinfo.Hash, _fileinfo.Filename, _fileinfo.Fullpath, _fileinfo.Filehash, _fileinfo.Filesize);
                if (result == null)
                {
                    continue;
                }
                if (result.Code == 202) //秒传
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

                long offset = checkSize;
                long rang_end = 0;
                long crc32 = 0;
                string range = "";
                bool uploadPartErr = false;
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

                    byte[] buffer;
                    if (offset + _blockSize >= _fileinfo.Filesize)
                    {
                        int length_end = (int)(_fileinfo.Filesize - offset);
                        buffer = new byte[length_end];
                        _stream.Read(buffer, 0, length_end);
                        crc32 = CRC32.Compute(buffer);
                        rang_end = _fileinfo.Filesize - 1;
                    }
                    else
                    {
                        buffer = new byte[_blockSize];
                        _stream.Read(buffer, 0, _blockSize);
                        crc32 = CRC32.Compute(buffer);
                        rang_end = offset + buffer.Length - 1;
                    }
                    range = offset + "-" + rang_end;

                    result = this.UploadPart(range, new MemoryStream(buffer), crc32);
                    code = result.Code;

                    if (code == (int)HttpStatusCode.OK)
                    {
                        offset += _blockSize;
                    }
                    else if (code == (int)HttpStatusCode.Accepted)
                    {
                        uploadPartErr = true;
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
                        long part_range_start = Convert.ToInt64(json["expect"]);
                        offset = part_range_start;
                        _stream.Seek(part_range_start, SeekOrigin.Begin);
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
            return;
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
        /// <param name="hash"></param>
        /// <param name="filename"></param>
        /// <param name="filehash"></param>
        /// <param name="filesize"></param>
        private ReturnResult UploadInit(string hash, string filename, string fullpath, string filehash, long filesize)
        {
            string url = _fileinfo.UploadServer + UrlUploadInit + "?org_client_id=" + _orgClientId;

            var headParameter = new Dictionary<string, string>();
            headParameter.Add("x-gk-upload-pathhash", hash);
            headParameter.Add("x-gk-upload-filename", filename);
            headParameter.Add("x-gk-upload-filehash", filehash);
            headParameter.Add("x-gk-upload-filesize", filesize.ToString());

            ReturnResult result = new RequestHelper().SetHeadParams(headParameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
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
            else if (result.Code != 202)
            {
                throw new YunkuException("fail to init upload", result);
            }
            return result;
        }

        /// <summary>
        /// 断点续传检查，返回-1表示服务器异常
        /// </summary>
        public long UploadReq()
        {
            string url = _fileinfo.UploadServer + UrlUploadReq;

            var headParameter = new Dictionary<string, string>();
            headParameter.Add("x-gk-upload-session", _session);

            long checkSize = 0;
            ReturnResult result = new RequestHelper().SetHeadParams(headParameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
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
        private ReturnResult UploadPart(String range, MemoryStream data, long crc32)
        {
            string url = _fileinfo.UploadServer + UrlUploadPart;

            var headParameter = new Dictionary<string, string>();
            //headParameter.Add("Connection", "keep-alive");
            headParameter.Add("x-gk-upload-session", _session);
            headParameter.Add("x-gk-upload-range", range);
            headParameter.Add("x-gk-upload-crc", crc32.ToString());

            return new RequestHelper().SetContent(data).SetHeadParams(headParameter).SetUrl(url).SetMethod(RequestType.Put).ExecuteSync();
        }

        /// <summary>
        /// 传输结束
        /// </summary>
        public void UploadFinish()
        {
            string url = _fileinfo.UploadServer + UrlUploadFinish;

            var headParameter = new Dictionary<string, string>();
            headParameter.Add("x-gk-upload-session", _session);

            ReturnResult result = new RequestHelper().SetHeadParams(headParameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
            if (result.Code != (int)HttpStatusCode.OK)
            {
                throw new YunkuException("fail to finish upload", result);
            }
        }

        private void UploadAbort()
        {
            if (String.IsNullOrEmpty(_fileinfo.UploadServer) || String.IsNullOrEmpty(_session))
            {
                return;
            }
            string url = _fileinfo.UploadServer + UrlUploadAbort;
            var headParameter = new Dictionary<string, string>();
            headParameter.Add("x-gk-upload-session", _session);

            new RequestHelper().SetHeadParams(headParameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        private ReturnResult AddFile()
        {
            string url = _apiUrl;

            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _orgClientId);
            parameter.Add("dateline", _dateline + "");
            parameter.Add("fullpath", _fileinfo.Fullpath);

            if (_opId > 0)
            {
                parameter.Add("op_id", _opId + "");
            }
            else if (!string.IsNullOrEmpty(_opName))
            {
                parameter.Add("op_name", _opName);
            }

            parameter.Add("overwrite", (_overWrite ? 1 : 0) + "");
            parameter.Add("sign", GenerateSign(parameter));

            parameter.Add("filesize", _fileinfo.Filesize + "");
            parameter.Add("filehash", _fileinfo.Filehash);

            ReturnResult result = new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
            return result;
        }
    }
}