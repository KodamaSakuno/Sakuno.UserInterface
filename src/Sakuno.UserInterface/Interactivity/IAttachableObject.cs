using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public interface IAttachableObject
    {
        void Attach(DependencyObject target);
        void Detach();
    }
}
