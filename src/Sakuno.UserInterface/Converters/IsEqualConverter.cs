using System;
using System.Globalization;
using System.Windows.Data;

namespace Sakuno.UserInterface.Converters
{
    public class IsEqualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            BooleanUtil.GetBoxed(Equals(value, parameter) || (value != null && value.Equals(parameter)));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            Equals(value, BooleanUtil.True) ? parameter : Binding.DoNothing;
    }
}
