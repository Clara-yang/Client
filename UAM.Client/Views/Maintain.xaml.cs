using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UAM.Client.ViewModel;
using UAM.Model.Models;

namespace UAM.Client.Views
{
    /// <summary>
    /// Maintain.xaml 的交互逻辑
    /// </summary>
    public partial class Maintain : Page
    {
        MaintainViewModel vm = new MaintainViewModel();
        public Maintain()
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        public void onScreenJoystickLeft_Moved(CustomGraphics.JoystickLeft sender, VirtualJoystickEventArgs args)
        {
            var lX = Convert.ToInt32(args.leftMovedX);
            var lY = Convert.ToInt32(args.leftMovedY);

            vm.GetLeftStickValue(lX, lY);
        }

        public void onScreenJoystickRight_Moved(CustomGraphics.JoystickRight sender, VirtualJoystickEventArgs args)
        {
            var rX = Convert.ToDouble(args.righMovedX);
            var rY = Convert.ToDouble(args.righMovedY);

            vm.GetRightStickValue(rX, rY);
        }

    }
}
