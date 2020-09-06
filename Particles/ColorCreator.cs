using System;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;

namespace Particles
{
    [Serializable]
    [TypeConverter(typeof(ColorCreatorConverter))]
    [XmlInclude(typeof(RangeColors))]
    [XmlInclude(typeof(Fun1))]
    [XmlInclude(typeof(Fun2))]
    [XmlInclude(typeof(FromImage))]
    public abstract class ColorCreator
    {
        public abstract Color GetColor(float x, float y, float Alpha);

        private static float[] hsvRanges => new float[] { 0 / 6f, 1 / 6f, 2 / 6f, 3 / 6f, 4 / 6f, 5 / 6f, 6 / 6f };

        private static Color[] hsvColors => new Color[] { rgb(255, 0, 0), rgb(255, 255, 0), rgb(0, 255, 0), rgb(0, 255, 255), rgb(0, 0, 255), rgb(255, 0, 255), rgb(255, 0, 0) };
#pragma warning disable IDE0032
        private static readonly ColorCreator
            rgb1 = new Fun1(),
            rgb2 = new Fun2(),
            hsv1 = new RangeColors(hsvRanges, hsvColors, 1, 0),
            hsv2 = new RangeColors(hsvRanges, hsvColors, 0, 1),
            rus = new RangeColors(
                new float[] { 0, 1 / 3f, 1 / 3f, 2 / 3f, 2 / 3f, 1 },
                new Color[] { rgb(255, 255, 255), rgb(255, 255, 255), rgb(0, 57, 166), rgb(0, 57, 166), rgb(213, 43, 30), rgb(213, 43, 30) },
                0, 1, false),
            ukr = new RangeColors(
                new float[] { 0, 1 / 2f, 1 / 2f, 1 },
                new Color[] { rgb(0, 87, 184), rgb(0, 87, 184), rgb(255, 215, 0), rgb(255, 215, 0), },
                0, 1, false),
            fimg = new FromImage();
#pragma warning restore IDE0032
        public static ColorCreator RGB1 => rgb1;
        public static ColorCreator RGB2 => rgb2;
        public static ColorCreator HSVx => hsv1;
        public static ColorCreator HSVy => hsv2;
        public static ColorCreator RUSf => rus;
        public static ColorCreator UKRf => ukr;
        public static ColorCreator FIMG => fimg;

        private static Color rgb(int r, int g, int b)
        {
            return Color.FromArgb(r, g, b);
        }

        private static Color argb(float a, float r, float g, float b)
        {
            return Color.FromArgb((byte)(255 * a), (byte)(255 * r), (byte)(255 * g), (byte)(255 * b));
        }

        [Serializable]
        public class Fun1 : ColorCreator
        {
            public override Color GetColor(float x, float y, float Alpha)
            {
                return argb(Alpha, 1 - x, 1 - y, (x + y) / 2);
            }
        }
        [Serializable]
        public class Fun2 : ColorCreator
        {
            public override Color GetColor(float x, float y, float Alpha)
            {
                return argb(Alpha, x, y, 1 - (x + y) / 2);
            }
        }
        [Serializable]
        public class RangeColors : ColorCreator
        {
            private float[] ranges;
            private Color[] colors;
            private float dx, dy;
            private bool gradient;
            public RangeColors() { }
            public RangeColors(float[] ranges, Color[] colors, float dx, float dy, bool gradient = true)
            {
                this.ranges = ranges;
                this.colors = colors;
                this.dx = dx; this.dy = dy;
                this.gradient = gradient;
            }

            public float[] Ranges { get => ranges; set => ranges = value; }
            public Color[] Colors { get => colors; set => colors = value; }
            public float Dx { get => dx; set => dx = value; }
            public float Dy { get => dy; set => dy = value; }
            public bool Gradient { get => gradient; set => gradient = value; }

            public override Color GetColor(float x, float y, float Alpha)
            {
                float r = 0, g = 0, b = 0, t, d = 1;
                int len = Math.Min(ranges.Length, colors.Length);
                t = x * dx + y * dy;
                //x = (x - 0.5f) * 2; y = (y - 0.5f) * 2;
                //t = (float)Math.Sqrt(x * x * dx + y * y * dy);
                /*t = (float)(Math.Atan2(dy*y - 0.5, dx*x - 0.5)/Math.PI/2+0.5);// Radius*/
                for (int i = 0; i < len - 1; ++i)
                {
                    if ((ranges[i] <= t) && (t < ranges[i + 1]))
                    {
                        float l = 255 * (ranges[i + 1] - ranges[i]);
                        t -= ranges[i];
                        Color lc = colors[i], rc = colors[i + 1];
                        if (gradient)
                        {
                            d = (float)(Math.Sin(((1 - dx) * x + (1 - dy) * y) * Math.PI));
                        }

                        r = d * (t * (rc.R - lc.R) / l + lc.R / 255f);
                        g = d * (t * (rc.G - lc.G) / l + lc.G / 255f);
                        b = d * (t * (rc.B - lc.B) / l + lc.B / 255f);
                        ///
                        //r = g = b = t;
                        break;
                    }
                }
                return argb(Alpha, r, g, b);
            }
        }
        [Serializable]
        public class FromImage : ColorCreator
        {
            private Bitmap img = null;
            private Size size;
            public Bitmap Image { get => img; set { img = value; size = img.Size; } }
            public override Color GetColor(float x, float y, float Alpha)
            {
                if (img == null || x >= 1 || y >= 1 || x < 0 || y < 0)
                {
                    return Color.Transparent;
                }

                int ix = (int)(size.Width * x);
                int iy = (int)(size.Height * y);
                try
                {
                    Color c = img.GetPixel(ix, iy);
                    c = Color.FromArgb((byte)(255 * Alpha), c);
                    return c;
                }
                catch (Exception)
                {

                }
                return Color.White;
            }

        }
    }
}
