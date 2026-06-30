using System;
using System.Windows.Forms;

namespace RemoteDesktop
{
    public partial class FormSettings : Form
    {
        public int Quality { get; private set; }
        public int CompressionLevel { get; private set; } // 0: Low, 1: Medium, 2: High
        public int InputMethod { get; private set; }      // 0: Full Control, 1: View Only
        public PictureBoxSizeMode DisplayMode { get; private set; }

        public FormSettings(int curQuality, int curCompression, int curInput, PictureBoxSizeMode curDisplay)
        {
            // Lệnh này cực kỳ quan trọng để vẽ giao diện từ file Designer
            InitializeComponent();

            // Gán dữ liệu truyền vào lên giao diện
            tbQuality.Value = curQuality;
            lblQualityValue.Text = $"{curQuality}%";

            cbCompression.SelectedIndex = curCompression;
            cbInputMethod.SelectedIndex = curInput;
            cbDisplayMode.SelectedIndex = GetDisplayIndex(curDisplay);
        }

        // Sự kiện khi kéo thanh cuộn Trackbar
        private void tbQuality_Scroll(object sender, EventArgs e)
        {
            lblQualityValue.Text = $"{tbQuality.Value}%";
        }

        // Sự kiện khi bấm nút Save
        private void BtnSave_Click(object sender, EventArgs e)
        {
            Quality = tbQuality.Value;
            CompressionLevel = cbCompression.SelectedIndex;
            InputMethod = cbInputMethod.SelectedIndex;

            switch (cbDisplayMode.SelectedIndex)
            {
                case 0: DisplayMode = PictureBoxSizeMode.StretchImage; break;
                case 1: DisplayMode = PictureBoxSizeMode.Zoom; break;
                case 2: DisplayMode = PictureBoxSizeMode.CenterImage; break;
                default: DisplayMode = PictureBoxSizeMode.Normal; break;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private int GetDisplayIndex(PictureBoxSizeMode mode)
        {
            if (mode == PictureBoxSizeMode.StretchImage) return 0;
            if (mode == PictureBoxSizeMode.Zoom) return 1;
            if (mode == PictureBoxSizeMode.CenterImage) return 2;
            return 3;
        }
    }
}