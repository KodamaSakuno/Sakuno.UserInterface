using System;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Markup
{
    public sealed class DoubleExtension : MarkupExtension
    {
        object _boxedValue;

        public double Value => (double)_boxedValue;

        public DoubleExtension(double value)
        {
            if (DoubleUtil.IsCloseToZero(value))
                _boxedValue = BoxedConstants.Double.Zero;
            else if (DoubleUtil.IsCloseToOne(value))
                _boxedValue = BoxedConstants.Double.One;
            else if (value.IsNaN())
                _boxedValue = BoxedConstants.Double.NaN;

            _boxedValue = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => _boxedValue;
    }
}
