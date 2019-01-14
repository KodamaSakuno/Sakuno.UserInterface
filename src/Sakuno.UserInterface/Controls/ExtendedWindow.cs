using Sakuno.SystemLayer;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Sakuno.UserInterface.Controls
{
    public class ExtendedWindow : Window
    {
        static readonly DependencyPropertyKey ScreenOrientationPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ScreenOrientation), typeof(ScreenOrientation), typeof(ExtendedWindow),
                new UIPropertyMetadata(EnumUtil.GetBoxed(ScreenOrientation.Landscape)));

        public static readonly DependencyProperty ScreenOrientationProperty = ScreenOrientationPropertyKey.DependencyProperty;

        public ScreenOrientation ScreenOrientation
        {
            get => (ScreenOrientation)GetValue(ScreenOrientationProperty);
            private set => SetValue(ScreenOrientationPropertyKey, EnumUtil.GetBoxed(value));
        }

        public static readonly DependencyProperty AutoRotationProperty =
            DependencyProperty.Register(nameof(AutoRotation), typeof(bool), typeof(ExtendedWindow),
                new PropertyMetadata(BoxedConstants.Boolean.False));

        public bool AutoRotation
        {
            get => (bool)GetValue(AutoRotationProperty);
            set => SetValue(AutoRotationProperty, BooleanUtil.GetBoxed(value));
        }

        protected IntPtr Handle { get; private set; }

        static ExtendedWindow()
        {
            SnapsToDevicePixelsProperty.OverrideMetadata(typeof(ExtendedWindow), new FrameworkPropertyMetadata(BoxedConstants.Boolean.True));
            UseLayoutRoundingProperty.OverrideMetadata(typeof(ExtendedWindow), new FrameworkPropertyMetadata(BoxedConstants.Boolean.True));
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            Handle = new WindowInteropHelper(this).Handle;

            HwndSource.FromHwnd(Handle).AddHook(WndProc);

            InitializeScreenOrientation();
        }
        void InitializeScreenOrientation()
        {
            var info = new NativeStructs.MONITORINFO() { cbSize = Marshal.SizeOf<NativeStructs.MONITORINFO>() };
            var monitor = NativeMethods.User32.MonitorFromWindow(Handle, NativeConstants.MFW.MONITOR_DEFAULTTONEAREST);

            NativeMethods.User32.GetMonitorInfo(monitor, ref info);

            var width = info.rcMonitor.Width;
            var height = info.rcMonitor.Height;
            var orientation = width > height ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;

            ScreenOrientation = orientation;
        }

        unsafe IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch ((NativeConstants.WindowMessage)msg)
            {
                case NativeConstants.WindowMessage.WM_DISPLAYCHANGE:
                    var screenWidth = lParam.LoWord();
                    var screenHeight = lParam.HiWord();
                    var orientation = screenWidth > screenHeight ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;

                    if (orientation != ScreenOrientation)
                    {
                        ScreenOrientation = orientation;

                        if (AutoRotation)
                        {
                            NativeMethods.User32.GetWindowRect(Handle, out var rect);
                            NativeMethods.User32.SetWindowPos(Handle, IntPtr.Zero, rect.Left, rect.Top, rect.Height, rect.Width, NativeEnums.SetWindowPosition.SWP_NOZORDER | NativeEnums.SetWindowPosition.SWP_NOACTIVATE);
                        }
                    }
                    break;
            }

            return IntPtr.Zero;
        }
    }
}
