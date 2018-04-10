using System;
using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public class MethodArgument : Freezable
    {
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(nameof(Type), typeof(Type), typeof(MethodArgument),
                new PropertyMetadata(OnTypeChanged));

        static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((MethodArgument)d).PropertyChanged?.Invoke((MethodArgument)d);

        public Type Type
        {
            get => (Type)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(object), typeof(MethodArgument),
                new PropertyMetadata(OnValueChanged));

        static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((MethodArgument)d).PropertyChanged?.Invoke((MethodArgument)d);

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public Type DesiredType => Type ?? Value?.GetType() ?? typeof(object);

        internal event Action<MethodArgument> PropertyChanged;

        protected override Freezable CreateInstanceCore() => new MethodArgument();
    }
}
