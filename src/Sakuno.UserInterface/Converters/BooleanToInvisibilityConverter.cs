using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sakuno.UserInterface.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    [ValueConversion(typeof(bool?), typeof(Visibility))]
    public sealed class BooleanToInvisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            EnumUtil.GetBoxed(Equals(value, BooleanUtil.True) ? Visibility.Collapsed : Visibility.Visible);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
                return BooleanUtil.GetBoxed(visibility == Visibility.Collapsed);

            return BooleanUtil.False;
        }
    }
}
