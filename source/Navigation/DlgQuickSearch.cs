using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;

namespace Navigation
{
    public partial class DlgQuickSearch : BaseForm
    {
        private DateTime m_LastTextChangeTime = DateTime.Now;
        private string m_TextToSearch;

        public DlgQuickSearch()
        {
            InitializeComponent();
            listView1.View = View.Details;
            
            ColumnHeader column1 = listView1.Columns.Add("Nr");
            column1.Width = 60;
            ColumnHeader column2 = listView1.Columns.Add("Treść");
            column2.Width = listView1.ClientRectangle.Width - column1.Width - 5;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            m_LastTextChangeTime = DateTime.Now;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(DateTime.Now - m_LastTextChangeTime> TimeSpan.FromMilliseconds(300) && m_TextToSearch != textBox1.Text)
            {
                m_TextToSearch = textBox1.Text;

                listView1.BeginUpdate();
                listView1.Items.Clear();
                IList<SearchResult> results = DatabaseSearchCache.Find(m_TextToSearch);
                //if (results.Count > 0)
                //{
                //    SuperMemo.GoToPage(results[0].ItemNumber);
                //}

                foreach (var result in results)
                {
                    ListViewItem listViewItem = listView1.Items.Add(result.ItemNumber.ToString());
                    listViewItem.Tag = result.ItemNumber;
                    listViewItem.ImageIndex = 1;
                    listViewItem.SubItems.Add(result.Example);
                }

                listView1.EndUpdate();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && listView1.Items.Count > 0)
            {
                int i = 0;
                foreach (ListViewItem item in listView1.Items)
                {
                    item.Selected = i == 0;
                    i++;
                }
                listView1.Focus();
            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && (listView1.Items.Count == 0 || listView1.Items[0].Selected))
            {
                textBox1.Focus();
            }
        }

        private void DlgQuickSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    SuperMemo.GoToPage((int) listView1.SelectedItems[0].Tag);
                    if (e.Modifiers != Keys.Shift)
                    {
                        Close();
                    }
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}