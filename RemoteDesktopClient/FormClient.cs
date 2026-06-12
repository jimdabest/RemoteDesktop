using System.Diagnostics;
using System.Net.Sockets;

namespace RemoteDesktopClient;

public partial class FormClient : Form
{
    private TcpClient? tcpClient;
    private NetworkStream? networkStream;
    private bool isConnected = false;
    private Thread? receiveThread;
    private Stopwatch latencyStopwatch = new();

    public FormClient()
    {
        InitializeComponent();
    }

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
            tcpClient = new TcpClient();
            latencyStopwatch.Restart();
            tcpClient.BeginConnect(serverIP, port, ConnectCallback, tcpClient);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to connect: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ConnectCallback(IAsyncResult result)
    {
        try
        {
            tcpClient?.EndConnect(result);

            if (tcpClient?.Connected ?? false)
            {
                latencyStopwatch.Stop();
                isConnected = true;
                networkStream = tcpClient.GetStream();

                UpdateUI(() =>
                {
                    btnConnect.Enabled = false;
                    btnDisconnect.Enabled = true;
                    txtServerIP.Enabled = false;
                    txtPort.Enabled = false;
                    txtPassword.Enabled = false;
                    lblConnectionStatusValue.Text = "Connected";
                    lblConnectionStatusValue.ForeColor = Color.Green;
                    lblServerInfoValue.Text = $"{txtServerIP.Text}:{txtPort.Text}";
                    lblLatency.Text = $"Latency: {latencyStopwatch.ElapsedMilliseconds} ms";
                });

                // Start receiving data from server
                receiveThread = new Thread(ReceiveData) { IsBackground = true };
                receiveThread.Start();
            }
            else
            {
                UpdateUI(() =>
                {
                    MessageBox.Show("Failed to connect to server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
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
            while (isConnected && networkStream != null)
            {
                int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    break;
                }

                // Handle received screenshot data here
                // This would decode and display the image
                ProcessReceivedData(buffer, bytesRead);
            }
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

    private void btnDisconnect_Click(object sender, EventArgs e)
    {
        DisconnectFromServer();
    }

    private void DisconnectFromServer()
    {
        try
        {
            isConnected = false;
            networkStream?.Close();
            tcpClient?.Close();
            networkStream = null;
            tcpClient = null;

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

    private void UpdateUI(Action action)
    {
        if (InvokeRequired)
        {
            Invoke(action);
        }
        else
        {
            action();
        }
    }
}
