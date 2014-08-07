using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YunkuEntSDK;
using YunkuEntSDK.UtilClass;
using YunkuSDKEntDemo.Model;

namespace YunkuSDKEntDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        const string UESRNAME = "Brandon";
        const string PASSWORD = "123456";
        const string CLIENT_ID = "b2b54fa4261f9cf5e4772e6359f96161";
        const string CLIENT_SECRET = "134dba8e0adc4e59b511c09aa1ebf71e";

        /// <summary>
        /// 分析错误信息
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private void DeserializeErrorMsg(string result, HttpStatusCode code)
        {
            string msg = "";
            //复制到剪贴板
            Clipboard.SetDataObject(result);
            if (code == HttpStatusCode.OK)
            {
                //成功则返回结果
                
                if (result.Equals(string.Empty))
                {
                    msg= "返回成功";
                }
            }
            else
            {
                //返回错误信息
                BaseData data = BaseData.Create(result);
                if (data != null)
                {
                    msg= data.ErrorCode + ":" + data.ErrorMessage;
                }
            }
            TB_Result.Text += result+"\r\n";

        }

        private void OnEnterKeyPress(object sender, KeyPressEventArgs e)
        {
            YunkuEngine.LogPrintAvialable = true;
            TB_Result.Text = "doing\r\n";
            YunkuEngine yunku = new YunkuEngine(UESRNAME, PASSWORD, CLIENT_ID, CLIENT_SECRET);
            //获取授权
            yunku.AccessToken(true);
            //获取库列表
            //DeserializeErrorMsg(yunku.GetLibList(), yunku.StatusCode);

            //获取库授权
            //DeserializeErrorMsg(yunku.BindLib(146540, "", ""), yunku.StatusCode);
            //取消库授权
            //DeserializeErrorMsg(yunku.UnBindLib("9affb8f78fd5914a7218d7561db6ddec"), yunku.StatusCode);
            //获取文件列表
            //DeserializeErrorMsg(yunku.GetFileList("0c0aa6d2274dc51e5cb6c0cf8e13b25e", "b219ae7113d098249fef36d261360d0b", 
            //    (int)Util.GetUnixDataline(), 0, ""), yunku.StatusCode);
            //获取更新列表
            //DeserializeErrorMsg(yunku.GetUpdateList("0c0aa6d2274dc51e5cb6c0cf8e13b25e", "b219ae7113d098249fef36d261360d0b",
            //    (int)Util.GetUnixDataline(),false, 0), yunku.StatusCode);
            //获取文件（夹）信息
            //DeserializeErrorMsg(yunku.GetFileInfo("0c0aa6d2274dc51e5cb6c0cf8e13b25e", "b219ae7113d098249fef36d261360d0b",
            //    (int)Util.GetUnixDataline(), "test"), yunku.StatusCode);
            //创建文件夹
            //DeserializeErrorMsg(yunku.CreateFolder("0c0aa6d2274dc51e5cb6c0cf8e13b25e", "b219ae7113d098249fef36d261360d0b",
            //    (int)Util.GetUnixDataline(), "testFolder", "Brandon"), yunku.StatusCode);
            //上传文件 文件不得超过50MB
            //DeserializeErrorMsg(yunku.CreateFile("0c0aa6d2274dc51e5cb6c0cf8e13b25e", "b219ae7113d098249fef36d261360d0b",
            //    (int)Util.GetUnixDataline(), "test", "Brandon", "D:\\test.txt"), yunku.StatusCode);

            //删除文件
            //DeserializeErrorMsg(yunku.Del("0c0aa6d2274dc51e5cb6c0cf8e13b25e", "b219ae7113d098249fef36d261360d0b",
            //    (int)Util.GetUnixDataline(), "test","Brandon"), yunku.StatusCode);

            //移动文件
            //DeserializeErrorMsg(yunku.Move("0c0aa6d2274dc51e5cb6c0cf8e13b25e", "b219ae7113d098249fef36d261360d0b",
            //    (int)Util.GetUnixDataline(), "testFolder","1/testFolder", "Brandon"), yunku.StatusCode);
            ////文件连接
            //DeserializeErrorMsg(yunku.Link("0c0aa6d2274dc51e5cb6c0cf8e13b25e", "b219ae7113d098249fef36d261360d0b",
            //    (int)Util.GetUnixDataline(), "test"), yunku.StatusCode);
            ////发送消息
            DeserializeErrorMsg(yunku.SendMsg("0c0aa6d2274dc51e5cb6c0cf8e13b25e", "b219ae7113d098249fef36d261360d0b",
                (int)Util.GetUnixDataline(), "msgTest", "msg", "", "", "Brandon"), yunku.StatusCode);
        }


    }
}
