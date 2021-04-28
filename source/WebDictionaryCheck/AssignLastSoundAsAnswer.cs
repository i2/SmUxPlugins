using System;
using System.IO;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Helpers;
using PluginSystem.Plugin;

namespace WebDictionaries
{
    public class AssignLastSoundAsAnswer : SuperMemoExtension
    {
        public override int Order
        {
            get { return 54; }
        }

        public override string Name
        {
            get
            {
                return @"&S³owniki\Pod³¹cz nagranie jako odpowiedŸ";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.F8 | Keys.Shift;
        }

        public override bool Enabled
        {
            get
            {
                ToolStripButton button = SuperMemo.GetMediaAnswerButton();

                return button != null && button.Visible && (!string.IsNullOrEmpty(DownloadPronunciation.LastDownloadedFile) || !string.IsNullOrEmpty(SuperMemo.GetSelectedText()));
            }
        }

        public override void Execute()
        {
            if (!string.IsNullOrEmpty(SuperMemo.GetSelectedText()))
            {
                DownloadPronunciation.DownloadPronunciationFile();    
            }
            
            string itemMediaAnser = SuperMemo.CurrentItemFileWithoutExtension.Replace("item", string.Empty) + "a.mp3";
            string destinationFile = Path.Combine(SuperMemo.CurrentCourseMediaPath, itemMediaAnser);
            if (!Directory.Exists(SuperMemo.CurrentCourseMediaPath))
            {
                Directory.CreateDirectory(SuperMemo.CurrentCourseMediaPath);
            }

            if (File.Exists(destinationFile))
            {
                if (
                    MessageBox.Show("Plik z nagraniem odpowiedzi ju¿ istnieje. Nadpisaæ? ", "Pytanie",
                                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Delete(destinationFile);
                }
                else
                {
                    return;
                }
            }

            ToolStripButton button = SuperMemo.GetMediaAnswerButton();
            if (button.Checked)
            {
                ReflectionHelper.InvokeMethod(button, "OnClick", EventArgs.Empty);
            }
 
            File.Copy(DownloadPronunciation.LastDownloadedFile, destinationFile);

            ReflectionHelper.InvokeMethod(button, "OnClick", EventArgs.Empty);

            StatusMessage.ShowInfo("Pod³aczono dŸwiêk odpowiedzi: " + itemMediaAnser);
        }
    }
}