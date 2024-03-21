using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace UAM.Model.Models
{
    public class MalfunctionModel : NotifyObject
    {
        public int ATAChapterID { get; set; }
        public string ATAChapterName { get; set; }
        public int MalfunctionID { get; set; }
        public string MalfunctionName { get; set; }
        public string ATADescription { get; set; }
        public string MalfunctionDescription { get; set; }
        public string MalMsg { get; set; }
        public bool Visible { get; set; }

        private bool _ATASelected;
        public bool ATASelected
        {
            get { return _ATASelected; }
            set { _ATASelected = value; RaisePropertyChanged("ATASelected"); }
        }
        private bool _MalfunctionSelected = false;
        public bool MalfunctionSelected
        {
            get { return _MalfunctionSelected; }
            set { _MalfunctionSelected = value; RaisePropertyChanged("MalfunctionSelected"); }
        }
        private bool _IsEnable = true;
        public bool IsEnable
        {
            get { return _IsEnable; }
            set { _IsEnable = value; RaisePropertyChanged("IsEnable"); }
        }
        [XmlIgnore]
        public object ATASelectEvent { get; set; }
        public object MalfunctionSelectEvent { get; set; }
        public object ActiveMalEvent { get; set; }

        public string VdnActiveField { get; set; }
        public string VdnEnableField { get; set; }

        public Visibility _ValueEnable = Visibility.Collapsed;
        public Visibility ValueEnable
        {
            get { return _ValueEnable; }
            set { _ValueEnable = value; RaisePropertyChanged("ValueEnable"); }
        }
        private string _SelectedValue;
        public string SelectedValue
        {
            get { return _SelectedValue; }
            set { _SelectedValue = value; RaisePropertyChanged("SelectedValue"); }
        }

    }

}
