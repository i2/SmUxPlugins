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
            get { return @"&Zak�adki\Usu� zak�adk�"; }
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
                StatusMessage.Show(MessageType.Info, "Usuni�to zak�adk�");
                RefreshCurrentPage();
            }
        }
    }
}