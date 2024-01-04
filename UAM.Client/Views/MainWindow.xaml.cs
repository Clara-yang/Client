using CommonServiceLocator;
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
using System.Xml.Serialization;
using UAM.Client.ViewModel;
using UAM.Client.Views;
using UAM.Plugin;

namespace UAM.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        VDNConnectHelper vdnHelper = new VDNConnectHelper();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            vdnHelper.StopProgram();
            vdnHelper.StopProcess("AircraftSimulation");
            Application.Current.Shutdown();
            Environment.Exit(0);
        }
    }
}