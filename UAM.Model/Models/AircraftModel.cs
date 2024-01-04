using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace UAM.Model.Models
{
    [Serializable]
    public class AircraftModel
    { 
        public string AircraftName { get; set; }
        public string Configuration { get; set; }
        public string Publisher { get; set; }
        public string ZeroWeight { get; set; }
        public string Usage { get; set; }
        public string Range { get; set; }
        public string MaxCruiseSpeedkts { get; set; }
        public string MaxCruiseSpeedkmh { get; set; }
        public string MaxAltitudeft { get; set; }
        public string MaxAltitudem { get; set; }
        public string Load { get; set; }
        public string PilotSeat { get; set; }
        public string AircraftCode { get; set; }
        public bool IsAircraftDefault { get; set; }
        public bool Visible { get; set; }
    }
}