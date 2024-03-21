using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using UAM.Plugin;
using UAM.Plugin.Common;
using UAM.Plugin.NetVDN;
using static UAM.Model.Models.VDNModel;
using Csf.Imets.ToolCore.AutoFidelityTesting.FileIo;
using UAM.Client.Common;
using CefSharp.Structs;

namespace UAM.Client.ViewModel
{
    public class EnvironmentViewModel : ViewModelBase
    {
        private VDNData vdnClient = new VDNData();
        private DispatcherTimer timer;
        VDNDataUpadate dataUpdate = VDNDataUpadate.Instance;
        public List<object> param;
        public SendRequest controlRequest = new SendRequest();

        #region property
        private string m_HollowDirection;
        public string HollowDirection
        {
            get { return m_HollowDirection; }
            set { Set(ref m_HollowDirection, value); }
        }

        private string m_HollowVelocity;
        public string HollowVelocity
        {
            get { return m_HollowVelocity; }
            set { Set(ref m_HollowVelocity, value); }
        }

        private string m_GroundWindDirection;
        public string GroundWindDirection
        {
            get { return m_GroundWindDirection; }
            set { Set(ref m_GroundWindDirection, value); }
        }

        private string m_GroundWindVelocity;
        public string GroundWindVelocity
        {
            get { return m_GroundWindVelocity; }
            set { Set(ref m_GroundWindVelocity, value); }
        }

        private string m_Precipitation;
        public string Precipitation
        {
            get { return m_Precipitation; }
            set { Set(ref m_Precipitation, value); }
        }

        private int m_Direction;
        public int Direction
        {
            get { return m_Direction; }
            set { Set(ref m_Direction, value); }
        }

        private int m_Speed;
        public int Speed
        {
            get { return m_Speed; }
            set { Set(ref m_Speed, value); }
        }

        private string m_GustSpeed;
        public string GustSpeed
        {
            get { return m_GustSpeed; }
            set { Set(ref m_GustSpeed, value); }
        }

        private int m_WindAltitude;
        public int WindAltitude
        {
            get { return m_WindAltitude; }
            set { Set(ref m_WindAltitude, value); }
        }

        private int m_CloudType1;
        public int CloudType1
        {
            get { return m_CloudType1; }
            set { Set(ref m_CloudType1, value); }
        }

        private double m_CloudHeight1;
        public double CloudHeight1
        {
            get { return m_CloudHeight1; }
            set { Set(ref m_CloudHeight1, value); }
        }

        private int m_CloudDensity1;
        public int CloudDensity1
        {
            get { return m_CloudDensity1; }
            set { Set(ref m_CloudDensity1, value); }
        }

        private int m_CloudType2;
        public int CloudType2
        {
            get { return m_CloudType2; }
            set { Set(ref m_CloudType2, value); }
        }

        private double m_CloudHeight2;
        public double CloudHeight2
        {
            get { return m_CloudHeight2; }
            set { Set(ref m_CloudHeight2, value); }
        }

        private int m_CloudDensity2;
        public int CloudDensity2
        {
            get { return m_CloudDensity2; }
            set { Set(ref m_CloudDensity2, value); }
        }

        private int m_CloudType3;
        public int CloudType3
        {
            get { return m_CloudType3; }
            set { Set(ref m_CloudType3, value); }
        }

        private double m_CloudHeight3;
        public double CloudHeight3
        {
            get { return m_CloudHeight3; }
            set { Set(ref m_CloudHeight3, value); }
        }

        private int m_CloudDensity3;
        public int CloudDensity3
        {
            get { return m_CloudDensity3; }
            set { Set(ref m_CloudDensity3, value); }
        }

        private int m_Temperature = 20;
        public int Temperature
        {
            get { return m_Temperature; }
            set { Set(ref m_Temperature, value); }
        }

        private int m_Humidity = 25;
        public int Humidity
        {
            get { return m_Humidity; }
            set { Set(ref m_Humidity, value); }
        }

        private string m_Season = "Summer";
        public string Season
        {
            get { return m_Season; }
            set { Set(ref m_Season, value); }
        }

        private string m_TimeOfDay;
        public string TimeOfDay
        {
            get { return m_TimeOfDay; }
            set { Set(ref m_TimeOfDay, value); }
        }

        private int m_Hour = 12;
        public int Hour
        {
            get { return m_Hour; }
            set { Set(ref m_Hour, value); }
        }

        private int m_Minute = 0;
        public int Minute
        {
            get { return m_Minute; }
            set { Set(ref m_Minute, value); }
        }

