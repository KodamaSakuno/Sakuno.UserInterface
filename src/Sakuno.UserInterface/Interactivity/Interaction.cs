using System;
using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public static class Interaction
    {
        static readonly DependencyProperty BehaviorsProperty =
            DependencyProperty.RegisterAttached("ShadowBehaviors", typeof(BehaviorCollection), typeof(Interaction),
                new PropertyMetadata(OnBehaviorsChanged));

        public static BehaviorCollection GetBehaviors(DependencyObject obj)
        {
            var result = (BehaviorCollection)obj.GetValue(BehaviorsProperty);
            if (result == null)
            {
                result = new BehaviorCollection();

                obj.SetValue(BehaviorsProperty, result);
            }

            return result;
        }

        static void OnBehaviorsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (BehaviorCollection)e.OldValue;
            var newValue = (BehaviorCollection)e.NewValue;

            if (oldValue == newValue)
                return;

            if (oldValue != null && oldValue._associatedObject != null)
                oldValue.Detach();

            if (newValue == null)
                return;

            if (newValue._associatedObject != null)
                throw new InvalidOperationException("The collection cannot be attached to multiple objects.");

            newValue.Attach(obj);
        }
    }
}
