using System;
using System.IO;
using System.Windows.Forms;
using PluginSystem.Common;

namespace Bookmarks
{
    public class RemoveBookmark : BookmarkBase
    {
        public override int Order
        {
            get { return 31; }
        }

        public override string Name
        {
            get { return @"&Zak³adki\Usuñ zak³adkê"; }
        }

        public override Keys GetShortcut()
        {
            return Keys.Shift | Keys.F4;
        }

        public override void Execute()
        {
            string bookmarkFileName = GetBookmarkFileName();

            if (File.Exists(bookmarkFileName))
            {
                File.Delete(bookmarkFileName);
                m_LastBookmark = null;
                StatusMessage.Show(MessageType.Info, "Usuniêto zak³adkê");
                RefreshCurrentPage();
            }
        }
    }
}