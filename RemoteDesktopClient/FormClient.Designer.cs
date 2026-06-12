namespace RemoteDesktopClient;

partial class FormClient
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.panelHeader = new System.Windows.Forms.Panel();
        this.lblTitle = new System.Windows.Forms.Label();
        this.panelConnection = new System.Windows.Forms.Panel();
        this.btnDisconnect = new System.Windows.Forms.Button();
        this.btnConnect = new System.Windows.Forms.Button();
        this.txtPassword = new System.Windows.Forms.TextBox();
        this.lblPassword = new System.Windows.Forms.Label();
        this.txtPort = new System.Windows.Forms.TextBox();
        this.lblPort = new System.Windows.Forms.Label();
        this.txtServerIP = new System.Windows.Forms.TextBox();
        this.lblServerIP = new System.Windows.Forms.Label();
        this.panelStatus = new System.Windows.Forms.Panel();
        this.lblConnectionStatusValue = new System.Windows.Forms.Label();
        this.lblConnectionStatus = new System.Windows.Forms.Label();
        this.lblServerInfoValue = new System.Windows.Forms.Label();
        this.lblServerInfo = new System.Windows.Forms.Label();
        this.panelDesktop = new System.Windows.Forms.Panel();
        this.picDesktop = new System.Windows.Forms.PictureBox();
        this.panelControls = new System.Windows.Forms.Panel();
        this.btnFullscreen = new System.Windows.Forms.Button();
        this.btnSettings = new System.Windows.Forms.Button();
        this.lblLatency = new System.Windows.Forms.Label();
        this.panelHeader.SuspendLayout();
        this.panelConnection.SuspendLayout();
        this.panelStatus.SuspendLayout();
        this.panelDesktop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.picDesktop)).BeginInit();
        this.panelControls.SuspendLayout();
        this.SuspendLayout();
        // 
        // panelHeader
        // 
        this.panelHeader.BackColor = System.Drawing.Color.DodgerBlue;
        this.panelHeader.Controls.Add(this.lblTitle);
        this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelHeader.Location = new System.Drawing.Point(0, 0);
        this.panelHeader.Name = "panelHeader";
        this.panelHeader.Size = new System.Drawing.Size(1200, 60);
        this.panelHeader.TabIndex = 0;
        // 
        // lblTitle
        // 
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold);
        this.lblTitle.ForeColor = System.Drawing.Color.White;
        this.lblTitle.Location = new System.Drawing.Point(20, 15);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(260, 32);
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "Remote Desktop Client";
        // 
        // panelConnection
        // 
        this.panelConnection.BackColor = System.Drawing.Color.WhiteSmoke;
        this.panelConnection.Controls.Add(this.btnDisconnect);
        this.panelConnection.Controls.Add(this.btnConnect);
        this.panelConnection.Controls.Add(this.txtPassword);
        this.panelConnection.Controls.Add(this.lblPassword);
        this.panelConnection.Controls.Add(this.txtPort);
        this.panelConnection.Controls.Add(this.lblPort);
        this.panelConnection.Controls.Add(this.txtServerIP);
        this.panelConnection.Controls.Add(this.lblServerIP);
        this.panelConnection.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelConnection.Location = new System.Drawing.Point(0, 60);
        this.panelConnection.Name = "panelConnection";
        this.panelConnection.Size = new System.Drawing.Size(1200, 110);
        this.panelConnection.TabIndex = 1;
        // 
        // btnDisconnect
        // 
        this.btnDisconnect.BackColor = System.Drawing.Color.Red;
        this.btnDisconnect.Enabled = false;
        this.btnDisconnect.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
        this.btnDisconnect.ForeColor = System.Drawing.Color.White;
        this.btnDisconnect.Location = new System.Drawing.Point(550, 60);
        this.btnDisconnect.Name = "btnDisconnect";
        this.btnDisconnect.Size = new System.Drawing.Size(100, 40);
        this.btnDisconnect.TabIndex = 7;
        this.btnDisconnect.Text = "Disconnect";
        this.btnDisconnect.UseVisualStyleBackColor = false;
        this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
        // 
        // btnConnect
        // 
        this.btnConnect.BackColor = System.Drawing.Color.Green;
        this.btnConnect.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
        this.btnConnect.ForeColor = System.Drawing.Color.White;
        this.btnConnect.Location = new System.Drawing.Point(440, 60);
        this.btnConnect.Name = "btnConnect";
        this.btnConnect.Size = new System.Drawing.Size(100, 40);
        this.btnConnect.TabIndex = 6;
        this.btnConnect.Text = "Connect";
        this.btnConnect.UseVisualStyleBackColor = false;
        this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
        // 
        // txtPassword
        // 
        this.txtPassword.Font = new System.Drawing.Font("Arial", 10F);
        this.txtPassword.Location = new System.Drawing.Point(800, 35);
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.Size = new System.Drawing.Size(150, 23);
        this.txtPassword.TabIndex = 5;
        this.txtPassword.UseSystemPasswordChar = true;
        // 
        // lblPassword
        // 
        this.lblPassword.AutoSize = true;
        this.lblPassword.Font = new System.Drawing.Font("Arial", 10F);
        this.lblPassword.Location = new System.Drawing.Point(720, 38);
        this.lblPassword.Name = "lblPassword";
        this.lblPassword.Size = new System.Drawing.Size(74, 16);
        this.lblPassword.TabIndex = 4;
        this.lblPassword.Text = "Password:";
        // 
        // txtPort
        // 
        this.txtPort.Font = new System.Drawing.Font("Arial", 10F);
        this.txtPort.Location = new System.Drawing.Point(540, 35);
        this.txtPort.Name = "txtPort";
        this.txtPort.Size = new System.Drawing.Size(80, 23);
        this.txtPort.TabIndex = 3;
        this.txtPort.Text = "5000";
        // 
        // lblPort
        // 
        this.lblPort.AutoSize = true;
        this.lblPort.Font = new System.Drawing.Font("Arial", 10F);
        this.lblPort.Location = new System.Drawing.Point(503, 38);
        this.lblPort.Name = "lblPort";
        this.lblPort.Size = new System.Drawing.Size(31, 16);
        this.lblPort.TabIndex = 2;
        this.lblPort.Text = "Port:";
        // 
        // txtServerIP
        // 
        this.txtServerIP.Font = new System.Drawing.Font("Arial", 10F);
        this.txtServerIP.Location = new System.Drawing.Point(350, 35);
        this.txtServerIP.Name = "txtServerIP";
        this.txtServerIP.Size = new System.Drawing.Size(140, 23);
        this.txtServerIP.TabIndex = 1;
        this.txtServerIP.Text = "127.0.0.1";
        // 
        // lblServerIP
        // 
        this.lblServerIP.AutoSize = true;
        this.lblServerIP.Font = new System.Drawing.Font("Arial", 10F);
        this.lblServerIP.Location = new System.Drawing.Point(280, 38);
        this.lblServerIP.Name = "lblServerIP";
        this.lblServerIP.Size = new System.Drawing.Size(64, 16);
        this.lblServerIP.TabIndex = 0;
        this.lblServerIP.Text = "Server IP:";
        // 
        // panelStatus
        // 
        this.panelStatus.BackColor = System.Drawing.Color.LightGray;
        this.panelStatus.Controls.Add(this.lblConnectionStatusValue);
        this.panelStatus.Controls.Add(this.lblConnectionStatus);
        this.panelStatus.Controls.Add(this.lblServerInfoValue);
        this.panelStatus.Controls.Add(this.lblServerInfo);
        this.panelStatus.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelStatus.Location = new System.Drawing.Point(0, 170);
        this.panelStatus.Name = "panelStatus";
        this.panelStatus.Size = new System.Drawing.Size(1200, 50);
        this.panelStatus.TabIndex = 2;
        // 
        // lblConnectionStatusValue
        // 
        this.lblConnectionStatusValue.AutoSize = true;
        this.lblConnectionStatusValue.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
        this.lblConnectionStatusValue.ForeColor = System.Drawing.Color.Red;
        this.lblConnectionStatusValue.Location = new System.Drawing.Point(150, 15);
        this.lblConnectionStatusValue.Name = "lblConnectionStatusValue";
        this.lblConnectionStatusValue.Size = new System.Drawing.Size(102, 16);
        this.lblConnectionStatusValue.TabIndex = 3;
        this.lblConnectionStatusValue.Text = "Disconnected";
        // 
        // lblConnectionStatus
        // 
        this.lblConnectionStatus.AutoSize = true;
        this.lblConnectionStatus.Font = new System.Drawing.Font("Arial", 10F);
        this.lblConnectionStatus.Location = new System.Drawing.Point(20, 15);
        this.lblConnectionStatus.Name = "lblConnectionStatus";
        this.lblConnectionStatus.Size = new System.Drawing.Size(124, 16);
        this.lblConnectionStatus.TabIndex = 2;
        this.lblConnectionStatus.Text = "Connection Status:";
        // 
        // lblServerInfoValue
        // 
        this.lblServerInfoValue.AutoSize = true;
        this.lblServerInfoValue.Font = new System.Drawing.Font("Arial", 10F);
        this.lblServerInfoValue.Location = new System.Drawing.Point(550, 15);
        this.lblServerInfoValue.Name = "lblServerInfoValue";
        this.lblServerInfoValue.Size = new System.Drawing.Size(11, 16);
        this.lblServerInfoValue.TabIndex = 1;
        this.lblServerInfoValue.Text = "-";
        // 
        // lblServerInfo
        // 
        this.lblServerInfo.AutoSize = true;
        this.lblServerInfo.Font = new System.Drawing.Font("Arial", 10F);
        this.lblServerInfo.Location = new System.Drawing.Point(380, 15);
        this.lblServerInfo.Name = "lblServerInfo";
        this.lblServerInfo.Size = new System.Drawing.Size(164, 16);
        this.lblServerInfo.TabIndex = 0;
        this.lblServerInfo.Text = "Connected Server Info:";
        // 
        // panelDesktop
        // 
        this.panelDesktop.BackColor = System.Drawing.Color.Black;
        this.panelDesktop.Controls.Add(this.picDesktop);
        this.panelDesktop.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelDesktop.Location = new System.Drawing.Point(0, 220);
        this.panelDesktop.Name = "panelDesktop";
        this.panelDesktop.Size = new System.Drawing.Size(1200, 430);
        this.panelDesktop.TabIndex = 3;
        // 
        // picDesktop
        // 
        this.picDesktop.BackColor = System.Drawing.Color.Black;
        this.picDesktop.Dock = System.Windows.Forms.DockStyle.Fill;
        this.picDesktop.Location = new System.Drawing.Point(0, 0);
        this.picDesktop.Name = "picDesktop";
        this.picDesktop.Size = new System.Drawing.Size(1200, 430);
        this.picDesktop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.picDesktop.TabIndex = 0;
        this.picDesktop.TabStop = false;
        // 
        // panelControls
        // 
        this.panelControls.BackColor = System.Drawing.Color.LightGray;
        this.panelControls.Controls.Add(this.lblLatency);
        this.panelControls.Controls.Add(this.btnFullscreen);
        this.panelControls.Controls.Add(this.btnSettings);
        this.panelControls.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panelControls.Location = new System.Drawing.Point(0, 650);
        this.panelControls.Name = "panelControls";
        this.panelControls.Size = new System.Drawing.Size(1200, 50);
        this.panelControls.TabIndex = 4;
        // 
        // btnFullscreen
        // 
        this.btnFullscreen.Location = new System.Drawing.Point(100, 12);
        this.btnFullscreen.Name = "btnFullscreen";
        this.btnFullscreen.Size = new System.Drawing.Size(80, 30);
        this.btnFullscreen.TabIndex = 2;
        this.btnFullscreen.Text = "Fullscreen";
        this.btnFullscreen.UseVisualStyleBackColor = true;
        this.btnFullscreen.Click += new System.EventHandler(this.btnFullscreen_Click);
        // 
        // btnSettings
        // 
        this.btnSettings.Location = new System.Drawing.Point(190, 12);
        this.btnSettings.Name = "btnSettings";
        this.btnSettings.Size = new System.Drawing.Size(80, 30);
        this.btnSettings.TabIndex = 1;
        this.btnSettings.Text = "Settings";
        this.btnSettings.UseVisualStyleBackColor = true;
        this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
        // 
        // lblLatency
        // 
        this.lblLatency.AutoSize = true;
        this.lblLatency.Font = new System.Drawing.Font("Arial", 10F);
        this.lblLatency.Location = new System.Drawing.Point(1000, 17);
        this.lblLatency.Name = "lblLatency";
        this.lblLatency.Size = new System.Drawing.Size(100, 16);
        this.lblLatency.TabIndex = 3;
        this.lblLatency.Text = "Latency: -- ms";
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1200, 700);
        this.Controls.Add(this.panelDesktop);
        this.Controls.Add(this.panelStatus);
        this.Controls.Add(this.panelConnection);
        this.Controls.Add(this.panelHeader);
        this.Controls.Add(this.panelControls);
        this.Font = new System.Drawing.Font("Arial", 9F);
        this.Name = "Form1";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Remote Desktop Client";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
        this.panelHeader.ResumeLayout(false);
        this.panelHeader.PerformLayout();
        this.panelConnection.ResumeLayout(false);
        this.panelConnection.PerformLayout();
        this.panelStatus.ResumeLayout(false);
        this.panelStatus.PerformLayout();
        this.panelDesktop.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.picDesktop)).EndInit();
        this.panelControls.ResumeLayout(false);
        this.panelControls.PerformLayout();
        this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Panel panelHeader;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Panel panelConnection;
    private System.Windows.Forms.Button btnDisconnect;
    private System.Windows.Forms.Button btnConnect;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtPort;
    private System.Windows.Forms.Label lblPort;
    private System.Windows.Forms.TextBox txtServerIP;
    private System.Windows.Forms.Label lblServerIP;
    private System.Windows.Forms.Panel panelStatus;
    private System.Windows.Forms.Label lblConnectionStatusValue;
    private System.Windows.Forms.Label lblConnectionStatus;
    private System.Windows.Forms.Label lblServerInfoValue;
    private System.Windows.Forms.Label lblServerInfoValue2;
    private System.Windows.Forms.Label lblServerInfo;
    private System.Windows.Forms.Panel panelDesktop;
    private System.Windows.Forms.PictureBox picDesktop;
    private System.Windows.Forms.Panel panelControls;
    private System.Windows.Forms.Button btnFullscreen;
    private System.Windows.Forms.Button btnSettings;
    private System.Windows.Forms.Label lblLatency;
}
