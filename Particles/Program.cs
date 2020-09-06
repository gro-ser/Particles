using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Particles
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            NumberFormatInfo nfi = new NumberFormatInfo
            {
                NumberGroupSeparator = "."
            };
            CultureInfo cci = new CultureInfo(1049)
            {
                NumberFormat = nfi
            };
            CultureInfo.CurrentCulture = cci;

            if (File.Exists("settings.xml"))
            {
                Particle.Settings = ParticleSettings.LoadXml();
            }
            else if (File.Exists("settings.bin"))
            {
                Particle.Settings = ParticleSettings.LoadBin();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainForm(AppDomain.CurrentDomain.FriendlyName.EndsWith(".scr", StringComparison.OrdinalIgnoreCase)));
        }
    }
}