using Csf.Imets.ToolCore.Vdn;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using UAM.Plugin.Common;
using UAM.Model.Models;
using System.Diagnostics;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace UAM.Plugin
{
    public class VDNConnectHelper
    {
        private static VDNConnectHelper _singleton = null;
        private static object singlenton_Lock = new object();
        public static CommunicationModel CommunicationModel = new CommunicationModel();
        public SshClient client;

        public void Init()
        {
            string xmlPath = @".\ConfigurationXml\" + PubVar.g_CurrentUser.UserName.Trim() + @"\StartXml.xml";
            CommunicationModel = XmlManager.createRunExec(xmlPath);

            PubVar.g_HostIP = CommunicationModel.Host_IP;
            PubVar.g_HostUser = CommunicationModel.Host_User;
            PubVar.g_HostPassword = CommunicationModel.Host_Password;
            PubVar.g_VdnPort = CommunicationModel.Vdn_Port;
            PubVar.g_VisualType = CommunicationModel.Visual_Type;

            RunProgram();
            ConnectHost();
        }

        public void RunProgram()
        {
            try
            {
                StopProgram();
                client = new SshClient(PubVar.g_HostIP, CommunicationModel.Host_User, CommunicationModel.Host_Password);
                client.Connect();
                Thread.Sleep(1000);
                SshCommand startCmd = client.CreateCommand(CommunicationModel.Host_Exec_Cmd);
                startCmd.BeginExecute();
                Thread.Sleep(1000);
                client.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void StopProgram()
        {
            if (PubVar.g_CurrentUser?.UserName.Trim() != "" && PubVar.g_CurrentUser?.UserName.Trim() != null)
            {
                client = new SshClient(PubVar.g_HostIP, CommunicationModel.Host_User, CommunicationModel.Host_Password);
                client.Connect();
                SshCommand stopCmd = client.CreateCommand(CommunicationModel.Host_Stop_Cmd);
                stopCmd.Execute();
                Thread.Sleep(1000);
                client.Dispose();
            }
        }
        public void StopProcess(string processName)
        {
            Process[] p = Process.GetProcessesByName(processName);
            if (p.Length > 0)
            {
                foreach (Process item in p)
                {
                    item.Kill();
                }
            }
        }

        public async void ConnectHost()
        {
            VdnConnectionInfo info = new VdnConnectionInfo(PubVar.g_HostIP, int.Parse(PubVar.g_VdnPort));
            info.Rate = 30;
            PubVar.g_NetVdnClient.Connect(info, false);

            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var task = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    // 检查条件是否满足
                    if (PubVar.g_NetVdnClient.State == HostConnectionState.Connected)
                    {
                        cts.Cancel(); // 取消令牌，结束循环
                    }

                    // 等待一段时间再继续检查条件
                    Thread.Sleep(1000);
                }
            });
            // 等待任务完成
            await task;
        }

        public static VDNConnectHelper CreateInstance()
        {
            // 先判断当前实例是否存在，不存在再加锁处理
            if (_singleton == null)
            {
                lock (singlenton_Lock)
                {
                    if (_singleton == null)
                    {
                        try
                        {
                            _singleton = new VDNConnectHelper();

                            PubVar.g_NetVdnClient = new NetVdnClient();
                        }
                        catch
                        {
                            PubVar.g_NetVdnClient = null;
                        }
                    }
                }
            }
            return _singleton;
        }
        public NetVdnClient GetClient()
        {
            return PubVar.g_NetVdnClient;
        }


    }
}
