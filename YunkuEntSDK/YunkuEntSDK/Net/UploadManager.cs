using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using YunkuEntSDK.UtilClass;
using YunkuEntSDK.Data;


namespace YunkuEntSDK.Net
{
    internal class UploadManager : SignAbility
    {
        private const string UrlUploadInit = "/upload_init";
        private const string UrlUploadPart = "/upload_part";
        private const string UrlUploadAbort = "/upload_abort";
        private const string UrlUploadFinish = "/upload_finish";
        private const int RangSize = 65536; // 上传分块大小-64K


        private string _server = ""; // 上传服务器地址
        private string _session = ""; // 上传session
        private string _apiUrl = "";

        private string _localFullPath;
        private string _fullPath;
        private readonly string _orgClientId;
        private long _dateline;
        private string _opName;
        private int _opId;
        private bool _overWrite;

        public event CompletedEventHandler Completed;
        public event ProgressChangeEventHandler ProgresChanged;

        public delegate void CompletedEventHandler(object sender, CompletedEventArgs e);

        public delegate void ProgressChangeEventHandler(object sender, ProgressEventArgs e);


        public UploadManager(string apiUrl, string localFullPath, string fullPath,
            string opName, int opId, string orgClientId, long dateline, string clientSecret, bool overWrite)
        {
            _apiUrl = apiUrl;
            _localFullPath = localFullPath;
            _fullPath = fullPath;
            _opName = opName;
            _opId = opId;
            _orgClientId = orgClientId;
            _dateline = dateline;
            _clientSecret = clientSecret;
            _overWrite = overWrite;
        }

