using CefSharp;
using CefSharp.DevTools.IO;
using Csf.Imets.ToolCore.AutoFidelityTesting;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using UAM.Client.Views;
using UAM.Model;
using UAM.Model.Models;
using UAM.Plugin;
using UAM.Plugin.Common;
using UAM.Plugin.NetVDN;
using static UAM.Model.Models.VDNModel;

namespace UAM.Client.ViewModel
{
    public class FlightViewModel : ViewModelBase
    {
        VDNConnectHelper vdnHelper = new VDNConnectHelper();
        VDNData vdnData = new VDNData();
        VDNDataUpadate dataUpdate = VDNDataUpadate.Instance;

        #region property
        public List<ButtonModel> BtnCollection { get; set; }
        public string UserName { get; set; } = PubVar.g_CurrentUser?.UserName.Trim();
        public string CurrentWeather { get; set; }
        public ObservableCollection<PageModel> pages;
        public ObservableCollection<PageModel> Pages
        {
            get { return pages; }
            set { Set(ref pages, value); }
        }
        public Page MainPage { get; set; }
        public UIElement functionPage;
        public UIElement FunctionPage
        {
            get { return functionPage; }
            set { Set(ref functionPage, value); }
        }
        #endregion

        #region ICommand
        private RelayCommand<PageModel> menuCommand;
        public RelayCommand<PageModel> MenuCommand
        {
            get
            {
                return new RelayCommand<PageModel>(e =>
                    {
                        MenuEvent(e);
                    });
            }
            set { menuCommand = value; }
        }
        private RelayCommand<ButtonModel> topBtnlick;
        public RelayCommand<ButtonModel> TopBtnClick
        {
            get
            {
                if (true) return new RelayCommand<ButtonModel>(e =>
                {
                    ExecClick(e);
                }
                );
            }
            set { topBtnlick = value; }
        }
        private ICommand m_CloseCommand;
        public ICommand CloseCommand
        {
            get { return m_CloseCommand ?? new RelayCommand(CloseWindow); }
        }
        #endregion

        #region Button
        ButtonModel simFreezeBtn;
        ButtonModel flightFreezeBtn;
        ButtonModel powerFreezeBtn;
        ButtonModel locationFreezeBtn;
        ButtonModel highlyFreezeBtn;
        ButtonModel powerResetBtn;
        ButtonModel powerOnBtn;
        ButtonModel enginOnBtn;
        ButtonModel crashBtn;
        ButtonModel cleanCrashBtn;
        ButtonModel hostConnectBtn;
        ButtonModel visualConnectBtn;
        ButtonModel motionPreBtn;
        ButtonModel motionFreezeBtn;
        #endregion

        public FlightViewModel()
        {
            BtnCollection = new List<ButtonModel>();

            Pages = new ObservableCollection<PageModel>();

            dataUpdate.PropertyChanged += DataUpdate_PropertyChanged;

            LoadPages();
            LoadBtns();

            foreach (var b in BtnCollection)
            {
                b.BtnClickCommand = TopBtnClick;
            }

            foreach (var page in Pages)
            {
                if (page == Pages.FirstOrDefault())
                {
                    MenuEvent(page);
                }
                page.SelectEvent = MenuCommand;
            }
        }

        /// <summary>
        /// 刷新subscrib的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataUpdate_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PubVar.g_CurrentUser.UserName.Trim() == "BoundaryAI")
            {
                simFreezeBtn.IsChecked = dataUpdate.BJKernelModeString == true ? true : false;
            }
            else
            {
                simFreezeBtn.IsChecked = (dataUpdate.KernelModeString == "freeze:normal") ? true : false;
            }
            flightFreezeBtn.IsChecked = dataUpdate.FlightFreeze == true ? true : false;
            powerFreezeBtn.IsChecked = dataUpdate.PowerFreeze == true ? true : false;
            locationFreezeBtn.IsChecked = dataUpdate.LocationFreeze == true ? true : false;
            highlyFreezeBtn.IsChecked = dataUpdate.HighlyFreeze == true ? true : false;
            powerOnBtn.IsChecked = dataUpdate.PowerOn == true ? true : false;
            enginOnBtn.IsChecked = dataUpdate.EngineOn == true ? true : false;
            if (PubVar.g_CurrentUser.UserName.Trim() == "BoundaryAI")
            {
                crashBtn.IsChecked = dataUpdate.BJCrashOverride == true ? true : false;
            }
            else
            {
                crashBtn.IsChecked = dataUpdate.CrashOverride == true ? true : false;
            }
            cleanCrashBtn.IsChecked = dataUpdate.BJClearCrash == true ? true : false;
            hostConnectBtn.IsChecked = dataUpdate.HostConnect == "Connected" ? true : false;
            visualConnectBtn.IsChecked = dataUpdate.VisualConnect == true ? true : false;
        }

