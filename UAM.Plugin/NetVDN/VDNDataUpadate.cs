using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using UAM.Model;

namespace UAM.Plugin.NetVDN
{
    public class VDNDataUpadate : NotifyObject
    {
        private DispatcherTimer _dispatcherTimer;
        VDNData vdnData = new VDNData();
        private static readonly VDNDataUpadate dataUpdate = new VDNDataUpadate();
        public static VDNDataUpadate Instance => dataUpdate;

        #region VDNData Fields
        private string _KernelModeString;
        public string KernelModeString
        {
            get => _KernelModeString;
            set { _KernelModeString = value; RaisePropertyChanged("KernelModeString"); }
        }
        private bool _BJKernelModeString;
        public bool BJKernelModeString
        {
            get => _BJKernelModeString;
            set { _BJKernelModeString = value; RaisePropertyChanged("BJKernelModeString"); }
        }
        private bool _FlightFreeze;
        public bool FlightFreeze
        {
            get => _FlightFreeze;
            set { _FlightFreeze = value; RaisePropertyChanged("FlightFreeze"); }
        }
        private bool _PowerFreeze;
        public bool PowerFreeze
        {
            get => _PowerFreeze;
            set { _PowerFreeze = value; RaisePropertyChanged("PowerFreeze"); }
        }
        private bool _LocationFreeze;
        public bool LocationFreeze
        {
            get => _LocationFreeze;
            set { _LocationFreeze = value; RaisePropertyChanged("LocationFreeze"); }
        }
        private bool _HighlyFreeze;
        public bool HighlyFreeze
        {
            get => _HighlyFreeze;
            set { _HighlyFreeze = value; RaisePropertyChanged("HighlyFreeze"); }
        }
        private bool _PowerOn;
        public bool PowerOn
        {
            get => _PowerOn;
            set { _PowerOn = value; RaisePropertyChanged("PowerOn"); }
        }
        private bool _EngineOn;
        public bool EngineOn
        {
            get => _EngineOn;
            set { _EngineOn = value; RaisePropertyChanged("EngineOn"); }
        }
        private bool _CrashOverride;
        public bool CrashOverride
        {
            get => _CrashOverride;
            set { _CrashOverride = value; RaisePropertyChanged("CrashOverride"); }
        }
        private bool _BJCrashOverride;
        public bool BJCrashOverride
        {
            get => _BJCrashOverride;
            set { _BJCrashOverride = value; RaisePropertyChanged("BJCrashOverride"); }
        }
        private bool _BJClearCrash;
        public bool BJClearCrash
        {
            get => _BJClearCrash;
            set { _BJClearCrash = value; RaisePropertyChanged("BJClearCrash"); }
        }
        private string _HostConnect;
        public string HostConnect
        {
            get => _HostConnect;
            set { _HostConnect = value; RaisePropertyChanged("HostConnect"); }
        }
        private bool _VisualConnect;
        public bool VisualConnect
        {
            get => _VisualConnect;
            set { _VisualConnect = value; RaisePropertyChanged("VisualConnect"); }
        }
        private string _Weather;
        public string Weather
        {
            get => _Weather;
            set { _Weather = value; RaisePropertyChanged("Weather"); }
        }
        private string _GroundWindDirection;
        public string GroundWindDirection
        {
            get => _GroundWindDirection;
            set { _GroundWindDirection = value; RaisePropertyChanged("GroundWindDirection"); }
        }
        private string _GroundWindVelocity;
        public string GroundWindVelocity
        {
            get => _GroundWindVelocity;
            set { _GroundWindVelocity = value; RaisePropertyChanged("GroundWindVelocity"); }
        }
        private string _HollowDirection;
        public string HollowDirection
        {
            get => _HollowDirection;
            set { _HollowDirection = value; RaisePropertyChanged("HollowDirection"); }
        }
        private string _HollowVelocity;
        public string HollowVelocity
        {
            get => _HollowVelocity;
            set { _HollowVelocity = value; RaisePropertyChanged("HollowVelocity"); }
        }
        private string _GustSpeed;
        public string GustSpeed
        {
            get => _GustSpeed;
            set { _GustSpeed = value; RaisePropertyChanged("GustSpeed"); }
        }
        private string _Latitude;
        public string Latitude
        {
            get => _Latitude;
            set { _Latitude = value; RaisePropertyChanged("Latitude"); }
        }
        private string _Longitude;
        public string Longitude
        {
            get => _Longitude;
            set { _Longitude = value; RaisePropertyChanged("Longitude"); }
        }
        private string _Altitude;
        public string Altitude
        {
            get => _Altitude;
            set { _Altitude = value; RaisePropertyChanged("Altitude"); }
        }
        private string _Pitch;
        public string Pitch
        {
            get => _Pitch;
            set { _Pitch = value; RaisePropertyChanged("Pitch"); }
        }
        private string _Roll;
        public string Roll
        {
            get => _Roll;
            set { _Roll = value; RaisePropertyChanged("Roll"); }
        }
        private string _Heading;
        public string Heading
        {
            get => _Heading;
            set { _Heading = value; RaisePropertyChanged("Heading"); }
        }
        private string _Current_View;
        public string Current_View
        {
            get => _Current_View;
            set { _Current_View = value; RaisePropertyChanged("Current_View"); }
        }
        private bool _FlightPlanGenerated;
        public bool FlightPlanGenerated
        {
            get => _FlightPlanGenerated;
            set { _FlightPlanGenerated = value; RaisePropertyChanged("FlightPlanGenerated"); }
        }

