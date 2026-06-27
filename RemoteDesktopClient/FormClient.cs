using System.Diagnostics;
using System.Net.Sockets;
using RemoteDesktopShared;

namespace RemoteDesktopClient;

public partial class FormClient : Form
{
    #region 1. Variables & Fields
    private Socket clientSocket;
    private bool isConnected = false;
    private Thread? receiveThread;
    private Stopwatch latencyStopwatch = new();
    private ScreenReceiver screenReceiver;
    private int targetPIN = 0;
    private int currentQuality = 50;
    private int settingQuality = 50;
    private int settingCompression = 0; // 0: Low, 1: Med, 2: High
    private int settingInput = 0;       // 0: Full Control, 1: View Only
    private PictureBoxSizeMode settingDisplay = PictureBoxSizeMode.StretchImage;
    #endregion

    #region 2. Constructor & Initialization
    public FormClient()
    {
        InitializeComponent();
        picDesktop.SizeMode = PictureBoxSizeMode.StretchImage;
        // Bật nhận diện phím cho Form
        this.KeyPreview = true;

        // Đăng ký các event chuột và phím
        picDesktop.MouseMove += PicDesktop_MouseMove;
        picDesktop.MouseDown += PicDesktop_MouseDown;
        picDesktop.MouseUp += PicDesktop_MouseUp;

        this.KeyDown += FormClient_KeyDown;
        this.KeyUp += FormClient_KeyUp;

        picDesktop.MouseWheel += PicDesktop_MouseWheel;
        picDesktop.MouseEnter += (s, e) => picDesktop.Focus();
    }
    #endregion