        /// <summary>
        /// 读xml文件，加载子页面
        /// </summary>
        public void LoadPages()
        {
            var pageList = PubVar.g_PageList.FindAll(s => s.IsSelected == true);
            foreach (var item in pageList)
            {
                switch (item.PageName)
                {
                    case "MapPage": Pages.Add(new PageModel(item.DisplayName, PackIconKind.FlightTakeoff, new MapPage())); break;
                    case "EnvironmentPage": Pages.Add(new PageModel(item.DisplayName, PackIconKind.WeatherSunny, new EnvironmentPage())); break;
                    case "Maintain": Pages.Add(new PageModel(item.DisplayName, PackIconKind.Build, new Maintain())); break;
                    case "Malfunction": Pages.Add(new PageModel(item.DisplayName, PackIconKind.Construction, new Malfunction())); break;
                    case "FlightParametersPage": Pages.Add(new PageModel(item.DisplayName, PackIconKind.Monitor, new Page())); break;
                    case "Plot": Pages.Add(new PageModel(item.DisplayName, PackIconKind.Timeline, new Plot())); break;
                    case "Sensor": Pages.Add(new PageModel(item.DisplayName, PackIconKind.Memory, new Sensor())); break;
                    case "AircraftStatusPage": Pages.Add(new PageModel(item.DisplayName, PackIconKind.Stairs, new Page())); break;
                    case "OtherPage": Pages.Add(new PageModel(item.DisplayName, PackIconKind.Chips, new Page())); break;
                    case "SlewPage": Pages.Add(new PageModel(item.DisplayName, PackIconKind.Pill, new Page())); break;
                }
            }
        }

        /// <summary>
        /// 加载btn
        /// </summary>
        public void LoadBtns()
        {
            var btns = PubVar.g_ButtonList.FindAll(s => s.Visible == true && s.Position == "Top");
            foreach (var btn in btns)
            {
                BtnCollection.Add(btn);
            }

            simFreezeBtn = BtnCollection.Find(s => s.ButtonName == "simFreezeBtn");
            flightFreezeBtn = BtnCollection.Find(s => s.ButtonName == "flightFreezeBtn");
            highlyFreezeBtn = BtnCollection.Find(s => s.ButtonName == "highlyFreezeBtn");
            locationFreezeBtn = BtnCollection.Find(s => s.ButtonName == "locationFreezeBtn");
            powerFreezeBtn = BtnCollection.Find(s => s.ButtonName == "powerFreezeBtn");
            powerResetBtn = BtnCollection.Find(s => s.ButtonName == "powerResetBtn");
            powerOnBtn = BtnCollection.Find(s => s.ButtonName == "powerOnBtn");
            enginOnBtn = BtnCollection.Find(s => s.ButtonName == "enginOnBtn");
            crashBtn = BtnCollection.Find(s => s.ButtonName == "crashBtn");
            cleanCrashBtn = BtnCollection.Find(s => s.ButtonName == "cleanCrashBtn");
            hostConnectBtn = BtnCollection.Find(s => s.ButtonName == "hostConnectBtn");
            visualConnectBtn = BtnCollection.Find(s => s.ButtonName == "visualConnectBtn");
            motionPreBtn = BtnCollection.Find(s => s.ButtonName == "motionPreBtn"); ;
            motionFreezeBtn = BtnCollection.Find(s => s.ButtonName == "motionFreezeBtn");
        }