        private bool _APState;
        public bool APState
        {
            get => _APState;
            set { _APState = value; RaisePropertyChanged("APState"); }
        }

        private bool _CtrSurLock0IsActive;
        public bool CtrSurLock0IsActive
        {
            get => _CtrSurLock0IsActive;
            set { _CtrSurLock0IsActive = value; RaisePropertyChanged("CtrSurLock0IsActive"); }
        }
        private bool _CtrSurLock1IsActive;
        public bool CtrSurLock1IsActive
        {
            get => _CtrSurLock1IsActive;
            set { _CtrSurLock1IsActive = value; RaisePropertyChanged("CtrSurLock1IsActive"); }
        }
        private bool _CtrSurLock2IsActive;
        public bool CtrSurLock2IsActive
        {
            get => _CtrSurLock2IsActive;
            set { _CtrSurLock2IsActive = value; RaisePropertyChanged("CtrSurLock2IsActive"); }
        }
        private bool _CtrSurLock3IsActive;
        public bool CtrSurLock3IsActive
        {
            get => _CtrSurLock3IsActive;
            set { _CtrSurLock3IsActive = value; RaisePropertyChanged("CtrSurLock3IsActive"); }
        }
        private bool _CtrSurLock4IsActive;
        public bool CtrSurLock4IsActive
        {
            get => _CtrSurLock4IsActive;
            set { _CtrSurLock4IsActive = value; RaisePropertyChanged("CtrSurLock4IsActive"); }
        }
        private bool _CtrSurLock5IsActive;
        public bool CtrSurLock5IsActive
        {
            get => _CtrSurLock5IsActive;
            set { _CtrSurLock5IsActive = value; RaisePropertyChanged("CtrSurLock5IsActive"); }
        }
        private bool _CtrSurLock6IsActive;
        public bool CtrSurLock6IsActive
        {
            get => _CtrSurLock6IsActive;
            set { _CtrSurLock6IsActive = value; RaisePropertyChanged("CtrSurLock6IsActive"); }
        }
        private bool _CtrSurLock7IsActive;
        public bool CtrSurLock7IsActive
        {
            get => _CtrSurLock7IsActive;
            set { _CtrSurLock7IsActive = value; RaisePropertyChanged("CtrSurLock7IsActive"); }
        }
        private bool _CtrSurLock8IsActive;
        public bool CtrSurLock8IsActive
        {
            get => _CtrSurLock8IsActive;
            set { _CtrSurLock8IsActive = value; RaisePropertyChanged("CtrSurLock8IsActive"); }
        }
        private bool _CtrSurLock9IsActive;
        public bool CtrSurLock9IsActive
        {
            get => _CtrSurLock9IsActive;
            set { _CtrSurLock9IsActive = value; RaisePropertyChanged("CtrSurLock9IsActive"); }
        }

