using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace Sakuno.UserInterface.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public sealed class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ConvertCore(value);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => ConvertCore(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        object ConvertCore(object value)
        {
            if (value is bool boolean)
                return BooleanUtil.GetBoxed(!boolean);

            return DependencyProperty.UnsetValue;
        }
    }
}
