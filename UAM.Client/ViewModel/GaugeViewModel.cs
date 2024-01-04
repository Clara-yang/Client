using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAM.Model;

namespace UAM.Client.ViewModel
{
    public class GaugeViewModel : NotifyObject
    {  
        public GaugeViewModel()
        {
            Angle = 0;
            Value = 0;
        }

        private int _angle;
        public int Angle
        {
            get { return _angle; }
            private set
            {
                if (value != _angle)
                {
                    _angle = value;
                    RaisePropertyChanged("Angle");
                }
            }
        }

        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                if (value != _value && value >= 0 && value <= 180)
                {
                    _value = value;
                    Angle = value - 90;
                    RaisePropertyChanged("Value");
                }
            }
        }
    }
}
