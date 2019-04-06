using System;
using System.Windows;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Markup
{
    public sealed class StaticResourceKeyOfTypeExtension : MarkupExtension
    {
        public Type Type { get; }

        public StaticResourceKeyOfTypeExtension(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public override object ProvideValue(IServiceProvider serviceProvider) =>
            new StaticResourceExtension(Type).ProvideValue(serviceProvider);
    }
}
