using System;
using System.Windows.Forms;

namespace RemoteDesktopClient
{
    public partial class FormLauncher : Form
    {
        public FormLauncher()
        {
            InitializeComponent();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            this.Hide(); // Ẩn Menu
            FormServer frmServer = new FormServer();
            frmServer.FormClosed += (s, args) => this.Show(); // Bật lại Menu khi tắt Form
            frmServer.Show();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            this.Hide(); // Ẩn Menu
            FormClient frmClient = new FormClient();
            frmClient.FormClosed += (s, args) => this.Show(); // Bật lại Menu khi tắt Form
            frmClient.Show();
        }
    }
}