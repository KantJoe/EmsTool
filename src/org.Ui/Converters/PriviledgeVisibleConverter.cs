using org.Models.Priviledge;
using org.Utils.Global;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace org.Ui.Converters
{
    /// <summary>
    /// 权限可见性转换器
    /// 非严格模式: actualValue >= priviledge
    /// 严格模式: (actualValue & priviledge) == priviledge
    /// </summary>
    public class PriviledgeVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PriviledgeLevel actualValue
                && parameter is PriviledgeLevel priviledge)
            {
                return actualValue >= priviledge? Visibility.Visible:Visibility.Collapsed;
            }

            LogFactory.Error("PriviledgeVisibleConverter.Convert Error {val},{param}", value, parameter);
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
