using Sakuno.UserInterface.Interactivity.Primitives;
using System;
using System.Windows;
using System.Windows.Input;

namespace Sakuno.UserInterface.Interactivity
{
    public sealed class MouseDoubleClickCondition : ConditionBase
    {
        public override bool Evaluate(object args)
        {
            if (!(args is MouseButtonEventArgs e) ||
                (e.RoutedEvent != UIElement.PreviewMouseDownEvent &&
                 e.RoutedEvent != UIElement.PreviewMouseLeftButtonDownEvent &&
                 e.RoutedEvent != UIElement.PreviewMouseRightButtonDownEvent &&
                 e.RoutedEvent != UIElement.MouseDownEvent &&
                 e.RoutedEvent != UIElement.MouseLeftButtonDownEvent &&
                 e.RoutedEvent != UIElement.MouseRightButtonDownEvent))
                throw new InvalidOperationException("The MouseDoubleClickCondition must be declared as a condition of event trigger for mouse down events.");

            return e.ClickCount % 2 == 0;
        }

        protected override Freezable CreateInstanceCore() => new MouseDoubleClickCondition();
    }
}
