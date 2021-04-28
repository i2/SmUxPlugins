using System.IO;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;

namespace Bookmarks
{
    public class RemoveAllBookmarks : BookmarkBase
    {
        public override int Order
        {
            get { return 32; }
        }

        public override string Name
        {
            get { return @"&Zak�adki\Usu� wszystkie..."; }
        }

        public override void Execute()
        {
            if (MessageBox.Show("Czy usun�� wszystkie zak�adki z danego kursu?", "Pytanie", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string currentCoursePath = SuperMemo.CurrentCoursePath;

                if (currentCoursePath == null)
                {
                    return;
                }

                string overridePath = Path.Combine(currentCoursePath, "Override");

                if (!Directory.Exists(overridePath))
                {
                    return;
                }

                foreach (string fileName in GetAllBookmarks())
                {
                    File.Delete(fileName);
                }

                m_LastBookmark = null;
                RefreshCurrentPage();
                StatusMessage.Show(MessageType.Info, "Usuni�to wszystkie zak�adki");
            }
        }
    }
}