using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;

namespace Bookmarks
{
    public class GoToPreviousBookmark : BookmarkBase
    {
        public override int Order
        {
            get { return 33; }
        }

        public override string Name
        {
            get { return @"&Zak≥adki\Idü do poprzedniej"; }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Alt | Keys.Left;
        }

        public override void Execute()
        {
            int bookmarkPosition;
            int number = TranslateRelativeBookmarkNumberToItemNumber(-1, out bookmarkPosition);
            int bookmarksCount = GetAllBookmarks().Length;
            if (number != -1)
            {
                SuperMemo.GoToPage(number);
                BookmarkDefinition bookmarkDefinition = LoadCurrentItemBookmarkDefinition();
                if (bookmarkDefinition != null)
                {
                    StatusMessage.Show(MessageType.Info, string.Format("Zak≥adka nr {0} z {1} [{2}]", (bookmarkPosition + 1), bookmarksCount, bookmarkDefinition.BookmarkText));
                }
            }
            else
            {
                StatusMessage.Show(MessageType.Warning, "Brak zak≥adek w danym kursie");
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