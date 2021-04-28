using System.Drawing;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Plugin;

namespace Navigation
{
    public class NavigationGoToAnswer : SuperMemoExtension
    {
        public override int Order
        {
            get { return 22; }
        }

        public override string Name
        {
            get
            {
                return @"&Nawigacja\Przejd� do pola odpowiedzi";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Alt | Keys.PageDown;
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 1 | SuperMemo.VisualizerMode == 0;}
        }

        public override void Execute()
        {
            if (SuperMemo.VisualizerMode == 0)
            {
                SuperMemo.QuitEditing();
            }

            SuperMemo.InvokeJavaScript(@"window.external.AreaEdit(""answer"");");
        }
    }
}