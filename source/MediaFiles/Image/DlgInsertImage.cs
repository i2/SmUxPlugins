using System;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using PluginSystem.Helpers;

namespace MediaFiles.Image
{
    public partial class DlgInsertImage : Form
    {
        private int m_StartWith;

        private System.Drawing.Image SelectedImage
        {
            get; set;
        }

        public DlgInsertImage()
        {
            InitializeComponent();
        }

        private void BtnGetImagesClick(object sender, EventArgs e)
        {
            m_StartWith = 0;

            DownloadImages();
        }

        private void DownloadImages()
        {
            var list = flowLayoutPanel1.Controls.Cast<Control>().ToList();

            foreach (var control in list)
            {
                control.Dispose();
            }

            var url = string.Format("http://images.google.com/images?source=hp&start={1}&q={0}", txtSearchingWord.Text, m_StartWith);
            
            foreach (string image in ImageDownloadHelper.ParSeImageUrls(url))
            {
                if (!image.StartsWith("http://images.google.com"))
                {
                    var pictureBox = new PictureBox();
                    flowLayoutPanel1.Controls.Add(pictureBox);
                    pictureBox.AutoSize = true;
                    pictureBox.Tag = image;
                    pictureBox.DoubleClick += PictureBoxOnDoubleClick;
                    ThreadPool.QueueUserWorkItem(CallBack, pictureBox);
                }            
            }

            Cursor = Cursors.Default;
        }

        private void PictureBoxOnDoubleClick(object sender, EventArgs eventArgs)
        {
            SelectedImage = ((PictureBox) sender).Image;
            Close();
        }

        private void CallBack(object state)
        {
            var pictureBox = (PictureBox)state;
            var url = (string)pictureBox.Tag;
            var imageFromUrl = WebHelper.ImageFromUrl(url);
            Invoke((Action<PictureBox, System.Drawing.Image>)SetImage,pictureBox, imageFromUrl);
        }

        private void SetImage(PictureBox pictureBox,System.Drawing.Image obj)
        {
            if (!IsDisposed && !pictureBox.IsDisposed)
            {
                pictureBox.Image = obj;
            }
        }

        private void Button1Click(object sender, EventArgs e)
        {
            m_StartWith -= flowLayoutPanel1.Controls.Count;
            DownloadImages();
        }

        private void Button2Click(object sender, EventArgs e)
        {
            m_StartWith += flowLayoutPanel1.Controls.Count;
            DownloadImages();
        }

        public static System.Drawing.Image GetImage()
        {
            using (var dlgInsertImage = new DlgInsertImage())
            {
                dlgInsertImage.ShowDialog();
                return dlgInsertImage.SelectedImage;
            }
        }
    }
}