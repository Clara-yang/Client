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
    public class ADSViewModel : ViewModelBase
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

        #region Porperty
        public double _CAS;
        public double CAS
        {
            get { return _CAS; }
            set { _CAS = value; RaisePropertyChanged("CAS"); }
        }

        public double _TAS;
        public double TAS
        {
            get { return _TAS; }
            set { _TAS = value; RaisePropertyChanged("TAS"); }
        }

        public double _BaroAlt;
        public double BaroAlt
        {
            get { return _BaroAlt; }
            set { _BaroAlt = value; RaisePropertyChanged("BaroAlt"); }
        }

        public bool _CASAvailability;
        public bool CASAvailability
        {
            get { return _CASAvailability; }
            set { _CASAvailability = value; RaisePropertyChanged("CASAvailability"); }
        }

        public bool _TASAvailability;
        public bool TASAvailability
        {
            get { return _TASAvailability; }
            set { _TASAvailability = value; RaisePropertyChanged("TASAvailability"); }
        }

        public bool _BaroAltAvailability;
        public bool BaroAltAvailability
        {
            get { return _BaroAltAvailability; }
            set { _BaroAltAvailability = value; RaisePropertyChanged("BaroAltAvailability"); }
        }
        #endregion

        public void Reset(string param)
        {
            if (param == "ADSBias")
            {
                CAS = 0;
                TAS = 0;
                BaroAlt = 0;
            }
            if (param == "ADSfunctionAvailability")
            {
                CASAvailability = true;
                TASAvailability = true;
                BaroAltAvailability = true;
            }
        }

        public void SetUp(string param)
        {
            var sendProp = new List<object>();
            var reqs = PubVar.g_SendRequestList.FindAll(s => s.ControlName == param).OrderBy(s => s.ParameterIndex).ToList();

            if (param == "ADSBias")
            {
                sendProp.Add(CAS);
                sendProp.Add(TAS);
                sendProp.Add(BaroAlt);
            }
            if (param == "ADSfunctionAvailability")
            {
                sendProp.Add(CASAvailability);
                sendProp.Add(TASAvailability);
                sendProp.Add(BaroAltAvailability);
            }

            CommonMethod.SendRequest_Sensor(reqs, sendProp);

        }
    }
}
