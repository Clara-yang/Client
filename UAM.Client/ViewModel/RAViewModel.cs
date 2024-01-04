using Csf.Imets.ToolCore.Vdn;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using UAM.Client.Common;
using UAM.Plugin.Common;

namespace UAM.Client.ViewModel
{
    public class RAViewModel : ViewModelBase
    {
        #region ICommand
        private ICommand _ResetCommand;
        public ICommand ResetCommand
        {
            get { return _ResetCommand ?? new RelayCommand<string>(Reset); }
        }

        private ICommand _SetUpCommand;
        public ICommand SetUpCommand
        {
            get { return _SetUpCommand ?? new RelayCommand<string>(SetUp); }
        }
        #endregion

        #region Property
        public double _RABias;
        public double RABias
        {
            get { return _RABias; }
            set { _RABias = value; RaisePropertyChanged("RABias"); }
        }

        public bool _Valid;
        public bool Valid
        {
            get { return _Valid; }
            set { _Valid = value; RaisePropertyChanged("Valid"); }
        }
        #endregion

        public void Reset(string param)
        {
            if (param == "RABiasAndValid")
            {
                RABias = 0;
                Valid = true;
            }
        }

        public void SetUp(string param)
        {
            var sendProp = new List<object>();
            var reqs = PubVar.g_SendRequestList.FindAll(s => s.ControlName == param).OrderBy(s => s.ParameterIndex).ToList();

            if (param == "RABiasAndValid")
            {
                sendProp.Add(RABias);
                sendProp.Add(Valid);
            }

            CommonMethod.SendRequest_Sensor(reqs, sendProp);

        }
    }
}
