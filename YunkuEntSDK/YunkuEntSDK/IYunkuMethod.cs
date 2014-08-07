using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunkuEntSDK
{
    interface IYunkuMethod
    {
        private const string OAUTH_HOST = "http://a.goukuai.cn";
        private const string LIB_HOST = "http://a-lib.goukuai.cn";
        private const string URL_API_TOKEN = OAUTH_HOST + "/oauth2/token";
        private const string URL_API_CREATE_LIB = LIB_HOST + "/1/org/create";
        private const string URL_API_GET_LIB_LIST = LIB_HOST + "/1/org/ls";
        private const string URL_API_BIND = LIB_HOST + "/1/org/bind";
        private const string URL_API_UNBIND = LIB_HOST + "/1/org/unbind";
        private const string URL_API_FILELIST = LIB_HOST + "/1/file/ls";
        private const string URL_API_UPDATE_LIST = LIB_HOST + "/1/file/updates";
        private const string URL_API_FILE_INFO = LIB_HOST + "/1/file/info";
        private const string URL_API_CREATE_FOLDER = LIB_HOST + "/1/file/create_folder";
        private const string URL_API_CREATE_FILE = LIB_HOST + "/1/file/create_file";
        private const string URL_API_DEL_FILE = LIB_HOST + "/1/file/del";
        private const string URL_API_MOVE_FILE = LIB_HOST + "/1/file/move";
        private const string URL_API_LINK_FILE = LIB_HOST + "/1/file/link";
        private const string URL_API_SENDMSG = LIB_HOST + "/1/file/sendmsg";

    }
}
