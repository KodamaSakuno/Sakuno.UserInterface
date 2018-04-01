using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Sakuno.UserInterface.Interactivity
{
    public abstract class TriggerAction : Animatable, IAttachableObject
    {
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

        protected TriggerAction() : this(typeof(DependencyObject)) { }
        private protected TriggerAction(Type associatedType)
        {
            _associatedType = associatedType;
        }

        public void Attach(DependencyObject target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            ReadPreamble();

            if (_associatedObject == target)
                return;

            if (_associatedObject != null)
                throw new InvalidOperationException("The trigger action cannot be attached to multiple objects.");

            if (!_associatedType.IsAssignableFrom(target.GetType()))
                throw new InvalidOperationException($"Cannot attach to the target. The target should be an object of type \"{_associatedType.FullName}\".");

            WritePreamble();
            _associatedObject = target;
            WritePostscript();

            OnAttached();
        }
        public void Detach()
        {
            ReadPreamble();
            OnDetaching();

            WritePreamble();
            _associatedObject = null;
            WritePostscript();
        }

        protected virtual void OnAttached() { }
        protected virtual void OnDetaching() { }

        internal void InternalInvoke(object args) => Invoke(args);
        protected abstract void Invoke(object args);

        protected override Freezable CreateInstanceCore() => (Freezable)Activator.CreateInstance(GetType());
    }

    public abstract class TriggerAction<T> : TriggerAction where T : DependencyObject
    {
        public new T AssociatedObject => (T)base.AssociatedObject;

        protected TriggerAction() : base(typeof(T)) { }
    }
}
