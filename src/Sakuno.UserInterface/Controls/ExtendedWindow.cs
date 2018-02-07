using Sakuno.SystemLayer;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

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
                new PropertyMetadata(BooleanUtil.False));

        public bool AutoRotation
        {
            get => (bool)GetValue(AutoRotationProperty);
            set => SetValue(AutoRotationProperty, BooleanUtil.GetBoxed(value));
        }

        static readonly DependencyPropertyKey DpiScaleTransformPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(DpiScaleTransform), typeof(Transform), typeof(ExtendedWindow),
                new UIPropertyMetadata(Transform.Identity));

        public static readonly DependencyProperty DpiScaleTransformProperty = DpiScaleTransformPropertyKey.DependencyProperty;

        public Transform DpiScaleTransform
        {
            get => (Transform)GetValue(DpiScaleTransformProperty);
            private set => SetValue(DpiScaleTransformPropertyKey, value);
        }

        protected IntPtr Handle { get; private set; }

        Dpi _systemDpi, _currentDpi;

        static ExtendedWindow()
        {
            SnapsToDevicePixelsProperty.OverrideMetadata(typeof(ExtendedWindow), new FrameworkPropertyMetadata(BooleanUtil.True));
            UseLayoutRoundingProperty.OverrideMetadata(typeof(ExtendedWindow), new FrameworkPropertyMetadata(BooleanUtil.True));
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            Handle = new WindowInteropHelper(this).Handle;

            var hwndSource = HwndSource.FromHwnd(Handle);

            var monitor = NativeMethods.User32.MonitorFromWindow(Handle, NativeConstants.MFW.MONITOR_DEFAULTTONEAREST);

            InitializeDpi(hwndSource.CompositionTarget.TransformToDevice, monitor);
            InitializeScreenOrientation(monitor);

            hwndSource.AddHook(WndProc);
        }
        void InitializeDpi(Matrix transformation, IntPtr monitor)
        {
            var x = Dpi.Default.X * transformation.M11;
            var y = Dpi.Default.Y * transformation.M22;

            _systemDpi = new Dpi((int)x, (int)y);

            if (!OS.IsWin8Point1OrLater)
            {
                _currentDpi = _systemDpi;
                return;
            }

            NativeMethods.SHCore.GetDpiForMonitor(monitor, NativeConstants.MONITOR_DPI_TYPE.MDT_EFFECTIVE_DPI, out var dpiX, out var dpiY);

            ChangeDpi(new Dpi(dpiX, dpiY));
        }
        void InitializeScreenOrientation(IntPtr monitor)
        {
            var info = new NativeStructs.MONITORINFO() { cbSize = Marshal.SizeOf<NativeStructs.MONITORINFO>() };

            NativeMethods.User32.GetMonitorInfo(monitor, ref info);

            var width = info.rcMonitor.Width;
            var height = info.rcMonitor.Height;
            var orientation = width > height ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;

            ScreenOrientation = orientation;
        }

        void ChangeDpi(Dpi dpi)
        {
            DpiScaleTransform = dpi == _systemDpi ? Transform.Identity
                                                  : new ScaleTransform(dpi.X / (double)_systemDpi.X, dpi.Y / (double)_systemDpi.Y);

            _currentDpi = dpi;
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

                case NativeConstants.WindowMessage.WM_DPICHANGED:
                    var dpiX = NativeUtils.LoWord(wParam);
                    var dpiY = NativeUtils.HiWord(wParam);
                    var newRect = (NativeStructs.RECT*)lParam;

                    ChangeDpi(new Dpi(dpiX, dpiY));

                    NativeMethods.User32.SetWindowPos(Handle, IntPtr.Zero, newRect->Left, newRect->Top, newRect->Width, newRect->Height, NativeEnums.SetWindowPosition.SWP_NOZORDER | NativeEnums.SetWindowPosition.SWP_NOACTIVATE);
                    break;
            }

            return IntPtr.Zero;
        }
    }
}
