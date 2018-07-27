using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Sakuno.UserInterface.Shell
{
    class GlowingEdgeOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            Color result;

            if (value is Color color)
                result = color;
            else if (value is SolidColorBrush solidColorBrush)
                result = solidColorBrush.Color;
            else
                throw new InvalidOperationException("Invalid color value.");

            result.A = (byte)(result.A * double.Parse(parameter.ToString()));

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
