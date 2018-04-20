using Sakuno.UserInterface.Interactivity.Primitives;
using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public class DataTrigger : PropertyChangedTrigger
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(object), typeof(DataTrigger),
                new PropertyMetadata(OnValueChanged));

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((DataTrigger)d).Evaulate(e);

        public static readonly DependencyProperty ComparisonProperty =
            DependencyProperty.Register(nameof(Comparison), typeof(ComparisonType), typeof(DataTrigger),
                new PropertyMetadata(EnumUtil.GetBoxed(ComparisonType.Equal), OnComparisionChanged));

        public ComparisonType Comparison
        {
            get => (ComparisonType)GetValue(ComparisonProperty);
            set => SetValue(ComparisonProperty, EnumUtil.GetBoxed(value));
        }
        static void OnComparisionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((DataTrigger)d).Evaulate(e);

        protected override void Evaulate(DependencyPropertyChangedEventArgs e)
        {
            if (_associatedObject == null)
                return;

            if (!ComparisonUtil.Evaluate(Binding, Value, Comparison))
                return;

            base.Evaulate(e);
        }
    }
}
