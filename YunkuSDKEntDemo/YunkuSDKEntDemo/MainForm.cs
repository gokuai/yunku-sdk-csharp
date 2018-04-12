﻿using System;
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
            TB_Result.Text += result + "\r\n";
            TB_Result.Text += "==========================\r\n";
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            //=======条件设置=======//
            DebugConfig.LogPrintAvialable = false;
            DebugConfig.LogPath = ""; //日志文件没有做大小限制

            //ConfigHelper.SetApiHost("http://server/m-open");
            //ConfigHelper.SetWebHost("http://server");
            //ConfigHelper.SetUploadBlockSize(5 * 1024 * 1024);

            //==========企业文件操作============//
            var entFileManager = new EntFileManager(SdkConfig.orgClientId, SdkConfig.orgClientSecret);

            //DeserializeReturn(entFileManager.GetFileList());

            //获取文件列表
            //DeserializeReturn(entFileManager.GetFileList());

            //异步获取文件列表
            //entFileManager.GetFileListAsync("", 0, 100, true, (s,requestEvent)=> {
            //    DeserializeReturn(requestEvent.Result);
            //} );

            //获取更新列表
            //DeserializeReturn(entFileManager.GetUpdateList(false, 0));

            //long now = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now);
            //long beigin = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now.AddDays(-1));

            //文件更新数量
            //DeserializeReturn(entFileManager.GetUpdateCount(beigin, now, true));

            //获取文件（夹）信息
            //DeserializeReturn(entFileManager.GetFileInfoByFullpath("testRangSize.txt"));

            //创建文件夹
            //DeserializeReturn(entFileManager.CreateFolder("test", ""));

            //分块上传，默认分块上传大小为10MB
            //FileInfo info = entFileManager.UploadByBlock("testRangSize.txt", "", 0, @"D:\testRangSize.txt", true);

            //异步方式分块上传，默认分块上传大小为10MB
            //entFileManager.UploadByBlockAsync("testRangSize.txt", "", 0,
            //    @"D:\test.txt", true, UploadCompeleted, ProgressChanged);

            //通过链接上传文件
            //DeserializeReturn(entFileManager.CreateFileByUrl("1q.jpg", 0, "", true,
            //    "http://www.sinaimg.cn/dy/slidenews/1_img/2015_27/2841_589214_521618.jpg"));

            //删除文件 如果是多个文件则用逗号隔开fullpath,例如"test1,test2"
            //DeserializeReturn(entFileManager.Del("test", "", true));

            //移动文件
            //DeserializeReturn(entFileManager.Move("qq.jpg", "test/qq.jpg", ""));

            ////文件外链
            //DeserializeReturn(entFileManager.Link("qq.jpg", 0, EntFileManager.AuthType.Default, null));

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
            //DeserializeReturn(entFileManager.GetDownloadUrlByFullpath("test/qq.jpg"));

            //通过文件唯一标识获取下载地址
            //DeserializeReturn(entFileManager.GetDownloadUrlByHash("5ef2b3b8449cf3440b8a3b1874da5e4236b06dd8"));

            //文件搜索
            //DeserializeReturn(entFileManager.Search("test", "", 0, 100, EntFileManager.ScopeType.FileName, EntFileManager.ScopeType.Tag));

            //文件预览地址
            //DeserializeReturn(entFileManager.GetPreviewUrlByFullpath("test/qq.jpg", false, "", false));

            //获取文件夹权限
            //DeserializeReturn(entFileManager.GetPermission("test", 4));

            //修改文件夹权限
            //DeserializeReturn(entFileManager.SetPermission("test", 4, EntFileManager.FilePermissions.FileDelete, EntFileManager.FilePermissions.FilePreview));

            //添加标签
            //DeserializeReturn(entFileManager.AddTag("test", new string[] { "test", "testTag" }));

            //删除标签
            //DeserializeReturn(entFileManager.DelTag("test", new string[] { "test", "testTag" }));



            //=========企业库操作============//
            //var entLibManager = new EntLibManager(SdkConfig.ClientId, SdkConfig.ClientSecret);

            //创建库 1T="1099511627776" 1G＝"1073741824"
            //DeserializeReturn(entLibManager.Create("qpTest", "1073741824", "destroy", ""));

            //修改库信息 1T="1099511627776" 1G＝“1073741824”；
            //DeserializeReturn(entLibManager.Set(1317448, "qp", "1099511627776", ""));

            //获取库信息
            //DeserializeReturn(entLibManager.GetInfo(1317448));

            //获取库列表
            //DeserializeReturn(entLibManager.GetLibList());

            //获取库授权
            //DeserializeReturn(entLibManager.Bind(1317448, "", ""));

            //取消库授权
            //DeserializeReturn(entLibManager.UnBind("l56SYNzK6N9z2ZDMhYqC1Oo"));

            //添加成员
            //DeserializeReturn(entLibManager.AddMembers(1317448, 3208, new int[] { 216144 }));

            //批量修改单库中成员角色
            //DeserializeReturn(entLibManager.SetMemberRole(1317448, 3208, new int[] { 216144 }));

            //获取某一个库的成员
            //DeserializeReturn(entLibManager.GetMembers(0, 20, 1317448));

            //查询库成员信息
            //DeserializeReturn(entLibManager.GetMember(1317448, EntLibManager.MemberType.Account, new[] { "qwdqwdq1" }));

            //从库中删除成员
            //DeserializeReturn(entLibManager.DelMember(1317448, new int[] { 216144 }));

            //获取某一个企业的分组列表
            //DeserializeReturn(entLibManager.GetGroups(1317448));

            //库上添加分组
            //DeserializeReturn(entLibManager.AddGroup(1317448, 4448, 3208));

            //设置分组上的角色
            //DeserializeReturn(entLibManager.SetGroupRole(1317448, 4448, 1194));

            //库上删除分组
            //DeserializeReturn(entLibManager.DelGroup(1317448, 4448));

            //删除库
            //DeserializeReturn(entLibManager.Destroy("jvj4DYQFFPoV98to7wu4ZUQ"));

            //=======企业操作========//
            //var entManager = new EntManager(SdkConfig.ClientId, SdkConfig.ClientSecret);

            //获取成员
            //DeserializeReturn(entManager.GetMembers(0, 20));

            //获取分组
            //DeserializeReturn(entManager.GetGroups());

            //获取角色
            //DeserializeReturn(entManager.GetRoles());

            //分组成员列表获取
            //DeserializeReturn(entManager.GetGroupMembers(1317448, 0, 3, true));

            //根据成员id获取成员个人库外链
            //DeserializeReturn(entManager.GetMemberFileLink(4, true));

            //根据成员Id查询企业成员信息
            //DeserializeReturn(entManager.GetMemberById(4));

            //根据外部系统唯一id查询企业成员信息
            //DeserializeReturn(entManager.GetMemberByAccount("nishuonishuo"));

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