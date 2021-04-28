using System;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Plugin;

namespace EditorExtension
{
    public class InsertLink : SuperMemoExtension
    {
        public override int Order
        {
            get { return 4; }
        }

        public override string Name
        {
            get
            {
                return @"&Formatowanie tekstu\Wstaw odnoœnik...";
            }
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 0; }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Alt | Keys.H;
        }

        public override void Execute()
        {
            string link = string.Empty;
            
            if (DlgInputBox.GetText("Tworzenie odnoœnika", "Podaj adres odnosnika", ref link) && !string.IsNullOrEmpty(link))
            {
                string textToSurround = SuperMemo.GetSelectedText();
                
                if (string.IsNullOrEmpty(textToSurround))
                {
                    textToSurround = link;
                }
                
                string linkHtml = string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", link, textToSurround);
                SuperMemo.ReplaceSelectedTextWithHtml(linkHtml);                
            }
        }
    }
}