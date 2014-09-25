using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using YunkuEntSDK;
using YunkuEntSDK.Data;
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
        private void DeserializeReturn(string result, HttpStatusCode code)
        {
            string msg = "";
            //复制到剪贴板
            
            if (code == HttpStatusCode.OK)
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
            var entLibManager = new EntLibManager(OuathConfig.Uesrname, OuathConfig.Password, OuathConfig.ClientId, OuathConfig.ClientSecret);
            //获取授权
            entLibManager.AccessToken(true);
            //创建库 1T="1099511627776" 1G＝“1073741824”；
            //DeserializeReturn(entLibManager.Create("destroy","1073741824","destroy","testlib"),entLibManager.StatusCode);

            //修改库信息 1T="1099511627776" 1G＝“1073741824”；
//            DeserializeReturn(entLibManager.Set(109654, "sss", "1099511627776", "", ""), entLibManager.StatusCode);

            //获取库信息
            DeserializeReturn(entLibManager.GetInfo(109654), entLibManager.StatusCode);

            //获取库列表
//            DeserializeReturn(entLibManager.GetLibList(), entLibManager.StatusCode);

            //获取库授权
//            DeserializeReturn(entLibManager.Bind(48716, "", ""), entLibManager.StatusCode);

            //取消库授权
//            DeserializeReturn(entLibManager.UnBind("9affb8f78fd5914a7218d7561db6ddec"), entLibManager.StatusCode);

            //DeserializeReturn(entLibManager.AddMembers(32662, 1194, new int[] { 21122 }), entLibManager.StatusCode);
            //添加成员

            //批量修改单库中成员角色
            //DeserializeReturn(entLibManager.SetMemberRole(32662, 1194, new int[] { 21122 }), entLibManager.StatusCode);

            //获取某一个库的成员
//            DeserializeReturn(entLibManager.GetMembers(0, 20, 32662),entLibManager.StatusCode);

            //查询库成员信息
            //            DeserializeReturn(entLibManager.GetMember(4405, EntLibManager.MemberType.Account, new[] { "qwdqwdq1" }), entLibManager.StatusCode);

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

            //删除库
//            DeserializeReturn(entLibManager.Destroy("651a1ce28c4ad834ae6cb90ba494a755"), entLibManager.StatusCode);



            //==========企业文件操作============//
            const string orgClientId = "183925acbe239a820aea71862e4b44a2";
            const string orgClientSecret = "cc7c632dc5e8bdb1651e0113a600c000";

            var entFileManager = new EntFileManager(orgClientId, orgClientSecret);


            //获取文件列表
            //DeserializeReturn(entFileManager.GetFileList((int)Util.GetUnixDataline(), 0, ""), entFileManager.StatusCode);

            //获取更新列表
            //DeserializeReturn(entFileManager.GetUpdateList((int)Util.GetUnixDataline(), false, 0), entFileManager.StatusCode);

//            long now = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now);
//            long beigin = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now.AddDays(-1));

            //文件更新数量
//            DeserializeReturn(entFileManager.GetUpdateCount((int)Util.GetUnixDataline(),beigin ,now , true), entFileManager.StatusCode);

            //获取文件（夹）信息
            //DeserializeReturn(entFileManager.GetFileInfo((int)Util.GetUnixDataline(), "test"), entFileManager.StatusCode);

            //创建文件夹
            //DeserializeReturn(entFileManager.CreateFolder((int)Util.GetUnixDataline(), "testFolder", "Brandon"), entFileManager.StatusCode);

            //上传文件 文件不得超过50MB
            //DeserializeReturn(entFileManager.CreateFile((int)Util.GetUnixDataline(), "test", "Brandon", "D:\\test.txt"), entFileManager.StatusCode);

            //删除文件
            //DeserializeReturn(entFileManager.Del((int)Util.GetUnixDataline(), "test", "Brandon"), entFileManager.StatusCode);

            //移动文件
            //DeserializeReturn(entFileManager.Move((int)Util.GetUnixDataline(), "testFolder", "1/testFolder", "Brandon"), entFileManager.StatusCode);

            ////文件连接
            //DeserializeReturn(entFileManager.Link((int)Util.GetUnixDataline(), "test"), entFileManager.StatusCode);

            ////发送消息
            //DeserializeReturn(entFileManager.SendMsg((int)Util.GetUnixDataline(), "msgTest", "msg", "", "", "Brandon"), entFileManager.StatusCode);

            //获取当前库所有外链
//            DeserializeReturn(entFileManager.Links((int)Util.GetUnixDataline()), entFileManager.StatusCode);

            //=======企业操作========//
            var entManager = new EntManager(OuathConfig.Uesrname, OuathConfig.Password, OuathConfig.ClientId, OuathConfig.ClientSecret);
            //获取授权
//            entManager.AccessToken(true);

            //获取成员
//            DeserializeReturn(entManager.GetMembers(0, 20), entManager.StatusCode);

            //获取分组
            //DeserializeReturn(entManager.GetGroups(), entManager.StatusCode);

            //获取角色
            //DeserializeReturn(entManager.GetRoles(), entManager.StatusCode);

            //分组成员列表获取
//            DeserializeReturn(entManager.GetGroupMembers(1086, 0, 3, true),entManager.StatusCode);

            //根据成员id获取成员个人库外链
//            DeserializeReturn(entManager.GetMemberFileLink(52,true), entManager.StatusCode);

            //根据外部成员id获取成员信息
//            DeserializeReturn(entManager.GetMemberByOutid(new [] { "nishuonishuo", "dqwdqw" }),entManager.StatusCode);

            //根据外部成员登陆帐号获取成员信息
//            DeserializeReturn(entManager.GetMemberByUserIds(new[] { "nishuonishuo" }), entManager.StatusCode);

            //添加或修改同步成员
//            DeserializeReturn(entManager.AddSyncMember("MemberTest1", "Member1", "Member1", "", ""), entManager.StatusCode);
//             DeserializeReturn(entManager.AddSyncMember("MemberTest2", "Member2", "Member2", "", ""), entManager.StatusCode);
//             DeserializeReturn(entManager.AddSyncMember("MemberTest3", "Member3", "Member3", "", ""), entManager.StatusCode);

            //删除同步成员
//            DeserializeReturn(entManager.DelSyncMember(new []{"MemberTest", "MemberTest1", "MemberTest2"}), entManager.StatusCode);
//
            //添加或修改同步分组
//            DeserializeReturn(entManager.AddSyncGroup("ParentGroup","ParentGroup",""),entManager.StatusCode);
//            DeserializeReturn(entManager.AddSyncGroup("GroupTest","Group","ParentGroup"),entManager.StatusCode);

            //删除同步分组
//             DeserializeReturn(entManager.DelSyncGroup(new[] { "ParentGroup", "GroupTest" }), entManager.StatusCode);

            //添加同步分组的成员
//            DeserializeReturn(entManager.AddSyncGroupMember("GroupTest",new []{"MemberTest1"}),entManager.StatusCode);
//            DeserializeReturn(entManager.AddSyncGroupMember("ParentGroup", new[] { "MemberTest2","MemberTest3" }), entManager.StatusCode);

            //删除同步分组的成员
//            DeserializeReturn(entManager.DelSyncGroupMember("ParentGroup", new[] { "MemberTest2", "MemberTest3" }), entManager.StatusCode);
            
        }
    }

}