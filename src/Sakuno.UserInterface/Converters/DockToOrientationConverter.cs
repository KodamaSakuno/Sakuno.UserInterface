using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Sakuno.UserInterface.Converters
{
    [ValueConversion(typeof(Dock), typeof(Orientation))]
    public sealed class DockToOrientationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dock = (Dock)value;

            return EnumUtil.GetBoxed(dock == Dock.Left || dock == Dock.Right ? Orientation.Horizontal : Orientation.Vertical);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
