namespace RemoteDesktopClient
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
            this.PnlTop = new System.Windows.Forms.Panel();
            this.BtnConnect = new System.Windows.Forms.Button();
            this.TxtPort = new System.Windows.Forms.TextBox();
            this.LblPort = new System.Windows.Forms.Label();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.LblServerIP = new System.Windows.Forms.Label();
            this.PnlMain = new System.Windows.Forms.Panel();
            this.PtbScreenView = new System.Windows.Forms.PictureBox();
            this.PnlTop.SuspendLayout();
            this.PnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PtbScreenView)).BeginInit();
            this.SuspendLayout();
            // 
            // PnlTop
            // 
            this.PnlTop.Controls.Add(this.BtnConnect);
            this.PnlTop.Controls.Add(this.TxtPort);
            this.PnlTop.Controls.Add(this.LblPort);
            this.PnlTop.Controls.Add(this.txtServerIP);
            this.PnlTop.Controls.Add(this.LblServerIP);
            this.PnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlTop.Location = new System.Drawing.Point(0, 0);
            this.PnlTop.Name = "PnlTop";
            this.PnlTop.Size = new System.Drawing.Size(800, 56);
            this.PnlTop.TabIndex = 0;
            // 
            // BtnConnect
            // 
            this.BtnConnect.Location = new System.Drawing.Point(228, 23);
            this.BtnConnect.Name = "BtnConnect";
            this.BtnConnect.Size = new System.Drawing.Size(75, 23);
            this.BtnConnect.TabIndex = 4;
            this.BtnConnect.Text = "Connect";
            this.BtnConnect.UseVisualStyleBackColor = true;
            // 
            // TxtPort
            // 
            this.TxtPort.Location = new System.Drawing.Point(122, 23);
            this.TxtPort.Name = "TxtPort";
            this.TxtPort.Size = new System.Drawing.Size(100, 22);
            this.TxtPort.TabIndex = 3;
            // 
            // LblPort
            // 
            this.LblPort.AutoSize = true;
            this.LblPort.Location = new System.Drawing.Point(121, 4);
            this.LblPort.Name = "LblPort";
            this.LblPort.Size = new System.Drawing.Size(31, 16);
            this.LblPort.TabIndex = 2;
            this.LblPort.Text = "Port";
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(16, 23);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(100, 22);
            this.txtServerIP.TabIndex = 1;
            // 
            // LblServerIP
            // 
            this.LblServerIP.AutoSize = true;
            this.LblServerIP.Location = new System.Drawing.Point(13, 4);
            this.LblServerIP.Name = "LblServerIP";
            this.LblServerIP.Size = new System.Drawing.Size(62, 16);
            this.LblServerIP.TabIndex = 0;
            this.LblServerIP.Text = "Server IP";
            // 
            // PnlMain
            // 
            this.PnlMain.Controls.Add(this.PtbScreenView);
            this.PnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlMain.Location = new System.Drawing.Point(0, 56);
            this.PnlMain.Name = "PnlMain";
            this.PnlMain.Size = new System.Drawing.Size(800, 394);
            this.PnlMain.TabIndex = 1;
            // 
            // PtbScreenView
            // 
            this.PtbScreenView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PtbScreenView.Location = new System.Drawing.Point(0, 0);
            this.PtbScreenView.Name = "PtbScreenView";
            this.PtbScreenView.Size = new System.Drawing.Size(800, 394);
            this.PtbScreenView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PtbScreenView.TabIndex = 0;
            this.PtbScreenView.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PnlMain);
            this.Controls.Add(this.PnlTop);
            this.Name = "Form1";
            this.Text = "Form1";
            this.PnlTop.ResumeLayout(false);
            this.PnlTop.PerformLayout();
            this.PnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PtbScreenView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlTop;
        private System.Windows.Forms.Label LblPort;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.Label LblServerIP;
        private System.Windows.Forms.Button BtnConnect;
        private System.Windows.Forms.TextBox TxtPort;
        private System.Windows.Forms.Panel PnlMain;
        private System.Windows.Forms.PictureBox PtbScreenView;
    }
}

