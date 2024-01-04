using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Workflow.ComponentModel;
using DependencyObject = System.Windows.DependencyObject;
using DependencyProperty = System.Windows.DependencyProperty;
using PropertyMetadata = System.Windows.PropertyMetadata;

namespace UAM.Plugin
{
    public class PasswordHelper
    {
        public static int GetMyProperty(DependencyObject obj)
        {
            return (int)obj.GetValue(MyPropertyProperty);
        }

        public static void SetMyProperty(DependencyObject obj, int value)
        {
            obj.SetValue(MyPropertyProperty, value);
        }

        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.RegisterAttached("MyProperty", typeof(int), typeof(PasswordHelper), new PropertyMetadata(0));

        public static string GetPasswordContent(DependencyObject obj) => (string)obj.GetValue(PasswordContentProperty);

        public static void SetPasswordContent(DependencyObject obj, string value) => obj.SetValue(PasswordContentProperty, value);

        public static readonly DependencyProperty PasswordContentProperty =
            DependencyProperty.RegisterAttached("PasswordContent", typeof(string), typeof(PasswordHelper),
            new PropertyMetadata(string.Empty, OnPasswordContentPropertyChanged));

        private static void OnPasswordContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as PasswordBox;
            box.PasswordChanged -= OnPasswordChanged;
            var password = (string)e.NewValue;
            if (box != null && box.Password != password)
                box.Password = password;
            box.PasswordChanged += OnPasswordChanged;
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var box = sender as PasswordBox;
            SetPasswordContent(box, box.Password);
        }
    }
}

