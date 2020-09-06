using System;
using System.Drawing;
using static System.Math;

namespace Particles
{
    internal static class MyMath
    {
        private const int r_scale = 1000;
        private static readonly Random r = new Random();
        public static float sin(float a)
        {
            return (float)Sin(a * PI / 180);
        }

        public static float cos(float a)
        {
            return (float)Cos(a * PI / 180);
        }

        public static float abs(float a)
        {
            return Abs(a);
        }

        public static float sqr(float a)
        {
            return a * a;
        }

        public static float sqrt(float a)
        {
            return (float)Sqrt(a);
        }

        public static float rnd_f(float a, float b, int scale = r_scale)
        {
            return rnd_f(b - a, scale) + a;
        }

        public static float rnd_f(float a, int scale = r_scale)
        {
            return 1f * r.Next((int)(a * scale)) / scale;
        }

        public static int rnd_i()
        {
            return r.Next();
        }

        public static Color rnd_cl()
        {
            return Color.FromArgb(~0xFFFFFF | rnd_i());
        }

        public static float Angle(SizeF s)
        {
            return (float)Atan2(s.Width, s.Height);
        }
    }
}