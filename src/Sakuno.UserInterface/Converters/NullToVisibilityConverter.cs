using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sakuno.UserInterface.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            EnumUtil.GetBoxed(value == null ? Visibility.Collapsed : Visibility.Visible);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
