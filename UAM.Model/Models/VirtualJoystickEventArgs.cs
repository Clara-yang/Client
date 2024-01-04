using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAM.Model.Models
{
    public class VirtualJoystickEventArgs : EventArgs
    {
        public double Angle { get; set; }
        public double Distance { get; set; }
        public double leftMovedX { get; set; }
        public double leftMovedY { get; set; }
        public double righMovedX { get; set; }
        public double righMovedY { get; set; }
    }
}
