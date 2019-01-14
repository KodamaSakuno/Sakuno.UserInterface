using Sakuno.SystemLayer;
using Sakuno.UserInterface.Shell;
using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Shell;

namespace Sakuno.UserInterface.Controls
{
    public class ModernWindow : ExtendedWindow
    {
        static WindowChrome _defaultWindowChrome;

        public static readonly DependencyProperty HideDefaultTitleBarProperty =
            DependencyProperty.Register(nameof(HideDefaultTitleBar), typeof(bool), typeof(ModernWindow),
                new PropertyMetadata(BoxedConstants.Boolean.False));

        public bool HideDefaultTitleBar
        {
            get => (bool)GetValue(HideDefaultTitleBarProperty);
            set => SetValue(HideDefaultTitleBarProperty, BooleanUtil.GetBoxed(value));
        }

        ModernWindowChrome _chrome;

        bool _suspendWindowPosChanged;

        static ModernWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernWindow), new FrameworkPropertyMetadata(typeof(ModernWindow)));

            _defaultWindowChrome = new WindowChrome()
            {
                CornerRadius = default,
                GlassFrameThickness = default,
            };
        }
        public ModernWindow()
        {
            WindowChrome.SetWindowChrome(this, _defaultWindowChrome);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            HwndSource.FromHwnd(Handle).AddHook(WndProc);

            _chrome = new ModernWindowChrome(this, Handle);
        }

        unsafe IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch ((NativeConstants.WindowMessage)msg)
            {
                case NativeConstants.WindowMessage.WM_WINDOWPOSCHANGED:
                    if (!_suspendWindowPosChanged)
                        OnWindowPositionChanged(lParam);
                    break;

                case NativeConstants.WindowMessage.WM_DPICHANGED:
                    var suggestedRect = *(NativeStructs.RECT*)lParam;
                    OnDpiChanged(suggestedRect);
                    break;
            }

            return IntPtr.Zero;
        }

        protected override void OnClosed(EventArgs e)
        {
            _chrome.Detach();

            base.OnClosed(e);
        }

        unsafe void OnWindowPositionChanged(IntPtr lParam)
        {
            var info = (NativeStructs.WINDOWPOS*)lParam;

            var isShowing = (info->flags & NativeEnums.SetWindowPosition.SWP_SHOWWINDOW) == 0;
            var rect = new NativeStructs.RECT(info->x, info->y, info->x + info->cx, info->y + info->cy);

            _chrome.UpdateGlowWindow(isShowing, rect);
            _chrome.UpdateGlowWindowZOrder();
        }

        async void OnDpiChanged(NativeStructs.RECT suggestedRect)
        {
            _suspendWindowPosChanged = true;

            await _chrome.WaitForDpiChangedHandledAsync();

            _suspendWindowPosChanged = false;

            _chrome.UpdateGlowWindowRect(suggestedRect);
        }
    }
}