        private bool _CtrSurLock8IsEnable;
        public bool CtrSurLock8IsEnable
        {
            get => _CtrSurLock8IsEnable;
            set { _CtrSurLock8IsEnable = value; RaisePropertyChanged("CtrSurLock8IsEnable"); }
        }
        private bool _CtrSurLock7IsEnable;
        public bool CtrSurLock7IsEnable
        {
            get => _CtrSurLock7IsEnable;
            set { _CtrSurLock7IsEnable = value; RaisePropertyChanged("CtrSurLock7IsEnable"); }
        }
        private bool _CtrSurLock6IsEnable;
        public bool CtrSurLock6IsEnable
        {
            get => _CtrSurLock6IsEnable;
            set { _CtrSurLock6IsEnable = value; RaisePropertyChanged("CtrSurLock6IsEnable"); }
        }
        private bool _CtrSurLock5IsEnable;
        public bool CtrSurLock5IsEnable
        {
            get => _CtrSurLock5IsEnable;
            set { _CtrSurLock5IsEnable = value; RaisePropertyChanged("CtrSurLock5IsEnable"); }
        }
        private bool _CtrSurLock4IsEnable;
        public bool CtrSurLock4IsEnable
        {
            get => _CtrSurLock4IsEnable;
            set { _CtrSurLock4IsEnable = value; RaisePropertyChanged("CtrSurLock4IsEnable"); }
        }
        private bool _CtrSurLock3IsEnable;
        public bool CtrSurLock3IsEnable
        {
            get => _CtrSurLock3IsEnable;
            set { _CtrSurLock3IsEnable = value; RaisePropertyChanged("CtrSurLock3IsEnable"); }
        }
        private bool _CtrSurLock2IsEnable;
        public bool CtrSurLock2IsEnable
        {
            get => _CtrSurLock2IsEnable;
            set { _CtrSurLock2IsEnable = value; RaisePropertyChanged("CtrSurLock2IsEnable"); }
        }
        private bool _CtrSurLock1IsEnable;
        public bool CtrSurLock1IsEnable
        {
            get => _CtrSurLock1IsEnable;
            set { _CtrSurLock1IsEnable = value; RaisePropertyChanged("CtrSurLock1IsEnable"); }
        }
        private bool _CtrSurLock0IsEnable;
        public bool CtrSurLock0IsEnable
        {
            get => _CtrSurLock0IsEnable;
            set { _CtrSurLock0IsEnable = value; RaisePropertyChanged("CtrSurLock0IsEnable"); }
        }
        private bool _CtrSurLock9IsEnable;
        public bool CtrSurLock9IsEnable
        {
            get => _CtrSurLock9IsEnable;
            set { _CtrSurLock9IsEnable = value; RaisePropertyChanged("CtrSurLock9IsEnable"); }
        }

