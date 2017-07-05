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

            if (code == (int)HttpStatusCode.OK)
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
            DebugConfig.LogPrintAvialable = false;
            DebugConfig.LogPath = ""; //日志文件没有做大小限制

            //=========企业库操作============//
            var entLibManager = new EntLibManager(SdkConfig.ClientId, SdkConfig.ClientSecret);

            //创建库 1T="1099511627776" 1G＝"1073741824"
            //DeserializeReturn(entLibManager.Create("qpTest", "1073741824", "destroy", ""));

            //修改库信息 1T="1099511627776" 1G＝“1073741824”；
            //DeserializeReturn(entLibManager.Set(1307065, "sss", "1099511627776", ""));

            //获取库信息
            //DeserializeReturn(entLibManager.GetInfo(1258748));

            //获取库列表
            //DeserializeReturn(entLibManager.GetLibList());

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
            var entFileManager = new EntFileManager(SdkConfig.orgClientId, SdkConfig.orgClientSecret);

            //获取文件列表
            //DeserializeReturn(entFileManager.GetFileList());

            //获取更新列表
            //DeserializeReturn(entFileManager.GetUpdateList(false, 0));

            //            long now = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now);
            //            long beigin = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now.AddDays(-1));

            //文件更新数量
            //            DeserializeReturn(entFileManager.GetUpdateCount(beigin ,now , true));

            //获取文件（夹）信息
            //DeserializeReturn(entFileManager.GetFileInfo("test", EntFileManager.NetType.Default, false));

            //创建文件夹
            //DeserializeReturn(entFileManager.CreateFolder("test", "Brandon"));

            //上传文件 文件不得超过50MB
            //DeserializeReturn(entFileManager.CreateFile("testCsharp.txt", "Brandon", "D:\\test.txt"));

            //entFileManager.UploadByBlock("testCsharpUp.txt", "Brandon", 0,
            //    "D:\\test.txt", true, UploadCompeleted, ProgressChanged);

            //通过链接上传文件
            //            DeserializeReturn( entFileManager.CreateFileByUrl("1.jpg", 0, "Brandon", true,
            //                "http://www.sinaimg.cn/dy/slidenews/1_img/2015_27/2841_589214_521618.jpg")); 

            //删除文件 如果是多个文件则用逗号隔开fullpath,例如"test1,test2"
            //DeserializeReturn(entFileManager.Del("test", "Brandon"));

            //移动文件
            //DeserializeReturn(entFileManager.Move("qq.jpg", "test/qq.jpg", "Brandon"));

            ////文件连接
            //DeserializeReturn(entFileManager.Link("qq.jpg", 0, EntFileManager.AuthType.Default, null));

            ////发送消息
            //DeserializeReturn(entFileManager.SendMsg( "msgTest", "msg", "", "", "Brandon"));

            //获取当前库所有外链
            //            DeserializeReturn(entFileManager.Links(true));

            //获取文件历史
            //            DeserializeReturn(entFileManager.History("test", 0 ,100));

            //复制文件
            //DeserializeReturn(entFileManager.Copy("test/qq.jpg", "qq.jpg", "qp"));

            // 回收站
            //DeserializeReturn(entFileManager.Recyle(0, 100));

            //恢复删除文件
            //DeserializeReturn(entFileManager.Recover("test", "qp"));

            //彻底删除文件
            //DeserializeReturn(entFileManager.CompletelyDelFile(new string[] {"aaa.jpg"}, "qp"));

            //通过文件路径获取下载地址
            //DeserializeReturn(entFileManager.GetDownloadUrlByFullPath("test/qq.jpg", false, EntFileManager.NetType.Default));

            //通过文件唯一标识获取下载地址
            //DeserializeReturn(entFileManager.GetDownloadUrlByHash("5ef2b3b8449cf3440b8a3b1874da5e4236b06dd8", false, EntFileManager.NetType.Default));

            //文件搜索
            //DeserializeReturn(entFileManager.Search("test", "", 0, 100, EntFileManager.ScopeType.FileName, EntFileManager.ScopeType.Tag));

            //文件预览地址
            //DeserializeReturn(entFileManager.PreviewUrl("test/qq.jpg", false, ""));

            //获取文件夹权限
            //DeserializeReturn(entFileManager.GetPermission("test", 4));

            //修改文件夹权限
            //DeserializeReturn(entFileManager.SetPermission("test", 4, EntFileManager.FilePermissions.FileDelete, EntFileManager.FilePermissions.FilePreview));

            //添加标签
            //DeserializeReturn(entFileManager.AddTag("test", new string[] { "test", "testTag" }));

            //删除标签
            //DeserializeReturn(entFileManager.DelTag("test", new string[] { "test", "testTag" }));

            //=======企业操作========//
            var entManager = new EntManager(SdkConfig.ClientId, SdkConfig.ClientSecret);

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

            //根据成员Id查询企业成员信息
            //            DeserializeReturn(entManager.GetMemberById(43));

            //根据外部系统唯一id查询企业成员信息
            //            DeserializeReturn(entManager.GetMemberByAccount("nishuonishuo"));

            //根据外部系统登录帐号查询企业成员信息
            //            DeserializeReturn(entManager.GetMemberByOutId("dqwdqw"));

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

            //添加管理员
            //DeserializeReturn(entManager.AddSyncAdmin("$:LWCP_v1:$ypc3i0Op0Tn0Ge2GvyShWA==", "", false));

            //合作方API
            var thirdParty = new ThirdPartyManager(SdkConfig.ClientId, SdkConfig.ClientSecret, SdkConfig.outId);

            //开通企业
            //DeserializeReturn(thirdParty.CreateEnt("yunku3", "yunku3", "", "", ""));

            //获取企业信息
            //DeserializeReturn(thirdParty.GetEntInfo());

            //获取企业token
            //DeserializeReturn(thirdParty.GetEntToken());

            //获取单点登录地址
            //DeserializeReturn(thirdParty.GetSsoUrl(""));

            //购买
            //DeserializeReturn(thirdParty.OrderSubscribe(-1, 1, 12));

            //续费
            //DeserializeReturn(thirdParty.OrderRenew(12));

            //升级
            //DeserializeReturn(thirdParty.OrderUpgrade(-1, 1));

            //退订
            //DeserializeReturn(thirdParty.OrderUnsubscribe());

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
                Invoke((MethodInvoker)delegate { UploadCompeleted(sender, e); });
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