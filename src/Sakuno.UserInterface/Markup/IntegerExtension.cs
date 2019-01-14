using System;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Markup
{
    public sealed class IntegerExtension : MarkupExtension
    {
        object _boxedValue;

        public int Value => (int)_boxedValue;

        public IntegerExtension(int value)
        {
            if (value == 0)
                _boxedValue = BoxedConstants.Int32.Zero;
            else
                _boxedValue = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => _boxedValue;
    }
}
