using System;
using System.Windows.Media;

namespace Sakuno.UserInterface
{
    public struct Dpi : IEquatable<Dpi>
    {
        public static readonly Dpi Default = new Dpi(96, 96);

        public int X { get; }
        public int Y { get; }

        public double ScaleX { get; }
        public double ScaleY { get; }

        public Transform ScaleTransform => this == Default ? Transform.Identity : new ScaleTransform(ScaleX, ScaleY);

        public Dpi(int x, int y)
        {
            X = x;
            Y = y;

            ScaleX = x / (double)Default.X;
            ScaleY = y / (double)Default.Y;
        }
        public Dpi(double scaleX, double scaleY)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;

            X = (int)Math.Round(ScaleX * 96);
            Y = (int)Math.Round(ScaleY * 96);
        }

        public override bool Equals(object obj) => obj is Dpi dpi && Equals(dpi);
        public bool Equals(Dpi other) => X == other.X && Y == other.Y;

        public static bool operator ==(Dpi x, Dpi y) => x.Equals(y);
        public static bool operator !=(Dpi x, Dpi y) => !x.Equals(y);

        public override int GetHashCode() => (X * 401) ^ Y;
    }
}
