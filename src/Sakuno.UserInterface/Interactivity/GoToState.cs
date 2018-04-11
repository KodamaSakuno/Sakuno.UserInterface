using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public class GoToState : VisualStateTriggerAction
    {
        protected override void Invoke(object args)
        {
            if (_associatedObject == null)
                return;

            VisualStateManager.GoToState(Target, State, UseTransitions);
        }
    }
}
