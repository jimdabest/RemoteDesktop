using System;
using System.Drawing;
using System.Windows.Forms;

namespace RemoteDesktopClient
{
    public class FormSettings : Form
    {
        public int Quality { get; private set; }
        public int CompressionLevel { get; private set; } // 0: Low, 1: Medium, 2: High
        public int InputMethod { get; private set; }      // 0: Full Control, 1: View Only
        public PictureBoxSizeMode DisplayMode { get; private set; }

        private TrackBar tbQuality;
        private ComboBox cbCompression;
        private ComboBox cbInputMethod;
        private ComboBox cbDisplayMode;
        private Label lblQualityValue;

        public FormSettings(int curQuality, int curCompression, int curInput, PictureBoxSizeMode curDisplay)
        {
            this.Text = "Settings";
            this.Size = new Size(350, 360);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // 1. Image Quality
            Label lblQuality = new Label { Text = "Image Quality:", Location = new Point(20, 20), AutoSize = true, Font = new Font("Arial", 9F, FontStyle.Bold) };
            tbQuality = new TrackBar { Minimum = 10, Maximum = 100, Value = curQuality, TickFrequency = 10, Location = new Point(20, 40), Width = 200 };
            lblQualityValue = new Label { Text = $"{curQuality}%", Location = new Point(230, 45), AutoSize = true, Font = new Font("Arial", 9F, FontStyle.Bold) };
            tbQuality.Scroll += (s, e) => lblQualityValue.Text = $"{tbQuality.Value}%";

            // 2. Compression Level
            Label lblComp = new Label { Text = "Compression Level:", Location = new Point(20, 90), AutoSize = true, Font = new Font("Arial", 9F, FontStyle.Bold) };
            cbCompression = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(20, 115), Width = 280 };
            cbCompression.Items.AddRange(new string[] { "Low (Best Quality, High Bandwidth)", "Medium (Balanced)", "High (Fastest, Low Bandwidth)" });
            cbCompression.SelectedIndex = curCompression;

            // 3. Input Method
            Label lblInput = new Label { Text = "Input Method:", Location = new Point(20, 160), AutoSize = true, Font = new Font("Arial", 9F, FontStyle.Bold) };
            cbInputMethod = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(20, 185), Width = 280 };
            cbInputMethod.Items.AddRange(new string[] { "Full Control (Mouse & Keyboard)", "View Only" });
            cbInputMethod.SelectedIndex = curInput;

            // 4. Display Settings
            Label lblDisplay = new Label { Text = "Display Settings:", Location = new Point(20, 230), AutoSize = true, Font = new Font("Arial", 9F, FontStyle.Bold) };
            cbDisplayMode = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(20, 255), Width = 280 };
            cbDisplayMode.Items.AddRange(new string[] { "Stretch to Fit", "Zoom (Keep Aspect Ratio)", "Center Image", "Normal (Top Left)" });
            cbDisplayMode.SelectedIndex = GetDisplayIndex(curDisplay);

            // Save Button
            Button btnSave = new Button { Text = "Save Settings", Location = new Point(120, 300), Size = new Size(100, 30), BackColor = Color.DodgerBlue, ForeColor = Color.White };
            btnSave.Click += BtnSave_Click;

            this.Controls.AddRange(new Control[] { lblQuality, tbQuality, lblQualityValue, lblComp, cbCompression, lblInput, cbInputMethod, lblDisplay, cbDisplayMode, btnSave });
        }

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