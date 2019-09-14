using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection;
using static Particles.MyMath;
using Particles.Drawing;

namespace Particles
{
    static class Program
    {
        const string load = "MF";
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            var nfi = new NumberFormatInfo
            {
                NumberGroupSeparator = "."
            };
            var cci = new CultureInfo(1049)
            {
                NumberFormat = nfi
            };
            CultureInfo.CurrentCulture = cci;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            switch (load)
            {
                case "MF":
                    Application.Run(new MainForm());
                    break;
                case "D":
                    Form form = new Form();
                    form.Show();
                    GDIDrawning gdi = new GDIDrawning(form);
                    gdi.Init();
                    List<Particle> list = new List<Particle>(200);
                    for (int i = 0; i < 200; i++) list.Add(new Particle());
                    Timer timer = new Timer() { Enabled = true, Interval = 30 };
                    timer.Tick += (t, args) =>
                    {
                        gdi.BeginDraw();
                        foreach (var par in list)
                        {
                            par.Step();
                            gdi.Draw(par);
                        }
                        form.Refresh();
                    };
                    Application.Run(form);
                    break;
                default:
                    break;
            }            
        }
        static void debug()
        {
            var arr = typeof(Particle).GetFields(
                BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.Static|BindingFlags.SetProperty
                );
            foreach (var f in arr)
            {
                Console.Error.WriteLine(f.Name + " / " + f.Attributes);
            }
        }
    }
}