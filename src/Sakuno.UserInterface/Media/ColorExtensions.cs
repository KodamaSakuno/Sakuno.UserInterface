using System.Windows.Media;

namespace Sakuno.UserInterface.Media
{
    public static class ColorExtensions
    {
        public static byte GetLuminosity(this Color color) =>
            (byte)(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
    }
}
