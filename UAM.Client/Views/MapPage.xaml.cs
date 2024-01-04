﻿using System;
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
    /// MapPage.xaml 的交互逻辑
    /// </summary>
    public partial class MapPage : Page
    {
        MapViewModel vm = new MapViewModel();
        public MapPage()
        {
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}
