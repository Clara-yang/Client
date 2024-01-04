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
            switch (e.Parameter)
            {
                case "cockpitView":
                    SendRequest cockpitViewRequest = new SendRequest("Visual", "Default", "ChangeView");
                    cockpitViewRequest.SendParameters.Add("Cockpit_View");
                    vdnData.SendRequset(cockpitViewRequest);
                    break;
                case "thirdView":
                    SendRequest thirdViewRequest = new SendRequest("Visual", "Default", "ChangeView");
                    thirdViewRequest.SendParameters.Add("Third_View");
                    vdnData.SendRequset(thirdViewRequest);
                    break;
                case "newFlyPlans":
                    if (PubVar.g_CurrentUser.UserName.Trim() == "BoundaryAI")
                    {
                        SendRequest ActiveFlayPlanRequest = new SendRequest("Fms", "Default", "GetFixedRouteFlyPlan");
                        ActiveFlayPlanRequest.SendParameters.Add(true);
                        SendRequest CancelFlayPlanRequest = new SendRequest("Fms", "Default", "GetFixedRouteFlyPlan");
                        CancelFlayPlanRequest.SendParameters.Add(false);
                        if (VDNData.FlightPlanGenerated)
                        {
                            vdnData.SendRequset(CancelFlayPlanRequest);
                        }
                        else
                        {
                            vdnData.SendRequset(ActiveFlayPlanRequest);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Generate a flight plan for Vertax");
                        SendRequest flayPlanRequest = new SendRequest("Fms", "Default", "GetFixedRouteFlyPlan");
                        flayPlanRequest.SendParameters.Add(VDNData.DestinationLat);
                        flayPlanRequest.SendParameters.Add(VDNData.DestinationLon);
                        flayPlanRequest.SendParameters.Add(VDNData.DestinationAlt);
                        flayPlanRequest.SendParameters.Add(VDNData.DestinationHeading);
                        vdnData.SendRequset(flayPlanRequest);
                    }
                    break;
                case "APActive":
                    SendRequest apActiveRequest;
                    apActiveRequest = new SendRequest("FlightControlWrapper", "Default", "AP_Button_Available_Request");
                    var s = Int64.Parse("1");
                    apActiveRequest.SendParameters.Add(s);
                    vdnData.SendRequset(apActiveRequest);
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
            SendRequest requestModel = new SendRequest("ReLocate", "Default", "Vertiport_Position");
            var s = Int64.Parse(param);
            requestModel.SendParameters.Add(s);
            vdnData.SendRequset(requestModel);
            PubVar.g_AirRePosition = true;
        }
    }
}