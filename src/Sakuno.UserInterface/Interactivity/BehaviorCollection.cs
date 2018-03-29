using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public sealed class BehaviorCollection : AttachableObjectCollection<Behavior>
    {
        protected override void OnAttached()
        {
            foreach (var item in this)
                item.Attach(AssociatedObject);
        }

        protected override void OnDetaching()
        {
            foreach (var item in this)
                item.Detach();
        }

        internal override void OnItemAdded(Behavior item) => item.Attach(_associatedObject);
        internal override void OnItemRemoved(Behavior item)
        {
            if (item._associatedObject == null)
                return;

            item.Detach();
        }

        protected override Freezable CreateInstanceCore() => new BehaviorCollection();
    }
}
