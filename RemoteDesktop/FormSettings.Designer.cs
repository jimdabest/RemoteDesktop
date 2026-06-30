namespace RemoteDesktop
{
    partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblQuality = new System.Windows.Forms.Label();
            this.tbQuality = new System.Windows.Forms.TrackBar();
            this.lblQualityValue = new System.Windows.Forms.Label();
            this.lblComp = new System.Windows.Forms.Label();
            this.cbCompression = new System.Windows.Forms.ComboBox();
            this.lblInput = new System.Windows.Forms.Label();
            this.cbInputMethod = new System.Windows.Forms.ComboBox();
            this.lblDisplay = new System.Windows.Forms.Label();
            this.cbDisplayMode = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tbQuality)).BeginInit();
            this.SuspendLayout();
            // 
            // lblQuality
            // 
            this.lblQuality.AutoSize = true;
            this.lblQuality.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblQuality.Location = new System.Drawing.Point(20, 20);
            this.lblQuality.Name = "lblQuality";
            this.lblQuality.Size = new System.Drawing.Size(89, 15);
            this.lblQuality.TabIndex = 0;
            this.lblQuality.Text = "Image Quality:";
            // 
            // tbQuality
            // 
            this.tbQuality.Location = new System.Drawing.Point(20, 40);
            this.tbQuality.Maximum = 100;
            this.tbQuality.Minimum = 10;
            this.tbQuality.Name = "tbQuality";
            this.tbQuality.Size = new System.Drawing.Size(200, 45);
            this.tbQuality.TabIndex = 1;
            this.tbQuality.TickFrequency = 10;
            this.tbQuality.Value = 50;
            this.tbQuality.Scroll += new System.EventHandler(this.tbQuality_Scroll);
            // 
            // lblQualityValue
            // 
            this.lblQualityValue.AutoSize = true;
            this.lblQualityValue.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblQualityValue.Location = new System.Drawing.Point(230, 45);
            this.lblQualityValue.Name = "lblQualityValue";
            this.lblQualityValue.Size = new System.Drawing.Size(33, 15);
            this.lblQualityValue.TabIndex = 2;
            this.lblQualityValue.Text = "50%";
            // 
            // lblComp
            // 
            this.lblComp.AutoSize = true;
            this.lblComp.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblComp.Location = new System.Drawing.Point(20, 90);
            this.lblComp.Name = "lblComp";
            this.lblComp.Size = new System.Drawing.Size(123, 15);
            this.lblComp.TabIndex = 3;
            this.lblComp.Text = "Compression Level:";
            // 
            // cbCompression
            // 
            this.cbCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCompression.FormattingEnabled = true;
            this.cbCompression.Items.AddRange(new object[] {
            "Low (Best Quality, High Bandwidth)",
            "Medium (Balanced)",
            "High (Fastest, Low Bandwidth)"});
            this.cbCompression.Location = new System.Drawing.Point(20, 115);
            this.cbCompression.Name = "cbCompression";
            this.cbCompression.Size = new System.Drawing.Size(280, 23);
            this.cbCompression.TabIndex = 4;
            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblInput.Location = new System.Drawing.Point(20, 160);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(86, 15);
            this.lblInput.TabIndex = 5;
            this.lblInput.Text = "Input Method:";
            // 
            // cbInputMethod
            // 
            this.cbInputMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInputMethod.FormattingEnabled = true;
            this.cbInputMethod.Items.AddRange(new object[] {
            "Full Control (Mouse & Keyboard)",
            "View Only"});
            this.cbInputMethod.Location = new System.Drawing.Point(20, 185);
            this.cbInputMethod.Name = "cbInputMethod";
            this.cbInputMethod.Size = new System.Drawing.Size(280, 23);
            this.cbInputMethod.TabIndex = 6;
            // 
            // lblDisplay
            // 
            this.lblDisplay.AutoSize = true;
            this.lblDisplay.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblDisplay.Location = new System.Drawing.Point(20, 230);
            this.lblDisplay.Name = "lblDisplay";
            this.lblDisplay.Size = new System.Drawing.Size(104, 15);
            this.lblDisplay.TabIndex = 7;
            this.lblDisplay.Text = "Display Settings:";
            // 
            // cbDisplayMode
            // 
            this.cbDisplayMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDisplayMode.FormattingEnabled = true;
            this.cbDisplayMode.Items.AddRange(new object[] {
            "Stretch to Fit",
            "Zoom (Keep Aspect Ratio)",
            "Center Image",
            "Normal (Top Left)"});
            this.cbDisplayMode.Location = new System.Drawing.Point(20, 255);
            this.cbDisplayMode.Name = "cbDisplayMode";
            this.cbDisplayMode.Size = new System.Drawing.Size(280, 23);
            this.cbDisplayMode.TabIndex = 8;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(120, 300);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save Settings";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 341);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cbDisplayMode);
            this.Controls.Add(this.lblDisplay);
            this.Controls.Add(this.cbInputMethod);
            this.Controls.Add(this.lblInput);
            this.Controls.Add(this.cbCompression);
            this.Controls.Add(this.lblComp);
            this.Controls.Add(this.lblQualityValue);
            this.Controls.Add(this.tbQuality);
            this.Controls.Add(this.lblQuality);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.tbQuality)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblQuality;
        private System.Windows.Forms.TrackBar tbQuality;
        private System.Windows.Forms.Label lblQualityValue;
        private System.Windows.Forms.Label lblComp;
        private System.Windows.Forms.ComboBox cbCompression;
        private System.Windows.Forms.Label lblInput;
        private System.Windows.Forms.ComboBox cbInputMethod;
        private System.Windows.Forms.Label lblDisplay;
        private System.Windows.Forms.ComboBox cbDisplayMode;
        private System.Windows.Forms.Button btnSave;
    }
}