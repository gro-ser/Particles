using System;
using System.Drawing;
using static System.Math;

namespace Particles
{
    internal static class MyMath
    {
        private const int randomScale = 1000;
        private static readonly Random r = new Random();

        public static float Sin(float a)
        {
            return (float)Math.Sin(a * PI / 180);
        }

        public static float Cos(float a)
        {
            return (float)Math.Cos(a * PI / 180);
        }

        public static float Abs(float a)
        {
            return Abs(a);
        }

        public static float Sqr(float a)
        {
            return a * a;
        }

        public static float Sqrt(float a)
        {
            return (float)Math.Sqrt(a);
        }

        public static float RandomFloat(float a, float b, int scale = randomScale)
        {
            return RandomFloat(b - a, scale) + a;
        }

        public static float RandomFloat(float a, int scale = randomScale)
        {
            return 1f * r.Next((int)(a * scale)) / scale;
        }

        public static int RandomInt()
        {
            return r.Next();
        }

        public static Color RandomColor()
        {
            return Color.FromArgb(~0xFFFFFF | RandomInt());
        }

        public static float Angle(SizeF s)
        {
            return (float)Atan2(s.Width, s.Height);
        }
    }
}
