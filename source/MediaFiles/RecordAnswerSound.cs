using System;
using System.IO;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Helpers;

namespace MediaFiles
{
    public class RecordAnswerSound : RecordMediaFileBase
    {
        protected override void AttachRecordedFile(string fileName, string mediaExtension)
        {
            string itemMediaAnser = string.Format("{0}a" + mediaExtension, SuperMemo.CurrentItemFileWithoutExtension.Replace("item", string.Empty));
            string destinationFile = Path.Combine(SuperMemo.CurrentCourseMediaPath, itemMediaAnser);

            if (FileNotExistOrShouldOverwrite(destinationFile))
            {
                ToolStripButton button = SuperMemo.GetMediaAnswerButton();

                if (button.Checked)
                {
                    ReflectionHelper.InvokeMethod(button, "OnClick", EventArgs.Empty);
                }

                File.Copy(fileName, destinationFile);
                ReflectionHelper.InvokeMethod(button, "OnClick", EventArgs.Empty);

                StatusMessage.ShowInfo("Pod³aczono dŸwiêk odpowiedzi.");
            }
        }

        public override string Name
        {
            get
            {
                return @"N&agrywanie\Nagraj dŸwiêk &odpowiedzi...";
            }
        }

        public override bool Enabled
        {
            get
            {
                ToolStripButton button = SuperMemo.GetMediaAnswerButton();
                return button != null && button.Visible && button.Enabled;
            }
        }

        public override int Order
        {
            get { return 2100; }
        }

        public override Keys GetShortcut()
        {
            return Keys.A | Keys.Control | Keys.Shift;
        }
    }
}