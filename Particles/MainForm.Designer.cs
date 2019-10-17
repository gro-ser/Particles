namespace Particles
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.fpscounter = new System.Windows.Forms.Label();
            this.mode = new System.Windows.Forms.Label();
            this.versionlabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // fpscounter
            // 
            this.fpscounter.AutoSize = true;
            this.fpscounter.DataBindings.Add(new System.Windows.Forms.Binding("Visible", global::Particles.Properties.Settings.Default, "DebuggerLabels", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.fpscounter.ForeColor = System.Drawing.Color.Red;
            this.fpscounter.Location = new System.Drawing.Point(2, 2);
            this.fpscounter.Name = "fpscounter";
            this.fpscounter.Size = new System.Drawing.Size(35, 15);
            this.fpscounter.TabIndex = 0;
            this.fpscounter.Text = "fps:";
            this.fpscounter.Visible = global::Particles.Properties.Settings.Default.DebuggerLabels;
            // 
            // mode
            // 
            this.mode.AutoSize = true;
            this.mode.BackColor = System.Drawing.Color.Black;
            this.mode.DataBindings.Add(new System.Windows.Forms.Binding("Visible", global::Particles.Properties.Settings.Default, "DebuggerLabels", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.mode.ForeColor = System.Drawing.Color.Yellow;
            this.mode.Location = new System.Drawing.Point(2, 19);
            this.mode.Name = "mode";
            this.mode.Size = new System.Drawing.Size(35, 15);
            this.mode.TabIndex = 1;
            this.mode.Text = "None";
            this.mode.Visible = global::Particles.Properties.Settings.Default.DebuggerLabels;
            // 
            // versionlabel
            // 
            this.versionlabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.versionlabel.AutoSize = true;
            this.versionlabel.DataBindings.Add(new System.Windows.Forms.Binding("Visible", global::Particles.Properties.Settings.Default, "DebuggerLabels", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.versionlabel.ForeColor = System.Drawing.Color.Fuchsia;
            this.versionlabel.Location = new System.Drawing.Point(2, 456);
            this.versionlabel.Margin = new System.Windows.Forms.Padding(0);
            this.versionlabel.Name = "versionlabel";
            this.versionlabel.Size = new System.Drawing.Size(63, 15);
            this.versionlabel.TabIndex = 2;
            this.versionlabel.Text = "version:";
            this.versionlabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.versionlabel.Visible = global::Particles.Properties.Settings.Default.DebuggerLabels;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(496, 473);
            this.Controls.Add(this.versionlabel);
            this.Controls.Add(this.mode);
            this.Controls.Add(this.fpscounter);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Particles";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Mouse);
            this.Resize += new System.EventHandler(this.MainFormResize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label fpscounter;
        private System.Windows.Forms.Label mode;
        private System.Windows.Forms.Label versionlabel;
    }
}

