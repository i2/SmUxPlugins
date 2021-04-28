using System.IO;
using System.Windows.Forms;
using System.Xml;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Helpers;

namespace MediaFiles
{
    public class RecordSampleSound : RecordMediaFileBase
    {
        protected override void AttachRecordedFile(string fileName, string mediaExtension)
        {
            string mediaFileName = SuperMemo.GetFirstFreeMediaFileName(mediaExtension);
            
            if (string.IsNullOrEmpty(mediaFileName))
            {
                MessageBox.Show("Nie uda³o siê wyznaczyæ nazwy dla pliku. SprawdŸ, czy nie przekroczono limitu plików");
                return;
            }

            string destinationFile = Path.Combine(SuperMemo.CurrentCourseMediaPath, mediaFileName);

            if (FileNotExistOrShouldOverwrite(destinationFile))
            {
                string mediaFile = Path.GetFileNameWithoutExtension(destinationFile).Replace(SuperMemo.CurrentItemFileWithoutExtension.Replace("item", string.Empty), string.Empty);
                
                File.Copy(fileName, destinationFile);

                XmlNode node = XmlHelper.CreateNode("sfx");
                node.AddAttribute("file", mediaFile);
                SuperMemo.InsertComponent(node);

                string clipboardText = Clipboard.GetText();
                
                if ((Control.ModifierKeys & Keys.Shift) != 0 && !string.IsNullOrEmpty(clipboardText))
                {
                    SuperMemo.ReplaceSelectedText(clipboardText);
                    StatusMessage.ShowInfo("Pod³aczono dŸwiêk przyk³adu i wklejono tekst.");
                }
                else
                {
                    StatusMessage.ShowInfo("Pod³aczono dŸwiêk przyk³adu.");                    
                }
            }
        }

        public override string Name
        {
            get
            {
                return @"N&agrywanie\Nagraj dŸwiêk p&rzyk³adu...";
            }
        }

        public override bool Enabled
        {
            get
            {
                ToolStripButton button = SuperMemo.GetInsertMediaButton();
                return button != null && button.Visible && button.Enabled;
            }
        }

        public override int Order
        {
            get { return 2100; }
        }

        public override Keys GetShortcut()
        {
            return Keys.E | Keys.Control | Keys.Shift;
        }
    }
}