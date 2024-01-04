using System;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UAM.Model.Models;

namespace UAM.Client.Views.CustomGraphics
{
    /// <summary>
    /// JoystickLeft.xaml 的交互逻辑
    /// </summary>
    public partial class JoystickLeft : UserControl
    {
        /// <summary>委托抓住操纵手柄状态更改的数据</summary>
        /// <param name="sender"></param>
        /// <param name="args">保留角度和距离的新值</param>
        public delegate void OnScreenJoystickEventHandler(JoystickLeft sender, VirtualJoystickEventArgs args);

        /// <summary>无数据操纵杆事件的委托</summary>
        /// <param name="sender"></param>
        public delegate void EmptyJoystickEventHandler(JoystickLeft sender);

        /// <summary>角度和距离的新值</summary>
        public event OnScreenJoystickEventHandler Moved;

        /// <summary>只要操纵手柄移动，就会触发此事件</summary>
        public event EmptyJoystickEventHandler Released;

        /// <summary>一旦松开操纵手柄并重置其位置，就会触发此事件</summary>
        public event EmptyJoystickEventHandler Captured;

        private Point _startPosLeft; //左边摇杆鼠标按下的位置 
        private readonly Storyboard centerKnobLeft;

        public JoystickLeft()
        {
            InitializeComponent();

            KnobLeft.MouseLeftButtonDown += KnobLeft_MouseLeftButtonDown;
            KnobLeft.MouseLeftButtonUp += KnobLeft_MouseLeftButtonUp;
            KnobLeft.MouseMove += KnobLeft_MouseMove;
            centerKnobLeft = KnobLeft.Resources["CenterKnobLeft"] as Storyboard;
        }

        /// <summary>
        /// 左边摇杆鼠标左键按下时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KnobLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_startPosLeft.X == 0 && _startPosLeft.Y == 0)
            {
                _startPosLeft = e.GetPosition(BaseLeft);
            }
            Captured?.Invoke(this);
            KnobLeft.CaptureMouse(); // 捕获该元素的鼠标
            centerKnobLeft.Stop();
        }

        // 鼠标移动事件
        private void KnobLeft_MouseMove(object sender, MouseEventArgs e)
        {
            if (!KnobLeft.IsMouseCaptured) return;

            Point newPos = e.GetPosition(BaseLeft);
            Point deltaPos = new Point(newPos.X - _startPosLeft.X, newPos.Y - _startPosLeft.Y);

            // Math.Sqrt:平方根；Math.Atan2:反切(正切的倒数)
            double distance = Math.Round(Math.Sqrt(deltaPos.X * deltaPos.X + deltaPos.Y * deltaPos.Y) / 100 * 100);
            if (distance <= 100)
            {
                knobPositionLeft.X = deltaPos.X;
                knobPositionLeft.Y = deltaPos.Y;

                Moved?.Invoke(this, new VirtualJoystickEventArgs { leftMovedX = deltaPos.X, leftMovedY = deltaPos.Y });
            }
        }

        private void KnobLeft_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            KnobLeft.ReleaseMouseCapture();
            centerKnobLeft.Begin();
        }

        private void centerKnobLeft_Completed(object sender, EventArgs e)
        {
            Released?.Invoke(this);
        }
    }
}
