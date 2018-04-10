using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public class MethodArgumentCollection : FreezableCollection<MethodArgument>
    {
        List<MethodArgument> _snapshots = new List<MethodArgument>();
        List<object> _values = new List<object>();
        List<Type> _types = new List<Type>();

        Type[] _typeArray;
        public Type[] Types
        {
            get
            {
                if (_typeArray == null)
                    _typeArray = _types.Count > 0 ? _types.ToArray() : Array.Empty<Type>();

                return _typeArray;
            }
        }

        internal bool IsSignatureChanged { get; set; }

        object[] _valueArray;
        public object[] Values
        {
            get
            {
                if (_valueArray == null)
                    _valueArray = _values.Count > 0 ? _values.ToArray() : Array.Empty<object>();

                return _valueArray;
            }
        }

        public MethodArgumentCollection()
        {
            ((INotifyCollectionChanged)this).CollectionChanged += OnCollectionChanged;
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        var argument = (MethodArgument)e.NewItems[0];
                        argument.PropertyChanged += OnArgumentPropertyChanged;

                        var value = argument.Value;

                        _snapshots.Insert(e.NewStartingIndex, argument);
                        _values.Insert(e.NewStartingIndex, value);
                        _types.Insert(e.NewStartingIndex, argument.DesiredType);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    {
                        var argument = (MethodArgument)e.OldItems[0];
                        argument.PropertyChanged -= OnArgumentPropertyChanged;

                        _snapshots.RemoveAt(e.OldStartingIndex);
                        _values.RemoveAt(e.OldStartingIndex);
                        _types.RemoveAt(e.OldStartingIndex);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    var oldItem = (MethodArgument)e.OldItems[0];
                    var newItem = (MethodArgument)e.NewItems[0];

                    oldItem.PropertyChanged -= OnArgumentPropertyChanged;
                    newItem.PropertyChanged += OnArgumentPropertyChanged;

                    var index = e.NewStartingIndex;

                    _snapshots[index] = newItem;
                    _values[index] = newItem.Value;
                    _types[index] = newItem.DesiredType;
                    break;

                case NotifyCollectionChangedAction.Reset:
                    foreach (var argument in _snapshots)
                        argument.PropertyChanged -= OnArgumentPropertyChanged;

                    _snapshots.Clear();
                    _values.Clear();
                    _types.Clear();
                    break;
            }

            IsSignatureChanged = true;

            _valueArray = null;
            _typeArray = null;
        }

        void OnArgumentPropertyChanged(MethodArgument argument)
        {
            var index = IndexOf(argument);
            var value = argument.Value;
            var type = argument.DesiredType;

            _values[index] = value;

            if (_types[index] == type)
                return;

            _types[index] = type;

            IsSignatureChanged = true;
        }
    }
}
