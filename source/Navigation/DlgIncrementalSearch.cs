using System;
using System.Collections.Generic;
using System.Media;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;

namespace Navigation
{
    public partial class DlgIncrementalSearch : BaseForm
    {
        private DateTime m_LastTextChangeTime = DateTime.Now;
        private string m_TextToSearch;
        private int m_PositionInSearch = 0;
        private IList<SearchResult> m_Results;

        public DlgIncrementalSearch()
        {
            InitializeComponent();
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
                m_Results = DatabaseSearchCache.Find(m_TextToSearch);
                m_PositionInSearch = 0;
                ShowSearchResult();
            }
        }

        private void ShowSearchResult()
        {
            if (m_PositionInSearch >= 0 & m_PositionInSearch < m_Results.Count)
            {
                SuperMemo.GoToPage(m_Results[m_PositionInSearch].ItemNumber);
                string count = m_Results.Count != DatabaseSearchCache.MAX_SEARCH_RESULT_COUNT ? m_Results.Count.ToString() : "∞";
                lblResults.Text = string.Format("{0} z {1}", m_PositionInSearch + 1, count);
            }
            else
            {
                lblResults.Text = "Brak";
                SystemSounds.Beep.Play();
            }
        }

        private void DlgQuickSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Close();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if (e.KeyCode == Keys.Left && e.Modifiers == Keys.Alt)
            {
                btnPrevious_Click(this, EventArgs.Empty);
            }
            else if (e.KeyCode == Keys.Right && e.Modifiers == Keys.Alt)
            {
                btnNext_Click(this, EventArgs.Empty);
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            ShowOtherSearchResult(-1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ShowOtherSearchResult(1);
        }

        private void ShowOtherSearchResult(int delta)
        {
            int oldPosition = m_PositionInSearch;
            if (m_Results.Count != 0)
            {
                m_PositionInSearch = (m_PositionInSearch + delta + m_Results.Count) % m_Results.Count;

                if (m_PositionInSearch != oldPosition)
                {
                    ShowSearchResult();
                }
                else
                {
                    SystemSounds.Beep.Play();
                }
            }
        }
    }
}