using System.Collections.Generic;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;

namespace Bookmarks
{
    public class GoToNextBookmark : BookmarkBase
    {
        public override int Order
        {
            get { return 34; }
        }

        public override string Name
        {
            get { return @"&Zak³adki\IdŸ do nastêpnej"; }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Alt | Keys.Right;
        }

        public override void Execute()
        {
            int bookmarkPosition;
            int number = TranslateRelativeBookmarkNumberToItemNumber(1, out bookmarkPosition);
            int bookmarksCount = GetAllBookmarks().Length;
            if (number != -1)
            {
                SuperMemo.GoToPage(number);
                BookmarkDefinition bookmarkDefinition = LoadCurrentItemBookmarkDefinition();
                if (bookmarkDefinition != null)
                {
                    StatusMessage.Show(MessageType.Info, string.Format("Zak³adka nr {0} z {1} [{2}]", (bookmarkPosition + 1), bookmarksCount, bookmarkDefinition.BookmarkText));
                }
            }
            else
            {
                StatusMessage.Show(MessageType.Warning, "Brak zak³adek w danym kursie");
            }
        }

        public override bool Enabled
        {
            get
            {
                return base.Enabled && SuperMemo.VisualizerMode != 2;
            }
        }
    }
}