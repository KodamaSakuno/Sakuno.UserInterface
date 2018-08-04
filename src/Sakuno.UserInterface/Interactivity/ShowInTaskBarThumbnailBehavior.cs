using Sakuno.UserInterface.Interactivity.Primitives;
using System;
using System.Windows;
using System.Windows.Shell;

namespace Sakuno.UserInterface.Interactivity
{
    public sealed class ShowInTaskBarThumbnailBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;
        }
        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            UodateThumbnail();

            AssociatedObject.LayoutUpdated += AssociatedObject_LayoutUpdated;
        }
        void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            ClearThumbnail();

            AssociatedObject.LayoutUpdated -= AssociatedObject_LayoutUpdated;
        }
        void AssociatedObject_LayoutUpdated(object sender, EventArgs e) => UodateThumbnail();

        void UodateThumbnail()
        {
            var element = AssociatedObject;
            if (PresentationSource.FromVisual(element) == null)
                return;

            var window = Window.GetWindow(element);
            if (window == null)
                return;

            if (window.TaskbarItemInfo == null)
                window.TaskbarItemInfo = new TaskbarItemInfo();

            var position = element.PointToScreen(default);
            position = window.PointFromScreen(position);

            var right = window.ActualWidth - position.X - element.ActualWidth;
            var bottom = window.ActualHeight - position.Y - element.ActualHeight;

            window.TaskbarItemInfo.ThumbnailClipMargin = new Thickness(position.X, position.Y, right, bottom);
        }
        void ClearThumbnail()
        {
            var window = Window.GetWindow(AssociatedObject);
            if (window == null || window.TaskbarItemInfo == null)
                return;

            window.TaskbarItemInfo.ThumbnailClipMargin = new Thickness();
        }
    }
}
