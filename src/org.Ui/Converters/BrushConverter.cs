using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace org.Ui.Converters
{
    public class BrushConverter<T> : IValueConverter
    {
        public Brush Default { get; set; }
        public Dictionary<T, Brush> Colors { get; set; }

        public BrushConverter(Dictionary<T, Brush> dict, Brush defaultVal)
        {
            Colors = dict ?? new Dictionary<T, Brush> { { default, Brushes.DimGray } };
            Default = defaultVal;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T key && (Colors.ContainsKey(key)) ? Colors[key] : Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Brush color && Colors.ContainsValue(color) 
                ? Colors.FirstOrDefault(f => f.Value == color) : default(T);
        }
    }
}
