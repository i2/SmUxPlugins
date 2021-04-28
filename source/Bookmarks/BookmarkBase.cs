using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Helpers;
using PluginSystem.Plugin;

namespace Bookmarks
{
    public abstract class BookmarkBase : SuperMemoExtension
    {
        protected const string BOOKMARK_ID = "bookmarkExtension";
        protected static string m_LastBookmarkFileName;
        protected static BookmarkDefinition m_LastBookmark;

        static BookmarkBase()
        {
            IDictionary<string, Stream> bookmarksIcons = GetBookmarkIcons();
            foreach (KeyValuePair<string, Stream> bookmarksIcon in bookmarksIcons)
            {
                StreamHelper.WriteStreamToFile(bookmarksIcon.Value, bookmarksIcon.Key);                
            }
        }

        internal static IDictionary<string, Stream> GetBookmarkIcons()
        {
            IDictionary<string, Stream> bookmarksIcons = new Dictionary<string, Stream>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (var name in GetBookmarksIconNames())
            {
                bookmarksIcons.Add(name, assembly.GetManifestResourceStream(Path.GetFileName(name)));
            }
            return bookmarksIcons;
        }

        internal static IList<string> GetBookmarksIconNames()
        {
            IList<string> result = new List<string>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (var s in assembly.GetManifestResourceNames())
            {
                if (s.EndsWith(".png"))
                {
                    string fileName = Path.Combine(Path.GetTempPath(), Path.GetFileName(s));
                    result.Add(fileName);
                }
            }
            return result;
        }

        public override bool Enabled
        {
            get { return CurrentState.IsCourseLoaded; }
        }

        public static string GetBookmarkFileName()
        {
            string currentCoursePath = SuperMemo.CurrentCoursePath;
            string currentItemFileWithoutExtension = SuperMemo.CurrentItemFileWithoutExtension;

            if (currentCoursePath == null || currentItemFileWithoutExtension == null)
            {
                return null;
            }

            string overridePath = Path.Combine(currentCoursePath, "Override");
            string fullFileName = Path.Combine(overridePath, currentItemFileWithoutExtension) + "bookmark.xml";

            return fullFileName;
        }

        protected static BookmarkDefinition LoadCurrentItemBookmarkDefinition()
        {
            if (SuperMemo.LearnManagerMode == 0)
            {
                return null;
            }

            string fileName = GetBookmarkFileName();

            if (m_LastBookmarkFileName == fileName)
            {
                return m_LastBookmark;
            }

            m_LastBookmark = File.Exists(fileName) ? SerializationHelper.DeserializeFromFile<BookmarkDefinition>(fileName) : null;
            m_LastBookmarkFileName = fileName;

            return m_LastBookmark;
        }

        protected static void RefreshCurrentPage()
        {
            SuperMemo.GetWebBrowser().Refresh2();
        }

        protected static string[] GetAllBookmarks()
        {
            string currentCoursePath = SuperMemo.CurrentCoursePath;

            if (currentCoursePath == null)
            {
                return null;
            }

            string overridePath = Path.Combine(currentCoursePath, "Override");

            if (!Directory.Exists(overridePath))
            {
                return null;
            }
            
            return Directory.GetFiles(overridePath, "*bookmark.xml");
        }

        protected static int TranslateRelativeBookmarkNumberToItemNumber(int relativeBookmarkNumber, out int bookmarkPosition)
        {
            string[] bookmarks = GetAllBookmarks();
            if (bookmarks.Length == 0)
            {
                bookmarkPosition = -1;
                return -1;
            }
            Array.Sort(bookmarks);

            bookmarkPosition = Array.IndexOf(bookmarks, GetBookmarkFileName());
            bookmarkPosition = ((bookmarkPosition + relativeBookmarkNumber) + bookmarks.Length) % bookmarks.Length;

            string fileName = Path.GetFileNameWithoutExtension(bookmarks[bookmarkPosition]);
            string numberAsString = fileName.Replace("item", String.Empty).Replace("bookmark", String.Empty);
            return Int32.Parse(numberAsString);
        }
    }
}