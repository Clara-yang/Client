using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UAM.Plugin.Common
{
    public class JoyControl
    {
        public static SharpDX.DirectInput.Joystick curJoystick;

        public double Throttle;///  x=>Throttle

        public double Axsix_y;

        public double Axsix_z;

        public double Axsix_X;///  =>X

        public bool[] buttons;///  =>X

        public bool AP_Button;

        static Thread joyThread;

        public void ThreadJoystick()
        {
            joyThread = new Thread(Joystick_Initialize_find);

            joyThread.Start();
        }

        public void StopThreadJoystick()
        {
            if (joyThread != null)
            {
                joyThread.Abort();
            }
        }

        public void Joystick_Initialize_find()
        {
            var dirInput = new SharpDX.DirectInput.DirectInput();
            var typeJoystick = SharpDX.DirectInput.DeviceType.FirstPerson;
            var allDevices = dirInput.GetDevices();
            bool isGetJoystick = false;

            foreach (var item in allDevices)
            {
                if (typeJoystick == item.Type)
                {
                    curJoystick = new SharpDX.DirectInput.Joystick(dirInput, item.InstanceGuid);
                    curJoystick.Acquire();
                    isGetJoystick = true;
                    Console.WriteLine("Find joystick");
                    while (true)
                    {
                        int x = curJoystick.GetCurrentState().VelocityX;
                        var joys = curJoystick.GetCurrentState();

                        Thread.Sleep(10);

                        Throttle = SacleX_Axsix(joys.Y);

                        Axsix_y = SacleY_Axsix(joys.X);

                        Axsix_z = SacleZ_Axsix(joys.RotationZ);

                        Axsix_X = SaclePedal(joys.Z);

                        buttons = joys.Buttons;

                        AP_Button = joys.Buttons[2];
                    }
                }
            }
            if (!isGetJoystick)
            {
                Console.WriteLine("No joystick");
            }
        }

        public static double SacleY_Axsix(int y) //-10~10
        {
            double result = 1.0;

            if (y == 32767 || y == 32768 || y == 32766)
            {
                return 0.0;
            }
            else if (y < 32766)
                result = result * (32768 - y) * (-10) / 32768;
            else
                result = result * (y - 32768) * (10) / 32768;

            return result;

        }

        public static double SacleX_Axsix(int x) //-90~90
        {
            double result = -1.0;

            if (x == 32767 || x == 32768 || x == 32766)
            {
                return 0.0;
            }
            else if (x < 32766)
                result = result * (32768 - x) * (-90) / 32768;
            else
                result = result * (x - 32768) * (90) / 32768;
            return result;

        }

        public static double SacleZ_Axsix(int z) //-90~90
        {
            double result = 1.0;

            if (z == 32767 || z == 32768 || z == 32766)
            {
                return 0.0;
            }
            else if (z < 32766)
                result = result * (32768 - z) * (-1.5) / 32768;
            else
                result = result * (z - 32768) * (1.5) / 32768;
            return result;

        }

        public static double SaclePedal(int Pedal) //-45.5~ 85.5
        {
            double result = 1.0;
            // result = result * (65535 - Pedal) * (48) / 65535 + 14;
            result = result * (65535 - Pedal) * (100) / 65535;
            return result;
        }

    }
}
