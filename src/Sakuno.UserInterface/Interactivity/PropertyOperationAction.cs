using Sakuno.Reflection;
using System;
using System.Windows;
using System.Windows.Data;

namespace Sakuno.UserInterface.Interactivity
{
    public abstract class PropertyOperationAction : TargetedTriggerAction
    {
        public static readonly DependencyProperty PropertyProperty =
            DependencyProperty.Register(nameof(Property), typeof(string), typeof(PropertyOperationAction));

        public string Property
        {
            get => (string)GetValue(PropertyProperty);
            set => SetValue(PropertyProperty, value);
        }

        Type _targetType;
        PropertyAccessor _propertyAccessor;

        protected PropertyOperationAction()
        {
            BindingOperations.SetBinding(this, TargetObjectProperty, new Binding());
        }
        protected sealed override void Invoke(object args)
        {
            var target = Target;
            if (target == null)
                return;

            var property = Property;
            if (property.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Property));

            var targetType = target.GetType();
            if (_targetType != targetType)
            {
                var propertyInfo = targetType.GetProperty(property) ?? throw new InvalidOperationException($"Property \"{property}\" does not exist on type \"{targetType.FullName}\".");

                if (!propertyInfo.CanWrite)
                    throw new InvalidOperationException($"Property \"{property}\" is read-only.");

                _targetType = targetType;
                _propertyAccessor = ReflectionCache.GetPropertyAccessor(propertyInfo);
            }

            Invoke(_propertyAccessor, target);
        }

        protected abstract void Invoke(PropertyAccessor propertyAccessor, object target);
    }
}
