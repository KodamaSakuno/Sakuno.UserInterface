using System.Windows;

namespace Sakuno.UserInterface.Interactivity.Primitives
{
    public sealed class TriggerActionCollection : AttachableObjectCollection<TriggerAction>
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

        internal override void OnItemAdded(TriggerAction item)
        {
            if (item._associatedObject == null)
                return;

            item.Attach(_associatedObject);
        }

        internal override void OnItemRemoved(TriggerAction item)
        {
            if (item._associatedObject == null)
                return;

            item.Detach();
        }

        protected override Freezable CreateInstanceCore() => new TriggerActionCollection();
    }
}
