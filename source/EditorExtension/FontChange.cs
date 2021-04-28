using System;
using System.Drawing;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Plugin;

namespace EditorExtension
{
    public class FontChange : SuperMemoExtension
    {
        public override int Order
        {
            get { return 6; }
        }

        public override string Name
        {
            get
            {
                return @"&Formatowanie tekstu\Ustaw &czcionkê...";
            }
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 0; }
        }

        public override bool Active
        {
            get { return false; }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Alt | Keys.F;
        }

        public override void Execute()
        {
            using (var fontDialog = new FontDialog())
            {
                fontDialog.ShowEffects = false;
                fontDialog.AllowScriptChange = false;
                fontDialog.AllowSimulations = false;
                fontDialog.AllowVectorFonts = false;
                fontDialog.AllowVerticalFonts = false;

                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedHtml = SuperMemo.GetSelectedHtml();
                    string newHtml = "<span style=\"font-name: \"" + fontDialog.Font.Name + " +\">" + selectedHtml + "</span>";
                    
                    SuperMemo.ReplaceSelectedTextWithHtml(newHtml);
                }
            }
        }
    }
}