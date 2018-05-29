using System;
using System.Net;
using System.Windows.Forms;
using YunkuEntSDK;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;
using YunkuSDKEntDemo.Model;

namespace YunkuSDKEntDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     分析错误信息
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private void DeserializeReturn(ReturnResult result)
        {
            int code = result.Code;
            string msg = "";
            string body = result.Body;
            //复制到剪贴板

            if (code == (int)HttpStatusCode.OK)
            {
                //成功则返回结果

                if (string.IsNullOrEmpty(body))
                {
                    msg = "返回成功";
                }
            }
            else
            {
                //返回错误信息
                BaseData data = BaseData.Create(body);
                if (data != null)
                {
                    msg = data.ErrorCode + ":" + data.ErrorMessage;
                }
            }
            Clipboard.SetDataObject(result);
            TB_Result.Text += msg + "\r\n";
            TB_Result.Text += result.Body + "\r\n";
            TB_Result.Text += "==========================\r\n";
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            //=======条件设置=======//
            DebugConfig.LogPrintAvialable = false;
            DebugConfig.LogPath = ""; //日志文件没有做大小限制
            
            ConfigHelper.SetApiHost("http://serverhost/m-open");
            ConfigHelper.SetWebHost("http://serverhost");
            ConfigHelper.SetLanguage("zh-CN");
            //ConfigHelper.SetUploadBlockSize(1024 * 1024);

            var entFileManager = new EntFileManager(SdkConfig.orgClientId, SdkConfig.orgClientSecret);

            //创建文件夹
            DeserializeReturn(entFileManager.CreateFolder("doc", ""));

            //分块上传，默认分块上传大小为10MB
            //try
            //{
            //    FileInfo info = entFileManager.UploadByBlock("doc/test.docx", "", 0, @"D:\test.docx", true);
            //    TB_Result.Text += info.Fullpath + " upload success\r\n";
            //}
            //catch (YunkuException ex)
            //{
            //    TB_Result.Text += "error:" + ex.Message + " ";
            //    DeserializeReturn(ex.Result);
            //}
            //catch (Exception ex)
            //{
            //    TB_Result.Text += "upload error:" + ex.Message + "\r\n";
            //}

            //异步方式分块上传，默认分块上传大小为10MB
            //entFileManager.UploadByBlockAsync("doc/curl.zip", "", 0, @"D:\curl.zip", true, UploadCompeleted, ProgressChanged);

            //获取下载地址
            //DeserializeReturn(entFileManager.GetDownloadUrlByFullpath("doc/test.docx"));


            //获取预览地址
            //DeserializeReturn(entFileManager.GetPreviewUrlByFullpath("doc/test.docx", true, "gogo", false));

            //获取文件（夹）信息
            //DeserializeReturn(entFileManager.GetFileInfoByFullpath("doc/test.docx"));

            //获取文件列表
            //DeserializeReturn(entFileManager.GetFileList());

            //删除文件 如果是多个文件则用逗号隔开fullpath,例如"test1,test2"
            //DeserializeReturn(entFileManager.Del("doc", "", true));
        }


        private void ProgressChanged(object sender, ProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { ProgressChanged(sender, e); });
                return;
            }

            TB_Result.Text += e.Fullpath + " progress:" + e.ProgressPercent + "\r\n";
        }

        private void UploadCompeleted(object sender, CompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { UploadCompeleted(sender, e); });
                return;
            }

            if (e.IsError)
            {
                TB_Result.Text += e.Fullpath + " upload error:" + e.ErrorMessage + "\r\n";
            }
            else
            {
                TB_Result.Text += e.Fullpath + " upload success\r\n";
            }
        }
    }
}