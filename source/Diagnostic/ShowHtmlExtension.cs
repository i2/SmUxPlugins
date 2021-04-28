using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using mshtml;
using PluginSystem.Helpers;
using PluginSystem.Plugin;
using PluginSystem.API;

namespace Diagnostic
{
    public class ShowHtmlExtension : SuperMemoExtension
    {
        public override int Order
        {
            get { return 40; }
        }

        public override string Name
        {
            get
            {
                return @"Diagnostyka\Pokaż podgląd strony";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.F11;
        }

        public override void Execute()
        {
            //SuperMemo.CopyCurrentItemToClipboard();
            //SuperMemo.PasteItemFromClipboard();
            string s = SuperMemo.GetPageAsHtml();
            string temporaryFile = @"c:\Strona.html";
            StringHelper.StringToFile(s, temporaryFile, Encoding.UTF32);
            Process.Start(@"iexplore", temporaryFile);

            //HTMLDocumentClass document = SuperMemo.GetDocument();
            //HTMLBodyClass htmlBodyClass = (HTMLBodyClass)document.body;
            //IHTMLTxtRange textRange = htmlBodyClass.createTextRange();
            //textRange.findText("dz", 1, 0);

            //IHTMLSelectionObject selection = document.selection;
            //IHTMLTxtRange range = (IHTMLTxtRange)selection.createRange();

            //range.findText("dzi", 0, 0);
            ////range.select();
            //int i1 = range.moveStart("character", 3);
            //int i2 = range.moveEnd("character", 47);
        }
    }
}
