using Sakuno.SystemLayer;
using Sakuno.UserInterface.Controls;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Shell
{
    class ModernWindowChrome
    {
        public ModernWindow Owner { get; private set; }

        public IntPtr OwnerHandle { get; }

        ModernWindowChromeEdge _leftGlowWindow, _topGlowWindow, _rightGlowWindow, _bottomGlowWindow;
        ModernWindowChromeEdge[] _edges;

        int _deferChangeCount;

        bool _isShowingGlowWindow;
        int _count;

        public ModernWindowChrome(ModernWindow window, IntPtr handle)
        {
            Owner = window;
            OwnerHandle = handle;

            _leftGlowWindow = new ModernWindowChromeEdge(this, handle, Dock.Left);
            _topGlowWindow = new ModernWindowChromeEdge(this, _leftGlowWindow.Window.Handle, Dock.Top);
            _rightGlowWindow = new ModernWindowChromeEdge(this, _topGlowWindow.Window.Handle, Dock.Right);
            _bottomGlowWindow = new ModernWindowChromeEdge(this, _rightGlowWindow.Window.Handle, Dock.Bottom);

            _edges = new[]
            {
                _leftGlowWindow,
                _topGlowWindow,
                _rightGlowWindow,
                _bottomGlowWindow,
            };
        }

        public void Detach()
        {
            Owner = null;

            foreach (var edge in _edges)
                edge.Window.Dispose();
        }

        public void UpdateGlowWindow(bool delayIfNecessary, in NativeStructs.RECT rect)
        {
            using (DeferGlowChanges())
            {
                UpdateGlowWindowVisibilityCore(delayIfNecessary);

                foreach (var edge in _edges)
                    edge.Window.UpdatePosition(rect);
            }
        }
        public void UpdateGlowWindowVisibility(bool delayIfNecessary)
        {
            using (DeferGlowChanges())
                UpdateGlowWindowVisibilityCore(delayIfNecessary);
        }
        public void UpdateGlowWindowRect(in NativeStructs.RECT rect)
        {
            using (DeferGlowChanges())
                foreach (var edge in _edges)
                    edge.Window.UpdatePosition(rect);
        }
        async void UpdateGlowWindowVisibilityCore(bool delayIfNecessary)
        {
            var shouldShow = NativeMethods.User32.IsWindowVisible(OwnerHandle) && !NativeMethods.User32.IsZoomed(OwnerHandle) && !NativeMethods.User32.IsIconic(OwnerHandle) &&
                Owner.ResizeMode != ResizeMode.NoResize;

            if (shouldShow == _isShowingGlowWindow)
                return;

            if (shouldShow && SystemParameters.MinimizeAnimation && delayIfNecessary)
            {
                _count++;

                await Task.Delay(200);

                _count--;

                if (_count == 0)
                    UpdateGlowWindowVisibility(false);
            }
            else
            {
                _isShowingGlowWindow = shouldShow;

                foreach (var edge in _edges)
                    edge.Window.IsVisible = shouldShow;
            }
        }

        public void UpdateGlowWindowZOrder()
        {
            foreach (var edge in _edges)
                edge.Window.UpdateZOrder();
        }

        public Task WaitForDpiChangedHandledAsync()
        {
            var tasks = new Task[_edges.Length];

            for (var i = 0; i < tasks.Length; i++)
                tasks[i] = _edges[i].Window.WaitForDpiChangedHandledAsync();

            return tasks.WhenAll();
        }

        ChangeScope DeferGlowChanges() => new ChangeScope(this);

        struct ChangeScope : IDisposable
        {
            ModernWindowChrome _owner;

            public ChangeScope(ModernWindowChrome owner)
            {
                _owner = owner;
                _owner._deferChangeCount++;
            }

            public void Dispose()
            {
                _owner._deferChangeCount--;

                if (_owner._deferChangeCount != 0)
                    return;

                foreach (var edge in _owner._edges)
                    edge.Window.CommitChanges();
            }
        }
    }
}