        public void DoUpload()
        {
            try
            {
                StartUpload();
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
                            LocalFullPath = _localFullPath
                        });
                }
                // ignored
                UploadAbort();
            }
        }


        /// <summary>
        /// 开始上传
        /// </summary>
        /// <param name="item"></param>
        private void StartUpload()
        {
            if (File.Exists(_localFullPath))
            {
                using (var fs = new FileStream(_localFullPath, FileMode.Open))
                {
                    int code;
                    Stream stream = fs;
                    string fullpath = _fullPath;
                    string filehash = Util.CaculateFileHashCode(stream);
                    string filename = Util.GetFileNameFromPath(_fullPath);
                    long filesize = stream.Length;


                    ReturnResult returnResult = ReturnResult.Create(AddFile(filesize, filehash, _fullPath));
                    FileOperationData data = FileOperationData.Create(returnResult.Result, returnResult.Code);
                    if (data != null)
                    {
                        if (data.Code == (int) HttpStatusCode.OK)
                        {
                            if (data.Status != FileOperationData.StateNoupload)
                            {
                                _server = data.Server;
                                UploadInit(data.UuidHash, filename, _fullPath, filehash, filesize);

//                                long range_index = 0;
//                                long range_end = 0;
//                                long datalength = -1;
                                long crc32 = 0;
                                long offset = 0;
                                long rang_end = 0;
                                String range = "";
                                byte[] dataBytes = Util.ReadToEnd(fs);

                                while (offset < filesize - 1)
                                {
                                    if (ProgresChanged != null)
                                    {
                                        ProgresChanged(this, new ProgressEventArgs()
                                        {
                                            ProgressPercent = (int) (((float) offset/filesize)*100),
                                            LocalFullPath = _localFullPath
                                        });
                                    }

                                    byte[] buffer = new byte[RangSize];

                                    if (offset + buffer.Length >= filesize)
                                    {
                                        int length_end = (int) (filesize - offset);
                                        byte[] buffer_end = new byte[length_end];
                                        for (int i = 0; i < buffer_end.Length; i++)
                                        {
                                            buffer_end[i] = dataBytes[i + offset];
                                        }
                                        crc32 = CRC32.Compute(buffer_end);
                                        rang_end = filesize - 1;
                                        buffer = buffer_end;
                                    }
                                    else
                                    {
                                        for (int i = 0; i < buffer.Length; i++)
                                        {
                                            buffer[i] = dataBytes[i + offset];
                                        }

//                                            crc.ComputeHash(buffer, 0, buffer.Length);
                                        crc32 = CRC32.Compute(buffer);
                                        rang_end = offset + buffer.Length - 1;
                                    }
                                    range = offset + "-" + rang_end;

                                    returnResult = UploadPart(range, new MemoryStream(buffer), crc32);
                                    code = returnResult.Code;

                                    if (code == (int) HttpStatusCode.OK)
                                    {
                                        offset += RangSize;
                                    }
                                    else if (code == (int) HttpStatusCode.Accepted)
                                    {
                                        break;
                                    }
                                    else if (code >= (int) HttpStatusCode.InternalServerError)
                                    {
                                        ReGetUpoadServer(fullpath, filehash, filesize);
                                        continue;
                                    }
                                    else if (code == (int) HttpStatusCode.Unauthorized)
                                    {
                                        UploadInit(data.UuidHash, filename, fullpath, filehash, filesize);
                                        continue;
                                    }
                                    else if (code == (int) HttpStatusCode.Conflict)
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
                                Completed(this, new CompletedEventArgs() {LocalFullPath = _localFullPath});
                            }
                        }
                        else
                        {
                            throw new Exception(data.ErrorCode + ":" + data.ErrorMessage);
                        }
                    }
                    else
                    {
                        throw new Exception("can't connect server");
                    }
                }
            }
            else
            {
                LogPrint.Print(_localFullPath + " not exist");
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
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestMethod = RequestType.Post;
            request.AppendHeaderParameter("x-gk-upload-pathhash", hash);
            request.AppendHeaderParameter("x-gk-upload-filename", filename);
            request.AppendHeaderParameter("x-gk-upload-filehash", filehash);
            request.AppendHeaderParameter("x-gk-upload-filesize", filesize.ToString());

            request.RequestUrl = _server + UrlUploadInit + "?org_client_id=" + _orgClientId;
            request.Request();

            ReturnResult returnResult = ReturnResult.Create(request.Result);
            if (returnResult.Code == (int) HttpStatusCode.OK)
            {
                var json = (IDictionary<string, object>) SimpleJson.DeserializeObject(returnResult.Result);
                _session = SimpleJson.TryStringValue(json, "session");
            }
            else if (returnResult.Code >= (int) HttpStatusCode.InternalServerError)
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
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = _server + UrlUploadPart;
            request.RequestMethod = RequestType.Put;
            //request.AppendHeaderParameter("Connection", "keep-alive");
            request.AppendHeaderParameter("x-gk-upload-session", _session);
            request.AppendHeaderParameter("x-gk-upload-range", range);
            request.AppendHeaderParameter("x-gk-upload-crc", crc32.ToString());
            request.Content = data;
            request.Request();
            return ReturnResult.Create(request.Result);
        }

        public void UploadCheck()
        {
            string returnString = UploadFinish();
            ReturnResult returnResult = ReturnResult.Create(returnString);
            if (returnResult.Code == (int) HttpStatusCode.OK)
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
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = _server + UrlUploadFinish;
            request.RequestMethod = RequestType.Post;
            request.AppendHeaderParameter("x-gk-upload-session", _session);
            request.Request();
            return request.Result;
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
            string url = _server + UrlUploadAbort;
            var request = new HttpRequestSyn {RequestUrl = url};
            request.AppendParameter("x-gk-upload-session", _session);
            request.Request();
        }

        private string AddFile(long filesize, string filehash, string fullpath)
        {
            var request = new HttpRequestSyn {RequestUrl = _apiUrl};
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", _dateline + "");
            request.AppendParameter("fullpath", fullpath);
            if (_opId > 0)
            {
                request.AppendParameter("op_id", _opId + "");
            }
            else if (!string.IsNullOrEmpty(_opName))
            {
                request.AppendParameter("op_name", _opName);
            }
            request.AppendParameter("overwrite", (_overWrite ? 1 : 0) + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));

            request.AppendParameter("filesize", filesize + "");
            request.AppendParameter("filehash", filehash);

            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }
    }
}