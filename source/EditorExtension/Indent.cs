using System.Drawing;
using System.Windows.Forms;
using PluginSystem.Plugin;
using PluginSystem.API;

namespace EditorExtension
{
    public class Indent : SuperMemoExtension
    {
        public override int Order
        {
            get { return 17; }
        }

        public override string Name
        {
            get
            {
                return @"&Paragraf\Wstaw wciêcie";
            }
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 0; }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Shift | Keys.L;
        }

        public override void Execute()
        {
            SuperMemo.ExecCommand("indent", false, null);
        }
    }
}