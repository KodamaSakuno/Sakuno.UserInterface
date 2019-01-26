using System.Windows;
using System.Windows.Data;

namespace Sakuno.UserInterface.Data
{
    public sealed class BindingProxy : Freezable
    {
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy));

        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public BindingProxy()
        {
            BindingOperations.SetBinding(this, DataProperty, new Binding());
        }

        protected override Freezable CreateInstanceCore() => new BindingProxy();
    }
}
