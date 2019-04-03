using Sakuno.SystemLayer;
using System;
using System.Windows;
using System.Windows.Media;

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

        public static Dpi GetDpi(Visual visual)
        {
            var result = VisualTreeHelper.GetDpi(visual);

            return new Dpi(result.DpiScaleX, result.DpiScaleY);
        }

        public static double RoundLayoutValue(double value, double scale)
        {
            if (DoubleUtil.IsCloseToOne(scale))
                return Math.Round(value);

            const long EncodingOfDoubleMaxValue = 0x7FEFFFFFFFFFFFFFL;

            var result = Math.Round(value * scale) / scale;
            if ((BitConverter.DoubleToInt64Bits(result) & long.MaxValue) < EncodingOfDoubleMaxValue)
                return result;

            return value;
        }

        public static Size RoundLayoutValue(in Size size, in DpiScale dpiScale) =>
            new Size(RoundLayoutValue(size.Width, dpiScale.DpiScaleX), RoundLayoutValue(size.Height, dpiScale.DpiScaleY));
        public static Point RoundLayoutValue(in Point point, in DpiScale dpiScale) =>
            new Point(RoundLayoutValue(point.X, dpiScale.DpiScaleX), RoundLayoutValue(point.Y, dpiScale.DpiScaleY));
        public static Thickness RoundLayoutValue(in Thickness thickness, in DpiScale dpiScale) =>
            new Thickness(
                RoundLayoutValue(thickness.Left, dpiScale.DpiScaleX),
                RoundLayoutValue(thickness.Top, dpiScale.DpiScaleY),
                RoundLayoutValue(thickness.Right, dpiScale.DpiScaleX),
                RoundLayoutValue(thickness.Bottom, dpiScale.DpiScaleY));
    }
}
