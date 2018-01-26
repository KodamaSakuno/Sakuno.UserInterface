using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sakuno.UserInterface.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    [ValueConversion(typeof(bool?), typeof(Visibility))]
    public class BooleanToInvisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = false;

            if (value is bool boolean)
                result = boolean;
            else if (value is bool?)
            {
                var nullableBoolean = (bool?)value;

                result = nullableBoolean.GetValueOrDefault();
            }

            return EnumUtil.GetBoxed(result ? Visibility.Collapsed : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
                return BooleanUtil.GetBoxed(visibility == Visibility.Collapsed);

            return BooleanUtil.False;
        }
    }
}
