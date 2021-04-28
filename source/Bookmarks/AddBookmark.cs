using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using mshtml;
using PluginSystem.API;
using PluginSystem.Helpers;
using PluginSystem.Plugin;

namespace Bookmarks
{
    public class AddBookmark : BookmarkBase, IPageRenderer
    {
        public override int Order
        {
            get { return 30; }
        }

        public override string Name
        {
            get { return @"&Zakładki\Dodaj zakładkę..."; }
        }

        public override Keys GetShortcut()
        {
            return Keys.F4;
        }

        public override void Execute()
        {
            string overridePath = Path.Combine(SuperMemo.CurrentCoursePath, "Override");
            if (!Directory.Exists(overridePath))
            {
                Directory.CreateDirectory(overridePath);
            }

            var b = LoadCurrentItemBookmarkDefinition();
            
            if (b == null)
            {
                IList<string> bookmarksIconNames = GetBookmarksIconNames();
                if (bookmarksIconNames != null && bookmarksIconNames.Count>0)
                {
                    b = new BookmarkDefinition(bookmarksIconNames[0], "Zakładka");
                }                
            }

            using (var bookmark = new DlgBookmark(b))
            {
                if (bookmark.ShowDialog() == DialogResult.OK)
                {
                    string fileName = GetBookmarkFileName();
                    SerializationHelper.SerializeToFile(fileName, bookmark.BookmarkDefinition);
                    m_LastBookmark = bookmark.BookmarkDefinition;
                    RefreshCurrentPage();
                }
            }
        }

        public void RenderPage()
        {
            bool alreadyAdded = SuperMemo.GetDocument().getElementById(BOOKMARK_ID) != null;

            if (!alreadyAdded) 
            {
                BookmarkDefinition bookmarkDefinition = LoadCurrentItemBookmarkDefinition();
                if (bookmarkDefinition != null)
                {
                    AddBookmarkToPage(bookmarkDefinition);
                }
            }
        }

        private static void AddBookmarkToPage(BookmarkDefinition bookmarkDefinition)
        {
            HTMLDocumentClass document = SuperMemo.GetDocument();
            var question = document.getElementById("question");
            var n = (IHTMLDOMNode)question.parentElement.parentElement;

            IHTMLElement img = document.createElement("img");
            img.setAttribute("src", bookmarkDefinition.ImageSource, 0);
            img.setAttribute("id", BOOKMARK_ID, 0);

            IHTMLElement bookmark = document.createElement("div");
            bookmark.setAttribute("title", bookmarkDefinition.BookmarkText, 0);

            ((IHTMLDOMNode) bookmark).appendChild((IHTMLDOMNode) img);
            n.appendChild((IHTMLDOMNode) bookmark);

            var a4 = (IHTMLDOMNode)SuperMemo.GetDocument().getElementById("area4");
            if (a4 != null)
            {
                a4.insertBefore((IHTMLDOMNode) bookmark, a4.firstChild);
            }
        }
    }
}
