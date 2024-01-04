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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UAM.Client.ViewModel;
using UAM.Model.Models;
using UAM.Plugin.Common;

namespace UAM.Client.Views
{
    /// <summary>
    /// Flight.xaml 的交互逻辑
    /// </summary>
    public partial class Flight : Page
    {
        private DispatcherTimer timer;
        Window win = App.Current.MainWindow;
        FlightViewModel vm = new FlightViewModel();
        public Flight()
        {
            InitializeComponent();
            this.DataContext = vm;
            this.Width = System.Windows.SystemParameters.WorkArea.Width;
            this.Height = System.Windows.SystemParameters.WorkArea.Height;
            win.Left = 0;
            win.Top = 0;
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.tbCurrent.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss dddd");
        }

        private void labMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                win.DragMove();
            }
        }
    }
}