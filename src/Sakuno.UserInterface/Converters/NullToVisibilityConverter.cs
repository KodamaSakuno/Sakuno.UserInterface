using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sakuno.UserInterface.Converters
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public sealed class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            EnumUtil.GetBoxed(value == null ? Visibility.Visible : Visibility.Collapsed);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
