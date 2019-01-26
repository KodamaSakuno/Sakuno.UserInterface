using Sakuno.UserInterface.Data;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

namespace Sakuno.UserInterface.Markup
{
    [ContentProperty(nameof(Name))]
    public sealed class DataContextOfExtension : MarkupExtension
    {
        public string Name { get; }

        public DataContextOfExtension(string name)
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

            if (xamlNameResolver.Resolve(Name) is FrameworkElement element)
                return new Binding(nameof(FrameworkElement.DataContext)) { Source = element, Mode = BindingMode.OneWay }.ProvideValue(serviceProvider);

            if (new StaticResourceExtension(Name).ProvideValue(serviceProvider) is BindingProxy proxy)
                return new Binding(nameof(BindingProxy.Data)) { Source = proxy, Mode = BindingMode.OneWay }.ProvideValue(serviceProvider);

            throw new InvalidOperationException("The provided name must be the one of a fully initialized FrameworkElement or a resource of type BindingProxy.");
        }
    }
}
