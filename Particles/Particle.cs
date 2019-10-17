using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using static Particles.MyMath;
using static System.Math;

namespace Particles
{
    public class Particle
    {
        readonly static FontFamily font = new FontFamily("Consolas");

        #region Constants
        const float minScale = 1f, maxScale = 5;
        #endregion

        #region StaticField
        static bool mybuf = true;
        static Bitmap buff;
        private static Graphics bg;
        internal static SolidBrush brsh = new SolidBrush(Color.White);
        static readonly Random r = new Random();
        private static RectangleF bounds;
        private static ParticleSettings set = ParticleSettings.Default();
        #endregion

        #region StaticProperties
        public static RectangleF Bounds
        {
            get => bounds;
            set
            {
                bounds = value;
                if (mybuf)
                {
                    buff?.Dispose();
                    buff = new Bitmap((int)bounds.Width + 1, (int)bounds.Height + 1);
                }
                bg = Graphics.FromImage(buff);
                bg.SmoothingMode = SmoothingMode.AntiAlias;
            }
        }
        public static ParticleSettings Settings { get => set; internal set => set = value; }
        #endregion

        #region Fields
        public PointF loc;
        public float scale; //speed-size
        public float angle;
        public int coob; // count out of bounds
        public Color color;
        public List<PointF> tracing = new List<PointF>();
        #endregion        

        #region Properties
        public float f_speed => scale * set._speedScale; // TODO RENAME
        public float f_size => scale * set._sizeScale;
        public SizeF a_vec => new SizeF(f_speed * cos(angle), f_speed * sin(angle));

        #endregion

        #region StaticMethods
        internal static void SetBuffer(Bitmap buf)
        {
            buff = buf;
            bg = Graphics.FromImage(buff);
            mybuf = false;
        }
        internal static void BeginDraw()
        {
            if (set._clearBuffer)
            {//5984
                //bg.Clear(set._backGroundColor);
                
                brsh.Color = set._backGroundColor;
                Brush b = brsh;
                if (bounds.Width != 0 && bounds.Height != 0)
                {
                    //b = new LinearGradientBrush(bounds, set.bgColorFirst, set.bgColorSecond, set.bgStyle);
                    bg.FillRectangle(b, bounds);
                }
                
            }
            switch (set._colorMode)
            {
                case ParticleSettings.EColorMode.Overall:
                    brsh.Color = set._overallColor;
                    break;
                case ParticleSettings.EColorMode.RandomOverall:
                    brsh.Color = rnd_cl();
                    break;
                default:
                    break;
            }
        }
        public static void Save(string name)
        {
            //buff.Save(Environment.CurrentDirectory + "\\" + name);
            var sw = new FileStream(name, FileMode.Create);
            var bmp = new Bitmap(buff, (int)bounds.Width, (int)bounds.Height);
            bmp.Save(sw, ImageFormat.Png);
            sw.Close();
        }//TODO!
        #endregion

        #region ctor-s
        public Particle(float Angle, float Scale, Color Color, PointF Location = new PointF())
        {
            scale = Scale;
            angle = Angle;
            if (Location != PointF.Empty)
                loc = Location;
            else ResetLocation();
            color = Color;
        }
        public Particle() : this(rnd_f(set._newAngleRange.X, set._newAngleRange.Y), rnd_f(minScale, maxScale, 10000), rnd_cl()) { }
        #endregion

