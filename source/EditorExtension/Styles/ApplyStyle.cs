using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Plugin;

namespace EditorExtension.Styles
{
    public class ApplyStyle : SuperMemoExtension
    {
        public override int Order
        {
            get { return 0; }
        }

        public override string Name
        {
            get
            {
                return @"&Style\Zastosuj styl...";
            }
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 0 && !string.IsNullOrEmpty(SuperMemo.GetSelectedText()); }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.S;
        }

        public override void Execute()
        {
            ManageStyles.ApplyStyle();
        }
    }
}