using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UAM.Model.Models
{
    [Serializable]
    public class ButtonModel : NotifyObject
    {
        public string Position { get; set; }

        public string ButtonName { get; set; }

        public string Text { get; set; }

        public string Parameter { get; set; }

        public bool Visible { get; set; }

        [XmlIgnore]
        private bool _IsChecked;
        public bool IsChecked
        {
            get { return _IsChecked; }
            set { _IsChecked = value; RaisePropertyChanged("IsChecked"); }
        }

        public object BtnClickCommand { get; set; }

    }
}
