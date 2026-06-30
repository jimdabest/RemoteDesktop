namespace RemoteDesktop
{
    partial class FormLauncher
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
            lblTitle = new Label();
            btnServer = new Button();
            btnClient = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Arial", 13F, FontStyle.Bold);
            lblTitle.Location = new Point(144, 22);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(222, 26);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "REMOTE DESKTOP\r\n";
            // 
            // btnServer
            // 
            btnServer.BackColor = Color.DodgerBlue;
            btnServer.Cursor = Cursors.Hand;
            btnServer.Font = new Font("Arial", 10F, FontStyle.Bold);
            btnServer.ForeColor = Color.White;
            btnServer.Location = new Point(12, 75);
            btnServer.Name = "btnServer";
            btnServer.Size = new Size(225, 80);
            btnServer.TabIndex = 1;
            btnServer.Text = "CHO PHÉP ĐIỀU KHIỂN\r\n(Run as Server)";
            btnServer.UseVisualStyleBackColor = false;
            btnServer.Click += btnServer_Click;
            // 
            // btnClient
            // 
            btnClient.BackColor = Color.SeaGreen;
            btnClient.Cursor = Cursors.Hand;
            btnClient.Font = new Font("Arial", 10F, FontStyle.Bold);
            btnClient.ForeColor = Color.White;
            btnClient.Location = new Point(268, 75);
            btnClient.Name = "btnClient";
            btnClient.Size = new Size(219, 80);
            btnClient.TabIndex = 2;
            btnClient.Text = "ĐIỀU KHIỂN MÁY KHÁC\r\n(Run as Client)";
            btnClient.UseVisualStyleBackColor = false;
            btnClient.Click += btnClient_Click;
            // 
            // FormLauncher
            // 
            AutoScaleDimensions = new SizeF(8F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(499, 200);
            Controls.Add(btnClient);
            Controls.Add(btnServer);
            Controls.Add(lblTitle);
            Font = new Font("Arial", 9F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormLauncher";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Remote Desktop - Startup";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnServer;
        private System.Windows.Forms.Button btnClient;
    }
}