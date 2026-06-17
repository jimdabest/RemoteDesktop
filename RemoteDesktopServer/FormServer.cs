using System;
using System.Collections.Generic;
using System.ComponentModel;
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

                serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
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

        private void AcceptCallback(IAsyncResult ar)
        {
            if (!isServerRunning) return;

            try
            {
                // 1. Kết thúc quá trình Accept và lấy ra Socket của Client
                Socket clientSocket = serverSocket.EndAccept(ar);

                // 2. NGAY LẬP TỨC đặt bẫy chờ Client tiếp theo (để Server không bị điếc)
                serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

                // 3. Khởi tạo kết nối cho Client hiện tại
                IPEndPoint remoteIpEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;
                ClientConnection connection = new ClientConnection
                {
                    ClientSocket = clientSocket,
                    IP = remoteIpEndPoint.Address.ToString(),
                    Port = remoteIpEndPoint.Port,
                    ConnectedTime = DateTime.Now
                };

                connectedClients.Add(connection);
                AppendLog($"[{DateTime.Now:HH:mm:ss}] Client connected: {connection.IP}:{connection.Port}");
                UpdateClientListUI();

                // Bật luồng UDP (ScreenCaster) như cũ
                connection.Caster = new ScreenCaster(connection.IP, 5001);
                connection.Caster.StartStreaming();

                // 4. CHĂNG LƯỚI NHẬN DỮ LIỆU TỪ CLIENT NÀY BẤT ĐỒNG BỘ
                StateObject state = new StateObject { Connection = connection };
                clientSocket.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (ObjectDisposedException) { /* Bỏ qua khi Server tắt */ }
            catch (Exception ex)
            {
                AppendLog($"[{DateTime.Now:HH:mm:ss}] Error in Accept: {ex.Message}");
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

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (!isServerRunning) return;

            StateObject state = (StateObject)ar.AsyncState;
            Socket clientSocket = state.Connection.ClientSocket;

            try
            {
                int bytesRead = clientSocket.EndReceive(ar);

                if (bytesRead > 0)
                {
                    try
                    {
                        CommandPacket packet = CommandPacket.FromBytes(state.Buffer, bytesRead);
                        ExecuteCommand(packet, state.Connection);
                    }
                    catch
                    {
                        // Bắt lỗi ở đây để ứng dụng không bị văng nếu TCP dồn cục gây sai cấu trúc byte
                    }
                    finally
                    {
                        Array.Clear(state.Buffer, 0, state.Buffer.Length);
                        clientSocket.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                    }
                }
                else
                {
                    DisconnectClient(state.Connection);
                }
            }
            catch (SocketException)
            {
                DisconnectClient(state.Connection);
            }
            catch (ObjectDisposedException) { /* Socket đã đóng */ }
            catch (Exception ex)
            {
                AppendLog($"[{DateTime.Now:HH:mm:ss}] Error receiving: {ex.Message}");
            }
        }

        private void DisconnectClient(ClientConnection connection)
        {
            if (connection == null || !connectedClients.Contains(connection)) return;

            connection.Caster?.StopStreaming(); // Tắt gửi hình

            try { connection.ClientSocket?.Shutdown(SocketShutdown.Both); } catch { }
            connection.ClientSocket?.Close();

            connectedClients.Remove(connection);
            AppendLog($"[{DateTime.Now:HH:mm:ss}] Client disconnected: {connection.IP}:{connection.Port}");
            UpdateClientListUI();
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

    // Class dùng để chứa dữ liệu truyền đi giữa các hàm Callback của TCP
    public class StateObject
    {
        public ClientConnection Connection { get; set; }
        public const int BufferSize = 65536; // Giỏ chứa 64KB
        public byte[] Buffer = new byte[BufferSize];
    }
}