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
using UAM.Client.Common;
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
        public List<object> param;

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
            param = new List<object>();
            switch (e.Parameter)
            {
                case "simFreeze":
                    SendRequest simFreezeRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "simFreeze");
                    SendRequest simRunRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "simRun");
                    var simFreezeVdnValue = CommonMethod.GetPropertiesValue(dataUpdate, simFreezeRequest.VdnField);
                    if (simFreezeVdnValue.Equals(true) || simFreezeVdnValue.Equals(false))
                    {
                        if (simFreezeVdnValue.Equals(true))
                        {
                            param.Add(false);
                            vdnData.SendRequset(CommonMethod.SendRequestMethod(simFreezeRequest.SendTopic, simFreezeRequest.SendQueue, simFreezeRequest.SendRequestName, param));
                        }
                        else
                        {
                            param.Add(true);
                            vdnData.SendRequset(CommonMethod.SendRequestMethod(simFreezeRequest.SendTopic, simFreezeRequest.SendQueue, simFreezeRequest.SendRequestName, param));
                        }
                    }
                    else
                    {
                        if (simFreezeVdnValue.ToString() == "freeze:normal")
                        {
                            param.Add(true);
                            vdnData.SendRequset(CommonMethod.SendRequestMethod(simRunRequest.SendTopic, simRunRequest.SendQueue, simRunRequest.SendRequestName, param));
                        }
                        else
                        {
                            param.Add(true);
                            vdnData.SendRequset(CommonMethod.SendRequestMethod(simFreezeRequest.SendTopic, simFreezeRequest.SendQueue, simFreezeRequest.SendRequestName, param));
                        }
                    }
                    break;
                case "flightFreeze":
                    SendRequest flightFreezeRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "flightFreeze");
                    var flightFreezeVdnValue = CommonMethod.GetPropertiesValue(dataUpdate, flightFreezeRequest.VdnField);
                    if (flightFreezeVdnValue.Equals(true))
                    {
                        param.Add(false);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(flightFreezeRequest.SendTopic, flightFreezeRequest.SendQueue, flightFreezeRequest.SendRequestName, param));
                    }
                    else
                    {
                        param.Add(true);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(flightFreezeRequest.SendTopic, flightFreezeRequest.SendQueue, flightFreezeRequest.SendRequestName, param));
                    }
                    break;
                case "powerFreeze":
                    SendRequest powerFreezeRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "powerFreeze");
                    var powerFreezeVdnValue = CommonMethod.GetPropertiesValue(dataUpdate, powerFreezeRequest.VdnField);
                    if (powerFreezeVdnValue.Equals(true))
                    {
                        param.Add(false);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(powerFreezeRequest.SendTopic, powerFreezeRequest.SendQueue, powerFreezeRequest.SendRequestName, param));
                    }
                    else
                    {
                        param.Add(true);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(powerFreezeRequest.SendTopic, powerFreezeRequest.SendQueue, powerFreezeRequest.SendRequestName, param));
                    }
                    break;
                case "locationFreeze":
                    SendRequest locationFreezeRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "locationFreeze");
                    var locationFreezeVdnValue = CommonMethod.GetPropertiesValue(dataUpdate, locationFreezeRequest.VdnField);
                    if (locationFreezeVdnValue.Equals(true))
                    {
                        param.Add(false);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(locationFreezeRequest.SendTopic, locationFreezeRequest.SendQueue, locationFreezeRequest.SendRequestName, param));
                    }
                    else
                    {
                        param.Add(true);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(locationFreezeRequest.SendTopic, locationFreezeRequest.SendQueue, locationFreezeRequest.SendRequestName, param));
                    }
                    break;
                case "highlyFreeze":
                    SendRequest highlyFreezeRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "highlyFreeze");
                    var highlyFreezeVdnValue = CommonMethod.GetPropertiesValue(dataUpdate, highlyFreezeRequest.VdnField);
                    if (highlyFreezeVdnValue.Equals(true))
                    {
                        param.Add(false);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(highlyFreezeRequest.SendTopic, highlyFreezeRequest.SendQueue, highlyFreezeRequest.SendRequestName, param));
                    }
                    else
                    {
                        param.Add(true);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(highlyFreezeRequest.SendTopic, highlyFreezeRequest.SendQueue, highlyFreezeRequest.SendRequestName, param));
                    }
                    break;
                case "powerOn":
                    SendRequest powerOnRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "powerOn");
                    param.Add(Int64.Parse("1"));
                    vdnData.SendRequset(CommonMethod.SendRequestMethod(powerOnRequest.SendTopic, powerOnRequest.SendQueue,
                        powerOnRequest.SendRequestName, param));
                    break;
                case "powerReset":
                    SendRequest powerResetRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "powerReset");
                    param.Add(true);
                    vdnData.SendRequset(CommonMethod.SendRequestMethod(powerResetRequest.SendTopic, powerResetRequest.SendQueue, powerResetRequest.SendRequestName, param));
                    break;
                case "engineOn":
                    SendRequest engineOnRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "engineOn");
                    var engineOnVdnValue = CommonMethod.GetPropertiesValue(dataUpdate, engineOnRequest.VdnField);
                    if (engineOnVdnValue.Equals(true) || engineOnVdnValue.Equals(false))
                    {
                        if (engineOnVdnValue.Equals(true))
                        {
                            param.Add(false);
                            vdnData.SendRequset(CommonMethod.SendRequestMethod(engineOnRequest.SendTopic, engineOnRequest.SendQueue, engineOnRequest.SendRequestName, param));
                        }
                        else
                        {
                            param.Add(true);
                            vdnData.SendRequset(CommonMethod.SendRequestMethod(engineOnRequest.SendTopic, engineOnRequest.SendQueue, engineOnRequest.SendRequestName, param));
                        }
                    }
                    else
                    {
                        param.Add(Int64.Parse("1"));
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(engineOnRequest.SendTopic, engineOnRequest.SendQueue, engineOnRequest.SendRequestName, param));
                    }

                    break;
                case "crashOverride":
                    SendRequest crashOverrideRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "crashOverride");
                    var crashOverrideVdnValue = CommonMethod.GetPropertiesValue(dataUpdate, crashOverrideRequest.VdnField);
                    if (crashOverrideVdnValue.Equals(true))
                    {
                        param.Add(false);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(crashOverrideRequest.SendTopic, crashOverrideRequest.SendQueue, crashOverrideRequest.SendRequestName, param));
                    }
                    else
                    {
                        param.Add(true);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(crashOverrideRequest.SendTopic, crashOverrideRequest.SendQueue, crashOverrideRequest.SendRequestName, param));
                    }
                    break;
                case "cleanCrash":
                    SendRequest cleanCrashRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "cleanCrash");
                    var cleanCrashVdnValue = CommonMethod.GetPropertiesValue(dataUpdate, cleanCrashRequest.VdnField);
                    if (cleanCrashVdnValue.Equals(true))
                    {
                        param.Add(false);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(cleanCrashRequest.SendTopic, cleanCrashRequest.SendQueue, cleanCrashRequest.SendRequestName, param));
                    }
                    else
                    {
                        param.Add(true);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(cleanCrashRequest.SendTopic, cleanCrashRequest.SendQueue, cleanCrashRequest.SendRequestName, param));
                    }
                    break;
                case "motionPre":
                    SendRequest motionPreRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "motionPre");
                    var motionPreVdnValue = CommonMethod.GetPropertiesValue(dataUpdate, motionPreRequest.VdnField);
                    if (motionPreVdnValue.Equals(1))
                    {
                        param.Add(2);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(motionPreRequest.SendTopic, motionPreRequest.SendQueue, motionPreRequest.SendRequestName, param));
                    }
                    else if (motionPreVdnValue.Equals(3))
                    {
                        param.Add(1);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(motionPreRequest.SendTopic, motionPreRequest.SendQueue, motionPreRequest.SendRequestName, param));
                    }
                    break;
                case "motionFreeze":
                    SendRequest motionFreezeRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "motionFreeze");
                    var motionFreezeVdnValue = CommonMethod.GetPropertiesValue(dataUpdate, motionFreezeRequest.VdnField);
                    if (motionFreezeVdnValue.Equals(1))
                    {
                        param.Add(false);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(motionFreezeRequest.SendTopic, motionFreezeRequest.SendQueue, motionFreezeRequest.SendRequestName, param));
                    }
                    else
                    {
                        param.Add(true);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(motionFreezeRequest.SendTopic, motionFreezeRequest.SendQueue, motionFreezeRequest.SendRequestName, param));
                    }
                    break;
                case "visualConnect":
                    SendRequest visualConnectRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "visualConnect");
                    var visualConnectVdnValue = CommonMethod.GetPropertiesValue(dataUpdate, visualConnectRequest.VdnField);
                    if (visualConnectVdnValue.Equals(true))
                    {
                        param.Add(false);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(visualConnectRequest.SendTopic, visualConnectRequest.SendQueue, visualConnectRequest.SendRequestName, param));
                    }
                    else
                    {
                        param.Add(true);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(visualConnectRequest.SendTopic, visualConnectRequest.SendQueue, visualConnectRequest.SendRequestName, param));
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
            vdnHelper.StopProcess("fcRehost");
            vdnHelper.StopProcess("hostPrevTrim");
            vdnHelper.StopProcess("PlaySound");
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

    }
}
