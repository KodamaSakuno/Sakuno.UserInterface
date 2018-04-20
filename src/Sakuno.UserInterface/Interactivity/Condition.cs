using Sakuno.UserInterface.Interactivity.Primitives;
using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public class Condition : ConditionBase
    {
        public static readonly DependencyProperty LeftOperandProperty =
            DependencyProperty.Register(nameof(LeftOperand), typeof(object), typeof(Condition));

        public object LeftOperand
        {
            get => (GetValue(LeftOperandProperty));
            set => SetValue(LeftOperandProperty, value);
        }

        public static readonly DependencyProperty RightOperandProperty =
            DependencyProperty.Register(nameof(RightOperand), typeof(object), typeof(Condition));

        public object RightOperand
        {
            get => GetValue(RightOperandProperty);
            set => SetValue(RightOperandProperty, value);
        }

        public static readonly DependencyProperty OperatorProperty =
            DependencyProperty.Register(nameof(Operator), typeof(ComparisonType), typeof(Condition),
                new PropertyMetadata(EnumUtil.GetBoxed(ComparisonType.Equal)));

        public ComparisonType Operator
        {
            get => (ComparisonType)GetValue(OperatorProperty);
            set => SetValue(OperatorProperty, EnumUtil.GetBoxed(value));
        }

        public override bool Evaluate()
        {
            this.UpdateBindingTarget(LeftOperandProperty);
            this.UpdateBindingTarget(RightOperandProperty);
            this.UpdateBindingTarget(OperatorProperty);

            return ComparisonUtil.Evaluate(LeftOperand, RightOperand, Operator);
        }

        protected override Freezable CreateInstanceCore() => new Condition();
    }
}
