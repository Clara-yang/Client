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
    /// JoystickRight.xaml 的交互逻辑
    /// </summary>
    public partial class JoystickRight : UserControl
    {
        /// <summary>委托抓住操纵手柄状态更改的数据</summary>
        /// <param name="sender"></param>
        /// <param name="args">保留角度和距离的新值</param>
        public delegate void OnScreenJoystickEventHandler(JoystickRight sender, VirtualJoystickEventArgs args);

        /// <summary>无数据操纵杆事件的委托</summary>
        /// <param name="sender"></param>
        public delegate void EmptyJoystickEventHandler(JoystickRight sender);

        /// <summary>角度和距离的新值</summary>
        public event OnScreenJoystickEventHandler Moved;

        /// <summary>只要操纵手柄移动，就会触发此事件</summary>
        public event EmptyJoystickEventHandler Released;

        /// <summary>一旦松开操纵手柄并重置其位置，就会触发此事件</summary>
        public event EmptyJoystickEventHandler Captured;

        private Point _startPosRight; //右边摇杆鼠标按下的位置 
        private readonly Storyboard centerKnobRight;

        public JoystickRight()
        {
            InitializeComponent();

            KnobRight.MouseLeftButtonDown += KnobRight_MouseLeftButtonDown;
            KnobRight.MouseLeftButtonUp += KnobRight_MouseLeftButtonUp;
            KnobRight.MouseMove += KnobRight_MouseMove;
            centerKnobRight = KnobRight.Resources["CenterKnobRight"] as Storyboard;
        }

        /// <summary>
        /// 右边摇杆鼠标左键按下时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KnobRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_startPosRight.X == 0 && _startPosRight.Y == 0)
            {
                _startPosRight = e.GetPosition(BaseRight);
            }
            Captured?.Invoke(this);
            KnobRight.CaptureMouse(); // 捕获该元素的鼠标
            centerKnobRight.Stop();
        }

        private void KnobRight_MouseMove(object sender, MouseEventArgs e)
        {
            if (!KnobRight.IsMouseCaptured) return;

            Point newPos = e.GetPosition(BaseRight);
            Point deltaPos = new Point(newPos.X - _startPosRight.X, newPos.Y - _startPosRight.Y);

            // Math.Sqrt:平方根；Math.Atan2:反切(正切的倒数)
            double distance = Math.Round(Math.Sqrt(deltaPos.X * deltaPos.X + deltaPos.Y * deltaPos.Y) / 100 * 100);
            if (distance <= 100)
            {
                knobPositionRight.X = deltaPos.X;
                knobPositionRight.Y = deltaPos.Y;

                Moved?.Invoke(this, new VirtualJoystickEventArgs { righMovedX = deltaPos.X, righMovedY = deltaPos.Y });
            }
        }

        private void KnobRight_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            KnobRight.ReleaseMouseCapture();
            centerKnobRight.Begin();
        }

        private void centerKnobRight_Completed(object sender, EventArgs e)
        {
            Released?.Invoke(this);
        }
    }
}
