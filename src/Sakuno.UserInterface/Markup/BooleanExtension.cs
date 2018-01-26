using System;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Markup
{
    public class BooleanExtension : MarkupExtension
    {
        object _boxedValue;

        public bool Value => (bool)_boxedValue;

        public BooleanExtension(bool value) => _boxedValue = BooleanUtil.GetBoxed(value);

        public override object ProvideValue(IServiceProvider serviceProvider) => _boxedValue;
    }
}
