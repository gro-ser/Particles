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
        #region Constants
        private const float minScale = 1, maxScale = 5;
        #endregion

        #region StaticField
        private static bool useBuffer = true;
        private static Bitmap buffer;
        private static Graphics bufferedGraphics;
        private static SolidBrush brush = new SolidBrush(Color.White);
        private static RectangleF bounds;
        private static ParticleSettings settings = ParticleSettings.Default();
        #endregion

        #region StaticProperties
        public static RectangleF Bounds
        {
            get => bounds;
            set
            {
                bounds = value;
                if (useBuffer)
                {
                    buffer?.Dispose();
                    buffer = new Bitmap((int)bounds.Width + 1, (int)bounds.Height + 1);
                }
                bufferedGraphics = Graphics.FromImage(buffer);
                bufferedGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            }
        }

        public static ParticleSettings Settings
        {
            get => settings;
            internal set => settings = value;
        }
        #endregion

        #region Fields
        private int outOfBoundsCounter; // count out of bounds
        internal List<PointF> oldLocations = new List<PointF>();
        public PointF Location;
        public float Scale; //speed and size
        public float Angle;
        public Color Color;
        #endregion

        #region Properties
        public float Speed => Scale * settings._speedScale;
        public float Size => Scale * settings._sizeScale;
        public SizeF MoveVector => new SizeF(Speed * Cos(Angle), Speed * Sin(Angle));
        #endregion

        #region StaticMethods
        internal static void SetBuffer(Bitmap buf)
        {
            buffer = buf;
            bufferedGraphics = Graphics.FromImage(buffer);
            useBuffer = false;
        }

        internal static void BeginDraw()
        {
            if (settings._clearBuffer)
            {
                brush.Color = settings._backGroundColor;
                if (bounds.Width != 0 && bounds.Height != 0)
                {
                    bufferedGraphics.FillRectangle(brush, bounds);
                }

            }
            switch (settings._colorMode)
            {
                case ParticleSettings.EColorMode.Overall:
                    brush.Color = settings._overallColor;
                    break;
                case ParticleSettings.EColorMode.RandomOverall:
                    brush.Color = RandomColor();
                    break;
                default:
                    break;
            }
        }

        public static void Save(string name)
        {
            //buff.Save(Environment.CurrentDirectory + "\\" + name);
            FileStream sw = new FileStream(name, FileMode.Create);
            Bitmap bmp = new Bitmap(buffer, (int)bounds.Width, (int)bounds.Height);
            bmp.Save(sw, ImageFormat.Png);
            sw.Close();
        }//TODO!
        #endregion

        #region constructors
        public Particle(float angle, float scale, Color color, PointF location = new PointF())
        {
            this.Scale = scale;
            this.Angle = angle;
            if (location != PointF.Empty)
            {
                this.Location = location;
            }
            else
            {
                ResetLocation();
            }

            this.Color = color;
        }

        public Particle() : this(RandomFloat(settings._newAngleRange.X, settings._newAngleRange.Y), RandomFloat(minScale, maxScale), RandomColor()) { }
        #endregion

        #region Methods
        public void ResetLocation()
        {
            switch (settings._newLocationMode)
            {
                case ParticleSettings.ENewLocationMode.Center:
                    Location = new PointF(bounds.Width / 2, bounds.Height / 2);
                    break;
                case ParticleSettings.ENewLocationMode.Random:
                    Location = new PointF(RandomFloat(bounds.Width), RandomFloat(bounds.Height));
                    break;
                case ParticleSettings.ENewLocationMode.Zeroes:
                    Location = new PointF();
                    break;
                case ParticleSettings.ENewLocationMode.Point:
                    Location = settings._newLocationPoint;
                    break;
                default:
                    break;
            }
            if ((settings._bounceMode & ParticleSettings.EBounceMode.ResetTracing) == ParticleSettings.EBounceMode.ResetTracing)
            {
                oldLocations.Clear();
            }

            if (settings._resetAngle)
            {
                Angle = RandomFloat(settings._newAngleRange.X, settings._newAngleRange.Y);
            }

            Scale = RandomFloat(minScale, maxScale);
        }
        public void Draw()
        {
            SizeF sz = new SizeF(Size, Size);
            SizeF hs = new SizeF(sz.Width / 2, sz.Height / 2);
            RectangleF rec = new RectangleF(PointF.Subtract(Location, hs), sz);
            switch (settings._colorMode)
            {
                case ParticleSettings.EColorMode.Own:
                    brush.Color = Color;
                    break;
                case ParticleSettings.EColorMode.Function:
                    brush.Color = settings._colorCreator.GetColor(Location.X / bounds.Width, Location.Y / bounds.Height, 1);
                    break;
                case ParticleSettings.EColorMode.RandomOwn:
                    brush.Color = RandomColor();
                    break;
                default:
                    break;
            }
            brush.Color = Color.FromArgb((byte)(255 * settings._alpha), brush.Color);
            if (settings._showTracingLine && oldLocations.Count > 1)
            {
                bufferedGraphics.DrawCurve(new Pen(brush.Color), oldLocations.ToArray());
            }

            switch (settings._shape)
            {
                case ParticleSettings.EShape.Sphere:
                    bufferedGraphics.FillEllipse(brush, rec);
                    break;
                case ParticleSettings.EShape.Rectangle:
                    bufferedGraphics.FillRectangle(brush, rec);
                    break;
                case ParticleSettings.EShape.Custom:
                    goto default;
                    string str = keywords[GetHashCode() % keycount];
                    Font fon = new Font(str, MyMath.Abs(Size));
                    sz = bufferedGraphics.MeasureString(str, fon);
                    hs = new SizeF(sz.Width / 2, sz.Height / 2);
                    bufferedGraphics.DrawString(str, fon, brush, PointF.Subtract(Location, hs));
                    break;
                default:
                    bufferedGraphics.FillClosedCurve(brush, Polygon(Location, hs, Angle), FillMode.Alternate, settings._tension);
                    //bg.FillPolygon(brsh, Polygon(loc, hs, angle), FillMode.Alternate);
                    break;
            }
        }

        private static PointF[] Polygon(PointF loc, SizeF hs, float angle)
        {
            return settings._shapecreator.GetPoints(loc, hs, angle);
        }

        public void Step()
        {
            PointF v = PointF.Add(Location, MoveVector);
            if (bounds.Contains(v)) { Location = v; outOfBoundsCounter = 0; }
            else
            {
                if (++outOfBoundsCounter == 5) { ResetLocation(); outOfBoundsCounter = 0; return; }
                ParticleSettings.EBounceMode bnc = settings._bounceMode;
                if ((bnc & ParticleSettings.EBounceMode.ReflectAngle) == ParticleSettings.EBounceMode.ReflectAngle)
                {
                    if (v.X < bounds.Left || bounds.Right < v.X)
                    {
                        Angle = 180 - Angle;
                    }

                    if (v.Y < bounds.Top || bounds.Bottom < v.Y)
                    {
                        Angle = -Angle;
                    }

                    Location = PointF.Add(Location, MoveVector);
                }
                if ((bnc & ParticleSettings.EBounceMode.OppositeSide) == ParticleSettings.EBounceMode.OppositeSide)
                {
                    if (v.X < bounds.Left) { v.X += bounds.Width; } else if (bounds.Right < v.X) { v.X -= bounds.Width; }
                    if (v.Y < bounds.Top) { v.Y += bounds.Height; } else if (bounds.Bottom < v.Y) { v.Y -= bounds.Height; }
                    Location = v;
                }
                if ((bnc & ParticleSettings.EBounceMode.ResetTracing) == ParticleSettings.EBounceMode.ResetTracing)
                {
                    oldLocations.Clear(); return;
                }
            }
            if (oldLocations.Count >= settings._tracingLen)
            {
                oldLocations.RemoveRange(0, oldLocations.Count - settings._tracingLen);
            }

            oldLocations.Add(Location);
        }
        public void InteractM(Point mloc, float scale = 1)
        {
            float tmp; SizeF sz;
            switch (settings._mouseClickAction)
            {
                case ParticleSettings.EMouseClickAction.ChangeAngle:
                    sz = new SizeF((-0.5f + mloc.Y / bounds.Height) * scale, (-0.5f + mloc.X / bounds.Width) * scale);
                    settings._speedScale = (float)Sqrt((double)(sz.Height * sz.Height + sz.Width * sz.Width)) * 2;
                    Angle = (float)(Atan2(sz.Width, sz.Height) * 180 / PI);
                    break;
                case ParticleSettings.EMouseClickAction.RotateParticles:
                    Angle = (float)(Atan2(scale * (Location.X - mloc.X), scale * (mloc.Y - Location.Y)) * 180 / PI + PI);
                    break;
                case ParticleSettings.EMouseClickAction.AngleToMouse:
                    Angle = (float)(Atan2(scale * (mloc.Y - Location.Y), scale * (mloc.X - Location.X)) * 180 / PI);
                    break;
                case ParticleSettings.EMouseClickAction.Bubble:
                    tmp = (Sqrt(Sqr(Location.X - mloc.X) + Sqr(Location.Y - mloc.Y)) / Sqrt(Sqr(bounds.Width) + Sqr(bounds.Height)));
                    tmp = (1 - scale) / 2 + scale * tmp;
                    this.Scale = tmp * maxScale + minScale;
                    break;
                default:
                    /*float dw = mloc.X / bounds.Width;
                    float dh = mloc.Y / bounds.Height;
                    set._sizeScale = dw;
                    set._speedScale = dh;*/
                    tmp = (float)(Atan2(scale * (mloc.Y - Location.Y), scale * (mloc.X - Location.X)) * 180 / PI);
                    sz = new SizeF(Speed * Cos(tmp), Speed * Sin(tmp));
                    tmp = 10f * Size * (Sqrt(Sqr(Location.X - mloc.X) + Sqr(Location.Y - mloc.Y)) / Sqrt(Sqr(bounds.Width) + Sqr(bounds.Height)));
                    if (float.IsInfinity(tmp))
                    {
                        tmp = 0;
                    }

                    sz = new SizeF(sz.Width * tmp, sz.Height * tmp);
                    Location = PointF.Add(Location, sz);
                    //angle += Abs(tmp - angle) / 30;
                    //angle = sqrt(sqr(angle) + sqr(tmp));
                    break;
            }
        }
        public void StepDraw() { Step(); Draw(); }
        #endregion

        public override string ToString()
        {
            string delimeter = "\r\n";
            Type type = typeof(Particle);
            System.Reflection.FieldInfo[] props = type.GetFields(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance
                );
            StringBuilder sb = new StringBuilder(type.Name).Append(" {").Append(delimeter);
            foreach (System.Reflection.FieldInfo prop in props)
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

        private static readonly string[] keywords =
            {"ХУЙ","ПИЗДА","Джигурда" };

        //{ "Consolas", "Courier New", "Fira Code", "Lucida Console", "Unispace"};
        //{ "VisualBasic", "CSharp", "Pascal", "JavaScript", "HTML","Assembler"};
        //{ "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "using", "static", "virtual", "void", "volatile", "while" };
        private static readonly int keycount = keywords.Length;
    }
}
