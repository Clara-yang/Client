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
    public class CustomGroupBox : GroupBox
    {

        public static CornerRadius GetTitleCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(TitleCornerRadiusProperty);
        }

        public static void SetTitleCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(TitleCornerRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for TitleCornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleCornerRadiusProperty =
            DependencyProperty.RegisterAttached("TitleCornerRadius", typeof(CornerRadius), typeof(CustomGroupBox), new PropertyMetadata());


        public static Brush GetTitleBackGround(DependencyObject obj)
        {
            return (Brush)obj.GetValue(TitleBackGroundProperty);
        }

        public static void SetTitleBackGround(DependencyObject obj, Brush value)
        {
            obj.SetValue(TitleBackGroundProperty, value);
        }

        // Using a DependencyProperty as the backing store for TitleBackGround.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleBackGroundProperty =
            DependencyProperty.RegisterAttached("TitleBackGround", typeof(Brush), typeof(CustomGroupBox), new PropertyMetadata());




        public static Thickness GetTitleBorderThickness(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(TitleBorderThicknessProperty);
        }

        public static void SetTitleBorderThickness(DependencyObject obj, Thickness value)
        {
            obj.SetValue(TitleBorderThicknessProperty, value);
        }

        // Using a DependencyProperty as the backing store for TitleBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleBorderThicknessProperty =
            DependencyProperty.RegisterAttached("TitleBorderThickness", typeof(Thickness), typeof(CustomGroupBox), new PropertyMetadata());




        public static CornerRadius GetContentCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(ContentCornerRadiusProperty);
        }

        public static void SetContentCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(ContentCornerRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for ContentCornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentCornerRadiusProperty =
            DependencyProperty.RegisterAttached("ContentCornerRadius", typeof(CornerRadius), typeof(CustomGroupBox), new PropertyMetadata());




    }
}
