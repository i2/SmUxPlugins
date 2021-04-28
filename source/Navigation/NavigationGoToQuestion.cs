using System;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Plugin;

namespace Navigation
{
    public class NavigationGoToQuestion : SuperMemoExtension
    {
        public override int Order
        {
            get { return 21; }
        }

        public override string Name
        {
            get
            {
                return @"&Nawigacja\Przejdü do pola pytania";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Alt | Keys.PageUp;
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 1 | SuperMemo.VisualizerMode == 0; }
        }

        public override void Execute()
        {
            if (SuperMemo.VisualizerMode == 0)
            {
                SuperMemo.QuitEditing();
            }

            SuperMemo.InvokeJavaScript(@"window.external.AreaEdit(""question"");");
        }
    }
}