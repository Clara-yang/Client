using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UAM.Model.Models
{
    public class LoginModel : NotifyObject
    {
        public List<String> UserList { get; set; }
        public ICommand LoginVerifyCommand { get; set; }
        public ICommand SendCommand { get; set; } 

        private string account;
        public string Account
        {
            get => account;
            set
            {
                account = value;
                RaisePropertyChanged("Account");
            }
        }
        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                RaisePropertyChanged("Account");
            }
        }
        public Visibility progressState;
        public Visibility ProgressState
        {
            get { return progressState; }
            set
            {
                progressState = value;
                RaisePropertyChanged("ProgressState");
            }
        }
        public string progressText;
        public string ProgressText
        {
            get { return progressText; }
            set
            {
                progressText = value;
                RaisePropertyChanged("ProgressText");
            }
        }
        private double progressBarValue;
        public double ProgressBarValue
        {
            get { return progressBarValue; }
            set
            {
                progressBarValue = value;
                RaisePropertyChanged("ProgressBarValue");
            }
        } 
          

    }
}
