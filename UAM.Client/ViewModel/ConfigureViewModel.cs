using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using UAM.Client.Common;
using UAM.Client.Views;
using UAM.Model.Models;
using UAM.Plugin;
using UAM.Plugin.Common;
using UAM.Plugin.NetVDN;
using static UAM.Model.Models.VDNModel;

namespace UAM.Client.ViewModel
{
    public class ConfigureViewModel : ViewModelHelper
    {
        public ConfigureModel configModel { get; set; } = new ConfigureModel();
        VDNData VDNData = new VDNData();
        VDNDataUpadate dataUpdate = VDNDataUpadate.Instance;
        SendRequest executeAllRequest;
        SendRequest execVehicleRequest;
        SendRequest destinationRequest;
        SendRequest departureRequest;
        SendRequest execChangeStickRequest;
        SendRequest execChangeDayRequestModel;
        SendRequest execChangeSeasonRequestModel;

        #region property
        private List<AircraftModel> gridDataAir;
        public List<AircraftModel> GridDataAir
        {
            get { return gridDataAir; }
            set { gridDataAir = value; }
        }
        private AircraftModel selectedAir;
        public AircraftModel SelectedAir
        {
            get { return selectedAir; }
            set { selectedAir = value; }
        }

        private List<RouteModel> gridDataRoute;
        public List<RouteModel> GridDataRoute
        {
            get { return gridDataRoute; }
            set { gridDataRoute = value; }
        }

        private RouteModel selectedRoute;
        public RouteModel SelectedRoute
        {
            get { return selectedRoute; }
            set { selectedRoute = value; }
        }

        private List<StickModel> stickList;
        public List<StickModel> StickList
        {
            get { return stickList; }
            set { stickList = value; }
        }
        private StickModel selectedStick;
        public StickModel SelectedStick
        {
            get { return selectedStick; }
            set { selectedStick = value; }
        }

        private string selectedDawnDusk;
        public string SelectedDawnDusk
        {
            get { return selectedDawnDusk; }
            set { selectedDawnDusk = value; }
        }

        private string selectedSeason;
        public string SelectedSeason
        {
            get { return selectedSeason; }
            set { selectedSeason = value; }
        }
        #endregion

        #region ICommand
        private ICommand m_ChangeStickCommand;
        public ICommand ChangeStickCommand => m_ChangeStickCommand ?? new RelayCommand(ExecChangeStick);

        private ICommand m_ChangeDayCommand;
        public ICommand ChangeDayCommand => m_ChangeDayCommand ?? new RelayCommand(ExecChangeDay);

        private ICommand m_ChangeSeasonCommand;
        public ICommand ChangeSeasonCommand => m_ChangeSeasonCommand ?? new RelayCommand(ExecChangeSeason);

        private ICommand m_ExecuteCommand;
        public ICommand ExecuteCommand => m_ExecuteCommand ?? new RelayCommand(ExecuteFlight);

        private ICommand m_VehicleCommand;
        public ICommand VehicleCommand => m_VehicleCommand ?? new RelayCommand(ExecVehicle);

        private ICommand m_RouteCommand;
        public ICommand RouteCommand => m_RouteCommand ?? new RelayCommand(ExecRoute);

        private ICommand m_ChangeNowTimeCommand;
        public ICommand ChangeNowTimeCommand => m_ChangeNowTimeCommand ?? new RelayCommand(ExecChangeTime);

        #endregion

        public ConfigureViewModel()
        {
            GetBinding();
            PubVar.g_CurrentDate = DateTime.Now;

            dataUpdate.PropertyChanged += DataUpdate_PropertyChanged;
        }
        private void DataUpdate_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HostConnect")
            {
                ExecRoute();
                ExecVehicle();
                ExecChangeDay();
                ExecChangeSeason();
                ExecChangeStick();
            }
        }

        public void GetBinding()
        {
            configModel.DawnDuskList = new List<string>() { "Day", "Dusk", "Down", "Night" };
            SelectedDawnDusk = configModel.DawnDuskList.First();

            configModel.SeasonList = new List<string>() { "Summer", "Winter" };
            SelectedSeason = configModel.SeasonList.First();

            configModel.YearMonth = PubVar.g_CurrentDate.ToString("D");
            configModel.NowTime = PubVar.g_CurrentDate.ToString("t");

            GridDataAir = PubVar.g_AircraftList;
            SelectedAir = GridDataAir.Find(s => s.IsAircraftDefault == true);

            GridDataRoute = PubVar.g_RouteList;
            selectedRoute = GridDataRoute.Find(s => s.IsRouteDefault == true);

            StickList = PubVar.g_StickList;
            SelectedStick = StickList.Find(s => s.Visible == true && s.Default == true);
        }

