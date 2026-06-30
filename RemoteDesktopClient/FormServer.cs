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

namespace RemoteDesktopClient
{
    public partial class FormServer : Form
    {
        private Socket serverSocket;
        private bool isServerRunning = false;
        private Thread listenerThread;
        private List<ClientConnection> connectedClients = new List<ClientConnection>();
        private int _serverPIN;

        #region Windows Thao Tac Phan Cung API
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        const uint MOUSEEVENTF_MOVE = 0x0001;
        const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        const uint MOUSEEVENTF_WHEEL = 0x0800;
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

                string userTyped = txtServerPIN.Text.Trim();

                if (string.IsNullOrEmpty(userTyped))
                {
                    // TRƯỜNG HỢP 1: Người dùng để trống -> Tự sinh ngẫu nhiên 4 số
                    Random rnd = new Random();
                    _serverPIN = rnd.Next(1000, 9999);
                }
                else
                {
                    // TRƯỜNG HỢP 2: Người dùng có tự gõ -> Kiểm tra xem họ gõ chữ hay gõ số
                    if (int.TryParse(userTyped, out int customNumber))
                    {
                        _serverPIN = customNumber; // Lấy luôn số họ vừa gõ làm PIN
                    }
                    else
                    {
                        MessageBox.Show("Auto convert to random PIN", "Incorrect format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Random rnd = new Random();
                        _serverPIN = rnd.Next(1000, 9999);
                    }
                }

                // Cập nhật ngược lại con số vừa chốt vào ô TextBox cho Host nhìn thấy
                txtServerPIN.Text = _serverPIN.ToString();
                txtServerPIN.Enabled = false; // Khóa ô nhập lại! Không cho sửa PIN khi Server đang chạy

                this.Text = $"Remote Desktop Server [PIN: {_serverPIN}]";

                btnStart.Enabled = false;
                btnStop.Enabled = true;
                txtPort.Enabled = false;
                lblStatusValue.Text = "Online";
                lblStatusValue.ForeColor = Color.Green;

                string localIP = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString() ?? "127.0.0.1";
                AppendLog($"[{DateTime.Now:HH:mm:ss}] Server started on {localIP}:{port}");

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
                txtServerPIN.Enabled = true;

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
            if (packet.Type == CommandType.ChangeQuality)
            {
                if (connection.Caster != null) connection.Caster.ImageQuality = packet.X;
                return;
            }
            else if (packet.Type == CommandType.ChangeCompression)
            {
                if (connection.Caster != null)
                {
                    // Ép kiểu ngược lại từ % về float (vd: 75 -> 0.75f)
                    connection.Caster.ImageScale = packet.X / 100f;
                }
                return;
            }
            if (!connection.IsAuthenticated && packet.Type != CommandType.Login) return;

            int targetX = packet.X;
            int targetY = packet.Y;

            int serverWidth = Screen.PrimaryScreen.Bounds.Width;
            int serverHeight = Screen.PrimaryScreen.Bounds.Height;

            if (packet.ClientWidth > 0 && packet.ClientHeight > 0)
            {
                targetX = (int)Math.Round((double)packet.X * serverWidth / packet.ClientWidth);
                targetY = (int)Math.Round((double)packet.Y * serverHeight / packet.ClientHeight);
            }

            switch (packet.Type)
            {
                case CommandType.Login:
                    if (packet.X == _serverPIN)
                    {
                        connection.IsAuthenticated = true;
                        AppendLog($"[{DateTime.Now:HH:mm:ss}] Client {connection.IP} authentication successful!");

                        CommandPacket reply = new CommandPacket { Type = CommandType.LoginResult, X = 1 };
                        connection.ClientSocket.Send(reply.ToBytes());
                    }
                    else
                    {
                        AppendLog($"[{DateTime.Now:HH:mm:ss}] Client {connection.IP} received an incorrect PIN ({packet.X}). Reject!");
                        CommandPacket reply = new CommandPacket { Type = CommandType.LoginResult, X = 0 };
                        connection.ClientSocket.Send(reply.ToBytes());
                        DisconnectClient(connection);
                    }
                    break;
                case CommandType.RegisterUdpPort:
                    int clientUdpPort = packet.X; // Port UDP mà Client vừa mở gửi sang
                    connection.Caster = new ScreenCaster(connection.IP, clientUdpPort);

                    // Gán ngay các thông số cài đặt ban đầu cho Caster
                    connection.Caster.StartStreaming();
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] The ScreenCaster UDP stream has connected to the port {clientUdpPort}");
                    break;
                case CommandType.MouseMove:
                    // Quy đổi tọa độ pixel (targetX, targetY) sang hệ tọa độ tuyệt đối (0 -> 65535)
                    uint absX = (uint)((targetX * 65535) / serverWidth);
                    uint absY = (uint)((targetY * 65535) / serverHeight);

                    mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, absX, absY, 0, 0);
                    break;
                case CommandType.MouseWheel:
                    mouse_event(MOUSEEVENTF_WHEEL, 0, 0, (uint)packet.KeyCode, 0);
                    break;
                case CommandType.LeftMouseDown: mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0); break;
                case CommandType.LeftMouseUp: mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0); break;
                case CommandType.RightMouseDown: mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0); break;
                case CommandType.RightMouseUp: mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0); break;
                case CommandType.KeyDown: keybd_event((byte)packet.KeyCode, 0, 0, 0); break;
                case CommandType.KeyUp: keybd_event((byte)packet.KeyCode, 0, KEYEVENTF_KEYUP, 0); break;
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
        public bool IsAuthenticated { get; set; } = false;
    }

    // Class dùng để chứa dữ liệu truyền đi giữa các hàm Callback của TCP
    public class StateObject
    {
        public ClientConnection Connection { get; set; }
        public const int BufferSize = 65536; // Giỏ chứa 64KB
        public byte[] Buffer = new byte[BufferSize];
    }
}