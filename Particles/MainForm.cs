using Particles.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Particles
{
    public partial class MainForm : Form
    {
        private const bool ShowFPS = true;
        private const int frames = 30;
        private static readonly int MAC = Enum.GetNames(typeof(ParticleSettings.EMouseClickAction)).Length;
        private readonly List<int> fps = new List<int>(frames);
        private readonly SettingsForm sett = new SettingsForm();
        private readonly List<Particle> list = new List<Particle>();
        private Point MLoc;
        private float scale;
        private readonly Stopwatch sw = new Stopwatch();

        public MainForm(bool isSCR)
        {
            InitializeComponent();
            ShowVersionString();
            if (isSCR)
            {
                Settings.Default.DebuggerLabels = false;
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }

            else
            {
                sett.Show();
            }

            Size sz = Screen.PrimaryScreen.Bounds.Size;
            Bitmap buf = new Bitmap(sz.Width, sz.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Particle.SetBuffer(buf);
            BackgroundImage = buf;
            for (int i = 0; i < frames; i++)
            {
                fps.Add(0);
            }

            MouseWheel += MainForm_MouseWheel;
        }

        private void ShowVersionString()
        {
            versionlabel.Text = AssemblyVersion(Assembly.GetCallingAssembly().GetName());
        }

        private string AssemblyVersion(AssemblyName name)
        {
            string ver; Graphics g = CreateGraphics();
            ver = string.Concat(name.Name, " Version:", name.Version);
            if (g.MeasureString(ver, Font).Width < Width - 12)
            {
                return ver;
            }

            ver = string.Concat(name.Name, ":", name.Version);
            if (g.MeasureString(ver, Font).Width < Width - 15)
            {
                return ver;
            }

            ver = string.Concat("ver:", name.Version);
            if (g.MeasureString(ver, Font).Width < Width - 15)
            {
                return ver;
            }

            ver = name.Version.ToString();
            return ver;
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            ParticleSettings.EMouseClickAction tmp = Particle.Settings.MouseClickAction;
            if (e.Delta < 0)
            {
                if (--tmp < 0)
                {
                    tmp = (ParticleSettings.EMouseClickAction)(MAC - 1);
                }
            }
            else
            {
                if ((int)++tmp >= MAC)
                {
                    tmp = 0;
                }
            }
            Particle.Settings.MouseClickAction = tmp;
            mode.Text = tmp.ToString();
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            Particle.Bounds = ClientRectangle;
            for (int i = Particle.Settings.CountOfParticles - 1; i >= 0; i--)
            {
                list.Add(new Particle());
            }
        }

        private void MainFormResize(object sender, EventArgs e)
        {
            Particle.Bounds = ClientRectangle;
            ShowVersionString();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            sw.Restart();
            Particle.BeginDraw();
            foreach (Particle p in list)
            {
                if (!MLoc.IsEmpty)
                {
                    p.InteractM(MLoc, scale);
                }

                p.StepDraw();
            }
            Refresh();
            if (ShowFPS)
            {
                fps.RemoveAt(0);
                fps.Add((int)sw.ElapsedMilliseconds);
                float avg = frames * 1000f / fps.Sum();
                fpscounter.Text = $"FPS: {avg:000.000}";
                fpscounter.Text = string.Format("FPS:{0,7:F3}", avg)
                //+$"\r\n{fps.Capacity}|{fps.Count}"
                ;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1: sett.Show(); break;
                case Keys.Space: timer1.Enabled = !timer1.Enabled; break;
                case Keys.R: FormBorderStyle = (FormBorderStyle)(4 - (int)FormBorderStyle); break;
                case Keys.S:
                    Particle.Save(DateTime.Now.Ticks + ".png");
                    break;
                case Keys.Delete:
                    list.Clear();
                    for (int i = Particle.Settings.CountOfParticles - 1; i >= 0; i--)
                    {
                        list.Add(new Particle());
                    }

                    break;
                case Keys.Z: Size = new Size(256, 256); break;
                case Keys.Tab:
                    Settings.Default.DebuggerLabels = !Settings.Default.DebuggerLabels;
                    break;
                case Keys.M:
                    WindowState = (FormWindowState)(2 - (int)WindowState);
                    break;
                case var key when key >= Keys.NumPad5 && key <= Keys.NumPad9:
                    int pow = 1 << (key - Keys.NumPad0);
                    Size = new Size(pow, pow);
                    ShowVersionString();
                    break;
                default: break;
            }
        }

        private void Mouse(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
            { MLoc = Point.Empty; return; }
            MLoc = e.Location;
            if (e.Button == MouseButtons.Left)
            {
                scale = 1;
            }
            else if (e.Button == MouseButtons.Right)
            {
                scale = -1;
            }
            else
            {
                scale = 0;
                fpscounter.Text = "Warning!\r\n" + ParticleSettings.Info(e);
            }
        }

        private void MainForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Scale(SizeF.Add(AutoScaleFactor, AutoScaleFactor));
            return;
            MessageBox.Show(ParticleSettings.Info(e));
            scale = 10;
            MLoc = e.Location;
        }
    }
}
