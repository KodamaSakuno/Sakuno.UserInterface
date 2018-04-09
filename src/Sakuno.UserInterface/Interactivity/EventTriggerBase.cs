using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Interactivity
{
    public abstract class EventTriggerBase : Trigger
    {
        public static readonly DependencyProperty SourceObjectProperty =
            DependencyProperty.Register(nameof(SourceObject), typeof(object), typeof(EventTriggerBase),
                new PropertyMetadata(OnSourceObjectChanged));

        [TypeConverter(typeof(NameReferenceConverter))]
        public object SourceObject
        {
            get => GetValue(SourceObjectProperty);
            set => SetValue(SourceObjectProperty, value);
        }

        static void OnSourceObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((EventTriggerBase)d).OnSourceObjectChanged(e.OldValue, e.NewValue);
        void OnSourceObjectChanged(object oldValue, object newValue)
        {
            if (newValue == null)
            {
                OnSourceChanged(oldValue, _associatedObject);
                return;
            }

            VerifySourceType(newValue);

            if (_associatedObject == null)
                return;

            OnSourceChanged(oldValue, newValue);
        }

        private protected static readonly MethodInfo _handlerMethod;

        Type _sourceTypeConstraint;

        protected object Source => SourceObject ?? AssociatedObject;

        static EventTriggerBase()
        {
            _handlerMethod = typeof(EventTriggerBase).GetMethod(nameof(OnEventRaisedCore), BindingFlags.Instance | BindingFlags.NonPublic);
        }
        protected EventTriggerBase() : this(null) { }
        private protected EventTriggerBase(Type sourceTypeConstraint)
        {
            _sourceTypeConstraint = sourceTypeConstraint;
        }

        protected override void OnAttached()
        {
            VerifySourceType(_associatedObject);
            OnSourceChanged(null, Source);
        }
        protected override void OnDetaching() => OnSourceChanged(Source, null);

        void VerifySourceType(object target)
        {
            if (_sourceTypeConstraint != null && !_sourceTypeConstraint.IsAssignableFrom(target.GetType()))
                throw new InvalidOperationException($"The type of source should be \"{_sourceTypeConstraint.FullName}\" or its derived type.");
        }
        void OnSourceChanged(object oldValue, object newValue)
        {
            if (oldValue != null)
                UnregisterEvent(oldValue);

            if (newValue != null)
                RegisterEvent(newValue);
        }

        protected abstract void RegisterEvent(object source);
        protected abstract void UnregisterEvent(object source);

        private protected void OnEventRaisedCore(object sender, EventArgs e) => OnEventRaised(e);
        protected virtual void OnEventRaised(EventArgs e) => InvokeActions(e);
    }

    public abstract class EventTriggerBase<T> : EventTriggerBase where T : class
    {
        protected EventTriggerBase() : base(typeof(T)) { }

        protected sealed override void RegisterEvent(object source) => RegisterEvent(source as T);
        protected sealed override void UnregisterEvent(object source) => UnregisterEvent(source as T);

        protected abstract void RegisterEvent(T source);
        protected abstract void UnregisterEvent(T source);
    }
}
