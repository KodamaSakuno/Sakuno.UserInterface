using System.Windows;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Interactivity
{
    [ContentProperty(nameof(Conditions))]
    public class ConditionGroup : ConditionBase
    {
        static readonly DependencyPropertyKey ConditionsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Conditions), typeof(ConditionCollection), typeof(ConditionGroup), null);

        public static readonly DependencyProperty ConditionsProperty = ConditionsPropertyKey.DependencyProperty;

        ConditionCollection _conditions;
        public ConditionCollection Conditions => _conditions;

        public static readonly DependencyProperty CombinationTypeProperty =
            DependencyProperty.Register(nameof(CombinationType), typeof(ConditionCombinationType), typeof(ConditionGroup),
                new PropertyMetadata(EnumUtil.GetBoxed(ConditionCombinationType.And)));

        public ConditionCombinationType CombinationType
        {
            get => (ConditionCombinationType)GetValue(CombinationTypeProperty);
            set => SetValue(CombinationTypeProperty, EnumUtil.GetBoxed(value));
        }

        public ConditionGroup()
        {
            SetValue(ConditionsPropertyKey, _conditions = new ConditionCollection());
        }

        public override bool Evaluate()
        {
            var combinationType = CombinationType;

            var result = false;

            foreach (var condition in _conditions)
            {
                result = condition.Evaluate();

                if (!result && combinationType == ConditionCombinationType.And)
                    return false;

                if (result && combinationType == ConditionCombinationType.Or)
                    return true;
            }

            return result;
        }

        protected override Freezable CreateInstanceCore() => new ConditionGroup();
    }
}
