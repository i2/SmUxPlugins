using System;
using System.IO;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Helpers;

namespace MediaFiles
{
    public class RecordQuestionSound : RecordMediaFileBase
    {
        protected override void AttachRecordedFile(string fileName, string mediaExtension)
        {
            string itemMediaQuestion = string.Format("{0}q" + mediaExtension, SuperMemo.CurrentItemFileWithoutExtension.Replace("item", string.Empty));
            string destinationFile = Path.Combine(SuperMemo.CurrentCourseMediaPath, itemMediaQuestion);

            if (FileNotExistOrShouldOverwrite(destinationFile))
            {
                ToolStripButton button = SuperMemo.GetMediaQuestionButton();
                
                if (button.Checked)
                {
                    ReflectionHelper.InvokeMethod(button, "OnClick", EventArgs.Empty);
                }

                File.Copy(fileName, destinationFile);
                ReflectionHelper.InvokeMethod(button, "OnClick", EventArgs.Empty);

                StatusMessage.ShowInfo("Pod³aczono dŸwiêk pytania.");
            }
        }

        public override string Name
        {
            get
            {
                return @"N&agrywanie\Nagraj dŸwiêk &pytania...";
            }
        }

        public override bool Enabled
        {
            get
            {
                ToolStripButton button = SuperMemo.GetMediaQuestionButton();
                return button != null && button.Visible && button.Enabled;
            }
        }

        public override int Order
        {
            get { return 2100; }
        }

        public override Keys GetShortcut()
        {
            return Keys.Q | Keys.Control | Keys.Shift;
        }
    }
}