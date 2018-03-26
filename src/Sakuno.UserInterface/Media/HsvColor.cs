using System;
using System.Windows.Media;

namespace Sakuno.UserInterface.Media
{
    public struct HsvColor : IEquatable<HsvColor>
    {
        short _h;
        public short H
        {
            get => _h;
            set
            {
                if (value < 0 || value >= 360)
                    throw new ArgumentOutOfRangeException();

                _h = value;
            }
        }

        byte _s;
        public byte S
        {
            get => _s;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException();

                _s = value;
            }
        }

        byte _v;
        public byte V
        {
            get => _v;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException();

                _v = value;
            }
        }

        public HsvColor(short h, byte s, byte v)
        {
            if (h < 0 || h >= 360)
                throw new ArgumentOutOfRangeException(nameof(h));
            if (s < 0 || s > 100)
                throw new ArgumentOutOfRangeException(nameof(s));
            if (v < 0 || v > 100)
                throw new ArgumentOutOfRangeException(nameof(v));

            _h = h;
            _s = s;
            _v = v;
        }

        public static HsvColor FromColor(Color color) =>
            FromRgb(color.R, color.G, color.B);
        public static HsvColor FromRgb(byte r, byte g, byte b)
        {
            var max = MathUtil.Max(r, g, b);
            var min = MathUtil.Min(r, g, b);
            var delta = max - min;

            double h, s;

            if (max == 0 || delta == 0)
                h = s = .0;
            else
            {
                s = delta / (double)max * 100.0;

                if (max == r)
                    h = (g - b) / delta;
                else if (max == g)
                    h = (b - r) / delta + 2;
                else
                    h = (r - g) / delta + 4;

                h *= 60;
            }

            if (h < 0)
                h += 360;

            var v = max / 2.55;

            return new HsvColor((short)h, (byte)s, (byte)v);
        }

        public Color ToColor()
        {
            double r, g, b;

            var v = _v * .01;

            if (_s == 0)
                r = g = b = v;
            else
            {
                var s = _s * .01;
                var sector = _h / 60.0;
                var sectorIndex = (int)sector;
                var frac = sector - sectorIndex;
                var p = v * (1 - s);
                var q = v * (1 - s * frac);
                var t = v * (1 - s * (1 - frac));

                switch (sectorIndex)
                {
                    case 0:
                        r = v;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = v;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = v;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = v;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = v;
                        break;

                    case 5:
                        r = v;
                        g = p;
                        b = q;
                        break;

                    default:
                        r = g = b = 0;
                        break;
                }
            }

            return Color.FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

        public override bool Equals(object obj) =>
            obj is HsvColor hsv && Equals(hsv);
        public bool Equals(HsvColor other) =>
            _h == other._h && _s == other._s && _v == other._v;

        public static bool operator ==(HsvColor x, HsvColor y) => x.Equals(y);
        public static bool operator !=(HsvColor x, HsvColor y) => !x.Equals(y);

        public override int GetHashCode() => (_h * 401) ^ (_s * 401) ^ _v;
    }
}
