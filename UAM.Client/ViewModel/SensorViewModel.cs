using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UAM.Client.Views.CustomGraphics;
using UAM.Model.Models;

namespace UAM.Client.ViewModel
{
    public class SensorViewModel : ViewModelBase
    {
        public UIElement functionPage;
        public UIElement FunctionPage
        {
            get { return functionPage; }
            set { Set(ref functionPage, value); }
        }

        public ObservableCollection<PageModel> pages;
        public ObservableCollection<PageModel> Pages
        {
            get { return pages; }
            set { Set(ref pages, value); }
        }

        private RelayCommand<PageModel> menuCommand;
        public RelayCommand<PageModel> MenuCommand
        {
            get
            {
                return new RelayCommand<PageModel>(e =>
                {
                    MenuEvent(e);
                });
            }
            set { menuCommand = value; }
        }

        public Page MainPage { get; set; }

        public SensorViewModel()
        {
            Pages = new ObservableCollection<PageModel>();

            LoadPages();

            foreach (var page in Pages)
            {
                if (page == Pages.FirstOrDefault())
                {
                    MenuEvent(page);
                }
                page.SelectEvent = MenuCommand;
            }
        }

        public void LoadPages()
        {
            Pages.Add(new PageModel("INS",PackIconKind.FormatItalic , new INS()));
            Pages.Add(new PageModel("ADS", PackIconKind.FormatColorText, new ADS()));
            Pages.Add(new PageModel("RA", PackIconKind.FormatSize, new RA()));
        }

        public void MenuEvent(PageModel obj)
        {
            functionPage = new Frame();
            MainPage = obj.PageView;
            FunctionPage = MainPage;
        }

    }
}