        /// <summary>
        /// 点击按钮发request
        /// </summary>
        /// <param name="e"></param>
        public void ExecClick(ButtonModel e)
        {
            switch (e.Parameter)
            {
                case "simFreeze":
                    SendRequest freezeRequest;
                    SendRequest runRequest;
                    if (PubVar.g_CurrentUser.UserName.Trim() == "BoundaryAI")
                    {
                        freezeRequest = new SendRequest("BJInterface", "Default", "Total_Freeze");
                        freezeRequest.SendParameters.Add(true);
                        runRequest = new SendRequest("BJInterface", "Default", "Total_Freeze");
                        runRequest.SendParameters.Add(false);
                        if (VDNData.BJKernelModeString)
                        {
                            vdnData.SendRequset(runRequest);
                        }
                        else
                        {
                            vdnData.SendRequset(freezeRequest);
                        }
                    }
                    else
                    {
                        freezeRequest = new SendRequest("Kernel", "Default", "Freeze");
                        freezeRequest.SendParameters.Add(true);
                        runRequest = new SendRequest("Kernel", "Default", "Run");
                        runRequest.SendParameters.Add(true);
                        if (VDNData.KernelModeString == "freeze:normal")
                        {
                            vdnData.SendRequset(runRequest);
                        }
                        else if (VDNData.KernelModeString == "run:normal")
                        {
                            vdnData.SendRequset(freezeRequest);
                        }
                    }
                    break;
                case "flightFreeze":
                    SendRequest flightRequest = new SendRequest("EquationsOfMotion", "Freezes", "Flight_Freeze");
                    flightRequest.SendParameters.Add(false);
                    SendRequest flightFreezeRequestModel = new SendRequest("EquationsOfMotion", "Freezes", "Flight_Freeze");
                    flightFreezeRequestModel.SendParameters.Add(true);

                    if (VDNData.FlightFreeze)
                    {
                        vdnData.SendRequset(flightRequest);
                    }
                    else
                    {
                        vdnData.SendRequset(flightFreezeRequestModel);
                    }
                    break;
                case "powerFreeze":
                    SendRequest activeFreezeRequest = new SendRequest("ElectricPower", "Default", "BATTERY_FREEZE");
                    activeFreezeRequest.SendParameters.Add(true);
                    SendRequest cancelFreezeRequest = new SendRequest("ElectricPower", "Default", "BATTERY_FREEZE");
                    cancelFreezeRequest.SendParameters.Add(false);

                    if (VDNData.PowerFreeze)
                    {
                        vdnData.SendRequset(cancelFreezeRequest);
                    }
                    else
                    {
                        vdnData.SendRequset(activeFreezeRequest);
                    }
                    break;
                case "locationFreeze":
                    SendRequest activeLocationFreezeRequest = new SendRequest("AircraftLocation", "Freezes", "Position");
                    activeLocationFreezeRequest.SendParameters.Add(true);
                    SendRequest cancelLocationFreezeRequest = new SendRequest("AircraftLocation", "Freezes", "Position");
                    cancelLocationFreezeRequest.SendParameters.Add(false);

                    if (VDNData.LocationFreeze)
                    {
                        vdnData.SendRequset(cancelLocationFreezeRequest);
                    }
                    else
                    {
                        vdnData.SendRequset(activeLocationFreezeRequest);
                    }
                    break;
                case "highlyFreeze":
                    SendRequest activeHighlyFreezeRequest = new SendRequest("AircraftLocation", "Freezes", "Altitude");
                    activeHighlyFreezeRequest.SendParameters.Add(true);
                    SendRequest cancelHighlyFreezeRequest = new SendRequest("AircraftLocation", "Freezes", "Altitude");
                    cancelHighlyFreezeRequest.SendParameters.Add(false);

                    if (VDNData.HighlyFreeze)
                    {
                        vdnData.SendRequset(cancelHighlyFreezeRequest);
                    }
                    else
                    {
                        vdnData.SendRequset(activeHighlyFreezeRequest);
                    }
                    break;
                case "powerOn":
                    SendRequest PowerOnRequest = new SendRequest("Cockpit_Control", "Default", "DI_BATTERY_PB_ON");
                    var a = Int64.Parse("1");
                    PowerOnRequest.SendParameters.Add(a);
                    vdnData.SendRequset(PowerOnRequest);
                    //To VertaxCockpitIo SE
                    //SendRequest requestModel = new SendRequest("CockpitIo", "Default", "PowerOn_Request");
                    //Int64 i = Int64.Parse("1");
                    //requestModel.SendParameters.Add(i); 
                    break;
                case "powerReset":
                    SendRequest PowerResetRequest = new SendRequest("ElectricPower", "Default", "BATTERY_RESET");
                    PowerResetRequest.SendParameters.Add(true);
                    vdnData.SendRequset(PowerResetRequest);
                    break;
                case "engineOn":
                    SendRequest engineOnRequest = new SendRequest("ElectricPower", "Default", "DI_PROPULSION_PB_ON");
                    engineOnRequest.SendParameters.Add(true);
                    SendRequest engineOffRequest = new SendRequest("ElectricPower", "Default", "DI_PROPULSION_PB_ON");
                    engineOffRequest.SendParameters.Add(false);
                    ////vertax
                    //VDNRequestModel requestModel = new VDNRequestModel("CockpitIo", "Default", "StartPropeller_Request");
                    //Int64 i = Int64.Parse("1");
                    //requestModel.Parameters.Add(i);
                    //vdnClient.SendRequset(requestModel);
                    if (VDNData.EngineOn)
                    {
                        vdnData.SendRequset(engineOffRequest);
                    }
                    else
                    {
                        vdnData.SendRequset(engineOnRequest);
                    }
                    break;
                case "crashOverride":
                    SendRequest crashOnRequest;
                    SendRequest crashOffRequest;

                    crashOnRequest = new SendRequest("Crash", "Default", "Crash_Override");
                    crashOnRequest.SendParameters.Add(true);
                    crashOffRequest = new SendRequest("Crash", "Default", "Crash_Override");
                    crashOffRequest.SendParameters.Add(false);

                    if (PubVar.g_CurrentUser.UserName.Trim() == "BoundaryAI")
                    {
                        if (VDNData.BJCrashOverride)
                        {
                            vdnData.SendRequset(crashOffRequest);
                        }
                        else
                        {
                            vdnData.SendRequset(crashOnRequest);
                        }
                    }
                    else
                    {
                        if (VDNData.CrashOverride)
                        {
                            vdnData.SendRequset(crashOffRequest);
                        }
                        else
                        {
                            vdnData.SendRequset(crashOnRequest);
                        }
                    }
                    break;
                case "cleanCrash":
                    SendRequest cleanCrashOnRequest = new SendRequest("Crash", "Default", "Clear_Crash");
                    cleanCrashOnRequest.SendParameters.Add(true);
                    SendRequest cleanCrashOffRequest = new SendRequest("Crash", "Default", "Clear_Crash");
                    cleanCrashOffRequest.SendParameters.Add(false);

                    if (VDNData.BJClearCrash)
                    {
                        vdnData.SendRequset(cleanCrashOffRequest);
                    }
                    else
                    {
                        vdnData.SendRequset(cleanCrashOnRequest);
                    }
                    break;
                case "motionPre":
                    SendRequest motionStartRequest = new SendRequest("Motion", "Default", "Toggle_Motion_State");
                    motionStartRequest.SendParameters.Add(1);
                    SendRequest motionRequest = new SendRequest("Motion", "Default", "Toggle_Motion_State");
                    motionRequest.SendParameters.Add(2);

                    if (VDNData.MotionSwitch == 1)
                    {
                        vdnData.SendRequset(motionRequest);
                    }
                    else if (VDNData.MotionSwitch == 3)
                    {
                        vdnData.SendRequset(motionStartRequest);
                    }
                    break;
                case "motionFreeze":
                    SendRequest cancelFreezeModel = new SendRequest("Motion", "Freezes", "Motion");
                    cancelFreezeModel.SendParameters.Add(false);
                    SendRequest freezeMotionRequest = new SendRequest("Motion", "Freezes", "Motion");
                    freezeMotionRequest.SendParameters.Add(true);

                    if (VDNData.MotionFreeze)
                    {
                        vdnData.SendRequset(cancelFreezeModel);
                    }
                    else
                    {
                        vdnData.SendRequset(freezeMotionRequest);
                    }
                    break;
                case "visualConnect":
                    SendRequest closeVisualModel = new SendRequest("Visual", "Default", "Set_Visual_On");
                    closeVisualModel.SendParameters.Add(false);
                    SendRequest openVisualRequest = new SendRequest("Visual", "Default", "Set_Visual_On");
                    openVisualRequest.SendParameters.Add(true);

                    if (VDNData.VisualConnect)
                    {
                        vdnData.SendRequset(closeVisualModel);
                    }
                    else
                    {
                        vdnData.SendRequset(openVisualRequest);
                    }
                    break;

                default: break;
            }
        }

        public void MenuEvent(PageModel obj)
        {
            functionPage = new Frame();
            MainPage = obj.PageView;
            var d = MainPage;
            FunctionPage = MainPage;
        }

        public void CloseWindow()
        {
            vdnHelper.StopProgram();
            vdnHelper.StopProcess("AircraftSimulation");
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

    }
}
