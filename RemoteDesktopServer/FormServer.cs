using System;
using System.Collections.Generic;
using System.ComponentModel;
// using System.Data; // Đã comment lại để tránh lỗi đụng độ CommandType
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using RemoteDesktopShared;

namespace RemoteDesktopServer
{
    public partial class FormServer : Form
    {
        private Socket serverSocket;
        private bool isServerRunning = false;
        private Thread listenerThread;
        private List<ClientConnection> connectedClients = new List<ClientConnection>();

        #region Windows Thao Tac Phan Cung API
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        // ✅ FIX: Import API lấy kích thước màn hình vật lý thực tế
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;
        const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        const uint KEYEVENTF_KEYUP = 0x0002;
        #endregion

        public FormServer()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPort.Text) || !int.TryParse(txtPort.Text, out int port))
            {
                MessageBox.Show("Invalid port number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                serverSocket.Listen(100);

                isServerRunning = true;

                btnStart.Enabled = false;
                btnStop.Enabled = true;
                txtPort.Enabled = false;
                lblStatusValue.Text = "Online";
                lblStatusValue.ForeColor = Color.Green;

                AppendLog($"[{DateTime.Now:HH:mm:ss}] Server started on port {port}");

                listenerThread = new Thread(AcceptConnections) { IsBackground = true };
                listenerThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start server: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppendLog($"[{DateTime.Now:HH:mm:ss}] Error starting server: {ex.Message}");
                isServerRunning = false;
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                isServerRunning = false;
                serverSocket?.Close();

                foreach (var client in connectedClients.ToArray())
                {
                    client.ClientSocket?.Shutdown(SocketShutdown.Both);
                    client.ClientSocket?.Close();
                }

                connectedClients.Clear();

                btnStart.Enabled = true;
                btnStop.Enabled = false;
                txtPort.Enabled = true;
                lblStatusValue.Text = "Offline";
                lblStatusValue.ForeColor = Color.Red;
                lvClients.Items.Clear();

                AppendLog($"[{DateTime.Now:HH:mm:ss}] Server stopped");
            }
            catch (Exception ex)
            {
                AppendLog($"[{DateTime.Now:HH:mm:ss}] Error stopping server: {ex.Message}");
            }
        }

        private void AcceptConnections()
        {
            while (isServerRunning)
            {
                try
                {
                    Socket clientSocket = serverSocket.Accept();
                    IPEndPoint remoteIpEndPoint = (IPEndPoint)clientSocket.RemoteEndPoint;

                    string clientIP = remoteIpEndPoint.Address.ToString();
                    int clientPort = remoteIpEndPoint.Port;

                    ClientConnection connection = new ClientConnection
                    {
                        ClientSocket = clientSocket,
                        IP = clientIP,
                        Port = clientPort,
                        ConnectedTime = DateTime.Now
                    };

                    connectedClients.Add(connection);
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] Client connected: {clientIP}:{clientPort}");

                    UpdateClientListUI();

                    Thread clientThread = new Thread(() => HandleClient(connection)) { IsBackground = true };
                    clientThread.Start();
                }
                catch (SocketException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] Error accepting connection: {ex.Message}");
                }
            }
        }

        private void HandleClient(ClientConnection connection)
        {
            try
            {
                // Gói tin giờ là 24 bytes (thêm Width và Height)
                byte[] buffer = new byte[24];

                while (isServerRunning && connection.ClientSocket.Connected)
                {
                    int bytesRead = 0;

                    // Đảm bảo nhận đủ 24 bytes dữ liệu thô
                    while (bytesRead < 24)
                    {
                        int read = connection.ClientSocket.Receive(buffer, bytesRead, 24 - bytesRead, SocketFlags.None);
                        if (read == 0) return;
                        bytesRead += read;
                    }

                    CommandPacket packet = CommandPacket.FromBytes(buffer);
                    ExecuteCommand(packet, connection);
                }
            }
            catch (SocketException)
            {
                // Bỏ qua khi mất kết nối đột ngột
            }
            catch (Exception ex)
            {
                AppendLog($"[{DateTime.Now:HH:mm:ss}] Error handling client {connection.IP}: {ex.Message}");
            }
            finally
            {
                connection.Caster?.StopStreaming();
                try { connection.ClientSocket?.Shutdown(SocketShutdown.Both); } catch { }
                connection.ClientSocket?.Close();
                connectedClients.Remove(connection);
                AppendLog($"[{DateTime.Now:HH:mm:ss}] Client disconnected: {connection.IP}:{connection.Port}");
                UpdateClientListUI();
            }
        }

        private void ExecuteCommand(CommandPacket packet, ClientConnection connection)
        {
            // 1. Tính toán nội suy tọa độ
            int targetX = packet.X;
            int targetY = packet.Y;

            int serverWidth = Screen.PrimaryScreen.Bounds.Width;
            int serverHeight = Screen.PrimaryScreen.Bounds.Height;

            // Chỉ tính tam suất nếu Client có gửi thông số khung ảnh (tránh lỗi chia cho 0)
            if (packet.ClientWidth > 0 && packet.ClientHeight > 0)
            {
                // FIX: Ép kiểu double để phép chia không bị mất phần thập phân gây sai số liên tục
                targetX = (int)Math.Round((double)packet.X * serverWidth / packet.ClientWidth);
                targetY = (int)Math.Round((double)packet.Y * serverHeight / packet.ClientHeight);
            }

            // 2. Gọi lệnh phần cứng
            switch (packet.Type)
            {
                case CommandType.RegisterUdpPort:
                    int udpPort = packet.X; // Lấy cái Port mà Client vừa gửi qua
                    connection.Caster = new ScreenCaster(connection.IP, udpPort);
                    connection.Caster.StartStreaming();
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] Started ScreenCaster (UDP: {udpPort}) for {connection.IP}");
                    break;
                case CommandType.MouseMove:
                    // Dùng tọa độ đã quy đổi chuẩn xác để di chuyển chuột
                    SetCursorPos(targetX, targetY);
                    break;

                case CommandType.LeftMouseDown:
                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    break;

                case CommandType.LeftMouseUp:
                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                    break;

                case CommandType.RightMouseDown:
                    mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                    break;

                case CommandType.RightMouseUp:
                    mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                    break;

                case CommandType.KeyDown:
                    keybd_event((byte)packet.KeyCode, 0, 0, 0);
                    break;

                case CommandType.KeyUp:
                    keybd_event((byte)packet.KeyCode, 0, KEYEVENTF_KEYUP, 0);
                    break;

                default:
                    break;
            }
        }

        private void UpdateClientListUI()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateClientListUI));
                return;
            }

            lvClients.Items.Clear();
            foreach (var client in connectedClients)
            {
                ListViewItem item = new ListViewItem(client.IP);
                item.SubItems.Add(client.Port.ToString());
                item.SubItems.Add(client.ConnectedTime.ToString("yyyy-MM-dd HH:mm:ss"));
                item.SubItems.Add("Connected");
                lvClients.Items.Add(item);
            }

            lblConnectedClientsValue.Text = connectedClients.Count.ToString();
        }

        private void AppendLog(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AppendLog(message)));
                return;
            }

            txtLogs.AppendText(message + Environment.NewLine);
            if (txtLogs.Lines.Length > 10000)
            {
                int linesToRemove = txtLogs.Lines.Length - 5000;
                int startIndex = 0;
                for (int i = 0; i < linesToRemove; i++)
                {
                    startIndex = txtLogs.Text.IndexOf(Environment.NewLine, startIndex) + Environment.NewLine.Length;
                }
                txtLogs.Text = txtLogs.Text.Substring(startIndex);
            }
        }

        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            txtLogs.Clear();
            AppendLog($"[{DateTime.Now:HH:mm:ss}] Logs cleared");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isServerRunning)
            {
                btnStop_Click(null, null);
            }
        }

        private void panelControl_Paint(object sender, PaintEventArgs e)
        {
        }
    }

    public class ClientConnection
    {
        public Socket ClientSocket { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public DateTime ConnectedTime { get; set; }
        public ScreenCaster Caster { get; set; }
    }
}