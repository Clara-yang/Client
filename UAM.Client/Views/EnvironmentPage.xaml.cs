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
using UAM.Client.ViewModel;

namespace UAM.Client.Views
{
    /// <summary>
    /// Environment.xaml 的交互逻辑
    /// </summary>
    public partial class EnvironmentPage : Page
    {
        public EnvironmentViewModel vm = new EnvironmentViewModel();
        public EnvironmentPage()
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        private void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            vm.SoliderCompleted(slider.Value);
        }

        private void sd_wind_height_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            vm.HollowHeightChanged(sd_wind_height.Value.ToString());
        }

        private void clearWind_Click(object sender, RoutedEventArgs e)
        {
            turbulence.SelectedIndex = 0;
            sd_wind_height.Value = sd_wind_height.Minimum;
            vm.WindClear();
        }

        private void turbulence_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbi = (ComboBoxItem)(sender as ComboBox).SelectedItem;
            string selectedText = cbi.Content.ToString();
            vm.TurbulenceChanged(selectedText);
        }

        private void tb_IncreasedVisibility_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                vm.SoliderCompleted(slider.Value);
            }
        }
    }
}
