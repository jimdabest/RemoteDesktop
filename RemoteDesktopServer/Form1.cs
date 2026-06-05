using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace RemoteDesktopServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Populate the server IP display
            txtIp.Text = GetLocalIPAddress();
        }

        private string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip))
                        return ip.ToString();
                }
                return IPAddress.Loopback.ToString();
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            Log($"Server started on {txtIp.Text}:{txtPort.Text}");
            // TODO: start listening for clients on txtIp.Text and txtPort.Text
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            Log("Server stopped");
            // TODO: stop listening
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItem != null)
            {
                // TODO: disconnect selected client
                Log("Disconnected client: " + lstClients.SelectedItem.ToString());
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // TODO: clean up network resources
        }

        private void Log(string text)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {text}{Environment.NewLine}");
        }
    }
}