    #region 3. UI Control Events
    private void btnConnect_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtServerIP.Text))
        {
            MessageBox.Show("Please enter a server IP address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!int.TryParse(txtPort.Text, out int port))
        {
            MessageBox.Show("Invalid port number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!int.TryParse(txtPassword.Text.Trim(), out targetPIN))
        {
            MessageBox.Show("Please enter PIN!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var serverIP = txtServerIP.Text;

        try
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            latencyStopwatch.Restart();

            // Kết nối bất đồng bộ
            clientSocket.BeginConnect(serverIP, port, ConnectCallback, clientSocket);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to connect: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnDisconnect_Click(object sender, EventArgs e)
    {
        DisconnectFromServer();
    }

    private void btnFullscreen_Click(object sender, EventArgs e)
    {
        if (!isConnected)
        {
            MessageBox.Show("Please connect to a server first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (this.FormBorderStyle == FormBorderStyle.None)
        {
            // Exit fullscreen
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.WindowState = FormWindowState.Normal;
            this.MaximizeBox = false;
        }
        else
        {
            // Enter fullscreen
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }
    }

    private void btnSettings_Click(object sender, EventArgs e)
    {
        using (var frmSettings = new FormSettings(settingQuality, settingCompression, settingInput, settingDisplay))
        {
            if (frmSettings.ShowDialog() == DialogResult.OK)
            {
                // Lưu state
                settingQuality = frmSettings.Quality;
                settingCompression = frmSettings.CompressionLevel;
                settingInput = frmSettings.InputMethod;
                settingDisplay = frmSettings.DisplayMode;

                // Áp dụng ngay Display Settings cho PictureBox
                picDesktop.SizeMode = settingDisplay;

                // Gửi cấu hình về Server (nếu đang kết nối)
                if (isConnected)
                {
                    // Lệnh 1: Gửi Quality
                    SendCommand(new CommandPacket { Type = CommandType.ChangeQuality, X = settingQuality, Y = 0, KeyCode = 0, ClientWidth = 0, ClientHeight = 0 });

                    // Lệnh 2: Gửi Compression Scale (Low=100%, Med=75%, High=50%)
                    int scalePercent = settingCompression == 0 ? 100 : (settingCompression == 1 ? 75 : 50);
                    SendCommand(new CommandPacket { Type = CommandType.ChangeCompression, X = scalePercent, Y = 0, KeyCode = 0, ClientWidth = 0, ClientHeight = 0 });
                }
            }
        }
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (isConnected)
        {
            DisconnectFromServer();
        }
    }
    #endregion

    #region 4. Input Capturing (Mouse & Keyboard)
    private void PicDesktop_MouseMove(object? sender, MouseEventArgs e)
    {
        //SendCommand(new CommandPacket { Type = CommandType.MouseMove, X = e.X, Y = e.Y, KeyCode = 0 });
        SendCommand(new CommandPacket
        {
            Type = CommandType.MouseMove,
            X = e.X,
            Y = e.Y,
            KeyCode = 0,
            ClientWidth = picDesktop.Width,   // Gửi kèm chiều rộng
            ClientHeight = picDesktop.Height  // Gửi kèm chiều cao
        });
    }

    private void PicDesktop_MouseWheel(object? sender, MouseEventArgs e)
    {
        SendCommand(new CommandPacket
        {
            Type = CommandType.MouseWheel,
            X = 0,
            Y = 0,
            KeyCode = e.Delta, // e.Delta chứa lực mang giá trị 120 (cuộn lên) hoặc -120 (cuộn xuống)
            ClientWidth = picDesktop.Width,
            ClientHeight = picDesktop.Height
        });
    }

    private void PicDesktop_MouseDown(object? sender, MouseEventArgs e)
    {
        var cmdType = CommandType.MouseMove;

        if (e.Button == MouseButtons.Left) cmdType = CommandType.LeftMouseDown;
        else if (e.Button == MouseButtons.Right) cmdType = CommandType.RightMouseDown;

        if (cmdType != CommandType.MouseMove)
        {
            SendCommand(new CommandPacket
            {
                Type = cmdType,
                X = e.X,
                Y = e.Y,
                KeyCode = 0,
                ClientWidth = picDesktop.Width,
                ClientHeight = picDesktop.Height
            });
        }
    }

    private void PicDesktop_MouseUp(object? sender, MouseEventArgs e)
    {
        var cmdType = CommandType.MouseMove;

        if (e.Button == MouseButtons.Left) cmdType = CommandType.LeftMouseUp;
        else if (e.Button == MouseButtons.Right) cmdType = CommandType.RightMouseUp;

        if (cmdType != CommandType.MouseMove)

            SendCommand(new CommandPacket
            {
                Type = cmdType,
                X = e.X,
                Y = e.Y,
                KeyCode = 0,
                ClientWidth = picDesktop.Width,
                ClientHeight = picDesktop.Height
            });
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (isConnected)
        {
            // Bắt thóp tất cả các "phím điều hướng giao diện" mà WinForms hay cướp:
            // Phím Cách (Space), Tab, Enter, và 4 phím mũi tên
            switch (keyData)
            {
                case Keys.Space:
                case Keys.Tab:
                case Keys.Enter:
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    // Tự tay gửi phím sang máy Server
                    SendCommand(new CommandPacket
                    {
                        Type = CommandType.KeyDown,
                        X = 0,
                        Y = 0,
                        KeyCode = (int)keyData
                    });

                    return true; // Trả về TRUE = Ra lệnh cho WinForms: "Tôi nuốt phím này rồi, cấm kích hoạt nút đang Highlight!"
            }
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void FormClient_KeyDown(object? sender, KeyEventArgs e)
    {
        SendCommand(new CommandPacket { Type = CommandType.KeyDown, X = 0, Y = 0, KeyCode = (int)e.KeyCode });
    }

    private void FormClient_KeyUp(object? sender, KeyEventArgs e)
    {
        SendCommand(new CommandPacket { Type = CommandType.KeyUp, X = 0, Y = 0, KeyCode = (int)e.KeyCode });
    }
    #endregion

    #region 5. Network Communication & Data Processing
    private void SendCommand(CommandPacket packet)
    {
        if (settingInput == 1)
        {
            // Nếu là lệnh điều khiển ngoại vi (chuột/phím) thì từ chối gửi đi
            if (packet.Type == CommandType.MouseMove ||
                packet.Type == CommandType.LeftMouseDown ||
                packet.Type == CommandType.LeftMouseUp ||
                packet.Type == CommandType.RightMouseDown ||
                packet.Type == CommandType.RightMouseUp ||
                packet.Type == CommandType.KeyDown ||
                packet.Type == CommandType.KeyUp)
            {
                return; // Chặn lập tức
            }
        }
        if (isConnected && clientSocket != null && clientSocket.Connected)
        {
            try
            {
                byte[] data = packet.ToBytes();
                clientSocket.Send(data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error command: {ex.Message}");
            }
        }
    }

    private void ConnectCallback(IAsyncResult result)
    {
        try
        {
            clientSocket?.EndConnect(result);

            if (clientSocket != null && clientSocket.Connected)
            {
                latencyStopwatch.Stop();
                isConnected = true;

                // KẾT NỐI XONG -> CHƯA BẬT UDP VỘI -> GỬI GÓI TIN MÃ PIN TRƯỚC
                SendCommand(new CommandPacket
                {
                    Type = CommandType.Login,
                    X = targetPIN,
                    Y = 0,
                    KeyCode = 0,
                    ClientWidth = 0,
                    ClientHeight = 0
                });

                ClientStateObject state = new ClientStateObject { WorkSocket = clientSocket };
                clientSocket.BeginReceive(state.Buffer, 0, ClientStateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
        }
        catch (Exception ex)
        {
            UpdateUI(() => MessageBox.Show($"Connection error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        if (!isConnected) return;
        try
        {
            ClientStateObject state = (ClientStateObject)ar.AsyncState;
            Socket socket = state.WorkSocket;
            int bytesRead = socket.EndReceive(ar);

            if (bytesRead > 0)
            {
                CommandPacket reply = CommandPacket.FromBytes(state.Buffer, bytesRead);

                // XỬ LÝ PHẢN HỒI TỪ SERVER
                if (reply.Type == CommandType.LoginResult)
                {
                    if (reply.X == 1) // PIN ĐÚNG
                    {
                        UpdateUI(() =>
                        {
                            btnConnect.Enabled = false;
                            btnDisconnect.Enabled = true;
                            txtServerIP.Enabled = false;
                            txtPort.Enabled = false;
                            txtPassword.Enabled = false;
                            lblConnectionStatusValue.Text = "Connected & Authenticated";
                            lblConnectionStatusValue.ForeColor = Color.Green;
                            lblServerInfoValue.Text = $"{txtServerIP.Text}:{txtPort.Text}";
                            lblLatency.Text = $"Latency: {latencyStopwatch.ElapsedMilliseconds} ms";
                        });

                        // PIN đúng rồi mới bắt đầu bật màn hình UDP
                        screenReceiver = new ScreenReceiver();
                        screenReceiver.OnImageReceived += UpdateDesktopImage;
                        int assignedUdpPort = screenReceiver.StartListening(0);

                        SendCommand(new CommandPacket { Type = CommandType.RegisterUdpPort, X = assignedUdpPort, Y = 0, KeyCode = 0, ClientWidth = 0, ClientHeight = 0 });
                    }
                    else // PIN SAI
                    {
                        UpdateUI(() => MessageBox.Show("PIN is incorect! Rejected.", "Rejected", MessageBoxButtons.OK, MessageBoxIcon.Stop));
                        DisconnectFromServer();
                        return;
                    }
                }

                Array.Clear(state.Buffer, 0, bytesRead);
                socket.BeginReceive(state.Buffer, 0, ClientStateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            else { DisconnectFromServer(); }
        }
        catch { DisconnectFromServer(); }
    }

    private void DisconnectFromServer()
    {
        if (!isConnected) return;

        try
        {
            isConnected = false;

            screenReceiver?.StopListening();
            screenReceiver = null;

            // Đóng luồng Socket an toàn (ngắt Send và Receive)
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
            }
            clientSocket?.Close();
            clientSocket = null;

            UpdateUI(() =>
            {
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                txtServerIP.Enabled = true;
                txtPort.Enabled = true;
                txtPassword.Enabled = true;
                lblConnectionStatusValue.Text = "Disconnected";
                lblConnectionStatusValue.ForeColor = Color.Red;
                lblServerInfoValue.Text = "-";
                lblLatency.Text = "Latency: -- ms";
                picDesktop.Image = null;
            });
        }
        catch (Exception ex)
        {
            UpdateUI(() =>
            {
                MessageBox.Show($"Error disconnecting: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }
    }

    private void UpdateDesktopImage(Image img)
    {
        if (InvokeRequired)
        {
            // Dùng BeginInvoke để luồng UI vẽ ảnh mượt mà không bị khựng
            BeginInvoke(new Action<Image>(UpdateDesktopImage), img);
            return;
        }

        var oldImage = picDesktop.Image;
        picDesktop.Image = img;

        // Xóa ảnh cũ khỏi RAM sau khi đã gắn ảnh mới
        oldImage?.Dispose();
    }

    private void UpdateUI(Action action)
    {
        if (InvokeRequired) Invoke(action);
        else action();
    }
    #endregion

    public class ClientStateObject
    {
        public Socket WorkSocket { get; set; }
        public const int BufferSize = 1024;
        public byte[] Buffer = new byte[BufferSize];
    }
}