        private bool _CtrSurLoss9IsEnable;
        public bool CtrSurLoss9IsEnable
        {
            get => _CtrSurLoss9IsEnable;
            set { _CtrSurLoss9IsEnable = value; RaisePropertyChanged("CtrSurLoss9IsEnable"); }
        }
        private bool _CtrSurLoss8IsEnable;
        public bool CtrSurLoss8IsEnable
        {
            get => _CtrSurLoss8IsEnable;
            set { _CtrSurLoss8IsEnable = value; RaisePropertyChanged("CtrSurLoss8IsEnable"); }
        }
        private bool _CtrSurLoss7IsEnable;
        public bool CtrSurLoss7IsEnable
        {
            get => _CtrSurLoss7IsEnable;
            set { _CtrSurLoss7IsEnable = value; RaisePropertyChanged("CtrSurLoss7IsEnable"); }
        }
        private bool _CtrSurLoss6IsEnable;
        public bool CtrSurLoss6IsEnable
        {
            get => _CtrSurLoss6IsEnable;
            set { _CtrSurLoss6IsEnable = value; RaisePropertyChanged("CtrSurLoss6IsEnable"); }
        }
        private bool _CtrSurLoss5IsEnable;
        public bool CtrSurLoss5IsEnable
        {
            get => _CtrSurLoss5IsEnable;
            set { _CtrSurLoss5IsEnable = value; RaisePropertyChanged("CtrSurLoss5IsEnable"); }
        }
        private bool _CtrSurLoss4IsEnable;
        public bool CtrSurLoss4IsEnable
        {
            get => _CtrSurLoss4IsEnable;
            set { _CtrSurLoss4IsEnable = value; RaisePropertyChanged("CtrSurLoss4IsEnable"); }
        }
        private bool _CtrSurLoss3IsEnable;
        public bool CtrSurLoss3IsEnable
        {
            get => _CtrSurLoss3IsEnable;
            set { _CtrSurLoss3IsEnable = value; RaisePropertyChanged("CtrSurLoss3IsEnable"); }
        }
        private bool _CtrSurLoss2IsEnable;
        public bool CtrSurLoss2IsEnable
        {
            get => _CtrSurLoss2IsEnable;
            set { _CtrSurLoss2IsEnable = value; RaisePropertyChanged("CtrSurLoss2IsEnable"); }
        }
        private bool _CtrSurLoss1IsEnable;
        public bool CtrSurLoss1IsEnable
        {
            get => _CtrSurLoss1IsEnable;
            set { _CtrSurLoss1IsEnable = value; RaisePropertyChanged("CtrSurLoss1IsEnable"); }
        }
        private bool _CtrSurLoss0IsEnable;
        public bool CtrSurLoss0IsEnable
        {
            get => _CtrSurLoss0IsEnable;
            set { _CtrSurLoss0IsEnable = value; RaisePropertyChanged("CtrSurLoss0IsEnable"); }
        }

        private bool _CtrSurLoss0IsActive;
        public bool CtrSurLoss0IsActive
        {
            get => _CtrSurLoss0IsActive;
            set { _CtrSurLoss0IsActive = value; RaisePropertyChanged("CtrSurLoss0IsActive"); }
        }
        private bool _CtrSurLoss1IsActive;
        public bool CtrSurLoss1IsActive
        {
            get => _CtrSurLoss1IsActive;
            set { _CtrSurLoss1IsActive = value; RaisePropertyChanged("CtrSurLoss1IsActive"); }
        }
        private bool _CtrSurLoss2IsActive;
        public bool CtrSurLoss2IsActive
        {
            get => _CtrSurLoss2IsActive;
            set { _CtrSurLoss2IsActive = value; RaisePropertyChanged("CtrSurLoss2IsActive"); }
        }
        private bool _CtrSurLoss3IsActive;
        public bool CtrSurLoss3IsActive
        {
            get => _CtrSurLoss3IsActive;
            set { _CtrSurLoss3IsActive = value; RaisePropertyChanged("CtrSurLoss3IsActive"); }
        }
        private bool _CtrSurLoss4IsActive;
        public bool CtrSurLoss4IsActive
        {
            get => _CtrSurLoss4IsActive;
            set { _CtrSurLoss4IsActive = value; RaisePropertyChanged("CtrSurLoss4IsActive"); }
        }
        private bool _CtrSurLoss5IsActive;
        public bool CtrSurLoss5IsActive
        {
            get => _CtrSurLoss5IsActive;
            set { _CtrSurLoss5IsActive = value; RaisePropertyChanged("CtrSurLoss5IsActive"); }
        }
        private bool _CtrSurLoss6IsActive;
        public bool CtrSurLoss6IsActive
        {
            get => _CtrSurLoss6IsActive;
            set { _CtrSurLoss6IsActive = value; RaisePropertyChanged("CtrSurLoss6IsActive"); }
        }
        private bool _CtrSurLoss7IsActive;
        public bool CtrSurLoss7IsActive
        {
            get => _CtrSurLoss7IsActive;
            set { _CtrSurLoss7IsActive = value; RaisePropertyChanged("CtrSurLoss7IsActive"); }
        }
        private bool _CtrSurLoss8IsActive;
        public bool CtrSurLoss8IsActive
        {
            get => _CtrSurLoss8IsActive;
            set { _CtrSurLoss8IsActive = value; RaisePropertyChanged("CtrSurLoss8IsActive"); }
        }
        private bool _CtrSurLoss9IsActive;
        public bool CtrSurLoss9IsActive
        {
            get => _CtrSurLoss9IsActive;
            set { _CtrSurLoss9IsActive = value; RaisePropertyChanged("CtrSurLoss9IsActive"); }
        }

