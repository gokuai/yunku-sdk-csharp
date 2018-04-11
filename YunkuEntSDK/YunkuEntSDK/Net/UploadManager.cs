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
        private const string UrlUploadPart = "/upload_part";
        private const string UrlUploadAbort = "/upload_abort";
        private const string UrlUploadFinish = "/upload_finish";

        private int _blockSize = 524288; // 上传分块大小-512K

        private string _server = ""; // 上传服务器地址
        private string _session = ""; // 上传session
        private string _apiUrl = "";
        
        private readonly string _orgClientId;
        private long _dateline;
        private string _opName;
        private int _opId;
        private bool _overWrite;
        private Stream _stream;

        private Data.FileInfo _fileinfo;
        private UploadResult _result;

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
            _fileinfo = new Data.FileInfo() { Fullpath = fullpath };
            _result = new UploadResult();
        }

        public void UploadAsync(object i)
        {
            try
            {
                StartUpload(true);
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
                            Fullpath = _fileinfo.Fullpath,
                            FileInfo = _fileinfo
                        });
                }
                UploadAbort();
            }
        }

        public UploadResult Upload()
        {
            try
            {
                StartUpload(false);
            }
            catch (Exception e)
            {
                UploadAbort();
                _result.IsError = true;
                _result.ErrorMessage = e.Message;
            }
            _result.FileInfo = _fileinfo;
            return _result;
        }


        /// <summary>
        /// 开始上传
        /// </summary>
        /// <param name="item"></param>
        private void StartUpload(bool async)
        {
            int code;
            _fileinfo.Filename = Util.GetFileNameFromPath(_fileinfo.Fullpath);
            _fileinfo.Filehash = Util.CaculateFileHashCode(_stream);
            _fileinfo.Filesize = _stream.Length;

            ReturnResult returnResult = ReturnResult.Create(AddFile(_fileinfo.Filesize, _fileinfo.Filehash, _fileinfo.Fullpath));
            FileOperationData data = FileOperationData.Create(returnResult.Result, returnResult.Code);
            if (data == null)
            {
                throw new Exception("can't connect server");
            }
            if (data.Code != (int)HttpStatusCode.OK)
            {
                throw new Exception(data.ErrorCode + ":" + data.ErrorMessage);
            }
            if (data.Status != FileOperationData.StateNoupload)
            {
                _fileinfo.Hash = data.UuidHash;
                _server = data.Server;

                if (string.IsNullOrEmpty(_server))
                {
                    throw new Exception(" The server is empty ");
                }
                else
                {
                    LogPrint.Print(" The server is " + _server);
                }

                UploadInit(data.UuidHash, _fileinfo.Filename, _fileinfo.Fullpath, _fileinfo.Filehash, _fileinfo.Filesize);

                long crc32 = 0;
                long offset = 0;
                long rang_end = 0;
                String range = "";

                while (offset < _fileinfo.Filesize - 1)
                {
                    if (ProgresChanged != null)
                    {
                        ProgresChanged(this, new ProgressEventArgs()
                        {
                            ProgressPercent = (int)(((float)offset / _fileinfo.Filesize) * 100),
                            Fullpath = _fileinfo.Fullpath
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

                    returnResult = UploadPart(range, new MemoryStream(buffer), crc32);
                    code = returnResult.Code;

                    if (code == (int)HttpStatusCode.OK)
                    {
                        offset += _blockSize;
                    }
                    else if (code == (int)HttpStatusCode.Accepted)
                    {
                        break;
                    }
                    else if (code >= (int)HttpStatusCode.InternalServerError)
                    {
                        ReGetUpoadServer(_fileinfo.Fullpath, _fileinfo.Filehash, _fileinfo.Filesize);
                        continue;
                    }
                    else if (code == (int)HttpStatusCode.Unauthorized)
                    {
                        UploadInit(data.UuidHash, _fileinfo.Filename, _fileinfo.Fullpath, _fileinfo.Filehash, _fileinfo.Filesize);
                        continue;
                    }
                    else if (code == (int)HttpStatusCode.Conflict)
                    {
                        var json =
                            (IDictionary<string, object>)
                                SimpleJson.DeserializeObject(returnResult.Result);
                        long part_range_start = Convert.ToInt64(json["expect"]);
                        offset = part_range_start;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                UploadCheck();
            }

            //file upload success if reach here
            if (Completed != null)
            {
                Completed(this, new CompletedEventArgs() { Fullpath = _fileinfo.Fullpath, FileInfo = _fileinfo });
            }
        }

        /// <summary>
        /// 上传初始化
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="filename"></param>
        /// <param name="filehash"></param>
        /// <param name="filesize"></param>
        private void UploadInit(string hash, string filename, string fullpath, string filehash, long filesize)
        {
            string url = _server + UrlUploadInit + "?org_client_id=" + _orgClientId;

            var headParameter = new Dictionary<string, string>();
            headParameter.Add("x-gk-upload-pathhash", hash);
            headParameter.Add("x-gk-upload-filename", filename);
            headParameter.Add("x-gk-upload-filehash", filehash);
            headParameter.Add("x-gk-upload-filesize", filesize.ToString());

            string returnString = new RequestHelper().SetHeadParams(headParameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
            ReturnResult returnResult = ReturnResult.Create(returnString);
            if (returnResult.Code == (int)HttpStatusCode.OK)
            {
                var json = (IDictionary<string, object>)SimpleJson.DeserializeObject(returnResult.Result);
                _session = SimpleJson.TryStringValue(json, "session");
            }
            else if (returnResult.Code >= (int)HttpStatusCode.InternalServerError)
            {
                ReGetUpoadServer(fullpath, filehash, filesize);
                UploadInit(hash, filename, fullpath, filehash, filesize);
            }
            else
            {
                throw new Exception();
            }
        }


        /// <summary>
        /// 分块上传
        /// </summary>
        /// <param name="range"></param>
        /// <param name="data"></param>
        /// <param name="crc32"></param>
        private ReturnResult UploadPart(String range, MemoryStream data, long crc32)
        {
            string url = _server + UrlUploadPart;

            var headParameter = new Dictionary<string, string>();
            //headParameter.Add("Connection", "keep-alive");
            headParameter.Add("x-gk-upload-session", _session);
            headParameter.Add("x-gk-upload-range", range);
            headParameter.Add("x-gk-upload-crc", crc32.ToString());

            string returnString = new RequestHelper().SetContent(data).SetHeadParams(headParameter).SetUrl(url).SetMethod(RequestType.Put).ExecuteSync();

            return ReturnResult.Create(returnString);
        }

        public void UploadCheck()
        {
            string returnString = UploadFinish();
            ReturnResult returnResult = ReturnResult.Create(returnString);
            if (returnResult.Code == (int)HttpStatusCode.OK)
            {
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 传输结束
        /// </summary>
        public string UploadFinish()
        {
            string url = _server + UrlUploadFinish;

            var headParameter = new Dictionary<string, string>();
            headParameter.Add("x-gk-upload-session", _session);

            return new RequestHelper().SetHeadParams(headParameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 更换上传地址
        /// </summary>
        /// <param name="filesize"></param>
        private void ReGetUpoadServer(string fullpath, string filehash, long filesize)
        {
            ReturnResult returnResult = ReturnResult.Create(AddFile(filesize, filehash, fullpath));
            FileOperationData data = FileOperationData.Create(returnResult.Result, returnResult.Code);
            if (data != null)
            {
                _server = data.Server;
            }
        }

        private void UploadAbort()
        {
            if (_server.Equals(String.Empty))
            {
                return;
            }
            string url = _server + UrlUploadAbort;
            var headParameter = new Dictionary<string, string>();
            headParameter.Add("x-gk-upload-session", _session);

            new RequestHelper().SetHeadParams(headParameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        private string AddFile(long filesize, string filehash, string fullpath)
        {
            string url = _apiUrl;

            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _orgClientId);
            parameter.Add("dateline", _dateline + "");
            parameter.Add("fullpath", fullpath);

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

            parameter.Add("filesize", filesize + "");
            parameter.Add("filehash", filehash);

            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }
    }
}