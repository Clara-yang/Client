using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAM.Model.Models
{
    public class CommunicationModel
    { 
        public string Host_IP { get; set; }
        public string Host_User { get; set; }
        public string Host_Password { get; set; } 
        public string Host_Exec_Cmd { get; set; }
        public string Host_Stop_Cmd { get; set; }
        public string Vdn_Port { get; set; } 

        public string Visual_Type { get; set; } 
        public string RunP3D { get; set; }
        public string P3DPath { get; set; }
    }
}