        private int m_Second = 0;
        public int Second
        {
            get { return m_Second; }
            set { Set(ref m_Second, value); }
        }

        private int m_TimeTick = 72;
        public int TimeTick
        {
            get { return m_TimeTick; }
            set
            {
                Set(ref m_TimeTick, value);
                Hour = (int)Math.Floor(TimeTick / 6d);
                Minute = (TimeTick % 6) * 10;
                Second = 0;
            }
        }

        private bool m_WindSign;
        public bool WindSign
        {
            get { return m_WindSign; }
            set { Set(ref m_WindSign, value); }
        }

        private bool m_CloudSign;
        public bool CloudSign
        {
            get { return m_CloudSign; }
            set { Set(ref m_CloudSign, value); }
        }

        private string m_Date;
        public string Date
        {
            get { return m_Date; }
            set { Set(ref m_Date, value); }
        }

        private string m_Weather;
        public string Weather
        {
            get { return m_Weather; }
            set { Set(ref m_Weather, value); }
        }
        #endregion

        #region ICommand
        private ICommand m_WeatherChangedCommand;

        public ICommand WeatherChangedCommand
        {
            get { return m_WeatherChangedCommand ?? new RelayCommand<string>(WeatherChanged); }
        }

        private ICommand m_PrecipitationChangedCommand;

        public ICommand PrecipitationChangedCommand
        {
            get { return m_PrecipitationChangedCommand ?? new RelayCommand<string>(PrecipitationChanged); }
        }

        private ICommand m_ChangeCloudCommand;

        public ICommand ChangeCloudCommand
        {
            get { return m_ChangeCloudCommand ?? new RelayCommand<string>(ChangeCloud); }
        }

        private ICommand m_ChangeTimeOfDayCommand;

        public ICommand ChangeTimeOfDayCommand
        {
            get { return m_ChangeTimeOfDayCommand ?? new RelayCommand<object>(ChangeTimeOfDay); }
        }

        private ICommand m_ChangeSeasonCommand;

        public ICommand ChangeSeasonCommand
        {
            get { return m_ChangeSeasonCommand ?? new RelayCommand<object>(ChangeSeason); }
        }

        private ICommand m_SetHumidityCommand;

        public ICommand SetHumidityCommand
        {
            get { return m_SetHumidityCommand ?? new RelayCommand(SetHumidity); }
        }

        private ICommand m_SetTemperatureCommand;

        public ICommand SetTemperatureCommand
        {
            get { return m_SetTemperatureCommand ?? new RelayCommand(SetTemperature); }
        }

        private ICommand m_SetCloudTypeCommand;

        public ICommand SetCloudTypeCommand
        {
            get { return m_SetCloudTypeCommand ?? new RelayCommand<string>(SetCloudType); }
        }

        private ICommand m_SetTimeCommand;

        public ICommand SetTimeCommand
        {
            get { return m_SetTimeCommand ?? new RelayCommand(SetTime); }
        }

        private ICommand m_CloudyClearCommand;

        public ICommand CloudyClearCommand
        {
            get { return m_CloudyClearCommand ?? new RelayCommand(CloudyClear); }
        }

        private ICommand m_GroundDirectionCommand;

        public ICommand GroundDirectionCommand
        {
            get { return m_GroundDirectionCommand ?? new RelayCommand<string>(GroundDirectionChanged); }
        }

        private ICommand m_GroundWindVelocityCommand;

        public ICommand GroundWindVelocityCommand
        {
            get { return m_GroundWindVelocityCommand ?? new RelayCommand<string>(GroundWindVelocityChanged); }
        }

        private ICommand m_HollowDirectionCommand;

        public ICommand HollowDirectionCommand
        {
            get { return m_HollowDirectionCommand ?? new RelayCommand<string>(HollowDirectionChanged); }
        }

        private ICommand m_HollowVelocityCommand;

        public ICommand HollowVelocityCommand
        {
            get { return m_HollowVelocityCommand ?? new RelayCommand<string>(HollowVelocityChanged); }
        }

        private ICommand m_GustSpeedCommand;

        public ICommand GustSpeedCommand
        {
            get { return m_GustSpeedCommand ?? new RelayCommand<string>(GustSpeedChanged); }
        }
        #endregion

        #region 
        public EnvironmentViewModel()
        {
            Date = PubVar.g_CurrentDate.ToString("D");
            dataUpdate.PropertyChanged += DataUpdate_PropertyChanged;
        }

