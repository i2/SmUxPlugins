using System.Drawing;
using System.Windows.Forms;
using PluginSystem.Plugin;
using PluginSystem.API;

namespace EditorExtension
{
    public class Outdent : SuperMemoExtension
    {
        public override int Order
        {
            get { return 12; }
        }

        public override string Name
        {
            get
            {
                return @"&Paragraf\Usuñ wciêcie";
            }
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 0; }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Shift | Keys.K;
        }

        public override void Execute()
        {
            SuperMemo.ExecCommand("outdent", false, null);
        }
    }
}