using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using UAM.Client.Common;
using UAM.Client.Views;
using UAM.Model.Models;
using UAM.Plugin;
using UAM.Plugin.Common;
using UAM.Plugin.NetVDN;
using static UAM.Model.Models.VDNModel;

namespace UAM.Client.ViewModel
{
    public class MapViewModel : ViewModelHelper
    {
        VDNData vdnData = new VDNData();
        VDNDataUpadate dataUpdate = VDNDataUpadate.Instance;
        public List<object> param;

        #region Property
        public int Route { get; set; } = PubVar.CurrentRoute;
        public string RouteName { get; set; } = PubVar.CurrentRouteName;
        public string Aircraft { get; set; } = PubVar.CurrentAircraftName?.ToString();
        public string Departure { get; set; } = PubVar.DepartureVertiport?.ToString();
        public string DeparturePad { get; set; } = PubVar.DeparturePad?.ToString();
        public string Destination { get; set; } = PubVar.DestinationVertiport?.ToString();
        public string DestinationPad { get; set; } = PubVar.DestinationPad?.ToString();
        public string Url { get; set; }
        //public ObservableCollection<ButtonProp> BtnCollection { get; set; }
        public List<ButtonModel> BtnCollection { get; set; }
        #endregion

        #region ICommand
        private ICommand _ChangeAirCommand;
        public ICommand ChangeAirCommand
        {
            get { return _ChangeAirCommand ?? new RelayCommand(ChangeAir); }
        }

        private ICommand m_ChangePositionCommand;
        public ICommand ChangePositionCommand
        {
            get { return m_ChangePositionCommand ?? new RelayCommand<string>(ChangePosition); }
        }

        private RelayCommand<ButtonModel> _bottomBtnlick;
        public RelayCommand<ButtonModel> BottomBtnClick
        {
            get
            {
                if (true) return new RelayCommand<ButtonModel>(e =>
                {
                    ExecClick(e);
                }
                );
            }
            set { _bottomBtnlick = value; }
        }
        #endregion

        #region Btn
        ButtonModel cockiBtn;
        ButtonModel thirdBtn;
        ButtonModel flyPlanBtn;
        ButtonModel apBtn;
        ButtonModel expectBtn;
        ButtonModel unexpectBtn;
        #endregion

        public MapViewModel()
        {
            BtnCollection = new List<ButtonModel>();
            Url = PubVar.g_ConfigureUrl.Find(s => s.MapName == "MainMap").MapUrl;

            LoadBtn();
            dataUpdate.PropertyChanged += DataUpdate_PropertyChanged;

            foreach (var btn in BtnCollection)
            {
                btn.BtnClickCommand = BottomBtnClick;
            }
        }

        private void DataUpdate_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (dataUpdate.Current_View == "Cockpit_View")
            {
                cockiBtn.IsChecked = true;
                thirdBtn.IsChecked = false;
            }
            else if (dataUpdate.Current_View == "Third_View")
            {
                cockiBtn.IsChecked = true;
                thirdBtn.IsChecked = false;
            }

            flyPlanBtn.IsChecked = dataUpdate.FlightPlanGenerated == true ? true : false;
            apBtn.IsChecked = dataUpdate.APState == true ? true : false;
        }

        public void LoadBtn()
        {
            var btns = PubVar.g_ButtonList.FindAll(s => s.Visible == true && s.Position == "Bottom");
            foreach (var btn in btns)
            {
                BtnCollection.Add(btn);
            }

            cockiBtn = BtnCollection.Find(s => s.ButtonName == "cockpitBtn");
            thirdBtn = BtnCollection.Find(s => s.ButtonName == "thirdBtn");
            flyPlanBtn = BtnCollection.Find(s => s.ButtonName == "flyPlanBtn");
            apBtn = BtnCollection.Find(s => s.ButtonName == "apBtn");
            expectBtn = BtnCollection.Find(s => s.ButtonName == "expectBtn");
            unexpectBtn = BtnCollection.Find(s => s.ButtonName == "unexpectBtn");
        }

        public void ExecClick(ButtonModel e)
        {
            param = new List<object>();
            switch (e.Parameter)
            {
                case "cockpitView":
                    SendRequest cockpitViewRequest = PubVar.g_SendRequestList.Find(q => q.ControlName == "cockpitView");
                    param.Add("Cockpit_View");
                    vdnData.SendRequset(CommonMethod.SendRequestMethod(cockpitViewRequest.SendTopic, cockpitViewRequest.SendQueue, cockpitViewRequest.SendRequestName, param));
                    break;
                case "thirdView":
                    SendRequest thirdViewRequest = PubVar.g_SendRequestList.Find(q => q.ControlName == "cockpitView");
                    param.Add("Third_View");
                    vdnData.SendRequset(CommonMethod.SendRequestMethod(thirdViewRequest.SendTopic, thirdViewRequest.SendQueue, thirdViewRequest.SendRequestName, param));
                    break;
                case "newFlyPlans":
                    SendRequest flayPlanRequest = PubVar.g_SendRequestList.Find(q => q.ControlName == "newFlyPlans");
                    var flightFreezeVdnValue = CommonMethod.GetPropertiesValue(dataUpdate, flayPlanRequest.VdnField);
                    if (flightFreezeVdnValue.ToString() != "true" && flightFreezeVdnValue.ToString() != "false")
                    {
                        if (flightFreezeVdnValue.ToString() == "true")
                        {
                            param.Add(false);
                            vdnData.SendRequset(CommonMethod.SendRequestMethod(flayPlanRequest.SendTopic, flayPlanRequest.SendQueue, flayPlanRequest.SendRequestName, param));
                        }
                        else
                        {
                            param.Add(true);
                            vdnData.SendRequset(CommonMethod.SendRequestMethod(flayPlanRequest.SendTopic, flayPlanRequest.SendQueue, flayPlanRequest.SendRequestName, param));
                        }
                    }
                    else
                    {
                        param.Add(VDNData.DestinationLat);
                        param.Add(VDNData.DestinationLon);
                        param.Add(VDNData.DestinationAlt);
                        param.Add(VDNData.DestinationHeading);
                        vdnData.SendRequset(CommonMethod.SendRequestMethod(flayPlanRequest.SendTopic, flayPlanRequest.SendQueue, flayPlanRequest.SendRequestName, param));
                    }
                    break;
                case "APActive":
                    SendRequest apActiveRequest = PubVar.g_SendRequestList.Find(q => q.ControlName == "APActive");
                    param.Add(Int64.Parse("1"));
                    vdnData.SendRequset(CommonMethod.SendRequestMethod(apActiveRequest.SendTopic, apActiveRequest.SendQueue, apActiveRequest.SendRequestName, param));
                    break;
                default: break;
            }
        }

        public void ChangeAir()
        {
            Configure Confogure = new Configure();
            NextPage(Confogure);
        }

        public void ChangePosition(string param)
        {
            SendRequest changePositionRequest = PubVar.g_SendRequestList.Find(q => q.ControlName == "ChangePosition");
            changePositionRequest.SendParameters.Add(Int64.Parse(param));
            vdnData.SendRequset(CommonMethod.SendRequestMethod(changePositionRequest.SendTopic, changePositionRequest.SendQueue,
                changePositionRequest.SendRequestName, changePositionRequest.SendParameters));
            PubVar.g_AirRePosition = true;
        }
    }
}