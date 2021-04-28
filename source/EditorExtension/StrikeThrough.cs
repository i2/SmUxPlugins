using System.Drawing;
using System.Windows.Forms;
using PluginSystem.Plugin;
using PluginSystem.API;

namespace EditorExtension
{
    public class StrikeThrough : SuperMemoExtension
    {
        public override int Order
        {
            get { return 1; }
        }

        public override string Name
        {
            get
            {
                return @"&Formatowanie tekstu\&Przekreœl";
            }
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 0; }
        }
        
        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Shift | Keys.S;
        }

        public override void Execute()
        {
            SuperMemo.ExecCommand("strikethrough", false, null);
        }
    }
}