using Sakuno.UserInterface.Converters;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Markup
{
    public class ValueConverterSingletonExtension : MarkupExtension
    {
        public Type Type { get; }

        public ValueConverterSingletonExtension(Type type)
        {
            if (!type.IsAssignableTo<IValueConverter>() && !type.IsAssignableTo<IMultiValueConverter>())
                throw new ArgumentException(nameof(type));

            Type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            var target = serviceProvider.GetService<IProvideValueTarget>() ??
                throw new InvalidOperationException("There is no ProvideValueTarget service in the service provider.");
            var targetProperty = target.TargetProperty;

            Type propertyType;

            if (targetProperty is PropertyInfo property)
                propertyType = property.PropertyType;
            else if (targetProperty is DependencyProperty dependencyProperty)
                propertyType = dependencyProperty.PropertyType;
            else
                throw new InvalidOperationException("Cannot obtain the property type.");

            if (propertyType.IsAssignableTo<IValueConverter>())
                return ValueConverterCache.GetValueConverter(Type);

            if (propertyType.IsAssignableTo<IMultiValueConverter>())
                return ValueConverterCache.GetMultiValueConverter(Type);

            throw new InvalidOperationException("The type of property must implement a value converter interface.");
        }
    }
}
