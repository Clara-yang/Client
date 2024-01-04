using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UAM.Model.Models;
using UAM.Plugin.Common;
using UAM.Plugin;
using UAM.Client.Common;
using System.Web.UI.WebControls.Expressions;
using static UAM.Model.Models.VDNModel;
using UAM.Client.Converters;

namespace UAM.Client.ViewModel
{
    public class INSViewModel : ViewModelBase
    {
        VDNData vdnClient = new VDNData();

        #region Icommand
        private ICommand _ResetCommand;
        public ICommand ResetCommand
        {
            get { return _ResetCommand ?? new RelayCommand<string>(Reset); }
        }

        private ICommand _SetUpCommand;
        public ICommand SetUpCommand
        {
            get { return _SetUpCommand ?? new RelayCommand<string>(SetUp); }
        }
        #endregion

        #region Porperty
        public double _Latitude;
        public double Latitude
        {
            get { return _Latitude; }
            set { _Latitude = value; RaisePropertyChanged("Latitude"); }
        }
        public double _Longitude;
        public double Longitude
        {
            get { return _Longitude; }
            set { _Longitude = value; RaisePropertyChanged("Longitude"); }
        }
        public double _Altitude;
        public double Altitude
        {
            get { return _Altitude; }
            set { _Altitude = value; RaisePropertyChanged("Altitude"); }
        }
        public double _NorthVelocity;
        public double NorthVelocity
        {
            get { return _NorthVelocity; }
            set { _NorthVelocity = value; RaisePropertyChanged("NorthVelocity"); }
        }
        public double _EastVelocity;
        public double EastVelocity
        {
            get { return _EastVelocity; }
            set { _EastVelocity = value; RaisePropertyChanged("EastVelocity"); }
        }
        public double _VerticalVelocity;
        public double VerticalVelocity
        {
            get { return _VerticalVelocity; }
            set { _VerticalVelocity = value; RaisePropertyChanged("VerticalVelocity"); }
        }
        public double _Pitch;
        public double Pitch
        {
            get { return _Pitch; }
            set { _Pitch = value; RaisePropertyChanged("Pitch"); }
        }
        public double _Phi;
        public double Phi
        {
            get { return _Phi; }
            set { _Phi = value; RaisePropertyChanged("Phi"); }
        }
        public double _Psi;
        public double Psi
        {
            get { return _Psi; }
            set { _Psi = value; RaisePropertyChanged("Psi"); }
        }
        public double _pBias;
        public double pBias
        {
            get { return _pBias; }
            set { _pBias = value; RaisePropertyChanged("pBias"); }
        }
        public double _qBias;
        public double qBias
        {
            get { return _qBias; }
            set { _qBias = value; RaisePropertyChanged("qBias"); }
        }
        public double _rBias;
        public double rBias
        {
            get { return _rBias; }
            set { _rBias = value; RaisePropertyChanged("rBias"); }
        }
        public double _ForX;
        public double ForX
        {
            get { return _ForX; }
            set { _ForX = value; RaisePropertyChanged("ForX"); }
        }
        public double _ForY;
        public double ForY
        {
            get { return _ForY; }
            set { _ForY = value; RaisePropertyChanged("ForY"); }
        }
        public double _ForZ;
        public double ForZ
        {
            get { return _ForZ; }
            set { _ForZ = value; RaisePropertyChanged("ForZ"); }
        }
        public bool _PositionAvailability;
        public bool PositionAvailability
        {
            get { return _PositionAvailability; }
            set { _PositionAvailability = value; RaisePropertyChanged("PositionAvailability"); }
        }
        public bool _HorizontalSpeedAvailability;
        public bool HorizontalSpeedAvailability
        {
            get { return _HorizontalSpeedAvailability; }
            set { _HorizontalSpeedAvailability = value; RaisePropertyChanged("HorizontalSpeedAvailability"); }
        }
        public bool _AltitudeAvailability;
        public bool AltitudeAvailability
        {
            get { return _AltitudeAvailability; }
            set { _AltitudeAvailability = value; RaisePropertyChanged("AltitudeAvailability"); }
        }
        public bool _VerticalSpeedAvailability;
        public bool VerticalSpeedAvailability
        {
            get { return _VerticalSpeedAvailability; }
            set { _VerticalSpeedAvailability = value; RaisePropertyChanged("VerticalSpeedAvailability"); }
        }
        public bool _HeadingAvailability;
        public bool HeadingAvailability
        {
            get { return _HeadingAvailability; }
            set { _HeadingAvailability = value; RaisePropertyChanged("HeadingAvailability"); }
        }
        public bool _AttitudeAvailability;
        public bool AttitudeAvailability
        {
            get { return _AttitudeAvailability; }
            set { _AttitudeAvailability = value; RaisePropertyChanged("AttitudeAvailability"); }
        }
        #endregion

        public INSViewModel()
        {

        }

        public void Reset(string param)
        {
            if (param == "INSposition")
            {
                Latitude = 0;
                Altitude = 0;
                Longitude = 0;
            }
            if (param == "INSspeed")
            {
                NorthVelocity = 0;
                EastVelocity = 0;
                VerticalVelocity = 0;
            }
            if (param == "INSposture")
            {
                Pitch = 0;
                Phi = 0;
                Psi = 0;
            }
            if (param == "INSangularSspeed")
            {
                pBias = 0;
                qBias = 0;
                rBias = 0;
            }
            if (param == "INSacceleration")
            {
                ForX = 0;
                ForY = 0;
                ForZ = 0;
            }
            if (param == "INSfunctionAvailability")
            {
                PositionAvailability = true;
                HorizontalSpeedAvailability = true;
                AltitudeAvailability = true;
                VerticalSpeedAvailability = true;
                HeadingAvailability = true;
                AttitudeAvailability = true;
            }
        }

        public void SetUp(string param)
        {
            var sendProp = new List<object>();
            var reqs = PubVar.g_SendRequestList.FindAll(s => s.ControlName == param).OrderBy(s => s.ParameterIndex).ToList();

            if (param == "INSposition")
            {
                sendProp.Add(Latitude);
                sendProp.Add(Longitude);
                sendProp.Add(Altitude);
            }
            if (param == "INSspeed")
            {
                sendProp.Add(NorthVelocity);
                sendProp.Add(EastVelocity);
                sendProp.Add(VerticalVelocity);
            }
            if (param == "INSposture")
            {
                sendProp.Add(Phi);
                sendProp.Add(Pitch);
                sendProp.Add(Psi);
            }
            if (param == "INSangularSspeed")
            {
                sendProp.Add(pBias);
                sendProp.Add(qBias);
                sendProp.Add(rBias);
            }
            if (param == "INSacceleration")
            {
                sendProp.Add(ForX);
                sendProp.Add(ForY);
                sendProp.Add(ForZ);
            }
            if (param == "INSfunctionAvailability")
            {
                sendProp.Add(PositionAvailability);
                sendProp.Add(AltitudeAvailability);
                sendProp.Add(HorizontalSpeedAvailability);
                sendProp.Add(VerticalSpeedAvailability);
                sendProp.Add(HeadingAvailability);
                sendProp.Add(AttitudeAvailability);
            }

            CommonMethod.SendRequest_Sensor(reqs, sendProp);

        }

    }
}
