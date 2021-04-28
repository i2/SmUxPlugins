using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using PluginSystem.Common;

namespace Bookmarks
{
    public partial class DlgBookmark : BaseForm
    {
        public BookmarkDefinition BookmarkDefinition { get; set; }

        public DlgBookmark() : this (null)
        {
        }

        public DlgBookmark(BookmarkDefinition bookmarkDefinition)
        {
            BookmarkDefinition = bookmarkDefinition;

            InitializeComponent();

            if (BookmarkDefinition != null)
            {
                IList<string> bookmarksIconNames = BookmarkBase.GetBookmarksIconNames();
                foreach (var name in bookmarksIconNames)
                {
                    AddIcon(name);                    
                }
                
                txbBookmarkText.Text = BookmarkDefinition.BookmarkText;
            }
        }

        private void AddIcon(string fileName)
        {
            var iconControl = new Panel();
            Bitmap bitmap = new Bitmap(fileName);
            iconControl.BackgroundImage = bitmap;
            iconControl.BackgroundImageLayout = ImageLayout.Center;
            iconControl.Size = new Size(24, 24);
            flowLayoutPanel1.Controls.Add(iconControl);
            iconControl.BorderStyle = BorderStyle.FixedSingle;
            iconControl.Click += ButtonOnClick;
            iconControl.Tag = fileName;

            if (fileName == BookmarkDefinition.ImageSource)
            {
                iconControl.BorderStyle = BorderStyle.Fixed3D;
            }
        }

        private static void ButtonOnClick(object sender, EventArgs args)
        {
            var panel = sender as Panel;

            foreach (Panel p in panel.Parent.Controls)
            {
                p.BorderStyle = BorderStyle.FixedSingle;
            }
            
            panel.BorderStyle = BorderStyle.Fixed3D;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            BookmarkDefinition.BookmarkText = txbBookmarkText.Text;
            foreach (Panel p in flowLayoutPanel1.Controls)
            {
                if (p.BorderStyle == BorderStyle.Fixed3D)
                {
                    BookmarkDefinition.ImageSource = (string) p.Tag;
                }
            }
            
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
