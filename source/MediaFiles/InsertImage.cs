using System;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Xml;
using MediaFiles.Image;
using PluginSystem.API;
using PluginSystem.Plugin;
using System.IO;

namespace MediaFiles
{
    public class InsertImage : SuperMemoExtension
    {
        public override string Name
        {
            get
            {
                return @"Inne\Wstaw rysunek...";
            }
        }

        public override bool Enabled
        {
            get
            {
                return SuperMemo.VisualizerMode == 0;
            }
        }

        public override int Order
        {
            get { return 2100; }
        }

        public override Keys GetShortcut()
        {
            return Keys.None;
        }

        public override void Execute()
        {
            try
            {
                using (var image = DlgInsertImage.GetImage())
                {
                    var mediaFile = SuperMemo.GetFirstFreeMediaFileName(".jpg");
                    image.Save(Path.Combine(SuperMemo.CurrentCourseMediaPath, mediaFile), ImageFormat.Jpeg);

                    var mediaFileName = Path.GetFileNameWithoutExtension(mediaFile);
                    string fileLetter = mediaFileName.Substring(mediaFileName.Length - 1);
                    var componentXml = string.Format(@"<gfx file=""{0}"" />", fileLetter);
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(componentXml);

                    foreach (XmlNode childNode in xmlDocument.ChildNodes)
                    {
                        SuperMemo.InsertComponent(childNode);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Błąd wstawiania rysunku", ex.ToString());
            }
        }
    }
}