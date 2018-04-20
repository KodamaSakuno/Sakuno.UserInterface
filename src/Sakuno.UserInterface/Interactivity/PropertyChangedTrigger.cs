using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public class PropertyChangedTrigger : Primitives.Trigger
    {
        public static readonly DependencyProperty BindingProperty =
            DependencyProperty.Register(nameof(Binding), typeof(object), typeof(PropertyChangedTrigger),
                new PropertyMetadata(OnBindingChanged));

        public object Binding
        {
            get => GetValue(BindingProperty);
            set => SetValue(BindingProperty, value);
        }

        static void OnBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((PropertyChangedTrigger)d).OnBindingChanged(e);
        void OnBindingChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_associatedObject == null)
                return;

            Evaulate(e);
        }

        protected virtual void Evaulate(DependencyPropertyChangedEventArgs args)
        {
            foreach (var action in Actions)
                action.UpdateBindingTargetsOfAllProperties();

            InvokeActions(args);
        }
    }
}