        public void ExecuteFlight()
        {
            executeAllRequest = new SendRequest("FlightControlWrapper", "Default", "UserName_Request");
            string name = PubVar.g_CurrentUser.UserName.Trim();
            executeAllRequest.SendParameters.Add(name);

            VDNData.SendRequset(executeAllRequest);
            VDNData.SendRequset(CommonMethod.SendRequestMethod(execVehicleRequest.SendTopic, execVehicleRequest.SendQueue,
                execVehicleRequest.SendRequestName, execVehicleRequest.SendParameters));
            VDNData.SendRequset(CommonMethod.SendRequestMethod(destinationRequest.SendTopic, destinationRequest.SendQueue,
                destinationRequest.SendRequestName, destinationRequest.SendParameters));
            VDNData.SendRequset(CommonMethod.SendRequestMethod(departureRequest.SendTopic, departureRequest.SendQueue,
                departureRequest.SendRequestName, departureRequest.SendParameters));
            VDNData.SendRequset(CommonMethod.SendRequestMethod(execChangeStickRequest.SendTopic, execChangeStickRequest.SendQueue,
                execChangeStickRequest.SendRequestName, execChangeStickRequest.SendParameters));
            VDNData.SendRequset(CommonMethod.SendRequestMethod(execChangeDayRequestModel.SendTopic, execChangeDayRequestModel.SendQueue,
                execChangeDayRequestModel.SendRequestName, execChangeDayRequestModel.SendParameters));
            VDNData.SendRequset(CommonMethod.SendRequestMethod(execChangeSeasonRequestModel.SendTopic, execChangeSeasonRequestModel.SendQueue,
                execChangeSeasonRequestModel.SendRequestName, execChangeSeasonRequestModel.SendParameters));

            Thread.Sleep(1000);

            Flight flight = new Flight();
            NextPage(flight);
        }

        public void ExecVehicle()
        {
            PubVar.CurrentAircraft = SelectedAir?.AircraftCode;
            PubVar.CurrentAircraftName = SelectedAir?.AircraftName;

            execVehicleRequest = PubVar.g_SendRequestList.Find(q => q.ControlName == "vehicleChange");
            execVehicleRequest.SendParameters.Add(PubVar.CurrentAircraft);
        }

        public void ExecRoute()
        {
            PubVar.CurrentRoute = SelectedRoute.RouteId;
            PubVar.CurrentRouteName = SelectedRoute.RouteName;
            PubVar.DeparturePad = selectedRoute.DeparturePad;
            PubVar.DepartureVertiport = selectedRoute.DepartureVertiport;
            PubVar.DestinationPad = selectedRoute.DestinationPad;
            PubVar.DestinationVertiport = selectedRoute.DestinationVertiport;

            Int64 destination = Int64.Parse(selectedRoute.DestinationId);
            Int64 departure = Int64.Parse(selectedRoute.DepartureId);

            destinationRequest = PubVar.g_SendRequestList.Find(q => q.ControlName == "RouteDestination");
            destinationRequest.SendParameters.Add(destination);

            departureRequest = PubVar.g_SendRequestList.Find(q => q.ControlName == "RouteActive");
            departureRequest.SendParameters.Add(departure);
        }

        public void ExecChangeTime()
        {
            configModel.YearMonth = DateTime.Now.ToString("D");
            configModel.NowTime = DateTime.Now.ToString("t");
        }

        public void ExecChangeStick()
        {
            PubVar.g_CurrentStick = SelectedStick;

            execChangeStickRequest = PubVar.g_SendRequestList.Find(q => q.ControlName == "stickChange");
            execChangeStickRequest.SendParameters.Add(SelectedStick.Id);
        }

        public void ExecChangeDay()
        {
            PubVar.CurrentDay = selectedDawnDusk;
            execChangeDayRequestModel = PubVar.g_SendRequestList.Find(q => q.ControlName == "DayChange");
            execChangeDayRequestModel.SendParameters.Add(PubVar.CurrentDay);
        }

        public void ExecChangeSeason()
        {
            PubVar.CurrentSeason = selectedSeason;
            execChangeSeasonRequestModel = PubVar.g_SendRequestList.Find(q => q.ControlName == "SeasonChange");
            execChangeSeasonRequestModel.SendParameters.Add(PubVar.CurrentSeason);
        }


    }
}
