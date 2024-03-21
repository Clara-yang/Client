using GalaSoft.MvvmLight.Command;
using System;
using System.Windows;
using System.Windows.Input;
using static UAM.Model.Models.VDNModel;
using UAM.Plugin;
using Csf.Imets.ToolCore.Vdn;
using UAM.Client.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Interop;
using System.Net.Sockets;
using System.Threading;
using UAM.Plugin.Common;
using System.Collections.Generic;
using UAM.Model.Entity;
using System.Security.Principal;
using UAM.Model.Models;
using GalaSoft.MvvmLight.Ioc;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Threading.Tasks;
using Csf.Imets.ToolCore.AutoFidelityTesting.FileIo;
using System.Windows.Forms.DataVisualization.Charting;
using GalaSoft.MvvmLight;
using System.Windows.Forms;
using System.Linq;
using System.Windows.Media;
using System.Security.Policy;

namespace UAM.Client.ViewModel
{
    public class LoginViewModel : ViewModelHelper
    {
        VDNData vdnData = new VDNData();
        DBConnection DB = new DBConnection();
        private static int m_finishTaskCount = 0;
        private BackgroundWorker worker;

        #region property
        public List<EntityUser> QueryUsersList { get; set; }
        public LoginModel loginModel { get; set; } = new LoginModel();

        private double progressBarValue;
        public double ProgressBarValue
        {
            get { return progressBarValue; }
            set { Set(ref progressBarValue, value); }
        }

        public string progressText;
        public string ProgressText
        {
            get { return progressText; }
            set { Set(ref progressText, value); }
        }

        public Visibility progressState;
        public Visibility ProgressState
        {
            get { return progressState; }
            set { Set(ref progressState, value); }
        }
        #endregion

        public LoginViewModel()
        {
            loginModel.LoginVerifyCommand = new Command() { DoExecute = obj => { LoginVerify(); } };

            QueryUsersList = DB.QueryUsers();
            loginModel.UserList = new List<string>();
            foreach (var user in QueryUsersList)
            {
                loginModel.UserList.Add(user.UserName.Trim());
            }

            InitWork();
        }

        public void InitWork()
        {
            worker = new BackgroundWorker();

            worker.WorkerReportsProgress = true;
            worker.DoWork += new DoWorkEventHandler(DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(BgworkChange);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ProgressCompleted);
            ProgressState = Visibility.Hidden;
        }

        public void LoginVerify()
        {
            EntityUser currentUser = QueryUsersList.Find(s => s.UserName.Trim() == loginModel.Account);
            if (loginModel.Account != null && currentUser.UserPassword.Trim() == loginModel.Password)
            {
                PubVar.g_CurrentUser = currentUser;

                worker.RunWorkerAsync();
            }
            else
            {
                System.Windows.MessageBox.Show("Please check your password or name!");
            }
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            GetXMLData();
            ProgressState = Visibility.Visible;
            for (int i = 0; i <= 100; i++)
            {
                if (i == 1)
                {
                    ProgressText = "开启视景程序中...";
                    StartVisual();
                }
                else if (i == 40)
                {
                    ProgressText = "开启其他程序中...";
                    StartApp();
                }
                else if (i == 70)
                {
                    ProgressText = "连接主程序中...";
                    vdnData.Init();
                }
                else if (i == 100)
                {
                    do
                    {
                        Thread.Sleep(2000);
                    }
                    while (vdnData.GetConnectState() == "Connected");

                    ProgressText = "连接主程序成功";
                }
                //Reports the progress
                if (vdnData.GetConnectState() == "Connected")
                {
                    i = 100;
                }

                (sender as BackgroundWorker).ReportProgress(i);
            }
        }

        private void BgworkChange(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarValue = e.ProgressPercentage;
        }

        private void ProgressCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Configure Confogure = new Configure();
            NextPage(Confogure);
        }

        public void GetXMLData()
        {
            string requestXmlPath = @".\ConfigurationXml\" + PubVar.g_CurrentUser.UserName.Trim() + @"\SendRequest.xml";
            PubVar.g_SendRequestList = XmlManager.GetRequestList(requestXmlPath);

            string malfunctionXmlPath = @".\ConfigurationXml\" + PubVar.g_CurrentUser.UserName.Trim() + @"\Malfunction.xml";
            PubVar.g_MalfunctionList = XmlManager.GetMalfunctionList(malfunctionXmlPath).FindAll(s => s.Visible == true);

            string stickXmlPath = @".\ConfigurationXml\" + PubVar.g_CurrentUser.UserName.Trim() + @"\Stick.xml";
            PubVar.g_StickList = XmlManager.GetStickList(stickXmlPath).FindAll(s => s.Visible == true);

            string routeXmlPath = @".\ConfigurationXml\" + PubVar.g_CurrentUser.UserName.Trim() + @"\Route.xml";
            PubVar.g_RouteList = XmlManager.GetRouteList(routeXmlPath).FindAll(s => s.Visible == true);

            string aircraftXmlPath = @".\ConfigurationXml\" + PubVar.g_CurrentUser.UserName.Trim() + @"\Aircraft.xml";
            PubVar.g_AircraftList = XmlManager.GetAircraftList(aircraftXmlPath).FindAll(s => s.Visible == true);

            string pageXmlPath = @".\ConfigurationXml\" + PubVar.g_CurrentUser.UserName.Trim() + @"\Page.xml";
            PubVar.g_PageList = XmlManager.GetPageList(pageXmlPath).FindAll(s => s.IsSelected == true);

            string BtnXmlPath = @".\ConfigurationXml\" + PubVar.g_CurrentUser.UserName.Trim() + @"\Button.xml";
            PubVar.g_ButtonList = XmlManager.GetButtonList(BtnXmlPath).FindAll(s => s.Visible == true);

            string urlXmlPath = @".\ConfigurationXml\" + PubVar.g_CurrentUser.UserName.Trim() + @"\ConfigureURL.xml";
            PubVar.g_ConfigureUrl = XmlManager.GetMapList(urlXmlPath);


        }

        /// <summary>
        /// 启动指定程序
        /// </summary>
        private async void StartApp()
        {
            List<UrlModel> appUrls = new List<UrlModel>();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            appUrls = PubVar.g_ConfigureUrl.FindAll(s => s.AppName == "Binary");

            if (appUrls != null && appUrls.Count != 0)
            {
                foreach (var url in appUrls)
                {
                    if (url.AppUrl != "")
                    {
                        await Task.Run(() => StartProcesses(url.AppUrl));
                    }
                    Interlocked.Increment(ref m_finishTaskCount);
                }

                while (true)
                {
                    if (m_finishTaskCount >= appUrls.Count())
                    {
                        break;
                    }
                }
            }
            sw.Stop();
        }

        static async void StartProcesses(string exePath)
        {
            using (Process p = new Process())
            {
                try
                {
                    p.StartInfo.FileName = exePath;
                    p.Start();
                    //p.WaitForExit();
                }
                catch
                {
                    System.Windows.MessageBox.Show("无法打开" + exePath);
                }
                Interlocked.Increment(ref m_finishTaskCount);
            }
        }

        /// <summary>
        /// 启动视景程序
        /// </summary>
        private async void StartVisual()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var visualUrl = PubVar.g_ConfigureUrl.Find(s => s.AppName == "Visual").AppUrl;

            if (visualUrl != null && visualUrl != "")
            {
                await Task.Run(() => StartProcesses(visualUrl));

                while (true)
                {
                    if (m_finishTaskCount >= 1)
                    {
                        break;
                    }
                }
            }
            sw.Stop();
        }

    }
}
