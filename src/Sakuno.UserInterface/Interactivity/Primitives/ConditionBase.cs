using System.Windows;

namespace Sakuno.UserInterface.Interactivity.Primitives
{
    public abstract class ConditionBase : Freezable
    {
        public abstract bool Evaluate();
    }
}
