using GalaSoft.MvvmLight;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UAM.Client.Views;

namespace UAM.Client.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public static MainViewModel mainVM { get; set; }

        public MainViewModel()
        {
            mainVM = this;
            InitPage();
        }
        // 主标签页(登录界面)
        private Page _mainPage;
        public Page MainPage
        {
            get { return _mainPage; }
            set { Set(ref _mainPage, value); }
        }
        // 程序入口主页面 
        private Frame _mainFrame;
        public Frame MainFrame
        {
            get { return _mainFrame; }
            set { Set(ref _mainFrame, value); }
        }
         
        /// <summary>
        /// 初始化页面
        /// </summary>
        public void InitPage()
        {
            _mainFrame = new Frame();
            MainPage = new Login();
            MainFrame.Content = MainPage;
            MainFrame.Width = MainPage.Width;
            MainFrame.Height = MainPage.Height;
        }

    }
}
