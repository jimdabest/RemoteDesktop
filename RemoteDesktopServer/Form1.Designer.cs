namespace RemoteDesktopServer
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
            this.panelLeft = new System.Windows.Forms.Panel();
            this.LblServerIP = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.PnlTop = new System.Windows.Forms.Panel();
            this.PnlMain = new System.Windows.Forms.Panel();
            this.PtbScreenView = new System.Windows.Forms.PictureBox();
            this.panelLeft.SuspendLayout();
            this.PnlTop.SuspendLayout();
            this.PnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PtbScreenView)).BeginInit();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.PnlMain);
            this.panelLeft.Controls.Add(this.PnlTop);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(800, 450);
            this.panelLeft.TabIndex = 0;
            // 
            // LblServerIP
            // 
            this.LblServerIP.AutoSize = true;
            this.LblServerIP.Location = new System.Drawing.Point(12, 4);
            this.LblServerIP.Name = "LblServerIP";
            this.LblServerIP.Size = new System.Drawing.Size(62, 16);
            this.LblServerIP.TabIndex = 0;
            this.LblServerIP.Text = "Server IP";
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(12, 24);
            this.txtIp.Name = "txtIp";
            this.txtIp.ReadOnly = true;
            this.txtIp.Size = new System.Drawing.Size(150, 22);
            this.txtIp.TabIndex = 1;
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(180, 4);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(31, 16);
            this.labelPort.TabIndex = 2;
            this.labelPort.Text = "Port";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(180, 24);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 22);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "9000";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(300, 20);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(65, 30);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(370, 20);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(65, 30);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // PnlTop
            // 
            this.PnlTop.Controls.Add(this.txtIp);
            this.PnlTop.Controls.Add(this.LblServerIP);
            this.PnlTop.Controls.Add(this.btnStop);
            this.PnlTop.Controls.Add(this.btnStart);
            this.PnlTop.Controls.Add(this.labelPort);
            this.PnlTop.Controls.Add(this.txtPort);
            this.PnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlTop.Location = new System.Drawing.Point(0, 0);
            this.PnlTop.Name = "PnlTop";
            this.PnlTop.Size = new System.Drawing.Size(800, 67);
            this.PnlTop.TabIndex = 6;
            // 
            // PnlMain
            // 
            this.PnlMain.Controls.Add(this.PtbScreenView);
            this.PnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlMain.Location = new System.Drawing.Point(0, 67);
            this.PnlMain.Name = "PnlMain";
            this.PnlMain.Size = new System.Drawing.Size(800, 383);
            this.PnlMain.TabIndex = 7;
            // 
            // PtbScreenView
            // 
            this.PtbScreenView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PtbScreenView.Location = new System.Drawing.Point(0, 0);
            this.PtbScreenView.Name = "PtbScreenView";
            this.PtbScreenView.Size = new System.Drawing.Size(800, 383);
            this.PtbScreenView.TabIndex = 0;
            this.PtbScreenView.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelLeft);
            this.Name = "Form1";
            this.Text = "Remote Desktop - Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panelLeft.ResumeLayout(false);
            this.PnlTop.ResumeLayout(false);
            this.PnlTop.PerformLayout();
            this.PnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PtbScreenView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Label LblServerIP;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Panel PnlTop;
        private System.Windows.Forms.Panel PnlMain;
        private System.Windows.Forms.PictureBox PtbScreenView;
    }
}
