using Sakuno.SystemLayer;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Sakuno.UserInterface.Shell
{
    class GlowWindow : HwndSource
    {
        [Flags]
        enum InvalidationType
        {
            None,
            Position = 1,
            Size = 1 << 1,
            Visibility = 1 << 2,
        }

        ModernWindowChrome _owner;
        IntPtr _ownerHandle;
        IntPtr _previousWindow;

        const int GlowSize = 8;

        Dock _position;

        InvalidationType _invalidationType;

        int _left;
        public int Left
        {
            get => _left;
            set => UpdateProperty(ref _left, value, InvalidationType.Position);
        }

        int _top;
        public int Top
        {
            get => _top;
            set => UpdateProperty(ref _top, value, InvalidationType.Position);
        }

        int _width;
        public int Width
        {
            get => _width;
            set => UpdateProperty(ref _width, value, InvalidationType.Size);
        }

        int _height;
        public int Height
        {
            get => _height;
            set => UpdateProperty(ref _height, value, InvalidationType.Size);
        }

        bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set => UpdateProperty(ref _isVisible, value, InvalidationType.Visibility);
        }

        NativeStructs.RECT _ownerRect;

        TaskCompletionSource<object> _tcs;

        public GlowWindow(ModernWindowChrome owner, IntPtr previousWindow, Dock position) : base(CreateParameters(owner.OwnerHandle))
        {
            _owner = owner;
            _ownerHandle = owner.OwnerHandle;
            _previousWindow = previousWindow;

            _position = position;

            SizeToContent = SizeToContent.Manual;

            AddHook(WndProc);
        }
        static HwndSourceParameters CreateParameters(IntPtr ownerHandle) =>
            new HwndSourceParameters()
            {
                WindowClassStyle = 8, // CS_DBLCLKS
                WindowStyle = unchecked((int)(NativeEnums.WindowStyles.WS_POPUP | NativeEnums.WindowStyles.WS_CLIPSIBLINGS | NativeEnums.WindowStyles.WS_CLIPCHILDREN)),
                ExtendedWindowStyle = (int)(NativeEnums.ExtendedWindowStyles.WS_EX_LAYERED | NativeEnums.ExtendedWindowStyles.WS_EX_TOOLWINDOW),
                UsesPerPixelOpacity = true,
            };

        public void UpdatePosition(in NativeStructs.RECT rect)
        {
            _ownerRect = rect;

            if (!_isVisible)
                return;

            var glowSizeX = (int)Math.Round(GlowSize * CompositionTarget.TransformToDevice.M11);
            var glowSizeY = (int)Math.Round(GlowSize * CompositionTarget.TransformToDevice.M22);

            switch (_position)
            {
                case Dock.Left:
                    Left = _ownerRect.Left - glowSizeX;
                    Top = _ownerRect.Top - 1;
                    Width = glowSizeX;
                    Height = _ownerRect.Height + 2;
                    break;

                case Dock.Top:
                    Left = _ownerRect.Left - glowSizeX - 1;
                    Top = _ownerRect.Top - glowSizeY;
                    Width = _ownerRect.Width + glowSizeX * 2 + 2;
                    Height = glowSizeY;
                    break;

                case Dock.Right:
                    Left = _ownerRect.Left + _ownerRect.Width;
                    Top = _ownerRect.Top - 1;
                    Width = glowSizeX;
                    Height = _ownerRect.Height + 2;
                    break;

                case Dock.Bottom:
                    Left = _ownerRect.Left - glowSizeY - 1;
                    Top = _ownerRect.Top + _ownerRect.Height;
                    Width = _ownerRect.Width + glowSizeX * 2 + 2;
                    Height = glowSizeY;
                    break;

                default: throw new InvalidOperationException();
            }
        }

        void UpdateProperty<T>(ref T field, T value, InvalidationType type) where T : struct, IEquatable<T>
        {
            if (field.Equals(value))
                return;

            field = value;

            _invalidationType |= type;
        }

        public void CommitChanges()
        {
            if (_invalidationType.HasAny(InvalidationType.Position | InvalidationType.Size | InvalidationType.Visibility))
            {
                var flags = NativeEnums.SetWindowPosition.SWP_NOZORDER | NativeEnums.SetWindowPosition.SWP_NOACTIVATE;

                if (!_invalidationType.Has(InvalidationType.Position))
                    flags |= NativeEnums.SetWindowPosition.SWP_NOMOVE;

                if (!_invalidationType.Has(InvalidationType.Size))
                    flags |= NativeEnums.SetWindowPosition.SWP_NOSIZE;

                if (_invalidationType.Has(InvalidationType.Visibility))
                    if (_isVisible)
                        flags |= NativeEnums.SetWindowPosition.SWP_SHOWWINDOW;
                    else
                        flags |= NativeEnums.SetWindowPosition.SWP_HIDEWINDOW | NativeEnums.SetWindowPosition.SWP_NOSIZEORMOVE;

                NativeMethods.User32.SetWindowPos(Handle, IntPtr.Zero, _left, _top, _width, _height, flags);
            }

            _invalidationType = InvalidationType.None;
        }

        public void UpdateZOrder() =>
            NativeMethods.User32.SetWindowPos(Handle, _previousWindow, 0, 0, 0, 0, NativeEnums.SetWindowPosition.SWP_NOSIZEORMOVE | NativeEnums.SetWindowPosition.SWP_NOACTIVATE);

        unsafe IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var message = (NativeConstants.WindowMessage)msg;

            switch ((NativeConstants.WindowMessage)msg)
            {
                case NativeConstants.WindowMessage.WM_MOUSEACTIVATE:
                    handled = true;
                    return (IntPtr)NativeConstants.MouseActivate.MA_NOACTIVATE;

                case NativeConstants.WindowMessage.WM_LBUTTONDOWN:
                case NativeConstants.WindowMessage.WM_LBUTTONDBLCLK:
                case NativeConstants.WindowMessage.WM_RBUTTONDOWN:
                case NativeConstants.WindowMessage.WM_RBUTTONDBLCLK:
                case NativeConstants.WindowMessage.WM_MBUTTONDOWN:
                case NativeConstants.WindowMessage.WM_MBUTTONDBLCLK:
                case NativeConstants.WindowMessage.WM_XBUTTONDOWN:
                case NativeConstants.WindowMessage.WM_XBUTTONDBLCLK:
                    handled = true;

                    NativeMethods.User32.PostMessageW(_ownerHandle, NativeConstants.WindowMessage.WM_ACTIVATE, (IntPtr)NativeConstants.MouseActivate.MA_ACTIVATEANDEAT, IntPtr.Zero);
                    NativeMethods.User32.PostMessageW(_ownerHandle, message - (NativeConstants.WindowMessage.WM_LBUTTONDOWN - NativeConstants.WindowMessage.WM_NCLBUTTONDOWN), (IntPtr)GetHitTest(lParam), IntPtr.Zero);
                    break;
            }

            return IntPtr.Zero;
        }

        NativeConstants.HitTest GetHitTest(IntPtr lParam)
        {
            var x = lParam.LoWord();
            var y = lParam.HiWord();

            const int GlowSize = 8;

            var glowSizeX = (int)Math.Round(GlowSize * CompositionTarget.TransformToDevice.M11);
            var glowSizeY = (int)Math.Round(GlowSize * CompositionTarget.TransformToDevice.M22);

            NativeMethods.User32.GetWindowRect(Handle, out var rect);

            switch (_position)
            {
                case Dock.Left:
                    if (y < glowSizeY)
                        return NativeConstants.HitTest.HTTOPLEFT;

                    if (y > rect.Height - glowSizeY)
                        return NativeConstants.HitTest.HTBOTTOMLEFT;

                    return NativeConstants.HitTest.HTLEFT;

                case Dock.Top:
                    if (x < glowSizeX * 2 + 1)
                        return NativeConstants.HitTest.HTTOPLEFT;

                    if (x > rect.Width - glowSizeX * 2 - 1)
                        return NativeConstants.HitTest.HTTOPRIGHT;

                    return NativeConstants.HitTest.HTTOP;

                case Dock.Right:
                    if (y < glowSizeY * 2)
                        return NativeConstants.HitTest.HTTOPRIGHT;

                    if (y > rect.Height - glowSizeY)
                        return NativeConstants.HitTest.HTBOTTOMRIGHT;

                    return NativeConstants.HitTest.HTRIGHT;

                case Dock.Bottom:
                    if (x < glowSizeX * 2 + 1)
                        return NativeConstants.HitTest.HTBOTTOMLEFT;

                    if (x > rect.Width - glowSizeX * 2 - 1)
                        return NativeConstants.HitTest.HTBOTTOMRIGHT;

                    return NativeConstants.HitTest.HTBOTTOM;
            }

            throw new InvalidOperationException();
        }

        public Task WaitForDpiChangedHandledAsync()
        {
            _tcs = new TaskCompletionSource<object>();

            return _tcs.Task;
        }

        protected override void OnDpiChanged(HwndDpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);

            if (_tcs == null)
                return;

            _tcs.TrySetResult(null);
            _tcs = null;
        }
    }
}
