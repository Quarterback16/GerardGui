namespace GerardGui
{
    partial class GerardForm
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
         this.panel1 = new System.Windows.Forms.Panel();
         this.button1 = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.panel2 = new System.Windows.Forms.Panel();
         this.textBox1 = new System.Windows.Forms.TextBox();
         this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
         this.startDateLabel = new System.Windows.Forms.Label();
         this.panel1.SuspendLayout();
         this.panel2.SuspendLayout();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.startDateLabel);
         this.panel1.Controls.Add(this.button1);
         this.panel1.Controls.Add(this.label1);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(722, 250);
         this.panel1.TabIndex = 0;
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(24, 110);
         this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(138, 34);
         this.button1.TabIndex = 1;
         this.button1.Text = "GO";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(14, 22);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(148, 20);
         this.label1.TabIndex = 0;
         this.label1.Text = "Press GO to start ...";
         // 
         // panel2
         // 
         this.panel2.Controls.Add(this.textBox1);
         this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel2.Location = new System.Drawing.Point(0, 250);
         this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(722, 359);
         this.panel2.TabIndex = 1;
         // 
         // textBox1
         // 
         this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.textBox1.Location = new System.Drawing.Point(0, 0);
         this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.textBox1.Multiline = true;
         this.textBox1.Name = "textBox1";
         this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.textBox1.Size = new System.Drawing.Size(722, 359);
         this.textBox1.TabIndex = 0;
         // 
         // backgroundWorker1
         // 
         this.backgroundWorker1.WorkerReportsProgress = true;
         this.backgroundWorker1.WorkerSupportsCancellation = true;
         this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
         this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
         this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
         // 
         // startDateLabel
         // 
         this.startDateLabel.AutoSize = true;
         this.startDateLabel.Location = new System.Drawing.Point(20, 65);
         this.startDateLabel.Name = "startDateLabel";
         this.startDateLabel.Size = new System.Drawing.Size(66, 20);
         this.startDateLabel.TabIndex = 2;
         this.startDateLabel.Text = "Started:";
         // 
         // GerardForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(722, 609);
         this.Controls.Add(this.panel2);
         this.Controls.Add(this.panel1);
         this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.Name = "GerardForm";
         this.Text = "Gerard The Butler";
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         this.panel2.ResumeLayout(false);
         this.panel2.PerformLayout();
         this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label startDateLabel;
    }
}

