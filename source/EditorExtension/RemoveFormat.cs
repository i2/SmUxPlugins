using System.Drawing;
using System.Windows.Forms;
using PluginSystem.Plugin;
using PluginSystem.API;

namespace EditorExtension
{
    public class RemoveFormat : SuperMemoExtension
    {
        public override int Order
        {
            get { return 4; }
        }

        public override string Name
        {
            get
            {
                return @"&Formatowanie tekstu\Usuñ formatowanie";
            }
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 0; }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Alt | Keys.D0;
        }

        public override void Execute()
        {
            string text = SuperMemo.GetSelectedText();
            SuperMemo.ReplaceSelectedText(text);
        }
    }
}