        private void DataUpdate_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
           
            Weather = dataUpdate.Weather;
        }

        /// <summary>
        /// 清除所有风
        /// </summary>
        public void WindClear()
        {
            controlRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "WindClear");
            param = new List<object>();
            param.Add(Convert.ToBoolean(true));
            vdnClient.SendRequset(CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param));

            GroundWindDirection = "";
            GroundWindVelocity = "";
            HollowVelocity = "";
            HollowDirection = "";
            GustSpeed = "";

            GroundDirectionChanged("0");
            GroundWindVelocityChanged("0");
            HollowDirectionChanged("0");
            HollowVelocityChanged("0");
            GustSpeedChanged("0");
        }

        /// <summary>
        /// 风切变
        /// </summary>
        /// <param name="angle"></param>
        public void Windshear(string angle)
        {

        }

        /// <summary>
        /// 湍流
        /// </summary>
        /// <param name="angle"></param>
        public void TurbulenceChanged(string angle)
        {
            //int angletype = 0;
            //if (int.TryParse(angle, out angletype))
            //    switch (angle)
            //    {
            //        case "-60":
            //            angletype = 1;
            //            break;
            //        case "-15":
            //            angletype = 2;
            //            break;
            //        case "30":
            //            angletype = 3;
            //            break;
            //        case "75":
            //            angletype = 4;
            //            break;
            //        case "120":
            //            angletype = 5;
            //            break;
            //        default:
            //            break;
            //    }
            controlRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "Turbulence");
            param = new List<object>();
            param.Add(Convert.ToInt64(angle));
            vdnClient.SendRequset(CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param));
        }

        /// <summary>
        /// 改变地面风向
        /// </summary>
        public void GroundDirectionChanged(string num)
        {
            if (num != "")
            {
                controlRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "GroundDirection");
                param = new List<object>();
                param.Add(Convert.ToDouble(num));
                vdnClient.SendRequset(CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param));
            }
        }

        /// <summary>
        /// 改变地面风速
        /// </summary>

        public void GroundWindVelocityChanged(string num)
        {
            if (num != "")
            {
                controlRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "GroundWindVelocity");
                param = new List<object>();
                param.Add(Convert.ToDouble(num));
                vdnClient.SendRequset(CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param));
            }
        }

        /// <summary>
        /// 改变中空风向
        /// </summary>
        public void HollowDirectionChanged(string num)
        {
            if (num != "")
            {
                controlRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "HollowDirection");
                param = new List<object>();
                param.Add(Convert.ToDouble(num));
                vdnClient.SendRequset(CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param));
            }
        }

        /// <summary>
        /// 改变中空风速
        /// </summary>
        public void HollowVelocityChanged(string num)
        {
            if (num != "")
            {
                controlRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "HollowVelocity");
                param = new List<object>();
                param.Add(Convert.ToDouble(num));
                vdnClient.SendRequset(CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param));
            }
        }

        /// <summary>
        /// 改变中空风高度
        /// </summary>
        public void HollowHeightChanged(string num)
        {
            if (num != "")
            {
                controlRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "HollowHeight");
                param = new List<object>();
                param.Add(Convert.ToDouble(num));
                vdnClient.SendRequset(CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param));
            }

        }

        /// <summary>
        /// 改变阵风速度
        /// </summary>
        public void GustSpeedChanged(string num)
        {
            if (num != "")
            {
                controlRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "GustSpeed");
                param = new List<object>();
                param.Add(Convert.ToDouble(num));
                vdnClient.SendRequset(CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param));
            }
        }

        /// <summary>
        /// 清除云
        /// </summary>
        public void CloudyClear()
        {
            controlRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "CloudyClear");
            param = new List<object>();
            int Cloud_Type = 0;
            int Cloud_Density = 0;
            double Cloud_Altitude = 20000;
            param.Add(Cloud_Type);
            param.Add(Cloud_Density);
            param.Add(Cloud_Altitude);
            vdnClient.SendRequset(CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param));
        }

        public void SoliderCompleted(double num)
        {
            controlRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "VisibilityChange");
            var q = num * 1000;
            param = new List<object>();
            param.Add(q);
            vdnClient.SendRequset(CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param));
        }

        /// <summary>
        /// 天气类型修改
        /// </summary>
        /// <param name="weather"></param>
        private void WeatherChanged(string weather)
        {
            controlRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "WeatherChanged");
            param = new List<object>();
            param.Add((VDNData.CIGIWeather != weather) ? weather : "Sunny");
            var d = CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param);
            vdnClient.SendRequset(CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param));
        }

        /// <summary>
        /// 降水量修改
        /// </summary>
        /// <param name="obj"></param>
        private void PrecipitationChanged(string precipitation)
        {
            controlRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "PrecipitationChanged");
            param = new List<object>();
            var q = Int64.Parse(precipitation);
            param.Add(q);
            vdnClient.SendRequset(CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param));
            Precipitation = precipitation;
        }
        private void SetCloudType(string type)
        {
            int cloudtype = 0;
            if (int.TryParse(type, out cloudtype))
                switch (type)
                {
                    case "0":
                    case "2":
                    case "3":
                        CloudType1 = cloudtype;
                        break;
                    case "1":
                    case "8":
                        CloudType2 = cloudtype;
                        break;
                    case "6":
                    case "7":
                    case "9":
                    case "10":
                        CloudType3 = cloudtype;
                        break;
                    default:
                        break;
                }
        }

        /// <summary>
        /// 修改云
        /// </summary>
        private void ChangeCloud(string type)
        {
            int Cloud_Type = 0;
            int Cloud_Density = 0;
            double Cloud_Altitude = 0;
            switch (type)
            {
                case "cloudtype1":
                    Cloud_Type = CloudType1;
                    Cloud_Density = CloudDensity1;
                    Cloud_Altitude = CloudHeight1;
                    break;
                case "cloudtype2":
                    Cloud_Type = CloudType2;
                    Cloud_Density = CloudDensity2;
                    Cloud_Altitude = CloudHeight2;
                    break;
                case "cloudtype3":
                    Cloud_Type = CloudType3;
                    Cloud_Density = CloudDensity3;
                    Cloud_Altitude = CloudHeight3;
                    break;
                default:
                    break;
            }
            // Send Cloud Request
            controlRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "CloudChange");
            param = new List<object>();
            param.Add(Cloud_Type);
            param.Add(Cloud_Density);
            param.Add(Cloud_Altitude);
            vdnClient.SendRequset(CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param));
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// <param name="timeofday"></param>
        private void ChangeTimeOfDay(object timeofday)
        {
            TimeOfDay = (timeofday as System.Windows.Controls.ComboBoxItem).Tag.ToString();
        }
        /// <summary>
        /// 修改季节
        /// </summary>
        /// <param name="obj"></param>
        private void ChangeSeason(object season)
        {
            Season = (season as System.Windows.Controls.ComboBoxItem).Tag.ToString();
        }

        /// <summary>
        /// 设置湿度
        /// </summary>
        private void SetHumidity()
        {
            controlRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "HumiditySet");
            param = new List<object>();
            param.Add((double)Humidity);
            vdnClient.SendRequset(CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param));
        }

        /// <summary>
        /// 设置温度
        /// </summary>
        private void SetTemperature()
        {
            controlRequest = PubVar.g_SendRequestList.Find(s => s.ControlName == "TemperatureSet");
            param = new List<object>();
            param.Add((double)Temperature);
            vdnClient.SendRequset(CommonMethod.SendRequestMethod(controlRequest.SendTopic, controlRequest.SendQueue, controlRequest.SendRequestName, param));
        }
        /// <summary>
        /// 激活时间设置
        /// </summary>
        private void SetTime()
        {
            //修改季节 
            var seasonReq = PubVar.g_SendRequestList.Find(s => s.ControlName == "SeasonChange");
            seasonReq.SendParameters.Add(Season);
            vdnClient.SendRequset(CommonMethod.SendRequestMethod(seasonReq.SendTopic, seasonReq.SendQueue, seasonReq.SendRequestName, seasonReq.SendParameters));

            //修改时刻
            var dayReq = PubVar.g_SendRequestList.Find(s => s.ControlName == "DayChange");
            dayReq.SendParameters.Add(TimeOfDay);
            vdnClient.SendRequset(CommonMethod.SendRequestMethod(dayReq.SendTopic, dayReq.SendQueue, dayReq.SendRequestName, dayReq.SendParameters));

            //修改时间
            var timeReq = PubVar.g_SendRequestList.Find(s => s.ControlName == "TimeChange");
            timeReq.SendParameters.Add(Hour);
            timeReq.SendParameters.Add(Minute);
            timeReq.SendParameters.Add(Second);
            vdnClient.SendRequset(CommonMethod.SendRequestMethod(dayReq.SendTopic, dayReq.SendQueue, dayReq.SendRequestName, timeReq.SendParameters));
        }

        #endregion
    }
}
