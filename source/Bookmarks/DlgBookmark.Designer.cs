namespace Bookmarks
{
    partial class DlgBookmark
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.txbBookmarkText = new System.Windows.Forms.TextBox();
            this.gbxIcon = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblBookmarkText = new System.Windows.Forms.Label();
            this.gbxIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(319, 229);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Anuluj";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(238, 229);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txbBookmarkText
            // 
            this.txbBookmarkText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbBookmarkText.Location = new System.Drawing.Point(12, 180);
            this.txbBookmarkText.Name = "txbBookmarkText";
            this.txbBookmarkText.Size = new System.Drawing.Size(382, 20);
            this.txbBookmarkText.TabIndex = 2;
            // 
            // gbxIcon
            // 
            this.gbxIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxIcon.Controls.Add(this.flowLayoutPanel1);
            this.gbxIcon.Location = new System.Drawing.Point(12, 12);
            this.gbxIcon.Name = "gbxIcon";
            this.gbxIcon.Size = new System.Drawing.Size(382, 129);
            this.gbxIcon.TabIndex = 0;
            this.gbxIcon.TabStop = false;
            this.gbxIcon.Text = "Ikona zakładki";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 19);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(370, 104);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // lblBookmarkText
            // 
            this.lblBookmarkText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBookmarkText.AutoSize = true;
            this.lblBookmarkText.Location = new System.Drawing.Point(12, 164);
            this.lblBookmarkText.Name = "lblBookmarkText";
            this.lblBookmarkText.Size = new System.Drawing.Size(78, 13);
            this.lblBookmarkText.TabIndex = 1;
            this.lblBookmarkText.Text = "&Tekst zakładki";
            // 
            // DlgBookmark
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(406, 264);
            this.Controls.Add(this.lblBookmarkText);
            this.Controls.Add(this.gbxIcon);
            this.Controls.Add(this.txbBookmarkText);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Name = "DlgBookmark";
            this.Text = "Dodawanie zakładki";
            this.gbxIcon.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txbBookmarkText;
        private System.Windows.Forms.GroupBox gbxIcon;
        private System.Windows.Forms.Label lblBookmarkText;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}