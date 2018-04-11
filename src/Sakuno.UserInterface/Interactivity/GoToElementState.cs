using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public class GoToElementState : VisualStateTriggerAction
    {
        protected override void Invoke(object args)
        {
            if (_associatedObject == null)
                return;

            VisualStateManager.GoToElementState(Target, State, UseTransitions);
        }
    }
}
