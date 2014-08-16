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

        const string UESRNAME = "";
        const string PASSWORD = "";
        const string CLIENT_ID = "";
        const string CLIENT_SECRET = "";

        /// <summary>
        /// 分析错误信息
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private void DeserializeReturn(string result, HttpStatusCode code)
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


        private void MainForm_Load(object sender, EventArgs e)
        {
            //=======条件设置=======//
            DebugConfig.LogPrintAvialable = true;
            DebugConfig.LogPath = "";//日志文件没有做大小限制

            //=========企业库操作============//
            EntLibManager entLibManager = new EntLibManager(UESRNAME, PASSWORD, CLIENT_ID, CLIENT_SECRET);
            //获取授权
            //entLibManager.AccessToken(true);
            //获取库列表
            //DeserializeReturn(entLibManager.GetLibList(), entLibManager.StatusCode);

            //获取库授权
            //DeserializeReturn(entLibManager.BindLib(146540, "", ""), entLibManager.StatusCode);

            //取消库授权
            //DeserializeReturn(entLibManager.UnBindLib("9affb8f78fd5914a7218d7561db6ddec"), entLibManager.StatusCode);

            //添加成员
            //DeserializeReturn(entLibManager.AddMembers(32662, 1194, new int[] { 21122 }), entLibManager.StatusCode);

            //批量修改单库中成员角色
            //DeserializeReturn(entLibManager.SetMemberRole(32662, 1194, new int[] { 21122 }), entLibManager.StatusCode);

            //获取某一个库的成员
            //DeserializeReturn(entLibManager.GetMembers(0, 20, 32662),entLibManager.StatusCode);

            //从库中删除成员
            //DeserializeReturn(entLibManager.DelMember(32662, new int[] { 21122 }), entLibManager.StatusCode);

            //获取某一个企业的分组列表
            //DeserializeReturn(entLibManager.GetGroups(32662),entLibManager.StatusCode);

            //库上添加分组
            //DeserializeReturn(entLibManager.AddGroup(32662, 1086, 1194), entLibManager.StatusCode);
            
            //设置分组上的角色
            //DeserializeReturn(entLibManager.SetGroupRole(32662, 1086, 1194), entLibManager.StatusCode);

            //库上删除分组
            //DeserializeReturn(entLibManager.DelGroup(32662,1086), entLibManager.StatusCode);

            //==========企业文件操作============//
            string orgClientId = "0c0aa6d2274dc51e5cb6c0cf8e13b25e";
            string orgClientSecret = "b219ae7113d098249fef36d261360d0b";

            EntFileManager entFileManager = new EntFileManager(orgClientId, orgClientSecret);


            //获取文件列表
            //DeserializeReturn(entFileManager.GetFileList((int)Util.GetUnixDataline(), 0, ""), entLibManager.StatusCode);

            //获取更新列表
            //DeserializeReturn(entFileManager.GetUpdateList((int)Util.GetUnixDataline(), false, 0), entLibManager.StatusCode);

            //获取文件（夹）信息
            //DeserializeReturn(entFileManager.GetFileInfo((int)Util.GetUnixDataline(), "test"), entLibManager.StatusCode);

            //创建文件夹
            //DeserializeReturn(entFileManager.CreateFolder((int)Util.GetUnixDataline(), "testFolder", "Brandon"), entLibManager.StatusCode);

            //上传文件 文件不得超过50MB
            //DeserializeReturn(entFileManager.CreateFile((int)Util.GetUnixDataline(), "test", "Brandon", "D:\\test.txt"), entLibManager.StatusCode);

            //删除文件
            //DeserializeReturn(entFileManager.Del((int)Util.GetUnixDataline(), "test", "Brandon"), entLibManager.StatusCode);

            //移动文件
            //DeserializeReturn(entFileManager.Move((int)Util.GetUnixDataline(), "testFolder", "1/testFolder", "Brandon"), entLibManager.StatusCode);

            ////文件连接
            //DeserializeReturn(entFileManager.Link((int)Util.GetUnixDataline(), "test"), entLibManager.StatusCode);

            ////发送消息
            //DeserializeReturn(entFileManager.SendMsg((int)Util.GetUnixDataline(), "msgTest", "msg", "", "", "Brandon"), entLibManager.StatusCode);

            //=======企业操作========//
            EntManager entManager = new EntManager(UESRNAME, PASSWORD, CLIENT_ID, CLIENT_SECRET);
            //获取授权
            entManager.AccessToken(true);

            //获取成员
            //DeserializeReturn(entManager.GetMembers(0, 20), entLibManager.StatusCode);

            //获取分组
            //DeserializeReturn(entManager.GetGroups(), entLibManager.StatusCode);

            //获取角色
            //DeserializeReturn(entManager.GetRoles(), entLibManager.StatusCode);


        }


    }
}
