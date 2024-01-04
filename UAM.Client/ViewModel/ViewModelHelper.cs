using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace UAM.Client.ViewModel
{
    public class ViewModelHelper: ViewModelBase
    {
        public MainViewModel mainVm { get; set; }

        /// <summary>
        /// 加载下一个页面
        /// </summary>
        /// <param name="page"></param>
        public void NextPage(Page page)
        {
            mainVm = MainViewModel.mainVM;
            mainVm.MainFrame.Content = page; 
            mainVm.MainFrame.Width = page.Width;
            mainVm.MainFrame.Height = page.Height;
        }
         
    }
}
