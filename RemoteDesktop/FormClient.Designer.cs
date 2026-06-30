namespace RemoteDesktop;

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
        panelHeader = new Panel();
        lblTitle = new Label();
        panelConnection = new Panel();
        btnDisconnect = new Button();
        btnConnect = new Button();
        txtPassword = new TextBox();
        lblPassword = new Label();
        txtPort = new TextBox();
        lblPort = new Label();
        txtServerIP = new TextBox();
        lblServerIP = new Label();
        panelStatus = new Panel();
        lblConnectionStatusValue = new Label();
        lblConnectionStatus = new Label();
        lblServerInfoValue = new Label();
        lblServerInfo = new Label();
        panelDesktop = new Panel();
        picDesktop = new PictureBox();
        panelControls = new Panel();
        lblLatency = new Label();
        btnFullscreen = new Button();
        btnSettings = new Button();
        panelHeader.SuspendLayout();
        panelConnection.SuspendLayout();
        panelStatus.SuspendLayout();
        panelDesktop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picDesktop).BeginInit();
        panelControls.SuspendLayout();
        SuspendLayout();
        // 
        // panelHeader
        // 
        panelHeader.BackColor = Color.DodgerBlue;
        panelHeader.Controls.Add(lblTitle);
        panelHeader.Dock = DockStyle.Top;
        panelHeader.Location = new Point(0, 0);
        panelHeader.Name = "panelHeader";
        panelHeader.Size = new Size(843, 60);
        panelHeader.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Arial", 20F, FontStyle.Bold);
        lblTitle.ForeColor = Color.White;
        lblTitle.Location = new Point(20, 15);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(389, 40);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Remote Desktop Client";
        // 
        // panelConnection
        // 
        panelConnection.BackColor = Color.WhiteSmoke;
        panelConnection.Controls.Add(btnDisconnect);
        panelConnection.Controls.Add(btnConnect);
        panelConnection.Controls.Add(txtPassword);
        panelConnection.Controls.Add(lblPassword);
        panelConnection.Controls.Add(txtPort);
        panelConnection.Controls.Add(lblPort);
        panelConnection.Controls.Add(txtServerIP);
        panelConnection.Controls.Add(lblServerIP);
        panelConnection.Dock = DockStyle.Top;
        panelConnection.Location = new Point(0, 60);
        panelConnection.Name = "panelConnection";
        panelConnection.Size = new Size(843, 110);
        panelConnection.TabIndex = 1;
        // 
        // btnDisconnect
        // 
        btnDisconnect.BackColor = Color.Red;
        btnDisconnect.Enabled = false;
        btnDisconnect.Font = new Font("Arial", 10F, FontStyle.Bold);
        btnDisconnect.ForeColor = Color.White;
        btnDisconnect.Location = new Point(130, 50);
        btnDisconnect.Name = "btnDisconnect";
        btnDisconnect.Size = new Size(174, 40);
        btnDisconnect.TabIndex = 7;
        btnDisconnect.Text = "Disconnect";
        btnDisconnect.UseVisualStyleBackColor = false;
        btnDisconnect.Click += btnDisconnect_Click;
        // 
        // btnConnect
        // 
        btnConnect.BackColor = Color.Green;
        btnConnect.Font = new Font("Arial", 10F, FontStyle.Bold);
        btnConnect.ForeColor = Color.White;
        btnConnect.Location = new Point(20, 50);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(100, 40);
        btnConnect.TabIndex = 6;
        btnConnect.Text = "Connect";
        btnConnect.UseVisualStyleBackColor = false;
        btnConnect.Click += btnConnect_Click;
        // 
        // txtPassword
        // 
        txtPassword.Font = new Font("Arial", 10F);
        txtPassword.Location = new Point(453, 15);
        txtPassword.Name = "txtPassword";
        txtPassword.Size = new Size(150, 27);
        txtPassword.TabIndex = 5;
        txtPassword.UseSystemPasswordChar = true;
        // 
        // lblPassword
        // 
        lblPassword.AutoSize = true;
        lblPassword.Font = new Font("Arial", 10F);
        lblPassword.Location = new Point(373, 18);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(85, 19);
        lblPassword.TabIndex = 4;
        lblPassword.Text = "Password:";
        // 
        // txtPort
        // 
        txtPort.Font = new Font("Arial", 10F);
        txtPort.Location = new Point(284, 12);
        txtPort.Name = "txtPort";
        txtPort.Size = new Size(80, 27);
        txtPort.TabIndex = 3;
        txtPort.Text = "5000";
        // 
        // lblPort
        // 
        lblPort.AutoSize = true;
        lblPort.Font = new Font("Arial", 10F);
        lblPort.Location = new Point(243, 15);
        lblPort.Name = "lblPort";
        lblPort.Size = new Size(44, 19);
        lblPort.TabIndex = 2;
        lblPort.Text = "Port:";
        // 
        // txtServerIP
        // 
        txtServerIP.Font = new Font("Arial", 10F);
        txtServerIP.Location = new Point(105, 12);
        txtServerIP.Name = "txtServerIP";
        txtServerIP.Size = new Size(140, 27);
        txtServerIP.TabIndex = 1;
        txtServerIP.Text = "127.0.0.1";
        // 
        // lblServerIP
        // 
        lblServerIP.AutoSize = true;
        lblServerIP.Font = new Font("Arial", 10F);
        lblServerIP.Location = new Point(20, 15);
        lblServerIP.Name = "lblServerIP";
        lblServerIP.Size = new Size(83, 19);
        lblServerIP.TabIndex = 0;
        lblServerIP.Text = "Server IP:";
        // 
        // panelStatus
        // 
        panelStatus.BackColor = Color.LightGray;
        panelStatus.Controls.Add(lblConnectionStatusValue);
        panelStatus.Controls.Add(lblConnectionStatus);
        panelStatus.Controls.Add(lblServerInfoValue);
        panelStatus.Controls.Add(lblServerInfo);
        panelStatus.Dock = DockStyle.Top;
        panelStatus.Location = new Point(0, 170);
        panelStatus.Name = "panelStatus";
        panelStatus.Size = new Size(843, 50);
        panelStatus.TabIndex = 2;
        // 
        // lblConnectionStatusValue
        // 
        lblConnectionStatusValue.AutoSize = true;
        lblConnectionStatusValue.Font = new Font("Arial", 10F, FontStyle.Bold);
        lblConnectionStatusValue.ForeColor = Color.Red;
        lblConnectionStatusValue.Location = new Point(173, 15);
        lblConnectionStatusValue.Name = "lblConnectionStatusValue";
        lblConnectionStatusValue.Size = new Size(116, 19);
        lblConnectionStatusValue.TabIndex = 3;
        lblConnectionStatusValue.Text = "Disconnected";
        // 
        // lblConnectionStatus
        // 
        lblConnectionStatus.AutoSize = true;
        lblConnectionStatus.Font = new Font("Arial", 10F);
        lblConnectionStatus.Location = new Point(20, 15);
        lblConnectionStatus.Name = "lblConnectionStatus";
        lblConnectionStatus.Size = new Size(147, 19);
        lblConnectionStatus.TabIndex = 2;
        lblConnectionStatus.Text = "Connection Status:";
        // 
        // lblServerInfoValue
        // 
        lblServerInfoValue.AutoSize = true;
        lblServerInfoValue.Font = new Font("Arial", 10F);
        lblServerInfoValue.Location = new Point(550, 15);
        lblServerInfoValue.Name = "lblServerInfoValue";
        lblServerInfoValue.Size = new Size(15, 19);
        lblServerInfoValue.TabIndex = 1;
        lblServerInfoValue.Text = "-";
        // 
        // lblServerInfo
        // 
        lblServerInfo.AutoSize = true;
        lblServerInfo.Font = new Font("Arial", 10F);
        lblServerInfo.Location = new Point(380, 15);
        lblServerInfo.Name = "lblServerInfo";
        lblServerInfo.Size = new Size(179, 19);
        lblServerInfo.TabIndex = 0;
        lblServerInfo.Text = "Connected Server Info:";
        // 
        // panelDesktop
        // 
        panelDesktop.BackColor = Color.Black;
        panelDesktop.Controls.Add(picDesktop);
        panelDesktop.Dock = DockStyle.Fill;
        panelDesktop.Location = new Point(0, 220);
        panelDesktop.Name = "panelDesktop";
        panelDesktop.Size = new Size(843, 430);
        panelDesktop.TabIndex = 3;
        // 
        // picDesktop
        // 
        picDesktop.BackColor = Color.Black;
        picDesktop.Dock = DockStyle.Fill;
        picDesktop.Location = new Point(0, 0);
        picDesktop.Name = "picDesktop";
        picDesktop.Size = new Size(843, 430);
        picDesktop.TabIndex = 0;
        picDesktop.TabStop = false;
        // 
        // panelControls
        // 
        panelControls.BackColor = Color.LightGray;
        panelControls.Controls.Add(lblLatency);
        panelControls.Controls.Add(btnFullscreen);
        panelControls.Controls.Add(btnSettings);
        panelControls.Dock = DockStyle.Bottom;
        panelControls.Location = new Point(0, 650);
        panelControls.Name = "panelControls";
        panelControls.Size = new Size(843, 50);
        panelControls.TabIndex = 4;
        // 
        // lblLatency
        // 
        lblLatency.AutoSize = true;
        lblLatency.Font = new Font("Arial", 10F);
        lblLatency.Location = new Point(284, 17);
        lblLatency.Name = "lblLatency";
        lblLatency.Size = new Size(115, 19);
        lblLatency.TabIndex = 3;
        lblLatency.Text = "Latency: -- ms";
        // 
        // btnFullscreen
        // 
        btnFullscreen.Location = new Point(36, 11);
        btnFullscreen.Name = "btnFullscreen";
        btnFullscreen.Size = new Size(84, 30);
        btnFullscreen.TabIndex = 2;
        btnFullscreen.Text = "Fullscreen";
        btnFullscreen.UseVisualStyleBackColor = true;
        btnFullscreen.Click += btnFullscreen_Click;
        // 
        // btnSettings
        // 
        btnSettings.Location = new Point(140, 11);
        btnSettings.Name = "btnSettings";
        btnSettings.Size = new Size(80, 30);
        btnSettings.TabIndex = 1;
        btnSettings.Text = "Settings";
        btnSettings.UseVisualStyleBackColor = true;
        btnSettings.Click += btnSettings_Click;
        // 
        // FormClient
        // 
        AutoScaleDimensions = new SizeF(8F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(843, 700);
        Controls.Add(panelDesktop);
        Controls.Add(panelStatus);
        Controls.Add(panelConnection);
        Controls.Add(panelHeader);
        Controls.Add(panelControls);
        Font = new Font("Arial", 9F);
        Name = "FormClient";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Remote Desktop Client";
        FormClosing += Form1_FormClosing;
        panelHeader.ResumeLayout(false);
        panelHeader.PerformLayout();
        panelConnection.ResumeLayout(false);
        panelConnection.PerformLayout();
        panelStatus.ResumeLayout(false);
        panelStatus.PerformLayout();
        panelDesktop.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picDesktop).EndInit();
        panelControls.ResumeLayout(false);
        panelControls.PerformLayout();
        ResumeLayout(false);
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
    private Label label2;
    private Label label1;
}
