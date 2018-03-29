using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Sakuno.UserInterface.Interactivity
{
    public abstract class Behavior : Animatable, IAttachableObject
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

        protected Behavior() : this(typeof(DependencyObject)) { }
        private protected Behavior(Type associatedType)
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
                throw new InvalidOperationException("The behavior cannot be attached to multiple objects.");

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

        protected override Freezable CreateInstanceCore() => (Freezable)Activator.CreateInstance(GetType());
    }

    public abstract class Behavior<T> : Behavior where T : DependencyObject
    {
        public new T AssociatedObject => (T)base.AssociatedObject;

        protected Behavior() : base(typeof(T)) { }
    }
}
