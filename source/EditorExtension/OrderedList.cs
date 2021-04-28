using System.Drawing;
using System.Windows.Forms;
using PluginSystem.Plugin;
using PluginSystem.API;

namespace EditorExtension
{
    public class OrderedList : SuperMemoExtension
    {
        public override int Order
        {
            get { return 11; }
        }

        public override string Name
        {
            get
            {
                return @"&Paragraf\Wstaw wyliczanie";
            }
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 0; }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Shift | Keys.G;
        }

        public override void Execute()
        {
            SuperMemo.ExecCommand("insertorderedlist", false, null);
        }
    }
}