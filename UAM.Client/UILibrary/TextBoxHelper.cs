using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UAM.Client.UILibrary
{
    public class TextBoxHelper: DependencyObject
    {


        public static string GetWatermarks(DependencyObject obj)
        {
            return (string)obj.GetValue(WatermarksProperty);
        }

        public static void SetWatermarks(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarksProperty, value);
        }

        // Using a DependencyProperty as the backing store for Watermarks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarksProperty =
            DependencyProperty.RegisterAttached("Watermarks", typeof(string), typeof(TextBoxHelper), new PropertyMetadata(""));

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
            DependencyProperty.RegisterAttached("CornerRadius", typeof(float), typeof(TextBoxHelper), new PropertyMetadata(5.8f));
    }
}
