using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAM.Model.Models;
using UAM.Plugin.Common;
using UAM.Plugin;
using System.ComponentModel.Design;
using GalaSoft.MvvmLight.Command;
using static UAM.Model.Models.VDNModel;
using UAM.Plugin.NetVDN;
using System.ComponentModel;
using System.Windows.Threading;
using static Microsoft.Expression.Prototyping.Data.Serializer;
using System.Windows.Input;
using UAM.Client.Views.CustomGraphics;
using System.Windows.Controls;
using System.Windows;
using UAM.Client.Common;
using Csf.Imets.ToolCore.Vdn;

namespace UAM.Client.ViewModel
{
    public class MalfunctionViewModel : ViewModelBase
    {
        VDNData vdnClient = new VDNData();
        private DispatcherTimer timer;
        VDNDataUpadate dataUpdate = VDNDataUpadate.Instance;
        MalfunctionModel selectedMal = new MalfunctionModel();
        List<SendRequest> requests = new List<SendRequest>();
        object subValue = "";
        string vdnField = string.Empty;
        string keyValue = string.Empty;

        #region property
        public string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; RaisePropertyChanged("Description"); }
        }
        private List<MalfunctionModel> _Chapters = new List<MalfunctionModel>();
        public List<MalfunctionModel> Chapters
        {
            get { return _Chapters; }
            set { Set(ref _Chapters, value); }
        }
        private List<MalfunctionModel> _Malfunctions = new List<MalfunctionModel>();
        public List<MalfunctionModel> Malfunctions
        {
            get { return _Malfunctions; }
            set { Set(ref _Malfunctions, value); }
        }
        #endregion

        #region ICommand
        private RelayCommand<MalfunctionModel> _ATAClickCommand;
        public RelayCommand<MalfunctionModel> ATAClickCommand
        {
            get
            {
                return new RelayCommand<MalfunctionModel>(e =>
                {
                    ATAClickEvent(e);
                });
            }
            set { _ATAClickCommand = value; }
        }
        private RelayCommand<MalfunctionModel> _MalclickCommand;
        public RelayCommand<MalfunctionModel> MalClickCommand
        {
            get
            {
                return new RelayCommand<MalfunctionModel>(e =>
                {
                    MalClickEvent(e);
                });
            }
            set { _MalclickCommand = value; }
        }
        private RelayCommand<MalfunctionModel> _ActiveMalCommand;
        public RelayCommand<MalfunctionModel> ActiveMalCommand
        {
            get
            {
                return new RelayCommand<MalfunctionModel>(e =>
                {
                    ActiveMalEvent(e);
                });
            }
            set { _ActiveMalCommand = value; }
        }
        #endregion

        public MalfunctionViewModel()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            timer.Tick += new EventHandler(RefreshATAState);
            timer.Start();

            dataUpdate.PropertyChanged += DataUpdate_PropertyChanged;

            Chapters = PubVar.g_MalfunctionList.GroupBy(x => x.ATAChapterID).Select(x => x.First()).ToList();

            foreach (var chapter in Chapters)
            {
                chapter.ATASelectEvent = ATAClickCommand;
            }
        }

        /// <summary>
        /// 定时刷新故障是否被激活，激活后对应的ATA被点亮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshATAState(object sender, EventArgs e)
        {
            foreach (var item in Malfunctions)
            {
                item.ATASelected = Malfunctions.Exists(s => s.MalfunctionSelected == true && s.ATAChapterID == item.ATAChapterID) ? true : false;
            }

            if (vdnField != "")
            {
                subValue = CommonMethod.GetPropertiesValue(dataUpdate, vdnField);
                selectedMal.MalfunctionSelected = bool.Parse(subValue.ToString());
                if (selectedMal.MalfunctionSelected)
                {
                    selectedMal.ValueEnable = Visibility.Visible;
                    selectedMal.SelectedValue = keyValue.ToString();
                }
                else
                {
                    selectedMal.ValueEnable = Visibility.Collapsed;
                }
            }

            if (Malfunctions.Exists(s => s.MalfunctionSelected == true))
            {
                var selected = Malfunctions.Find(a => a.MalfunctionSelected == true);
                Malfunctions.Remove(selected);
                foreach (var item in Malfunctions)
                {
                    item.IsEnable = false;
                }
                Malfunctions.Add(selected);
            }
            else
            {
                foreach (var item in Malfunctions)
                {
                    item.IsEnable = true;
                }
            }
        }

        /// <summary>
        /// 刷新subscribe的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataUpdate_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        /// <summary>
        /// 点击ATA按钮，刷新具体故障，加载描述
        /// </summary>
        /// <param name="param"></param>
        public void ATAClickEvent(MalfunctionModel param)
        {
            Malfunctions = PubVar.g_MalfunctionList.FindAll(s => s.ATAChapterID == param.ATAChapterID);
            Description = Malfunctions[0].ATADescription;
            foreach (var mal in Malfunctions)
            {
                mal.MalfunctionSelectEvent = MalClickCommand;
                mal.ActiveMalEvent = ActiveMalCommand;
            }
        }

        /// <summary>
        /// 点击故障按钮，刷新描述
        /// </summary>
        /// <param name="param"></param>
        public void MalClickEvent(MalfunctionModel param)
        {
            selectedMal = Malfunctions.Find(s => s.MalfunctionName == param.MalfunctionName);
            Description = selectedMal.MalfunctionDescription;

            requests = PubVar.g_SendRequestList.FindAll(a => a.ControlName == param.MalfunctionName).OrderBy(x => x.ParameterIndex).ToList();
        }

        /// <summary>
        /// 点击故障右下方的按钮激活故障
        /// </summary>
        /// <param name="param"></param>
        public void ActiveMalEvent(MalfunctionModel param)
        {
            var returnValue = CommonMethod.SendRequest_Mal(requests);
            if (returnValue.Count > 1)
            {
                keyValue = returnValue[0];
                vdnField = returnValue[1];
            }
            else
            {
                vdnField = returnValue.FirstOrDefault();
            }
        }

    }
}
