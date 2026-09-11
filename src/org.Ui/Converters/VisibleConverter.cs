using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace org.Ui.Converters
{
    public abstract class VisibleConverter<T>(T trueValue, T falseValue) : IValueConverter
    {
        public T TrueValue { get; set; } = trueValue;
        public T FalseValue { get; set; } = falseValue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T val && EqualityComparer<T>.Default.Equals(val, TrueValue)
                ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility val && val == Visibility.Visible
                ? TrueValue : FalseValue;
        }
    }
}
