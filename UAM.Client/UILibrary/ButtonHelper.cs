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
    public class ButtonHelper : DependencyObject
    {
        public static bool GetIsSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectedProperty);
        }

        public static void SetIsSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSelectedProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(ButtonHelper), new PropertyMetadata(false));


        public static ImageSource GetMainIcon(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(MainIconProperty);
        }

        public static void SetMainIcon(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(MainIconProperty, value);
        }

        // Using a DependencyProperty as the backing store for MainIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainIconProperty =
            DependencyProperty.RegisterAttached("MainIcon", typeof(ImageSource), typeof(ButtonHelper), new PropertyMetadata());



        public static ImageSource GetMainIcon_Press(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(MainIcon_PressProperty);
        }

        public static void SetMainIcon_Press(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(MainIcon_PressProperty, value);
        }

        // Using a DependencyProperty as the backing store for MainIcon_Press.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainIcon_PressProperty =
            DependencyProperty.RegisterAttached("MainIcon_Press", typeof(ImageSource), typeof(ButtonHelper), new PropertyMetadata());




        public static ImageSource GetMinorIcon(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(MinorIconProperty);
        }

        public static void SetMinorIcon(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(MinorIconProperty, value);
        }

        // Using a DependencyProperty as the backing store for MinorIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinorIconProperty =
            DependencyProperty.RegisterAttached("MinorIcon", typeof(ImageSource), typeof(ButtonHelper), new PropertyMetadata());



        public static ImageSource GetMinorIcon_Press(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(MinorIcon_PressProperty);
        }

        public static void SetMinorIcon_Press(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(MinorIcon_PressProperty, value);
        }

        // Using a DependencyProperty as the backing store for MinorIcon_Press.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinorIcon_PressProperty =
            DependencyProperty.RegisterAttached("MinorIcon_Press", typeof(ImageSource), typeof(ButtonHelper), new PropertyMetadata());






        public static string GetMainText(DependencyObject obj)
        {
            return (string)obj.GetValue(MainTextProperty);
        }

        public static void SetMainText(DependencyObject obj, string value)
        {
            obj.SetValue(MainTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for MainText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainTextProperty =
            DependencyProperty.RegisterAttached("MainText", typeof(string), typeof(ButtonHelper), new PropertyMetadata());





        public static string GetMinorText(DependencyObject obj)
        {
            return (string)obj.GetValue(MinorTextProperty);
        }

        public static void SetMinorText(DependencyObject obj, string value)
        {
            obj.SetValue(MinorTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for MinorText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinorTextProperty =
            DependencyProperty.RegisterAttached("MinorText", typeof(string), typeof(ButtonHelper), new PropertyMetadata());




        public static float GetCornerRadius(DependencyObject obj)
        {
            return (float)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, float value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(float), typeof(ButtonHelper), new PropertyMetadata(8.5f));



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
            DependencyProperty.RegisterAttached("TextMargin", typeof(Thickness), typeof(ButtonHelper), new PropertyMetadata());



        public static Orientation GetOrientation(DependencyObject obj)
        {
            return (Orientation)obj.GetValue(OrientationProperty);
        }

        public static void SetOrientation(DependencyObject obj, Orientation value)
        {
            obj.SetValue(OrientationProperty, value);
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.RegisterAttached("Orientation", typeof(Orientation), typeof(ButtonHelper), new PropertyMetadata(Orientation.Vertical));


    }
}
