namespace Backup_Catalog
{
    partial class Progress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Progress));
            this.pauseResumeButtom = new System.Windows.Forms.Button();
            this.estimatedTimeLabel = new System.Windows.Forms.Label();
            this.percentageLabel = new System.Windows.Forms.Label();
            this.loadingLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.fileLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pauseResumeButtom
            // 
            this.pauseResumeButtom.Location = new System.Drawing.Point(314, 74);
            this.pauseResumeButtom.Name = "pauseResumeButtom";
            this.pauseResumeButtom.Size = new System.Drawing.Size(75, 23);
            this.pauseResumeButtom.TabIndex = 10;
            this.pauseResumeButtom.Text = "Pause";
            this.pauseResumeButtom.UseVisualStyleBackColor = true;
            this.pauseResumeButtom.Click += new System.EventHandler(this.PauseResumeButtom_Click);
            // 
            // estimatedTimeLabel
            // 
            this.estimatedTimeLabel.AutoSize = true;
            this.estimatedTimeLabel.Location = new System.Drawing.Point(10, 79);
            this.estimatedTimeLabel.Name = "estimatedTimeLabel";
            this.estimatedTimeLabel.Size = new System.Drawing.Size(98, 13);
            this.estimatedTimeLabel.TabIndex = 9;
            this.estimatedTimeLabel.Text = "Estimated time left: ";
            // 
            // percentageLabel
            // 
            this.percentageLabel.Location = new System.Drawing.Point(282, 12);
            this.percentageLabel.Name = "percentageLabel";
            this.percentageLabel.Size = new System.Drawing.Size(111, 13);
            this.percentageLabel.TabIndex = 8;
            this.percentageLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // loadingLabel
            // 
            this.loadingLabel.AutoSize = true;
            this.loadingLabel.Location = new System.Drawing.Point(9, 12);
            this.loadingLabel.Name = "loadingLabel";
            this.loadingLabel.Size = new System.Drawing.Size(50, 13);
            this.loadingLabel.TabIndex = 7;
            this.loadingLabel.Text = "Updating";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 28);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(377, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.Timer2_Tick);
            // 
            // fileLabel
            // 
            this.fileLabel.AutoSize = true;
            this.fileLabel.Location = new System.Drawing.Point(10, 54);
            this.fileLabel.Name = "fileLabel";
            this.fileLabel.Size = new System.Drawing.Size(0, 13);
            this.fileLabel.TabIndex = 11;
            // 
            // Progress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 106);
            this.Controls.Add(this.fileLabel);
            this.Controls.Add(this.pauseResumeButtom);
            this.Controls.Add(this.estimatedTimeLabel);
            this.Controls.Add(this.percentageLabel);
            this.Controls.Add(this.loadingLabel);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Progress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Updating";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button pauseResumeButtom;
        private System.Windows.Forms.Label estimatedTimeLabel;
        public System.Windows.Forms.Label percentageLabel;
        private System.Windows.Forms.Label loadingLabel;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        public System.Windows.Forms.Label fileLabel;
    }
}