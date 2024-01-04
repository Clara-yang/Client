using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UAM.Model.Models
{
    [Serializable]
    public class RouteModel
    {  
        public int RouteId { get; set; }
        public string RouteName { get; set; }  
        public string DepartureId { get; set; }  
        public string DepartureCity { get; set; }
        public string DepartureVertiport { get; set; }
        public string DeparturePad { get; set; }
        public string DestinationId { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationVertiport { get; set; }
        public string DestinationPad { get; set; } 
        public string Distance { get; set; } 
        public string Destination { get; set; } 
        public string Interval { get; set; }
        public string Description { get; set; }
        public bool IsRouteDefault { get; set; }
        public bool Visible { get; set; }
    }
}
