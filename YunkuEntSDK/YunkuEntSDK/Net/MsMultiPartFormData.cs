using YunkuEntSDK.UtilClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunkuEntSDK.Net
{
    class MsMultiPartFormData
    {
        private const int BLOCK_SIZE = 4096;
        private List<byte> formData;
        public string Boundary = string.Format("--{0}--", Guid.NewGuid());
        private string fileContentType = "Content-Type: {0}";
        private string fileField = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"";
        private string paramsFile = "Content-Disposition: form-data; name=\"{0}\"";
        private Encoding encode = Encoding.GetEncoding("UTF-8");
        public MsMultiPartFormData()
        {
            formData = new List<byte>();
        }

        public void AddFile(string FieldName, string FileName, byte[] FileContent, string ContentType)
        {
            string newFileField = fileField;
            string newFileContentType = fileContentType;
            newFileField = string.Format(newFileField, FieldName, FileName);
            newFileContentType = string.Format(newFileContentType, ContentType);
            formData.AddRange(encode.GetBytes("--" + Boundary + "\r\n"));
            formData.AddRange(encode.GetBytes(newFileField + "\r\n"));
            formData.AddRange(encode.GetBytes(newFileContentType + "\r\n\r\n"));
            int offset = 0;
            int fileSize = FileContent.Length;
            byte[] buffer = new byte[BLOCK_SIZE];
            LogPrint.Print("--------------------->Begin to Convert<--------------");
            while (offset <= fileSize)
            {
                
                if (offset + buffer.Length >= fileSize)
                {
                    int length_end = (int)(fileSize - offset);
                    byte[] buffer_end = new byte[length_end];
                    for (int i = 0; i < buffer_end.Length; i++)
                    {
                        buffer_end[i] = FileContent[i + offset];
                    }
                    buffer = buffer_end;
                }
                else
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = FileContent[i + offset];
                    }

                }
                offset += BLOCK_SIZE;
                formData.AddRange(buffer);
            }
            formData.AddRange(encode.GetBytes("\r\n"));
        }

        public void AddStreamFile(string FieldName, string FileName, byte[] FileContent)
        {
            AddFile(FieldName, FileName, FileContent, "application/octet-stream");
        }

        public void AddParams(string key,string value) 
        {
            string newFileField = fileField;
            string newFileContentType = fileContentType;
            string newParamKey = string.Format(paramsFile,key);
            string newParamValue = string.Format(paramsFile, value);

            formData.AddRange(encode.GetBytes("--" + Boundary + "\r\n"));
            formData.AddRange(encode.GetBytes(newParamKey + "\r\n"));
            formData.AddRange(encode.GetBytes("\r\n"));
            formData.AddRange(encode.GetBytes(value+"\r\n"));

        }

        public void PrepareFormData()
        {
            formData.AddRange(encode.GetBytes("--" + Boundary + "--"));
        }

        public List<byte> GetFormData()
        {
            return formData;
        }

    }  
}
