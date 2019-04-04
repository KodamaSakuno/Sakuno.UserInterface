using Sakuno.UserInterface.Controls.Primitives;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Sakuno.UserInterface.Controls
{
    [TemplatePart(Name = "PART_ToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    public class SplitButton : ButtonWithPopup
    {
        public static readonly DependencyProperty CommandProperty =
            Button.CommandProperty.AddOwner(typeof(SplitButton));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty =
            Button.CommandParameterProperty.AddOwner(typeof(SplitButton));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static readonly DependencyProperty CommandTargetProperty =
            Button.CommandTargetProperty.AddOwner(typeof(SplitButton));

        public IInputElement CommandTarget
        {
            get => (IInputElement)GetValue(CommandTargetProperty);
            set => SetValue(CommandTargetProperty, value);
        }

        static SplitButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButton), new FrameworkPropertyMetadata(typeof(SplitButton)));
        }
    }
}
