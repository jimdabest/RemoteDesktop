using System.Diagnostics;
using System.Net.Sockets;
using RemoteDesktopShared;

namespace RemoteDesktopClient;

public partial class FormClient : Form
{
    #region Bien toan cuc 
    private Socket clientSocket;
    private bool isConnected = false;
    private Thread? receiveThread;
    private Stopwatch latencyStopwatch = new();
    private ScreenReceiver screenReceiver;
    #endregion

    #region Constructor va Initialization
    public FormClient()
    {
        InitializeComponent();

        // Bật nhận diện phím cho Form
        this.KeyPreview = true;

        // Đăng ký các event chuột và phím
        picDesktop.MouseMove += PicDesktop_MouseMove;
        picDesktop.MouseDown += PicDesktop_MouseDown;
        picDesktop.MouseUp += PicDesktop_MouseUp;

        this.KeyDown += FormClient_KeyDown;
        this.KeyUp += FormClient_KeyUp;
    }
    #endregion

    #region UI Control Events
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
        MessageBox.Show("Settings window will be implemented.\n\nAvailable options:\n- Image quality\n- Compression level\n- Input method\n- Display settings",
            "Settings",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (isConnected)
        {
            DisconnectFromServer();
        }
    }
    #endregion

    #region Input capture events 
    private void PicDesktop_MouseMove(object? sender, MouseEventArgs e)
    {
        SendCommand(new CommandPacket { Type = CommandType.MouseMove, X = e.X, Y = e.Y, KeyCode = 0 });
    }

    private void PicDesktop_MouseDown(object? sender, MouseEventArgs e)
    {
        var cmdType = CommandType.MouseMove;

        if (e.Button == MouseButtons.Left) cmdType = CommandType.LeftMouseDown;
        else if (e.Button == MouseButtons.Right) cmdType = CommandType.RightMouseDown;

        if (cmdType != CommandType.MouseMove)
        {
            SendCommand(new CommandPacket { Type = cmdType, X = e.X, Y = e.Y, KeyCode = 0 });
        }
    }

    private void PicDesktop_MouseUp(object? sender, MouseEventArgs e)
    {
        var cmdType = CommandType.MouseMove;

        if (e.Button == MouseButtons.Left) cmdType = CommandType.LeftMouseUp;
        else if (e.Button == MouseButtons.Right) cmdType = CommandType.RightMouseUp;

        if (cmdType != CommandType.MouseMove)
        {
            SendCommand(new CommandPacket { Type = cmdType, X = e.X, Y = e.Y, KeyCode = 0 });
        }
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

    #region Network Communication & Data Processing
    private void SendCommand(CommandPacket packet)
    {
        if (isConnected && clientSocket != null && clientSocket.Connected)
        {
            try
            {
                byte[] data = packet.ToBytes();
                clientSocket.Send(data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi gửi lệnh: {ex.Message}");
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

                UpdateUI(() =>
                {
                    btnConnect.Enabled = false;
                    btnDisconnect.Enabled = true;
                    txtServerIP.Enabled = false;
                    txtPort.Enabled = false;
                    txtPassword.Enabled = false;
                    lblConnectionStatusValue.Text = "Connected (Pure Socket)";
                    lblConnectionStatusValue.ForeColor = Color.Green;
                    lblServerInfoValue.Text = $"{txtServerIP.Text}:{txtPort.Text}";
                    lblLatency.Text = $"Latency: {latencyStopwatch.ElapsedMilliseconds} ms";
                });

                // Khởi tạo ScreenReceiver để nhận ảnh qua UDP
                screenReceiver = new ScreenReceiver();
                screenReceiver.OnImageReceived += UpdateDesktopImage; // Gắn event khi có ảnh
                screenReceiver.StartListening(5001);

                receiveThread = new Thread(ReceiveData) { IsBackground = true };
                receiveThread.Start();
            }
        }
        catch (Exception ex)
        {
            UpdateUI(() =>
            {
                MessageBox.Show($"Connection error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }
    }

    private void ReceiveData()
    {
        byte[] buffer = new byte[65536]; // Large buffer for image data

        try
        {
            while (isConnected && clientSocket != null)
            {
                // Đọc dữ liệu trực tiếp bằng Socket
                int bytesRead = clientSocket.Receive(buffer);
                if (bytesRead == 0)
                {
                    break; // Server chủ động đóng kết nối
                }

                ProcessReceivedData(buffer, bytesRead);
            }
        }
        catch (SocketException)
        {
            // Mất kết nối mạng
        }
        catch (Exception ex)
        {
            UpdateUI(() =>
            {
                MessageBox.Show($"Error receiving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }
        finally
        {
            DisconnectFromServer();
        }
    }

    private void ProcessReceivedData(byte[] buffer, int bytesRead)
    {
        // TODO: Process screenshot data
        // This would typically:
        // 1. Decompress the image data
        // 2. Convert to Bitmap
        // 3. Display in picDesktop
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
}