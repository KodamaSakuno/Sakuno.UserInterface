using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public abstract class AttachableObjectCollection<T> : FreezableCollection<T> where T : DependencyObject, IAttachableObject
    {
        internal DependencyObject _associatedObject;
        internal protected DependencyObject AssociatedObject
        {
            get
            {
                ReadPreamble();

                return _associatedObject;
            }
        }

        List<T> _snapshot;

        public AttachableObjectCollection()
        {
            ((INotifyCollectionChanged)this).CollectionChanged += OnCollectionChanged;

            _snapshot = new List<T>();
        }

        public void Attach(DependencyObject target)
        {
            ReadPreamble();

            if (_associatedObject == target)
                return;

            if (_associatedObject != null)
                throw new InvalidOperationException("The collection cannot be attached to multiple objects.");

            WritePreamble();
            _associatedObject = target;
            WritePostscript();

            OnAttached();
        }
        public void Detach()
        {
            OnDetaching();

            WritePreamble();
            _associatedObject = null;
            WritePostscript();
        }

        protected abstract void OnAttached();
        protected abstract void OnDetaching();

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (T item in e.NewItems)
                    {
                        ThrowIfExists(item);
                        OnItemAdded(item);

                        _snapshot.Add(item);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (T item in e.OldItems)
                    {
                        OnItemRemoved(item);

                        _snapshot.Remove(item);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    foreach (T item in e.OldItems)
                    {
                        OnItemRemoved(item);

                        _snapshot.Remove(item);
                    }
                    foreach (T item in e.NewItems)
                    {
                        OnItemAdded(item);

                        _snapshot.Add(item);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    foreach (var item in _snapshot)
                        OnItemRemoved(item);

                    _snapshot.Clear();

                    foreach (var item in this)
                    {
                        ThrowIfExists(item);
                        OnItemAdded(item);

                        _snapshot.Add(item);
                    }
                    break;
            }
        }

        void ThrowIfExists(T item)
        {
            if (_snapshot.Contains(item))
                throw new InvalidOperationException("Duplicate item is not allowed.");
        }

        internal abstract void OnItemAdded(T item);
        internal abstract void OnItemRemoved(T item);
    }
}
