using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace Sakuno.UserInterface.Interactivity.Primitives
{
    [ContentProperty(nameof(Actions))]
    public abstract class Trigger : Animatable, IAttachableObject
    {
        static readonly DependencyPropertyKey ActionsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Actions), typeof(TriggerActionCollection), typeof(Trigger), null);

        public static readonly DependencyProperty ActionsProperty = ActionsPropertyKey.DependencyProperty;

        TriggerActionCollection _actions;
        public TriggerActionCollection Actions => _actions;

        public static readonly DependencyProperty ThrottleDueTimeProperty =
            DependencyProperty.Register(nameof(ThrottleDueTime), typeof(TimeSpan), typeof(Trigger),
                new PropertyMetadata(TimeSpan.Zero));
        public TimeSpan ThrottleDueTime
        {
            get => (TimeSpan)GetValue(ThrottleDueTimeProperty);
            set => SetValue(ThrottleDueTimeProperty, value);
        }

        public static readonly DependencyProperty ConditionProperty =
            DependencyProperty.Register(nameof(Condition), typeof(ConditionBase), typeof(Trigger));

        public ConditionBase Condition
        {
            get => (ConditionBase)GetValue(ConditionProperty);
            set => SetValue(ConditionProperty, value);
        }

        Type _associatedType;

        internal DependencyObject _associatedObject;
        protected DependencyObject AssociatedObject
        {
            get
            {
                ReadPreamble();

                return _associatedObject;
            }
        }

        int _invocationPendingCount;

        protected Trigger() : this(typeof(DependencyObject)) { }
        private protected Trigger(Type associatedType)
        {
            _associatedType = associatedType;

            SetValue(ActionsPropertyKey, _actions = new TriggerActionCollection());
        }

        public void Attach(DependencyObject target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            ReadPreamble();

            if (_associatedObject == target)
                return;

            if (_associatedObject != null)
                throw new InvalidOperationException("The trigger cannot be attached to multiple objects.");

            if (!_associatedType.IsAssignableFrom(target.GetType()))
                throw new InvalidOperationException($"Cannot attach to the target. The target should be an object of type \"{_associatedType.FullName}\".");

            WritePreamble();
            _associatedObject = target;
            WritePostscript();

            Actions.Attach(target);

            OnAttached();
        }
        public void Detach()
        {
            ReadPreamble();
            OnDetaching();

            WritePreamble();
            _associatedObject = null;
            WritePostscript();

            Actions.Detach();
        }

        protected virtual void OnAttached() { }
        protected virtual void OnDetaching() { }

        protected void InvokeActions(object args)
        {
            var condition = Condition;
            if (condition != null && !condition.Evaluate(args))
                return;

            var throttleDueTime = ThrottleDueTime;
            if (throttleDueTime == TimeSpan.Zero)
            {
                InvokeActionsCore(args);

                return;
            }

            ThrottleInvocation(throttleDueTime, args);
        }
        async void ThrottleInvocation(TimeSpan dueTime, object args)
        {
            _invocationPendingCount++;

            await Task.Delay(dueTime);

            _invocationPendingCount--;

            if (_invocationPendingCount == 0)
                InvokeActionsCore(args);
        }
        void InvokeActionsCore(object args)
        {
            foreach (var action in _actions)
                action.InternalInvoke(args);
        }

        protected override Freezable CreateInstanceCore() => (Freezable)Activator.CreateInstance(GetType());
    }

    public abstract class Trigger<T> : Trigger where T : DependencyObject
    {
        public new T AssociatedObject => (T)base.AssociatedObject;

        protected Trigger() : base(typeof(T)) { }
    }
}