        #region Methods
        public void ResetLocation()
        {
            switch (set._newLocationMode)
            {
                case ParticleSettings.ENewLocationMode.Center:
                    loc = new PointF(bounds.Width / 2, bounds.Height / 2);
                    break;
                case ParticleSettings.ENewLocationMode.Random:
                    loc = new PointF(rnd_f(bounds.Width), rnd_f(bounds.Height));
                    break;
                case ParticleSettings.ENewLocationMode.Zeroes:
                    loc = new PointF();
                    break;
                case ParticleSettings.ENewLocationMode.Point:
                    loc = set._newLocationPoint;
                    break;
                default:
                    break;
            }
            if ((set._bounceMode & ParticleSettings.EBounceMode.ResetTracing) == ParticleSettings.EBounceMode.ResetTracing)
                tracing.Clear();
            if (set._resetAngle)
                angle = rnd_f(set._newAngleRange.X, set._newAngleRange.Y);
            scale = rnd_f(minScale, maxScale);
        }
        public void Draw()
        {
            var sz = new SizeF(f_size, f_size);
            var hs = new SizeF(sz.Width / 2, sz.Height / 2);
            var rec = new RectangleF(PointF.Subtract(loc, hs), sz);
            switch (set._colorMode)
            {
                case ParticleSettings.EColorMode.Own:
                    brsh.Color = color;
                    break;
                case ParticleSettings.EColorMode.Function:
                    brsh.Color = set._colorCreator.GetColor(loc.X / bounds.Width, loc.Y / bounds.Height, 1);
                    break;
                case ParticleSettings.EColorMode.RandomOwn:
                    brsh.Color = rnd_cl();
                    break;
                default:
                    break;
            }
            brsh.Color = Color.FromArgb((byte)(255 * set._alpha), brsh.Color);
            if (set._showTracingLine && tracing.Count > 1) bg.DrawCurve(new Pen(brsh.Color), tracing.ToArray());
            switch (set._shape)
            {
                case ParticleSettings.EShape.Sphere:
                    bg.FillEllipse(brsh, rec);
                    break;
                case ParticleSettings.EShape.Rectangle:
                    bg.FillRectangle(brsh, rec);
                    break;
                case ParticleSettings.EShape.Custom:
                    goto default;
                    var str = keywords[GetHashCode() % keycount];
                    var fon = new Font(str, abs(f_size));
                    sz = bg.MeasureString(str, fon);
                    hs = new SizeF(sz.Width / 2, sz.Height / 2);
                    bg.DrawString(str, fon, brsh, PointF.Subtract(loc, hs));
                    break;
                default:
                    bg.FillClosedCurve(brsh, Polygon(loc, hs, angle), FillMode.Alternate, set._tension);
                    //bg.FillPolygon(brsh, Polygon(loc, hs, angle), FillMode.Alternate);
                    break;
            }
        }
        static PointF[] Polygon(PointF loc, SizeF hs, float angle) => set._shapecreator.GetPoints(loc, hs, angle);
        public void Step()
        {
            var v = PointF.Add(loc, a_vec);
            if (bounds.Contains(v)) { loc = v; coob = 0; }
            else
            {
                if (++coob == 5) { ResetLocation(); coob = 0; return; }
                var bnc = set._bounceMode;
                if ((bnc & ParticleSettings.EBounceMode.ReflectAngle) == ParticleSettings.EBounceMode.ReflectAngle)
                {
                    if (v.X < bounds.Left || bounds.Right < v.X) angle = 180 - angle;
                    if (v.Y < bounds.Top || bounds.Bottom < v.Y) angle = -angle;
                    loc = PointF.Add(loc, a_vec);
                }
                if ((bnc & ParticleSettings.EBounceMode.OppositeSide) == ParticleSettings.EBounceMode.OppositeSide)
                {
                    if (v.X < bounds.Left) { v.X += bounds.Width; } else if (bounds.Right < v.X) { v.X -= bounds.Width; }
                    if (v.Y < bounds.Top) { v.Y += bounds.Height; } else if (bounds.Bottom < v.Y) { v.Y -= bounds.Height; }
                    loc = v;
                }
                if ((bnc & ParticleSettings.EBounceMode.ResetTracing) == ParticleSettings.EBounceMode.ResetTracing)
                {
                    tracing.Clear(); return;
                }
            }
            if (tracing.Count >= set._tracingLen) tracing.RemoveRange(0, tracing.Count - set._tracingLen);
            tracing.Add(loc);
        }
        public void InteractM(Point mloc, float scale = 1)
        {
            float tmp; SizeF sz;
            switch (set._mouseClickAction)
            {
                case ParticleSettings.EMouseClickAction.ChangeAngle:
                    sz = new SizeF((-0.5f + mloc.Y / bounds.Height) * scale, (-0.5f + mloc.X / bounds.Width) * scale);
                    set._speedScale = (float)Sqrt(sz.Height * sz.Height + sz.Width * sz.Width) * 2;
                    angle = (float)(Atan2(sz.Width, sz.Height) * 180 / PI);
                    break;
                case ParticleSettings.EMouseClickAction.RotateParticles:
                    angle = (float)(Atan2(scale * (loc.X - mloc.X), scale * (mloc.Y - loc.Y)) * 180 / PI + PI);
                    break;
                case ParticleSettings.EMouseClickAction.AngleToMouse:
                    angle = (float)(Atan2(scale * (mloc.Y - loc.Y), scale * (mloc.X - loc.X)) * 180 / PI);
                    break;
                case ParticleSettings.EMouseClickAction.Bubble:
                    tmp = (sqrt(sqr(loc.X - mloc.X) + sqr(loc.Y - mloc.Y)) / sqrt(sqr(bounds.Width) + sqr(bounds.Height)));
                    tmp = (1 - scale) / 2 + scale * tmp;
                    this.scale = tmp * maxScale + minScale;
                    break;
                default:
                    /*float dw = mloc.X / bounds.Width;
                    float dh = mloc.Y / bounds.Height;
                    set._sizeScale = dw;
                    set._speedScale = dh;*/
                    tmp = (float)(Atan2(scale * (mloc.Y - loc.Y), scale * (mloc.X - loc.X)) * 180 / PI);
                    sz = new SizeF(f_speed * cos(tmp), f_speed * sin(tmp));
                    tmp = 10f * f_size * (sqrt(sqr(loc.X - mloc.X) + sqr(loc.Y - mloc.Y)) / sqrt(sqr(bounds.Width) + sqr(bounds.Height)));
                    if (float.IsInfinity(tmp)) tmp = 0;
                    sz = new SizeF(sz.Width * tmp, sz.Height * tmp);
                    loc = PointF.Add(loc, sz);
                    //angle += Abs(tmp - angle) / 30;
                    //angle = sqrt(sqr(angle) + sqr(tmp));
                    break;
            }
        }
        public void StepDraw() { Step(); Draw(); }
        #endregion

        public override string ToString()
        {
            var delimeter = "\r\n";
            var type = typeof(Particle);
            var props = type.GetFields(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance
                );
            var sb = new StringBuilder(type.Name).Append(" {").Append(delimeter);
            foreach (var prop in props)
            {
                sb
                    .Append('\t')
                    .Append(prop.Name)
                    .Append(':')
                    .Append(prop.GetValue(this))
                    .Append(";")
                    .Append(delimeter);
            }
            return sb.Append('}').ToString();
        }

        static string[] keywords =
            {"ХУЙ","ПИЗДА","Джигурда" };
            //{ "Consolas", "Courier New", "Fira Code", "Lucida Console", "Unispace"};
            //{ "VisualBasic", "CSharp", "Pascal", "JavaScript", "HTML","Assembler"};
            //{ "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "using", "static", "virtual", "void", "volatile", "while" };
        static int keycount = keywords.Length;
    }
}