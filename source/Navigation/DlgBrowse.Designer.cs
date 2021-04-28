using System.Data;

namespace Navigation
{
    partial class DlgBrowse
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.itemsGrid = new System.Windows.Forms.DataGridView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itemsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.itemsGrid);
            this.splitContainer1.Size = new System.Drawing.Size(910, 488);
            this.splitContainer1.SplitterDistance = 340;
            this.splitContainer1.TabIndex = 2;
            // 
            // itemsGrid
            // 
            this.itemsGrid.AllowUserToAddRows = false;
            this.itemsGrid.AllowUserToDeleteRows = false;
            this.itemsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.itemsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.itemsGrid.DataMember = "items";
            this.itemsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemsGrid.Location = new System.Drawing.Point(0, 0);
            this.itemsGrid.Name = "itemsGrid";
            this.itemsGrid.Size = new System.Drawing.Size(340, 488);
            this.itemsGrid.TabIndex = 1;
            this.itemsGrid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.itemsGrid_RowEnter_1);
            // 
            // DlgBrowse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 488);
            this.Controls.Add(this.splitContainer1);
            this.KeyPreview = true;
            this.Name = "DlgBrowse";
            this.Text = "Podgląd jednostek";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DlgBrowse_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.itemsGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;

        private int GetCurrentItemId()
        {
            var dataRow = (DataRowView) itemsGrid.CurrentRow.DataBoundItem;
            return (int)dataRow["id"];
        }

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView itemsGrid;
    }
}