        private bool _MotorLoss9IsActive;
        public bool MotorLoss9IsActive
        {
            get => _MotorLoss9IsActive;
            set { _MotorLoss9IsActive = value; RaisePropertyChanged("MotorLoss9IsActive"); }
        }
        private bool _MotorLoss8IsActive;
        public bool MotorLoss8IsActive
        {
            get => _MotorLoss8IsActive;
            set { _MotorLoss8IsActive = value; RaisePropertyChanged("MotorLoss8IsActive"); }
        }
        private bool _MotorLoss7IsActive;
        public bool MotorLoss7IsActive
        {
            get => _MotorLoss7IsActive;
            set { _MotorLoss7IsActive = value; RaisePropertyChanged("MotorLoss7IsActive"); }
        }
        private bool _MotorLoss6IsActive;
        public bool MotorLoss6IsActive
        {
            get => _MotorLoss6IsActive;
            set { _MotorLoss6IsActive = value; RaisePropertyChanged("MotorLoss6IsActive"); }
        }
        private bool _MotorLoss5IsActive;
        public bool MotorLoss5IsActive
        {
            get => _MotorLoss5IsActive;
            set { _MotorLoss5IsActive = value; RaisePropertyChanged("MotorLoss5IsActive"); }
        }
        private bool _MotorLoss4IsActive;
        public bool MotorLoss4IsActive
        {
            get => _MotorLoss4IsActive;
            set { _MotorLoss4IsActive = value; RaisePropertyChanged("MotorLoss4IsActive"); }
        }
        private bool _MotorLoss3IsActive;
        public bool MotorLoss3IsActive
        {
            get => _MotorLoss3IsActive;
            set { _MotorLoss3IsActive = value; RaisePropertyChanged("MotorLoss3IsActive"); }
        }
        private bool _MotorLoss2IsActive;
        public bool MotorLoss2IsActive
        {
            get => _MotorLoss2IsActive;
            set { _MotorLoss2IsActive = value; RaisePropertyChanged("MotorLoss2IsActive"); }
        }
        private bool _MotorLoss1IsActive;
        public bool MotorLoss1IsActive
        {
            get => _MotorLoss1IsActive;
            set { _MotorLoss1IsActive = value; RaisePropertyChanged("MotorLoss1IsActive"); }
        }
        private bool _MotorLoss0IsActive;
        public bool MotorLoss0IsActive
        {
            get => _MotorLoss0IsActive;
            set { _MotorLoss0IsActive = value; RaisePropertyChanged("MotorLoss0IsActive"); }
        }

        private bool _MotorLoss0IsEnable;
        public bool MotorLoss0IsEnable
        {
            get => _MotorLoss0IsEnable;
            set { _MotorLoss0IsEnable = value; RaisePropertyChanged("MotorLoss0IsEnable"); }
        }
        private bool _MotorLoss1IsEnable;
        public bool MotorLoss1IsEnable
        {
            get => _MotorLoss1IsEnable;
            set { _MotorLoss1IsEnable = value; RaisePropertyChanged("MotorLoss1IsEnable"); }
        }
        private bool _MotorLoss2IsEnable;
        public bool MotorLoss2IsEnable
        {
            get => _MotorLoss2IsEnable;
            set { _MotorLoss2IsEnable = value; RaisePropertyChanged("MotorLoss2IsEnable"); }
        }
        private bool _MotorLoss3IsEnable;
        public bool MotorLoss3IsEnable
        {
            get => _MotorLoss3IsEnable;
            set { _MotorLoss3IsEnable = value; RaisePropertyChanged("MotorLoss3IsEnable"); }
        }
        private bool _MotorLoss4IsEnable;
        public bool MotorLoss4IsEnable
        {
            get => _MotorLoss4IsEnable;
            set { _MotorLoss4IsEnable = value; RaisePropertyChanged("MotorLoss4IsEnable"); }
        }
        private bool _MotorLoss5IsEnable;
        public bool MotorLoss5IsEnable
        {
            get => _MotorLoss5IsEnable;
            set { _MotorLoss5IsEnable = value; RaisePropertyChanged("MotorLoss5IsEnable"); }
        }
        private bool _MotorLoss6IsEnable;
        public bool MotorLoss6IsEnable
        {
            get => _MotorLoss6IsEnable;
            set { _MotorLoss6IsEnable = value; RaisePropertyChanged("MotorLoss6IsEnable"); }
        }
        private bool _MotorLoss7IsEnable;
        public bool MotorLoss7IsEnable
        {
            get => _MotorLoss7IsEnable;
            set { _MotorLoss7IsEnable = value; RaisePropertyChanged("MotorLoss7IsEnable"); }
        }
        private bool _MotorLoss8IsEnable;
        public bool MotorLoss8IsEnable
        {
            get => _MotorLoss8IsEnable;
            set { _MotorLoss8IsEnable = value; RaisePropertyChanged("MotorLoss8IsEnable"); }
        }
        private bool _MotorLoss9IsEnable;
        public bool MotorLoss9IsEnable
        {
            get => _MotorLoss9IsEnable;
            set { _MotorLoss9IsEnable = value; RaisePropertyChanged("MotorLoss9IsEnable"); }
        }
        #endregion

