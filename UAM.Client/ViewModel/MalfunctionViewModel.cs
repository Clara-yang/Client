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
using static Csf.Imets.ToolCore.Common.Controls.UICustomEventEditor;

namespace UAM.Client.ViewModel
{
    public class MalfunctionViewModel : ViewModelBase
    {
        private DispatcherTimer timer;
        VDNDataUpadate dataUpdate = VDNDataUpadate.Instance;
        MalfunctionModel selectedMal = new MalfunctionModel();
        List<SendRequest> requests = new List<SendRequest>();

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
        private RelayCommand<MouseButtonEventArgs> _ActiveMalCommand;
        public RelayCommand<MouseButtonEventArgs> ActiveMalCommand
        {
            get
            {
                return new RelayCommand<MouseButtonEventArgs>(e =>
                {
                    ActiveMalEvent(selectedMal);
                    e.Handled = true;
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

                var vdnActive = CommonMethod.GetPropertiesValue(dataUpdate, item.VdnActiveField);
                item.MalfunctionSelected = bool.Parse(vdnActive.ToString());

                if (item.VdnEnableField != "" && item.VdnEnableField != null)
                {
                    var vdnEnable = CommonMethod.GetPropertiesValue(dataUpdate, item.VdnEnableField);
                    item.IsEnable = bool.Parse(vdnEnable.ToString());
                    if (item.MalfunctionSelected)
                    {
                        item.ActiveEnable = Visibility.Visible;
                        item.ValueEnable = Visibility.Visible;
                    }
                    else
                    {
                        item.ActiveEnable = Visibility.Collapsed;
                        item.ValueEnable = Visibility.Collapsed;
                    }
                }
                else
                {
                    item.ActiveEnable = item.MalfunctionSelected ? Visibility.Visible : Visibility.Collapsed;
                }

            }
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
            param.ActiveEnable = Visibility.Visible;

            requests = PubVar.g_SendRequestList.FindAll(a => a.ControlName == param.MalfunctionName).OrderBy(x => x.ParameterIndex).ToList();
        }

        /// <summary>
        /// 点击故障右下方的按钮激活故障
        /// </summary>
        /// <param name="param"></param>
        public void ActiveMalEvent(MalfunctionModel param)
        {
            var returnValue = CommonMethod.SendRequest_Mal(requests);
            string keyValue = returnValue.FirstOrDefault();
            param.SelectedValue = keyValue;
        }

    }
}
