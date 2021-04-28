namespace MediaFiles
{
    partial class DlgRecording
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
            this.components = new System.ComponentModel.Container();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbxDevice = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chbNomalize = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.chbConvertToMp3 = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(118, 127);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(199, 127);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Anuluj";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClik);
            // 
            // cbxDevice
            // 
            this.cbxDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDevice.FormattingEnabled = true;
            this.cbxDevice.Location = new System.Drawing.Point(82, 12);
            this.cbxDevice.Name = "cbxDevice";
            this.cbxDevice.Size = new System.Drawing.Size(192, 21);
            this.cbxDevice.TabIndex = 2;
            this.toolTip1.SetToolTip(this.cbxDevice, "Urządzenie, za pomocą którego nagrywany jest plik mediów");
            this.cbxDevice.SelectedIndexChanged += new System.EventHandler(this.CbxDeviceSelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Nagrywanie rozpoczęte.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Urządzenie:";
            // 
            // chbNomalize
            // 
            this.chbNomalize.AutoSize = true;
            this.chbNomalize.Location = new System.Drawing.Point(15, 39);
            this.chbNomalize.Name = "chbNomalize";
            this.chbNomalize.Size = new System.Drawing.Size(171, 17);
            this.chbNomalize.TabIndex = 5;
            this.chbNomalize.Text = "Normalizuj plik przed dodaniem";
            this.toolTip1.SetToolTip(this.chbNomalize, "Jeżeli zaznaczone, to w przypadku nagrania \r\nzawierającego fragmenty ciszy zostan" +
                    "ą one\r\nautomatycznie usunięte");
            this.chbNomalize.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.Timer1Tick);
            // 
            // chbConvertToMp3
            // 
            this.chbConvertToMp3.AutoSize = true;
            this.chbConvertToMp3.Location = new System.Drawing.Point(15, 62);
            this.chbConvertToMp3.Name = "chbConvertToMp3";
            this.chbConvertToMp3.Size = new System.Drawing.Size(150, 17);
            this.chbConvertToMp3.TabIndex = 6;
            this.chbConvertToMp3.Text = "Konwertuj do formatu Mp3";
            this.toolTip1.SetToolTip(this.chbConvertToMp3, "Jeżeli zaznaczone, przy zapisie plik mediów zostanie\r\nautomatycznie przekonwertow" +
                    "any do formatu Mp3");
            this.chbConvertToMp3.UseVisualStyleBackColor = true;
            // 
            // DlgRecording
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(286, 162);
            this.Controls.Add(this.chbConvertToMp3);
            this.Controls.Add(this.chbNomalize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbxDevice);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DlgRecording";
            this.Text = "Nagrywanie";
            this.Load += new System.EventHandler(this.DlgRecording_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbxDevice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chbNomalize;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox chbConvertToMp3;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}