using System.Drawing;
using System.Windows.Forms;
using PluginSystem.Plugin;
using PluginSystem.API;

namespace EditorExtension
{
    public class UnorderedList : SuperMemoExtension
    {
        public override int Order
        {
            get { return 10; }
        }

        public override string Name
        {
            get
            {
                return @"&Paragraf\Wstaw listê";
            }
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 0; }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Shift | Keys.H;
        }

        public override void Execute()
        {
            SuperMemo.ExecCommand("insertunorderedlist", false, null);
        }
    }
}