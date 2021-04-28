using System;
using System.Windows.Forms;
using PluginSystem.Plugin;
using PluginSystem.API;

namespace Diagnostic
{
    public class InvokeJavaScript : SuperMemoExtension
    {
        public override int Order
        {
            get { return 41; }
        }

        private string m_PreviousScript = @"document.getElementById('question').style.fontSize = ""34px"";\r\nalert(""test"");";

        public override string Name
        {
            get
            {
                return @"Diagnostyka\Wywo³aj JavaScript...";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Shift | Keys.S;
        }

        public override void Execute()
        {
            using (var dlgInvokeJavaScript = new DlgInvokeJavaScript())
            {
                dlgInvokeJavaScript.ScriptBody = m_PreviousScript;
                
                if (dlgInvokeJavaScript.ShowDialog() == DialogResult.OK)
                {
                    m_PreviousScript = dlgInvokeJavaScript.ScriptBody;
                    SuperMemo.InvokeJavaScript(dlgInvokeJavaScript.ScriptBody);
                }
            }
        }
    }
}