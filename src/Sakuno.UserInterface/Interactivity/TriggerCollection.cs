using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public sealed class TriggerCollection : AttachableObjectCollection<Trigger>
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

        internal override void OnItemAdded(Trigger item) => item.Attach(_associatedObject);
        internal override void OnItemRemoved(Trigger item)
        {
            if (item._associatedObject == null)
                return;

            item.Detach();
        }

        protected override Freezable CreateInstanceCore() => new TriggerCollection();
    }
}
