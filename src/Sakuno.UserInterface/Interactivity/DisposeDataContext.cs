using Sakuno.UserInterface.Interactivity.Primitives;
using System;
using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public sealed class DisposeDataContext : TriggerAction<FrameworkElement>
    {
        protected override void Invoke(object args) =>
            (AssociatedObject.DataContext as IDisposable)?.Dispose();
    }
}
