using System;

namespace Bookmarks
{
    [Serializable]
    public class BookmarkDefinition
    {
        public string ImageSource
        {
            get; set;
        }

        public string BookmarkText
        {
            get; set;
        }

        public BookmarkDefinition()
        {
        }

        public BookmarkDefinition(string imageSource, string bookmarkText)
        {
            ImageSource = imageSource;
            BookmarkText = bookmarkText;
        }
    }
}