using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Particles
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            grid.SelectedObject = Particle.Settings;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = checkBox1.Checked;
        }

        private void Form2_DoubleClick(object sender, EventArgs e)
        {
            grid.SelectedObject = grid;
            //MessageBox.Show(propertyGrid1.SelectedObject.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            grid.SelectedObject = Particle.Settings;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            grid.SelectedObject = Particle.Settings._colorCreator;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Particle.Settings.SaveXml();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            grid.SelectedObject = Particle.Settings = ParticleSettings.LoadXml();
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            label1.Text = ParticleSettings.Info(e, delimeter:" ");
        }

        private void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            label1.Text = ParticleSettings.Info(e, delimeter:" ");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Particle.Settings.SaveBin();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            grid.SelectedObject = Particle.Settings = ParticleSettings.LoadBin();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            grid.SelectedObject = SubSettings.Sub;
        }
    }
    class SubSettings
    {
        [TypeConverter(typeof(ColorCreatorConverter))]
        [Description("some settings")]
        public ParticleSettings Settings { get; set; } = Particle.Settings;
        static public SubSettings Sub { get; } = new SubSettings();
    }
}
