using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UAM.Model.Models
{
    public class ConfigureModel : NotifyObject
    {
        public List<string> DawnDuskList { get; set; }
        public List<string> SeasonList { get; set; }
        public string Distance { get; set; }
        public string Interval { get; set; }
        public string ZeroWeight { get; set; }
        public string Usage { get; set; }
        public string MaxCruiseSpeedkts { get; set; }
        public string MaxCruiseSpeedkmh { get; set; }
        public string MaxAltitudeft { get; set; }
        public string MaxAltitudem { get; set; }
        public string PilotSeat { get; set; }
        public string Load { get; set; }
        public bool IsAircraftDefault { get; set; }
        public string DepartureCity { get; set; }
        public string DepartureVertiport { get; set; }
        public string DeparturePad { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationVertiport { get; set; }
        public string DestinationPad { get; set; }
        public string RouteDescription { get; set; }
        public bool IsRouteDefault { get; set; }

        private string nowTime;
        public string NowTime
        {
            get { return nowTime; }
            set
            {
                nowTime = value;
                RaisePropertyChanged("NowTime");
            }
        }

        private string yearMonth;
        public string YearMonth
        {
            get { return yearMonth; }
            set
            {
                yearMonth = value;
                RaisePropertyChanged("YearMonth");
            }
        }



    }
}
