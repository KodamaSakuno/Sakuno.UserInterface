using Sakuno.SystemLayer;
using System;

namespace Sakuno.UserInterface
{
    public static class DpiUtil
    {
        public static Dpi SystemPrimary { get; }

        static DpiUtil()
        {
            var hdc = NativeMethods.User32.GetDC(IntPtr.Zero);
            if (hdc == IntPtr.Zero)
            {
                SystemPrimary = Dpi.Default;
                return;
            }

            var x = NativeMethods.Gdi32.GetDeviceCaps(hdc, NativeConstants.DeviceCap.LOGPIXELSX);
            var y = NativeMethods.Gdi32.GetDeviceCaps(hdc, NativeConstants.DeviceCap.LOGPIXELSY);

            SystemPrimary = new Dpi(x, y);

            NativeMethods.User32.ReleaseDC(IntPtr.Zero, hdc);
        }
    }
}
