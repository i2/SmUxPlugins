using System.Drawing;
using System.Windows.Forms;
using PluginSystem.Plugin;
using PluginSystem.API;

namespace EditorExtension
{
    public class JustificationChangeLeft : SuperMemoExtension
    {
        public override int Order
        {
            get { return 14; }
        }

        public override bool Active
        {
            //there is problem with paragraph formatting, because SM is removing it after finishing edition
            get { return false; }
        }

        public override string Name
        {
            get
            {
                return @"&Paragraf\Wyrównaj do lewej";
            }
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 0; }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.L;
        }

        public override void Execute()
        {
            SuperMemo.ExecCommand("justifyleft", false, null);
        }
    }
}