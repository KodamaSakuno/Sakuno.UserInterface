using System.Windows;

namespace Sakuno.UserInterface.Interactivity.Primitives
{
    public interface IAttachableObject
    {
        void Attach(DependencyObject target);
        void Detach();
    }
}
