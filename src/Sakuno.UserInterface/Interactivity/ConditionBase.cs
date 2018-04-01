using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public abstract class ConditionBase : Freezable
    {
        public abstract bool Evaluate();
    }
}
