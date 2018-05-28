using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace YunkuEntSDK.UtilClass
{
    /// <summary>
    /// 操作集
    /// </summary>
    public class Util
    {
        public const string LogTag = "Util";

        /// <summary>
        /// HMacsha1加密，最后做base64加密
        /// </summary>
        /// <param name="toEncodeString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncodeToHMACSHA1(string toEncodeString, string key)
        {
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.UTF8.GetBytes(key);

            byte[] dataBuffer = Encoding.UTF8.GetBytes(toEncodeString);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }

        public static string DecodeBase64(string toDecodeString)
        {
            byte[] decodedBytes = Convert.FromBase64String(toDecodeString);
            string decodedText = Convert.ToString(decodedBytes);
            return decodedText;
        }


        public static string EncodeBase64(string toEncodeString)
        {
            byte[] dataBuffer = Encoding.UTF8.GetBytes(toEncodeString);
            return Convert.ToBase64String(dataBuffer);
        }

        /// <summary>
        /// 流文件转byte
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadToEnd(Stream stream)
        {
            long originalPosition = stream.Position;
            stream.Position = 0;

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                stream.Position = originalPosition;
            }
        }

        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary>
        /// int 数组转string
        /// </summary>
        /// <param name="intArray"></param>
        /// <param name="conv"></param>
        /// <returns></returns>
        public static string IntArrayToString(int[] intArray, string conv)
        {
            string strReturn = "";
            int length = intArray.Length;
            if (length > 0)
            {
                for (int i = 0; i < length - 1; i++)
                {
                    strReturn += intArray[i] + conv;
                }
                strReturn += intArray[length - 1];
            }
            return strReturn;
        }

        public static string StrArrayToString(string[] strArray, string conv)
        {
            string strReturn = "";
            int length = strArray.Length;
            if (length > 0)
            {
                for (int i = 0; i < length - 1; i++)
                {
                    strReturn += strArray[i] + conv;
                }
                strReturn += strArray[length - 1];
            }
            return strReturn;

        }

        /// <summary>
        /// 获取文件父级路径
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="seperatorString"></param>
        /// <returns></returns>
        public static string GetParentPath(string path)
        {
            string seperatorString = UtilFile.Seperator;
            if (path.EndsWith(seperatorString))
            {
                int index = path.Remove(path.Length - seperatorString.Length).LastIndexOf(seperatorString);
                if (index == -1)
                {
                    return "";
                }
                return path.Substring(0, index);
            }
            else
            {
                int index = path.LastIndexOf(seperatorString);
                if (index == -1)
                {
                    return "";
                }
                return path.Substring(0, index);
            }

        }

        /// <summary>
        /// 在路径中获取文件名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="seperatorString"></param>
        /// <returns></returns>
        public static string GetFileNameFromPath(string path)
        {
            string seperatorString = UtilFile.Seperator;
            if (path.EndsWith(seperatorString))
            {
                string newPath = path.Remove(path.Length - seperatorString.Length);
                int index = newPath.LastIndexOf(seperatorString);
                if (index == -1)
                {
                    return newPath;
                }
                return newPath.Substring(index + seperatorString.Length);
            }
            else
            {
                int index = path.LastIndexOf(seperatorString);
                return path.Substring(index + seperatorString.Length);
            }
        }


        /// <summary>
        /// 文件大小转换
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string FormatSize(long size)
        {
            return String.Format(new FileSizeFormatProvider(), "{0:fs}", size);
        }

        ///// <summary>
        /////检验邮箱地址 
        ///// </summary>
        ///// <param name="emailString"></param>
        ///// <returns></returns>
        //public static bool CheckEmailFormat(string emailString)
        //{
        //    string regexEmail = "\\w{1,}@\\w{1,}\\.\\w{1,}";

        //    System.Text.RegularExpressions.RegexOptions options = ((System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace

        //                        | System.Text.RegularExpressions.RegexOptions.Multiline)

        //                                        | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //    System.Text.RegularExpressions.Regex regEmail = new System.Text.RegularExpressions.Regex(regexEmail, options);
        //    if (regEmail.IsMatch(emailString)) 
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public static bool IsSpecialEmail(string emailStr)
        //{
        //    string patternStr = @".+?@gk\.oauth\.([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)";
        //    if (!Regex.IsMatch(emailStr, patternStr))
        //    {
        //        return false;
        //    }
        //    return true;
        //}



        /// <summary>
        /// 计算sha1
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CaculateSha1(string text)
        {
            SHA1Managed s = new SHA1Managed();
            UTF8Encoding enc = new UTF8Encoding();
            s.ComputeHash(enc.GetBytes(text.ToCharArray()));
            return BitConverter.ToString(s.Hash).Replace("-", "");
        }

        /// <summary>
        /// RFC822标准的时间格式
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertToRFC822DateFormat(DateTime date)
        {

            string _rfc822Format = "ddd, dd MMM yyyy HH:mm:ss";
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            string _tmp = date.ToUniversalTime().ToString(_rfc822Format);
            return _tmp + " UT";

        }



        public const string TIMEFORMAT_YMD = "yyyyMMdd";
        public const string TIMEFORMAT_YMD_WITH_SEPERATE = "yyyy/MM/dd";
        public const string TIMEFORMAT = "yyyyMMdd HH:mm:ss";
        public const string TIMEFORMAT_WITHOUT_SECONDS = "yyyy/MM/dd HH:mm";
        public const string TIMEFORMAT_HS = "HH:mm";

        public static string TimeFormat(long dateline, string format)
        {
            DateTime time = UnixTimestampConverter.ConvertLocalFromTimestamp(dateline * 1000);
            return time.ToString(format);
        }

        /// <summary>
        /// 页面编码转化为ASCII
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string ConvertExtendedASCII(string HTML)
        {
            StringBuilder str = new StringBuilder();
            char c;
            for (int i = 0; i < HTML.Length; i++)
            {
                c = HTML[i];
                if (Convert.ToInt32(c) > 127)
                {
                    str.Append("&#" + Convert.ToInt32(c) + ";");
                }
                else
                {
                    str.Append(c);
                }
            }
            return str.ToString();
        }


        ///// <summary>
        ///// 路径缩略处理
        ///// </summary>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //public static string FilePathOmission(string path)
        //{
        //    int limit = 20;
        //    int len = path.Length;
        //    if (len > limit)
        //    {
        //        return path.Substring(0, limit/2) + "..." + path.Substring(len - limit/2, limit/2);

        //    }
        //    return path;

        //}

        /// <summary>
        /// 从文件流计算出filehash
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static string CaculateFileHashCode(Stream fileStream)
        {
            SHA1Managed s = new SHA1Managed();
            UTF8Encoding enc = new UTF8Encoding();
            s.ComputeHash(ReadToEnd(fileStream));
            return BitConverter.ToString(s.Hash).Replace("-", "").ToLower();

        }

        public static long GetUnixDataline()
        {
            return UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now) / 1000;
        }
    }
}