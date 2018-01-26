using System;
using System.Globalization;
using System.Windows.Data;

namespace Sakuno.UserInterface.Converters
{
    public class IsTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var target = parameter as Type ?? throw new ArgumentException(nameof(parameter));
            var type = value.GetType();

            if (target.IsInterface)
                return BooleanUtil.GetBoxed(type.IsAssignableFrom(target));

            return BooleanUtil.GetBoxed(type.IsSubclassOf(target));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
