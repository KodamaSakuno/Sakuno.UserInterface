using Sakuno.Reflection;
using Sakuno.UserInterface.Interactivity.Primitives;
using System;
using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public class EventTrigger : EventTriggerBase
    {
        public static readonly DependencyProperty EventProperty =
               DependencyProperty.Register(nameof(Event), typeof(string), typeof(EventTrigger),
                   new FrameworkPropertyMetadata(OnEventChanged));

        public string Event
        {
            get => (string)GetValue(EventProperty);
            set => SetValue(EventProperty, value);
        }

        static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((EventTrigger)d).OnEventChanged((string)e.OldValue, (string)e.NewValue);
        void OnEventChanged(string oldValue, string newValue)
        {
            if (_associatedObject == null)
                return;

            var source = Source;
            if (source == null)
                return;

            if (!oldValue.IsNullOrEmpty())
                UnregisterEvent(source);

            if (!newValue.IsNullOrEmpty())
                RegisterEvent(source);
        }

        EventAccessor _eventAccessor;

        Delegate _handler;

        protected override void RegisterEvent(object source)
        {
            var type = source.GetType();
            var eventInfo = type.GetEvent(Event) ?? throw new InvalidOperationException($"Event \"{Event}\" does not exist on type \"{type.FullName}\".");

            _eventAccessor = new EventAccessor(eventInfo);

            _handler = Delegate.CreateDelegate(_eventAccessor.Event.EventHandlerType, this, _handlerMethod);
            _eventAccessor.AddHandler(source, _handler);
        }
        protected override void UnregisterEvent(object source)
        {
            if (_eventAccessor == null)
                return;

            _eventAccessor.RemoveHandler(source, _handler);
            _handler = null;
        }
    }
}
