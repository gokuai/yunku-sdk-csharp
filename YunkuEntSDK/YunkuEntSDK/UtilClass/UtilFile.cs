using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace YunkuEntSDK.UtilClass
{
    class UtilFile
    {
        public const string SEPERATOR="/";
        public readonly static string[] SUPPORT_FILEVIEW_EXTENSION = { "mp3", "wma", "wav", "3gp", "avi", "wmv", "mp4", "bmp", "jpg", "jpeg", "gif", "png","txt" };
        public readonly static string[] SUPPORT_VIEW_DOCUMENTS_EXTENSION = {"ppt", "pptx", "doc", "docx", "xls",
			"xlsx","pdf" };
        private const string PATH_SMALL_FORMAT = "/Image/ic_{0}.png";
        private const string PATH_BIG_FORMAT = "/Image/ic_{0}_256.png";
        private readonly static string[] _img = { "png", "gif", "jpeg", "jpg", "bmp", "psd" };
        private readonly static string[] _program = { "ipa", "exe", "pxl", "apk", "bat", "com", "xap", "bak" };
        private readonly static string[] _compressFile = { "iso", "tar", "rar", "gz", "cab",
			"zip" };
        private readonly static string[] _video = { "3gp", "asf", "avi", "m4v", "flv",
			"mkv", "mov", "mp4", "mpeg", "mpg", "rm", "rmvb", "ts", "wmv",
			"3gp", "avi" };
        private readonly static string[] _music = { "flac", "m4a", "mp3", "ogg", "aac", "ape",
			"wma", "wav" };
        private readonly static string[] _wordsFile = { "odt", "txt" };
        private readonly static string[] _officeDoc = { "ppt", "pptx", "doc", "docx", "xls",
			"xlsx" };
        private readonly static string[] _pdf = { "pdf" };
        
        public static string [] HasThumbNailImg
        {
            get { return _img; }
        }
        /// <summary>
        /// 获得后缀名
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetExtension(string uri)
        {
            if (uri == null)
            {
                return null;
            }
            int dot = uri.LastIndexOf(".");
            if (dot >= 0)
            {
                //test
                return uri.Substring(dot + 1);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取文件列表中，数据对应类型的
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetImgSrc (string fileName,bool isBig)
        {
            string ext = GetExtension(fileName).ToLower();
            string pathFormat = isBig ? PATH_BIG_FORMAT : PATH_SMALL_FORMAT;
            //图片文件
            foreach (string str in _img)
            {
                if (ext.Equals(str))
                {
                    return string.Format(pathFormat, "img");
 
                }
            }

            //音乐文件
            foreach (string str in _music)
            {
                if (ext.Equals(str))
                {
                    return string.Format(pathFormat, "music");

                }
            }

            //office文件
            foreach (string str in _officeDoc)
            {
                if (ext.Equals(str))
                {
                    return string.Format(pathFormat, ext.Substring(0, 3));
                }
            }

            //程序文件，例如exe,apk等
            foreach (string str in _program)
            {
                if (ext.Equals(str))
                {
                    return string.Format(pathFormat, "program");

                }
            }

            //视频文件
            foreach (string str in _video)
            {
                if (ext.Equals(str))
                {
                    return string.Format(pathFormat, "video");

                }
            }

            //文档文件
            foreach (string str in _wordsFile)
            {
                if (ext.Equals(str))
                {
                    return string.Format(pathFormat, "words_file");

                }
            }

            //pdf类文件
            foreach (string str in _pdf)
            {
                if (ext.Equals(str))
                {
                    return string.Format(pathFormat, "pdf");
                }
 
            }

            foreach (string str in _compressFile)
            {
                if (ext.Equals(str))
                {
                    return string.Format(pathFormat, "compress_file");
                }
            }

            return string.Format(pathFormat, "other");
        }



        ///// <summary>
        ///// 从网络异步获取缩略图
        ///// </summary>
        ///// <param name="filehash"></param>
        ///// <param name="handler"></param>
        //public static void GetThumbNailImageAsyn(string thumbSmall, OpenReadCompletedEventHandler handler)
        //{
        //    WebClient webClient = new WebClient();
        //    webClient.OpenReadCompleted += handler;
        //    webClient.Headers[HttpRequestHeader.Referer] = HttpEngine.THUMBNAIL_HTTPREQUEST_HEAD_REFERER;
        //    webClient.OpenReadAsync(new Uri(thumbSmall));

        //}

    }
}
