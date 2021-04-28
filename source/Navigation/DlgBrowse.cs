using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using PluginSystem.API;
using PluginSystem.Common;

namespace Navigation
{
    public partial class DlgBrowse : BaseForm
    {
        private int m_LastPreviewedItem = -1;
        private DateTime m_LastRowChangeTime;

        public DlgBrowse(DataSet dataSet)
        {
            InitializeComponent();
            itemsGrid.DataSource = dataSet;
            itemsGrid.ReadOnly = true;
            Size = Screen.PrimaryScreen.Bounds.Size;
        }

        private void DlgBrowse_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (itemsGrid.CurrentRow != null)
                {
                    int currentItemId = GetCurrentItemId();
                    Close();
                    SuperMemo.GoToPage(currentItemId);
                }
            }

            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int currentItemId = GetCurrentItemId();

            if (currentItemId != m_LastPreviewedItem)
            {
                if ((DateTime.Now - m_LastRowChangeTime) > TimeSpan.FromMilliseconds(800))
                {
                    while (splitContainer1.Panel2.Controls.Count > 0)
                    {
                        Control c = splitContainer1.Panel2.Controls[0];
                        splitContainer1.Panel2.Controls.RemoveAt(0);
                        c.Dispose();
                    }

                    m_LastPreviewedItem = currentItemId;

                    Form form = SuperMemo.GetPreviewPage(currentItemId);

                    form.FormBorderStyle = FormBorderStyle.None;
                    form.Location = new Point(0, 0);
                    form.TopLevel = false;
                    form.Parent = splitContainer1.Panel2;
                    form.Size = splitContainer1.Panel2.ClientSize;
                    form.Visible = true;
                    form.Size = splitContainer1.Panel2.ClientSize;
                }
            }
        }

        private void itemsGrid_RowEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            m_LastRowChangeTime = DateTime.Now;
        }
    }
}
