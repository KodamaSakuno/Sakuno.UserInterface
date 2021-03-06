﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sakuno.UserInterface.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public sealed class IsNotEqualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            BooleanUtil.GetBoxed(!Equals(value, parameter));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            Equals(value, BoxedConstants.Boolean.True) ? parameter : DependencyProperty.UnsetValue;
    }
}
