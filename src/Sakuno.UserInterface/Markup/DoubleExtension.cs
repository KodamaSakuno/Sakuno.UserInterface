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
                _boxedValue = DoubleUtil.Zero;
            else if (DoubleUtil.IsCloseToOne(value))
                _boxedValue = DoubleUtil.One;
            else if (value.IsNaN())
                _boxedValue = DoubleUtil.NaN;

            _boxedValue = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => _boxedValue;
    }
}
