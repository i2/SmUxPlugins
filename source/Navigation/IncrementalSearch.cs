using System.Drawing;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Plugin;

namespace Navigation
{
    public class IncrementalSearch : SearchBase
    {
        public override int Order
        {
            get { return 23; }
        }

        private Rectangle m_LastDialogPosition;

        public override string Name
        {
            get
            {
                return @"&Nawigacja\Wyszukiwanie inkrementalne...";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.I;
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 1 | SuperMemo.VisualizerMode == 0; }
        }

        public override void Execute()
        {
            if (SuperMemo.VisualizerMode == 0)
            {
                SuperMemo.QuitEditing();
            }

            PrepareSearchCache();
            
            using (var dlgIncrementalSearch = new DlgIncrementalSearch())
            {
                if (m_LastDialogPosition.Width > 0 && m_LastDialogPosition.Height > 0)
                {
                    dlgIncrementalSearch.StartPosition = FormStartPosition.Manual;
                    dlgIncrementalSearch.Bounds = m_LastDialogPosition;
                }
                else
                {
                    dlgIncrementalSearch.StartPosition = FormStartPosition.CenterScreen;
                }
                dlgIncrementalSearch.ShowDialog();
                m_LastDialogPosition =  dlgIncrementalSearch.Bounds;
            }
        }
    }
}