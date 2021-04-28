namespace ItemsExchange.NodeAsText
{
    partial class DlgImportAsNode
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
            this.label1 = new System.Windows.Forms.Label();
            this.txbNodeAsTextFile = new System.Windows.Forms.TextBox();
            this.btnNodeAsTextFile = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnTranslationFile = new System.Windows.Forms.Button();
            this.txbTranslationFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Plik NodeAsText:";
            // 
            // txbNodeAsTextFile
            // 
            this.txbNodeAsTextFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbNodeAsTextFile.Location = new System.Drawing.Point(107, 6);
            this.txbNodeAsTextFile.Name = "txbNodeAsTextFile";
            this.txbNodeAsTextFile.Size = new System.Drawing.Size(173, 20);
            this.txbNodeAsTextFile.TabIndex = 1;
            // 
            // btnNodeAsTextFile
            // 
            this.btnNodeAsTextFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNodeAsTextFile.Location = new System.Drawing.Point(286, 6);
            this.btnNodeAsTextFile.Name = "btnNodeAsTextFile";
            this.btnNodeAsTextFile.Size = new System.Drawing.Size(22, 20);
            this.btnNodeAsTextFile.TabIndex = 2;
            this.btnNodeAsTextFile.Text = "...";
            this.btnNodeAsTextFile.UseVisualStyleBackColor = true;
            this.btnNodeAsTextFile.Click += new System.EventHandler(this.btnNodeAsTextFile_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(233, 76);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Anuluj";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(152, 76);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnTranslationFile
            // 
            this.btnTranslationFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTranslationFile.Location = new System.Drawing.Point(286, 32);
            this.btnTranslationFile.Name = "btnTranslationFile";
            this.btnTranslationFile.Size = new System.Drawing.Size(22, 20);
            this.btnTranslationFile.TabIndex = 7;
            this.btnTranslationFile.Text = "...";
            this.btnTranslationFile.UseVisualStyleBackColor = true;
            this.btnTranslationFile.Click += new System.EventHandler(this.btnTranslationFile_Click);
            // 
            // txbTranslationFile
            // 
            this.txbTranslationFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTranslationFile.Location = new System.Drawing.Point(107, 32);
            this.txbTranslationFile.Name = "txbTranslationFile";
            this.txbTranslationFile.Size = new System.Drawing.Size(173, 20);
            this.txbTranslationFile.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Plik tłumaczeń:";
            // 
            // DlgImportAsNode
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(320, 111);
            this.Controls.Add(this.btnTranslationFile);
            this.Controls.Add(this.txbTranslationFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNodeAsTextFile);
            this.Controls.Add(this.txbNodeAsTextFile);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DlgImportAsNode";
            this.Text = "Import bazy";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbNodeAsTextFile;
        private System.Windows.Forms.Button btnNodeAsTextFile;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnTranslationFile;
        private System.Windows.Forms.TextBox txbTranslationFile;
        private System.Windows.Forms.Label label2;
    }
}