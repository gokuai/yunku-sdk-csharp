using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using YunkuEntSDK;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;
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

        /// <summary>
        ///     分析错误信息
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private void DeserializeReturn(string jsonString)
        {
            ReturnResult returnResult = ReturnResult.Create(jsonString);
            int code = returnResult.Code;
            string msg = "";
            string result = returnResult.Result;
            //复制到剪贴板

            if (code == (int) HttpStatusCode.OK)
            {
                //成功则返回结果

                if (result.Equals(string.Empty))
                {
                    msg = "返回成功";
                }
            }
            else
            {
                //返回错误信息
                BaseData data = BaseData.Create(result);
                if (data != null)
                {
                    msg = data.ErrorCode + ":" + data.ErrorMessage;
                }
            }
            Clipboard.SetDataObject(result);
            TB_Result.Text += msg + "\r\n";
            TB_Result.Text += result + "\r\n";
            TB_Result.Text += "==========================\r\n";
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            //=======条件设置=======//
            DebugConfig.LogPrintAvialable = true;
            DebugConfig.LogPath = ""; //日志文件没有做大小限制

            //=========企业库操作============//
            var entLibManager = new EntLibManager(OuathConfig.Uesrname, OuathConfig.Password, OuathConfig.ClientId,
                OuathConfig.ClientSecret);
            //获取授权
//            entLibManager.AccessToken(true);
            //创建库 1T="1099511627776" 1G＝“1073741824”；
            //DeserializeReturn(entLibManager.Create("destroy","1073741824","destroy","testlib",""));

            //修改库信息 1T="1099511627776" 1G＝“1073741824”；
//            DeserializeReturn(entLibManager.Set(109654, "sss", "1099511627776", "", ""));

            //获取库信息
//            DeserializeReturn(entLibManager.GetInfo(109654));

            //获取库列表
//            DeserializeReturn(entLibManager.GetLibList());

            //获取库授权
//            DeserializeReturn(entLibManager.Bind(5235, "", ""));

            //取消库授权
//            DeserializeReturn(entLibManager.UnBind("9affb8f78fd5914a7218d7561db6ddec"));

            //DeserializeReturn(entLibManager.AddMembers(32662, 1194, new int[] { 21122 }));
            //添加成员

            //批量修改单库中成员角色
            //DeserializeReturn(entLibManager.SetMemberRole(32662, 1194, new int[] { 21122 }));

            //获取某一个库的成员
//            DeserializeReturn(entLibManager.GetMembers(0, 20, 32662));

            //查询库成员信息
            //            DeserializeReturn(entLibManager.GetMember(4405, EntLibManager.MemberType.Account, new[] { "qwdqwdq1" }));

            //从库中删除成员
            //DeserializeReturn(entLibManager.DelMember(32662, new int[] { 21122 }));

            //获取某一个企业的分组列表
            //DeserializeReturn(entLibManager.GetGroups(32662));

            //库上添加分组
            //DeserializeReturn(entLibManager.AddGroup(32662, 1086, 1194));

            //设置分组上的角色
            //DeserializeReturn(entLibManager.SetGroupRole(32662, 1086, 1194));

            //库上删除分组
            //DeserializeReturn(entLibManager.DelGroup(32662,1086));

            //删除库
//            DeserializeReturn(entLibManager.Destroy("651a1ce28c4ad834ae6cb90ba494a755"));


            //==========企业文件操作============//
            const string orgClientId = "294925cc5b65f075677a3227141b9467";
            const string orgClientSecret = "e195dbb3f9c263890a269010f18bea50";

            var entFileManager = new EntFileManager(orgClientId, orgClientSecret);


            //获取文件列表
//            DeserializeReturn(entFileManager.GetFileList((int)Util.GetUnixDataline(), 0, ""));

            //获取更新列表
            //DeserializeReturn(entFileManager.GetUpdateList((int)Util.GetUnixDataline(), false, 0));

//            long now = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now);
//            long beigin = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now.AddDays(-1));

            //文件更新数量
//            DeserializeReturn(entFileManager.GetUpdateCount((int)Util.GetUnixDataline(),beigin ,now , true));

            //获取文件（夹）信息
            //DeserializeReturn(entFileManager.GetFileInfo((int)Util.GetUnixDataline(), "test"));

            //创建文件夹
            //DeserializeReturn(entFileManager.CreateFolder((int)Util.GetUnixDataline(), "testFolder", "Brandon"));

            //上传文件 文件不得超过50MB
            //DeserializeReturn(entFileManager.CreateFile((int)Util.GetUnixDataline(), "test", "Brandon", "D:\\test.txt"));

            entFileManager.UploadByBlock((int) Util.GetUnixDataline(), "test.png", "Brandon", 0,
                "C:\\Users\\Brandon\\Desktop\\test.jpg", true, UploadCompeleted, ProgressChanged);

            //删除文件 如果是多个文件则用逗号隔开fullpath,例如"test1,test2"
//            DeserializeReturn(entFileManager.Del((int)Util.GetUnixDataline(), "test", "Brandon"));

            //移动文件
            //DeserializeReturn(entFileManager.Move((int)Util.GetUnixDataline(), "testFolder", "1/testFolder", "Brandon"));

            ////文件连接
//            DeserializeReturn(entFileManager.Link((int)Util.GetUnixDataline(), "test.png", 0, EntFileManager.AuthType.Default, null));

            ////发送消息
            //DeserializeReturn(entFileManager.SendMsg((int)Util.GetUnixDataline(), "msgTest", "msg", "", "", "Brandon"));

            //获取当前库所有外链
//            DeserializeReturn(entFileManager.Links((int)Util.GetUnixDataline(),true));

            //=======企业操作========//
            var entManager = new EntManager(OuathConfig.Uesrname, OuathConfig.Password, OuathConfig.ClientId,
                OuathConfig.ClientSecret);
            //获取授权
//            entManager.AccessToken(true);

            //获取成员
//            DeserializeReturn(entManager.GetMembers(0, 20));

            //获取分组
//            DeserializeReturn(entManager.GetGroups());

            //获取角色
            //DeserializeReturn(entManager.GetRoles());

            //分组成员列表获取
//            DeserializeReturn(entManager.GetGroupMembers(1086, 0, 3, true));

            //根据成员id获取成员个人库外链
//            DeserializeReturn(entManager.GetMemberFileLink(52,true));

            //根据外部成员id获取成员信息
//            DeserializeReturn(entManager.GetMemberByOutid(new [] { "nishuonishuo", "dqwdqw" }));

            //根据外部成员登陆帐号获取成员信息
//            DeserializeReturn(entManager.GetMemberByUserIds(new[] { "nishuonishuo" }));

            //添加或修改同步成员
//            DeserializeReturn(entManager.AddSyncMember("MemberTest1", "Member1", "Member1", "", "",""));
//             DeserializeReturn(entManager.AddSyncMember("MemberTest2", "Member2", "Member2", "", "",""));
//             DeserializeReturn(entManager.AddSyncMember("MemberTest3", "Member3", "Member3", "", "",""));

            //删除同步成员
//            DeserializeReturn(entManager.DelSyncMember(new []{"MemberTest", "MemberTest1", "MemberTest2"}));
//
            //添加或修改同步分组
//            DeserializeReturn(entManager.AddSyncGroup("ParentGroup","ParentGroup",""));
//            DeserializeReturn(entManager.AddSyncGroup("GroupTest","Group","ParentGroup"));

            //删除同步分组
//             DeserializeReturn(entManager.DelSyncGroup(new[] { "ParentGroup", "GroupTest" }));

            //添加同步分组的成员
//            DeserializeReturn(entManager.AddSyncGroupMember("GroupTest",new []{"MemberTest1"}));
//            DeserializeReturn(entManager.AddSyncGroupMember("ParentGroup", new[] { "MemberTest2","MemberTest3" }));

            //删除同步分组的成员
//            DeserializeReturn(entManager.DelSyncGroupMember("ParentGroup", new[] { "MemberTest2", "MemberTest3" }));
        }

        private void ProgressChanged(object sender, ProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { ProgressChanged(sender, e); });
                return;
            }

            TB_Result.Text += e.LocalFullPath + ":" + e.ProgressPercent + "\r\n";
            TB_Result.Text += "==========================\r\n";
        }

        private void UploadCompeleted(object sender, CompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) delegate { UploadCompeleted(sender, e); });
                return;
            }

            if (e.IsError)
            {
                TB_Result.Text += e.LocalFullPath + ":" + e.ErrorMessage + "\r\n";
                TB_Result.Text += "==========================\r\n";
            }
            else
            {
                TB_Result.Text += e.LocalFullPath + ":" + "success" + "\r\n";
                TB_Result.Text += "==========================\r\n";
            }
        }
    }
}