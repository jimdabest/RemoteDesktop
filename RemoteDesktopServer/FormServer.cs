using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace RemoteDesktopServer
{
    public partial class FormServer : Form
    {
        private Socket serverSocket;
        private bool isServerRunning = false;
        private Thread listenerThread;
        private List<ClientConnection> connectedClients = new List<ClientConnection>();

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

                    connection.Caster = new ScreenCaster(clientIP, 5001);
                    connection.Caster.StartStreaming();
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] Started ScreenCaster (UDP: 5001) for {clientIP}");

                    // Start a thread to handle this client
                    Thread clientThread = new Thread(() => HandleClient(connection)) { IsBackground = true };
                    clientThread.Start();
                }
                catch (SocketException)
                {
                    // Server is stopping
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
                byte[] buffer = new byte[1024];

                while (isServerRunning && connection.ClientSocket.Connected)
                {
                    // Nhận dữ liệu trực tiếp từ Socket
                    int bytesRead = connection.ClientSocket.Receive(buffer);
                    if (bytesRead == 0)
                    {
                        break; // Client ngắt kết nối
                    }

                    // Xử lý gói tin nhận được (ví dụ: CommandPacket) ở đây
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] Data received from {connection.IP}: {bytesRead} bytes");
                }
            }
            catch (SocketException)
            {
                // Bỏ qua lỗi ngắt kết nối đột ngột
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
            // Keep log size manageable - remove oldest lines if exceeds 10000
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
