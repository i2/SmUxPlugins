using System.Windows.Forms;

namespace Diagnostic
{
    public partial class HtmlPreviewForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code
        
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txbHtmlPreview = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // txbHtmlPreview
            // 
            this.txbHtmlPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbHtmlPreview.Location = new System.Drawing.Point(12, 12);
            this.txbHtmlPreview.Multiline = true;
            this.txbHtmlPreview.Name = "txbHtmlPreview";
            this.txbHtmlPreview.Size = new System.Drawing.Size(260, 240);
            this.txbHtmlPreview.TabIndex = 0;
            this.txbHtmlPreview.TextChanged += new System.EventHandler(this.TxbHtmlPreviewTextChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // HtmlPreviewForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.txbHtmlPreview);
            this.KeyPreview = true;
            this.Name = "HtmlPreviewForm";
            this.Text = "Podgl¹d HTML";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HtmlPreviewForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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

        private TextBox txbHtmlPreview;
        private Timer timer1;
    }
}