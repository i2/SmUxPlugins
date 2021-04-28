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
            get { return @"&Zak³adki\Usuñ wszystkie..."; }
        }

        public override void Execute()
        {
            if (MessageBox.Show("Czy usun¹æ wszystkie zak³adki z danego kursu?", "Pytanie", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                StatusMessage.Show(MessageType.Info, "Usuniêto wszystkie zak³adki");
            }
        }
    }
}