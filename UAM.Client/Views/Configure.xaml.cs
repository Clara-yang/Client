using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UAM.Client.ViewModel;
using UAM.Model.Models;

namespace UAM.Client.Views
{
    /// <summary>
    /// RouteSelect.xaml 的交互逻辑
    /// </summary> 
    public partial class Configure : Page
    {
        Window win = App.Current.MainWindow;
        ConfigureViewModel vm = new ConfigureViewModel();
        public Configure()
        {
            InitializeComponent();
            this.DataContext = vm;
            this.Width = 1450;
            this.Height = 900;
        }

        private void lbTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                win.DragMove();
            }
        }
    }
}
