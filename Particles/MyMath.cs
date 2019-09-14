using System;
using System.Drawing;
using static System.Math;

namespace Particles
{
    static class MyMath
    {
        const int r_scale = 1000;
        static Random r = new Random();
        public static float sin(float a) => (float)Sin(a * PI / 180);
        public static float cos(float a) => (float)Cos(a * PI / 180);
        public static float abs(float a) => Abs(a);
        public static float sqr(float a) => a * a;
        public static float sqrt(float a) => (float)Sqrt(a);
        public static float rnd_f(float a, float b, int scale = r_scale) => rnd_f(b - a, scale) + a;
        public static float rnd_f(float a, int scale = r_scale) => 1f*r.Next((int)(a * scale)) / scale;
        public static int rnd_i() => r.Next();
        public static Color rnd_cl() => Color.FromArgb (~0xFFFFFF | rnd_i());
        public static float Angle(SizeF s) => (float)Atan2(s.Width, s.Height);
    }
}