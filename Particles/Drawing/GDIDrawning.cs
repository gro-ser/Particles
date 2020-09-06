using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static Particles.MyMath;

namespace Particles.Drawing
{
    internal class GDIDrawning : IDrawing, IDisposable
    {
        private static readonly FontFamily font = new FontFamily("Consolas");
        private ParticleSettings settings;
        private readonly Graphics graphics;
        private Graphics bg;
        private readonly SolidBrush brsh = new SolidBrush(Color.White);
        private Bitmap buffer;
        private readonly Form form;
        private RectangleF bounds;
        public RectangleF Bounds
        {
            get => bounds;
            set => bounds = value;
        }
        public ParticleSettings Settings { get => settings; set => settings = value; }

        public GDIDrawning(Form form, ParticleSettings settings)
        {
            this.form = form;
            this.settings = settings;
        }

        public GDIDrawning(Form form) : this(form, ParticleSettings.Default()) { }

        public void Init()
        {
            Size sz = Screen.PrimaryScreen.Bounds.Size;
            buffer = new Bitmap(sz.Width, sz.Height);
            bg = Graphics.FromImage(buffer);
            form.BackgroundImage = buffer;
            form.Resize += (a, args) => bounds = form.ClientRectangle;
        }

        public void Dispose()
        {
            graphics.Dispose();
            buffer.Dispose();
            bg.Dispose();
        }

        public void BeginDraw()
        {
            if (settings._clearBuffer)
            {
                //bg.Clear(set._backGroundColor);
                brsh.Color = settings._backGroundColor;
                bg.FillRectangle(brsh, bounds);
            }
            switch (settings._colorMode)
            {
                case ParticleSettings.EColorMode.Overall:
                    brsh.Color = settings._overallColor;
                    break;
                case ParticleSettings.EColorMode.RandomOverall:
                    brsh.Color = rnd_cl();
                    break;
                default:
                    break;
            }
        }

        public void Draw(Particle p)
        {
            SizeF sz = new SizeF(p.f_size, p.f_size);
            SizeF hs = new SizeF(sz.Width / 2, sz.Height / 2);
            RectangleF rec = new RectangleF(PointF.Subtract(p.loc, hs), sz);
            switch (settings._colorMode)
            {
                case ParticleSettings.EColorMode.Own:
                    brsh.Color = p.color;
                    break;
                case ParticleSettings.EColorMode.Function:
                    brsh.Color = settings._colorCreator.GetColor(p.loc.X / bounds.Width, p.loc.Y / bounds.Height, 1);
                    break;
                case ParticleSettings.EColorMode.RandomOwn:
                    brsh.Color = rnd_cl();
                    break;
                default:
                    break;
            }
            brsh.Color = Color.FromArgb((byte)(255 * settings._alpha), brsh.Color);
            if (settings._showTracingLine && p.tracing.Count > 1)
            {
                bg.DrawCurve(new Pen(brsh.Color), p.tracing.ToArray());
            }

            switch (settings._shape)
            {
                case ParticleSettings.EShape.Sphere:
                    bg.FillEllipse(brsh, rec);
                    break;
                case ParticleSettings.EShape.Rectangle:
                    bg.FillRectangle(brsh, rec);
                    break;
                case ParticleSettings.EShape.Custom:
                    bg.DrawString(GetHashCode().ToString(), new Font(font, abs(p.f_size)), brsh, p.loc);
                    break;
                default:
                    bg.FillClosedCurve(brsh, settings._shapecreator.GetPoints(p.loc, hs, p.angle), FillMode.Alternate, settings._tension);
                    break;
            }
        }
    }
}
