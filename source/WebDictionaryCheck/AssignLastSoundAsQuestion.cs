using System;
using System.IO;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Helpers;
using PluginSystem.Plugin;

namespace WebDictionaries
{
    public class AssignLastSoundAsQuestion : SuperMemoExtension
    {
        public override int Order
        {
            get { return 54; }
        }

        public override string Name
        {
            get
            {
                return @"&Słowniki\Podłącz nagranie jako pytanie";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.None;
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

            string itemMediaAnser = SuperMemo.CurrentItemFileWithoutExtension.Replace("item", string.Empty) + "q.mp3";
            string destinationFile = Path.Combine(SuperMemo.CurrentCourseMediaPath, itemMediaAnser);
            if (!Directory.Exists(SuperMemo.CurrentCourseMediaPath))
            {
                Directory.CreateDirectory(SuperMemo.CurrentCourseMediaPath);
            }

            if (File.Exists(destinationFile))
            {
                if (
                    MessageBox.Show("Plik z nagraniem pytania już istnieje. Nadpisać? ", "Pytanie",
                                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Delete(destinationFile);
                }
                else
                {
                    return;
                }
            }

            ToolStripButton button = SuperMemo.GetMediaQuestionButton();
            if (button.Checked)
            {
                ReflectionHelper.InvokeMethod(button, "OnClick", EventArgs.Empty);
            }

            File.Copy(DownloadPronunciation.LastDownloadedFile, destinationFile);

            ReflectionHelper.InvokeMethod(button, "OnClick", EventArgs.Empty);

            StatusMessage.ShowInfo("Podłaczono dźwięk pytania: " + itemMediaAnser);
        }
    }
}