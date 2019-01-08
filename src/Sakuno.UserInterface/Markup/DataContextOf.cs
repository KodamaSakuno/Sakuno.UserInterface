using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

namespace Sakuno.KanColle.ShipCGManager.Markups
{
    [ContentProperty(nameof(Name))]
    public sealed class DataContextOf : MarkupExtension
    {
        public string Name { get; }

        public DataContextOf(string name)
        {
            Name = name;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            var xamlNameResolver = serviceProvider.GetService<IXamlNameResolver>() ??
                throw new InvalidOperationException("There is no XamlNameResolver service in the service provider.");

            if (Name.IsNullOrEmpty())
                throw new InvalidOperationException("It must have a Name to resolve.");

            if (!(xamlNameResolver.Resolve(Name) is FrameworkElement element))
                throw new InvalidOperationException("The resolved object must be of type FrameworkElement.");

            return new Binding(nameof(FrameworkElement.DataContext)) { Source = element, Mode = BindingMode.OneWay }.ProvideValue(serviceProvider);
        }
    }
}
