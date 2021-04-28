using System;
using System.Drawing;
using System.Windows.Forms;
using PluginSystem.Plugin;
using PluginSystem.API;

namespace EditorExtension
{
    public class JustificationChangeRight : SuperMemoExtension
    {
        public override int Order
        {
            get { return 13; }
        }

        public override bool Active
        {
            //there is problem with paragraph formatting, because SM is removing it after finishing edition
            get { return false; }
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 0; }
        }
        
        public override string Name
        {
            get
            {
                return @"&Paragraf\Wyrównaj do prawej";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.R;
        }

        public override void Execute()
        {
            SuperMemo.ExecCommand("justifyright", false, null);
        }
    }
}