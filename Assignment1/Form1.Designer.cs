namespace Assignment1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.alienMoveTimer = new System.Windows.Forms.Timer(this.components);
            this.bullet = new System.Windows.Forms.PictureBox();
            this.bulletTimer = new System.Windows.Forms.Timer(this.components);
            this.alienFireTimer = new System.Windows.Forms.Timer(this.components);
            this.alienBulletMoveTimer = new System.Windows.Forms.Timer(this.components);
            this.intensityTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bullet)).BeginInit();
            this.SuspendLayout();
            // 
            // alienMoveTimer
            // 
            this.alienMoveTimer.Enabled = true;
            this.alienMoveTimer.Interval = 50;
            this.alienMoveTimer.Tick += new System.EventHandler(this.alienMoveTimer_Tick);
            // 
            // bullet
            // 
            this.bullet.BackColor = System.Drawing.Color.Yellow;
            this.bullet.Location = new System.Drawing.Point(2, 1171);
            this.bullet.Margin = new System.Windows.Forms.Padding(2);
            this.bullet.Name = "bullet";
            this.bullet.Size = new System.Drawing.Size(10, 32);
            this.bullet.TabIndex = 5;
            this.bullet.TabStop = false;
            // 
            // bulletTimer
            // 
            this.bulletTimer.Interval = 50;
            this.bulletTimer.Tick += new System.EventHandler(this.bulletTimer_Tick);
            // 
            // alienFireTimer
            // 
            this.alienFireTimer.Enabled = true;
            this.alienFireTimer.Interval = 1000;
            this.alienFireTimer.Tick += new System.EventHandler(this.alienFireTimer_Tick);
            // 
            // alienBulletMoveTimer
            // 
            this.alienBulletMoveTimer.Enabled = true;
            this.alienBulletMoveTimer.Tick += new System.EventHandler(this.alientBulletMoveTimer_Tick);
            // 
            // intensityTimer
            // 
            this.intensityTimer.Enabled = true;
            this.intensityTimer.Interval = 10000;
            this.intensityTimer.Tick += new System.EventHandler(this.intensityTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(2456, 1204);
            this.Controls.Add(this.bullet);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Game_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bullet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer alienMoveTimer;
        private System.Windows.Forms.PictureBox bullet;
        private System.Windows.Forms.Timer bulletTimer;
        private System.Windows.Forms.Timer alienFireTimer;
        private System.Windows.Forms.Timer alienBulletMoveTimer;
        private System.Windows.Forms.Timer intensityTimer;
    }
}

