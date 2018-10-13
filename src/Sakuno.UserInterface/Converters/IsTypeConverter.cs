using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sakuno.UserInterface.Converters
{
    [ValueConversion(typeof(object), typeof(Type))]
    public class IsTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var target = parameter as Type;
            if (target == null)
                return DependencyProperty.UnsetValue;

            var type = value.GetType();

            if (target.IsInterface)
                return BooleanUtil.GetBoxed(type.IsAssignableFrom(target));

            return BooleanUtil.GetBoxed(type.IsSubclassOf(target));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
