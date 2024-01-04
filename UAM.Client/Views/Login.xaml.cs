using Csf.Imets.ToolCore.Vdn;
using System;
using System.Collections.Generic;
using System.IO;
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
using UAM.Client.ViewModel;

namespace UAM.Client.Views
{
    /// <summary>
    /// Main.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Page
    {
        Window win = App.Current.MainWindow;
        LoginViewModel vm = new LoginViewModel();
        public Login()
        {
            InitializeComponent();
            this.DataContext = vm;
            this.Width = double.NaN;
            this.Height = double.NaN;

            this.Height = 450;
            this.Width = 800;
            win.ResizeMode = ResizeMode.NoResize;
        }

        private void PwdBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PwdBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }


    }
}

