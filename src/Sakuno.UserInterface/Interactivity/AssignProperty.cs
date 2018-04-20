using Sakuno.Reflection;
using Sakuno.UserInterface.Interactivity.Primitives;
using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public sealed class AssignProperty : PropertyOperationAction
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(object), typeof(AssignProperty));

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        protected override void Invoke(PropertyAccessor propertyAccessor, object target) =>
            propertyAccessor.SetValue(target, GetValue(ValueProperty));
    }
}