        public VDNDataUpadate()
        {
            _dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _dispatcherTimer.Tick += OnTimer;

            _dispatcherTimer.Start();
        }

        private void OnTimer(object source, EventArgs e)
        {
            try
            {
                KernelModeString = VDNData.KernelModeString;
                BJKernelModeString = VDNData.BJKernelModeString;
                FlightFreeze = VDNData.FlightFreeze;
                PowerFreeze = VDNData.PowerFreeze;
                LocationFreeze = VDNData.LocationFreeze;
                HighlyFreeze = VDNData.HighlyFreeze;
                PowerOn = VDNData.PowerOn;
                EngineOn = VDNData.EngineOn;
                CrashOverride = VDNData.CrashOverride;
                BJCrashOverride = VDNData.BJCrashOverride;
                BJClearCrash = VDNData.BJClearCrash;
                HostConnect = vdnData.GetConnectState();
                VisualConnect = VDNData.VisualConnect;
                Weather = VDNData.CIGIWeather;
                GroundWindDirection = VDNData.GroundWindDirection;
                GroundWindVelocity = VDNData.GroundWindVelocity;
                HollowDirection = VDNData.HollowDirection;
                HollowVelocity = VDNData.HollowVelocity;
                GustSpeed = VDNData.GustSpeed;
                Latitude = VDNData.Latitude.ToString("0.00000");
                Longitude = VDNData.Longitude.ToString("0.00000");
                Altitude = VDNData.Altitude.ToString("0.00000");
                Roll = VDNData.Roll.ToString("0.000");
                Pitch = VDNData.Pitch.ToString("0.000");
                Heading = VDNData.Heading.ToString("0.000");
                Current_View = VDNData.Current_View;
                FlightPlanGenerated = VDNData.FlightPlanGenerated;
                APState = VDNData.APState;
                CtrSurLock0IsEnable = VDNData.CtrSurLock0IsEnable;
                CtrSurLock1IsEnable = VDNData.CtrSurLock1IsEnable;
                CtrSurLock2IsEnable = VDNData.CtrSurLock2IsEnable;
                CtrSurLock3IsEnable = VDNData.CtrSurLock3IsEnable;
                CtrSurLock4IsEnable = VDNData.CtrSurLock4IsEnable;
                CtrSurLock5IsEnable = VDNData.CtrSurLock5IsEnable;
                CtrSurLock6IsEnable = VDNData.CtrSurLock6IsEnable;
                CtrSurLock7IsEnable = VDNData.CtrSurLock7IsEnable;
                CtrSurLock8IsEnable = VDNData.CtrSurLock8IsEnable;
                CtrSurLock9IsEnable = VDNData.CtrSurLock9IsEnable;
                CtrSurLoss0IsEnable = VDNData.CtrSurLoss0IsEnable;
                CtrSurLoss1IsEnable = VDNData.CtrSurLoss1IsEnable;
                CtrSurLoss2IsEnable = VDNData.CtrSurLoss2IsEnable;
                CtrSurLoss3IsEnable = VDNData.CtrSurLoss3IsEnable;
                CtrSurLoss4IsEnable = VDNData.CtrSurLoss4IsEnable;
                CtrSurLoss5IsEnable = VDNData.CtrSurLoss5IsEnable;
                CtrSurLoss6IsEnable = VDNData.CtrSurLoss6IsEnable;
                CtrSurLoss7IsEnable = VDNData.CtrSurLoss7IsEnable;
                CtrSurLoss8IsEnable = VDNData.CtrSurLoss8IsEnable;
                CtrSurLoss9IsEnable = VDNData.CtrSurLoss9IsEnable;

                CtrSurLock0IsActive = VDNData.CtrSurLock0IsActive;
                CtrSurLock1IsActive = VDNData.CtrSurLock1IsActive;
                CtrSurLock2IsActive = VDNData.CtrSurLock2IsActive;
                CtrSurLock3IsActive = VDNData.CtrSurLock3IsActive;
                CtrSurLock4IsActive = VDNData.CtrSurLock4IsActive;
                CtrSurLock5IsActive = VDNData.CtrSurLock5IsActive;
                CtrSurLock6IsActive = VDNData.CtrSurLock6IsActive;
                CtrSurLock7IsActive = VDNData.CtrSurLock7IsActive;
                CtrSurLock8IsActive = VDNData.CtrSurLock8IsActive;
                CtrSurLock9IsActive = VDNData.CtrSurLock9IsActive;
                CtrSurLoss0IsActive = VDNData.CtrSurLoss0IsActive;
                CtrSurLoss1IsActive = VDNData.CtrSurLoss1IsActive;
                CtrSurLoss2IsActive = VDNData.CtrSurLoss2IsActive;
                CtrSurLoss3IsActive = VDNData.CtrSurLoss3IsActive;
                CtrSurLoss4IsActive = VDNData.CtrSurLoss4IsActive;
                CtrSurLoss5IsActive = VDNData.CtrSurLoss5IsActive;
                CtrSurLoss6IsActive = VDNData.CtrSurLoss6IsActive;
                CtrSurLoss7IsActive = VDNData.CtrSurLoss7IsActive;
                CtrSurLoss8IsActive = VDNData.CtrSurLoss8IsActive;
                CtrSurLoss9IsActive = VDNData.CtrSurLoss9IsActive;

                MotorLoss0IsEnable = VDNData.MotorLoss0IsEnable;
                MotorLoss1IsEnable = VDNData.MotorLoss1IsEnable;
                MotorLoss2IsEnable = VDNData.MotorLoss2IsEnable;
                MotorLoss3IsEnable = VDNData.MotorLoss3IsEnable;
                MotorLoss4IsEnable = VDNData.MotorLoss4IsEnable;
                MotorLoss5IsEnable = VDNData.MotorLoss5IsEnable;
                MotorLoss6IsEnable = VDNData.MotorLoss6IsEnable;
                MotorLoss7IsEnable = VDNData.MotorLoss7IsEnable;
                MotorLoss8IsEnable = VDNData.MotorLoss8IsEnable;
                MotorLoss9IsEnable = VDNData.MotorLoss9IsEnable;

                MotorLoss0IsActive = VDNData.MotorLoss0IsActive;
                MotorLoss1IsActive = VDNData.MotorLoss1IsActive;
                MotorLoss2IsActive = VDNData.MotorLoss2IsActive;
                MotorLoss3IsActive = VDNData.MotorLoss3IsActive;
                MotorLoss4IsActive = VDNData.MotorLoss4IsActive;
                MotorLoss5IsActive = VDNData.MotorLoss5IsActive;
                MotorLoss6IsActive = VDNData.MotorLoss6IsActive;
                MotorLoss7IsActive = VDNData.MotorLoss7IsActive;
                MotorLoss8IsActive = VDNData.MotorLoss8IsActive;
                MotorLoss9IsActive = VDNData.MotorLoss9IsActive;
            }
            catch (Exception ex)
            {
                Console.WriteLine("VDN连接失败,原因：" + ex.Message);
            }
        }
    }
}
