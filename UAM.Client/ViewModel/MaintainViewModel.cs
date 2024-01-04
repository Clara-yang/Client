using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using UAM.Model.Models;
using static UAM.Model.Models.VDNModel;
using UAM.Plugin;
using CefSharp.DevTools.Network;
using UAM.Plugin.Common;
using System.Windows;
using System.Windows.Threading;
using UAM.Plugin.NetVDN;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Windows.Controls;

namespace UAM.Client.ViewModel
{
    public class MaintainViewModel : ViewModelBase
    {
        VDNData vdnData = new VDNData();
        private static Thread thread;
        VDNDataUpadate dataUpdate = VDNDataUpadate.Instance;

        #region virualStick
        private double local_leftX = 0.0;
        private double local_leftY = 0.0;
        private double local_rightX = 0.0;
        private double local_rightY = 0.0;
        #endregion

        #region LJstick
        public JoyControl myControl = new JoyControl();
        private double local_Throttle = 0.0;
        private double local_Axsix_y = 0.0;
        private double local_Axsix_z = 0.0;
        private double local_Axsix_x = 0.0;
        #endregion

        private double leftX = 0.0;
        private double leftY = 0.0;
        private double rightX = 0.0;
        private double rightY = 0.0;

        #region property
        public Visibility IsVirualStick { get; set; } = Visibility.Hidden;

        private string _Latitude;
        public string Latitude
        {
            get { return _Latitude; }
            set { _Latitude = value; RaisePropertyChanged("Latitude"); }
        }
        private string _Longitude;
        public string Longitude
        {
            get { return _Longitude; }
            set { _Longitude = value; RaisePropertyChanged("Longitude"); }
        }
        private string _Altitude;
        public string Altitude
        {
            get { return _Altitude; }
            set { _Altitude = value; RaisePropertyChanged("Altitude"); }
        }
        private string _Pitch;
        public string Pitch
        {
            get { return _Pitch; }
            set { _Pitch = value; RaisePropertyChanged("Pitch"); }
        }
        private string _Roll;
        public string Roll
        {
            get { return _Roll; }
            set { _Roll = value; RaisePropertyChanged("Roll"); }
        }
        private string _Heading;
        public string Heading
        {
            get { return _Heading; }
            set { _Heading = value; RaisePropertyChanged("Heading"); }
        }
        #endregion

        public MaintainViewModel()
        {
            if (PubVar.g_CurrentStick.Id == 8)
            {
                IsVirualStick = Visibility.Visible;
                thread = new Thread(VirualStickSend);
                thread.Start();
            }
            else if(PubVar.g_CurrentStick.Id == 1)
            {
                IsVirualStick = Visibility.Hidden;
                thread = new Thread(LJStickSend);
                thread.Start();
            }
            else
            {
                IsVirualStick = Visibility.Hidden;
            }
            GetBinding();
            dataUpdate.PropertyChanged += DataUpdate_PropertyChanged;
        }

        private void GetBinding()
        {
            Latitude = dataUpdate.Latitude;
            Longitude = dataUpdate.Longitude;
            Altitude = dataUpdate.Altitude;
            Pitch = dataUpdate.Pitch;
            Roll = dataUpdate.Roll;
            Heading = dataUpdate.Heading;
        }

        public void GetLeftStickValue(double X, double Y)
        {
            leftX = X;
            leftY = Y;
            PubVar.g_AirRePosition = false;
        }

        public void GetRightStickValue(double X, double Y)
        {
            rightX = X;
            rightY = Y;
            PubVar.g_AirRePosition = false;
        }

        public void LJStickSend()
        {
            while (true)
            {
                Thread.Sleep(50);
                if (!(Math.Abs(myControl.Throttle - local_Throttle) < 0.00001))
                {
                    SendRequest requestModel = new SendRequest("FlightControlWrapper", "Default", "HeightRate_Request");
                    requestModel.SendParameters.Add(myControl.Throttle);
                    vdnData.SendRequset(requestModel);
                    local_Throttle = myControl.Throttle;
                }


                if (!(Math.Abs(myControl.Axsix_y - local_Axsix_y) < 0.00001))
                {
                    SendRequest requestModel = new SendRequest("FlightControlWrapper", "Default", "LeftRight_Request");
                    requestModel.SendParameters.Add(myControl.Axsix_y);
                    vdnData.SendRequset(requestModel);
                    local_Axsix_y = myControl.Axsix_y;
                }

                if (!(Math.Abs(myControl.Axsix_z - local_Axsix_z) < 0.00001))
                {
                    SendRequest requestModel = new SendRequest("FlightControlWrapper", "Default", "TurnRate_Request");
                    requestModel.SendParameters.Add(myControl.Axsix_z);
                    vdnData.SendRequset(requestModel);
                    local_Axsix_z = myControl.Axsix_z;
                }

                if (!(Math.Abs(myControl.Axsix_X - local_Axsix_x) < 0.00001))
                {
                    SendRequest requestModel = new SendRequest("FlightControlWrapper", "Default", "ForwardBack_Request");
                    requestModel.SendParameters.Add(myControl.Axsix_X);
                    vdnData.SendRequset(requestModel);
                    local_Axsix_x = myControl.Axsix_X;
                }

                if (true)
                {
                    SendRequest requestModel = new SendRequest("FlightControlWrapper", "Default", "AP_Button");
                    requestModel.SendParameters.Add(myControl.AP_Button);
                    vdnData.SendRequset(requestModel);
                }
            }
        }

        public void VirualStickSend()
        {
            if (PubVar.g_AirRePosition)
            {
                local_leftX = 0;
                local_rightX = 0;
                local_rightX = 0;
                local_rightY = 0;
            }
            while(true)
            {
                Thread.Sleep(50);
                if (!(Math.Abs(leftX - local_leftX) < 0.00001))
                {
                    SendRequest requestModel = new SendRequest("ControlStickWrapper", "Default", "Yaw_Request");
                    requestModel.SendParameters.Add(leftX);
                    vdnData.SendRequset(requestModel);
                    local_leftX = leftX;
                }

                if (!(Math.Abs(leftY - local_leftY) < 0.00001))
                {
                    SendRequest requestModel = new SendRequest("ControlStickWrapper", "Default", "HeightRate_Request");
                    requestModel.SendParameters.Add(leftY);
                    vdnData.SendRequset(requestModel);
                    local_leftY = leftY;
                }

                if (!(Math.Abs(rightX - local_rightX) < 0.00001))
                {
                    SendRequest requestModel = new SendRequest("ControlStickWrapper", "Default", "Roll_Request");
                    requestModel.SendParameters.Add(rightX);
                    vdnData.SendRequset(requestModel);
                    local_rightX = rightX;
                }

                if (!(Math.Abs(rightY - local_rightY) < 0.00001))
                {
                    SendRequest requestModel = new SendRequest("ControlStickWrapper", "Default", "Pitch_Request");
                    requestModel.SendParameters.Add(rightY);
                    vdnData.SendRequset(requestModel);
                    local_rightY = rightY;
                }
            }
        }

        private void DataUpdate_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Latitude = dataUpdate.Latitude;
            Longitude = dataUpdate.Longitude;
            Altitude = dataUpdate.Altitude;
            Roll = dataUpdate.Roll;
            Pitch = dataUpdate.Pitch;
            Heading = dataUpdate.Heading;
        }
    }
}
