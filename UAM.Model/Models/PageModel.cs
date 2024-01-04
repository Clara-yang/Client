using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;

namespace UAM.Model.Models
{
    [Serializable]
    public class PageModel : NotifyObject
    {
        public PageModel() { }
        public PageModel(string displayName, PackIconKind icon, Page pageView)
        {
            DisplayName = displayName;
            Icon = icon;
            PageView = pageView;
        }

        public string PageName { get; set; }
        public string DisplayName { get; set; }
        public bool IsSelected { get; set; }

        [XmlIgnore]
        public Page PageView { get; set; }
        public PackIconKind Icon { get; set; }
        [XmlIgnore]
        public object SelectEvent { get; set; }
    }
}
