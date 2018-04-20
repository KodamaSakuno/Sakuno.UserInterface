using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Interactivity.Primitives
{
    public abstract class TargetedTriggerAction : TriggerAction
    {
        public static readonly DependencyProperty TargetObjectProperty =
            DependencyProperty.Register(nameof(TargetObject), typeof(object), typeof(TargetedTriggerAction),
                new PropertyMetadata(OnTargetObjectChanged));

        [TypeConverter(typeof(NameReferenceConverter))]
        public object TargetObject
        {
            get => GetValue(TargetObjectProperty);
            set => SetValue(TargetObjectProperty, value);
        }

        static void OnTargetObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((TargetedTriggerAction)d).OnTargetObjectChanged(e.NewValue);
        void OnTargetObjectChanged(object newValue)
        {
            if (newValue == null)
                return;

            VerifyTargetType(newValue);
        }

        Type _targetTypeConstraint;

        protected object Target => TargetObject ?? AssociatedObject;

        protected TargetedTriggerAction() { }
        private protected TargetedTriggerAction(Type targetTypeConstraint)
        {
            _targetTypeConstraint = targetTypeConstraint;
        }

        protected override void OnAttached() => VerifyTargetType(_associatedObject);

        void VerifyTargetType(object target)
        {
            if (_targetTypeConstraint != null && !_targetTypeConstraint.IsAssignableFrom(target.GetType()))
                throw new InvalidOperationException($"The type of target should be \"{_targetTypeConstraint.FullName}\" or its derived type.");
        }
    }

    public abstract class TargetedTriggerAction<T> : TargetedTriggerAction where T : class
    {
        public new T Target => (T)base.Target;

        protected TargetedTriggerAction() : base(typeof(T)) { }
    }
}
