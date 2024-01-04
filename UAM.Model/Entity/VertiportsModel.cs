using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAM.Model.Models
{
    public class VertiportsModel
    {
        public string id { get; set; }
        public string city { get; set; }
        public string vertiport_name { get; set; }
        public string pad_name { get; set; }
        public string aircraft_name { get; set; }
        public string vertiport_type { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string altitude { get; set; }
        public string pad_heading { get; set; }
    }
}
