namespace PluginSystem.Plugins
{
    partial class DlgPluginManagment
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
            this.lbxPluginsList = new System.Windows.Forms.ListBox();
            this.btnAddPlugin = new System.Windows.Forms.Button();
            this.btnRemovePlugin = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbxPluginsList
            // 
            this.lbxPluginsList.FormattingEnabled = true;
            this.lbxPluginsList.Location = new System.Drawing.Point(12, 12);
            this.lbxPluginsList.Name = "lbxPluginsList";
            this.lbxPluginsList.Size = new System.Drawing.Size(385, 277);
            this.lbxPluginsList.TabIndex = 0;
            // 
            // btnAddPlugin
            // 
            this.btnAddPlugin.Location = new System.Drawing.Point(403, 12);
            this.btnAddPlugin.Name = "btnAddPlugin";
            this.btnAddPlugin.Size = new System.Drawing.Size(75, 23);
            this.btnAddPlugin.TabIndex = 1;
            this.btnAddPlugin.Text = "&Dodaj...";
            this.btnAddPlugin.UseVisualStyleBackColor = true;
            this.btnAddPlugin.Click += new System.EventHandler(this.btnAddPlugin_Click);
            // 
            // btnRemovePlugin
            // 
            this.btnRemovePlugin.Location = new System.Drawing.Point(403, 41);
            this.btnRemovePlugin.Name = "btnRemovePlugin";
            this.btnRemovePlugin.Size = new System.Drawing.Size(75, 23);
            this.btnRemovePlugin.TabIndex = 2;
            this.btnRemovePlugin.Text = "&Usuń";
            this.btnRemovePlugin.UseVisualStyleBackColor = true;
            this.btnRemovePlugin.Click += new System.EventHandler(this.btnRemovePlugin_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(403, 70);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(403, 99);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Anuluj";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DlgPluginManagment
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(486, 303);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnRemovePlugin);
            this.Controls.Add(this.btnAddPlugin);
            this.Controls.Add(this.lbxPluginsList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgPluginManagment";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zarządzanie wtyczkami";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbxPluginsList;
        private System.Windows.Forms.Button btnAddPlugin;
        private System.Windows.Forms.Button btnRemovePlugin;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}