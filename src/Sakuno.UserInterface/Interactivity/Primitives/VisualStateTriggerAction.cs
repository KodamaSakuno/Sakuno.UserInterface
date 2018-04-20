using System.Windows;

namespace Sakuno.UserInterface.Interactivity.Primitives
{
    public abstract class VisualStateTriggerAction : TargetedTriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty UseTransitionsProperty =
            DependencyProperty.Register(nameof(UseTransitions), typeof(bool), typeof(VisualStateTriggerAction),
                new PropertyMetadata(BooleanUtil.True));

        public bool UseTransitions
        {
            get => (bool)GetValue(UseTransitionsProperty);
            set => SetValue(UseTransitionsProperty, BooleanUtil.GetBoxed(value));
        }

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(nameof(State), typeof(string), typeof(VisualStateTriggerAction));

        public string State
        {
            get => (string)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }
    }
}
