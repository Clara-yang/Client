using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UAM.Client.Views.CustomGraphics
{
    /// <summary>
    /// NumricKeypad.xaml 的交互逻辑
    /// </summary>
    public partial class NumricKeypad : Window
    {
        public NumricKeypad()
        {
            InitializeComponent();
        }
        public bool PlusMinuxStaus { get; set; }

        public string Min
        {
            get { return (string)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Min.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(string), typeof(NumricKeypad), new PropertyMetadata("0", (d, e) =>
            {
                NumricKeypad control = d as NumricKeypad;

                if (control == null) return;

                string config = e.NewValue as string;

                control.minlbl.Content = config;
            }));

        public string Max
        {
            get { return (string)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(string), typeof(NumricKeypad), new PropertyMetadata("0", (d, e) =>
            {
                NumricKeypad control = d as NumricKeypad;

                if (control == null) return;

                string config = e.NewValue as string;

                control.maxlbl.Content = config;
            }));


        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Unit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(string), typeof(NumricKeypad), new PropertyMetadata("", (d, e) =>
            {
                NumricKeypad control = d as NumricKeypad;

                if (control == null) return;

                string config = e.NewValue as string;

                control.unitlbl.Content = "(" + config + ")";
            }));


        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(NumricKeypad), new PropertyMetadata("", (d, e) =>
            {
                NumricKeypad control = d as NumricKeypad;

                if (control == null) return;

                string config = e.NewValue as string;

                control.titlelbl.Content = config;
            }));

        public string InputText
        {
            get { return (string)GetValue(InputTextProperty); }
            set { SetValue(InputTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputTextProperty =
            DependencyProperty.Register("InputText", typeof(string), typeof(NumricKeypad), new PropertyMetadata("", (d, e) =>
            {
                NumricKeypad control = d as NumricKeypad;

                if (control == null) return;

                string config = e.NewValue as string;

                control.inputtxttb.Text = config;
                control.inputtxttb.SelectionStart = control.inputtxttb.Text.Length;
            }));



        public String ValueType
        {
            get { return (String)GetValue(ValueTypeProperty); }
            set { SetValue(ValueTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueTypeProperty =
            DependencyProperty.Register("ValueType", typeof(String), typeof(NumricKeypad), new PropertyMetadata("", (d, e) =>
            {
                NumricKeypad control = d as NumricKeypad;

                if (control == null) return;

                string config = e.NewValue as string;
                if (config == "Int64")
                {
                    control.backbtn.IsEnabled = false;
                }
                else
                {
                    control.backbtn.IsEnabled = true;
                }
            }));



        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;

            if (button == null) return;

            Debug.WriteLine(button.Content);

            string content = button.Content.ToString();

            if (content == "Enter")
            {
                if (Outofrange())
                {
                    outofrangelbl.Visibility = Visibility.Visible;
                }
                else
                {
                    this.DialogResult = true;
                    this.InputText = this.inputtxttb.Text;
                    this.Close();
                }
            }
            else if (content == "Cancel")
            {
                this.DialogResult = false;
                this.Close();
            }
            else if (content == "Back")
            {
                string str = this.InputText;
                if (str.Length >= 1)
                {
                    this.InputText = str.Substring(0, str.Length - 1);
                }
                else
                {
                    this.InputText = "";
                }
                if (Outofrange())
                {
                    outofrangelbl.Visibility = Visibility.Visible;
                }
                else
                {
                    outofrangelbl.Visibility = Visibility.Hidden;
                }
            }
            else if (content == "Clear")
            {
                this.InputText = "";
                outofrangelbl.Visibility = Visibility.Hidden;
            }
            else if (content == "+/-")
            {
                bool localb;
                localb = PlusMinuxStaus;
                outofrangelbl.Visibility = Visibility.Hidden;
                if (localb)
                {
                    string str = this.InputText;
                    this.InputText = str.Substring(1, str.Length - 1);
                    PlusMinuxStaus = false;
                }
                else
                {
                    string str = this.InputText;
                    this.InputText = "-" + str;
                    PlusMinuxStaus = true;
                }
            }
            else
            {
                outofrangelbl.Visibility = Visibility.Hidden;
                string val = button.Content.ToString();
                this.inputtxttb.Focus();

                this.InputText += val;
            }
        }

        private bool Outofrange()
        {
            double min = 0, max = 0, curval = 0;
            bool outofrange = true;
            try
            {
                min = Convert.ToDouble(Min);
                max = Convert.ToDouble(Max);
                curval = Convert.ToDouble(InputText);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed to Convert min max curval value: " + ex.Message);
            }
            if (min <= curval && curval <= max)
            {
                outofrange = false;
            }
            return outofrange;
        }
        private void tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");

            e.Handled = re.IsMatch(e.Text);
        }
    }
}
