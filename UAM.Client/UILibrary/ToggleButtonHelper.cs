using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UAM.Client.UILibrary
{
    public class ToggleButtonHelper : DependencyObject
    {


        public static ImageSource GetIcon_On(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(Icon_OnProperty);
        }

        public static void SetIcon_On(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(Icon_OnProperty, value);
        }

        // Using a DependencyProperty as the backing store for Icon_On.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Icon_OnProperty =
            DependencyProperty.RegisterAttached("Icon_On", typeof(ImageSource), typeof(ToggleButtonHelper), new PropertyMetadata());




        public static ImageSource GetIcon_Off(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(Icon_OffProperty);
        }

        public static void SetIcon_Off(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(Icon_OffProperty, value);
        }

        // Using a DependencyProperty as the backing store for Icon_Off.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Icon_OffProperty =
            DependencyProperty.RegisterAttached("Icon_Off", typeof(ImageSource), typeof(ToggleButtonHelper), new PropertyMetadata());




        public static Orientation GetOrientation(DependencyObject obj)
        {
            return (Orientation)obj.GetValue(OrientationProperty);
        }

        public static void SetOrientation(DependencyObject obj, int value)
        {
            obj.SetValue(OrientationProperty, value);
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.RegisterAttached("Orientation", typeof(Orientation), typeof(ToggleButtonHelper), new PropertyMetadata(Orientation.Vertical));



        public static Thickness GetTextMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(TextMarginProperty);
        }

        public static void SetTextMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(TextMarginProperty, value);
        }

        // Using a DependencyProperty as the backing store for TextMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextMarginProperty =
            DependencyProperty.RegisterAttached("TextMargin", typeof(Thickness), typeof(ToggleButtonHelper), new PropertyMetadata());


    }
}
