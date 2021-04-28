using System.Threading;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Plugin;

namespace Diagnostic
{
    public class PlayMacro : SuperMemoExtension
    {
        public override bool Active
        {
            get { return false; }
        }

        private string m_PreviousScript = @"{Esc}^{Tab}^{Tab}{Right}{Right}";

        public override int Order
        {
            get { return 27; }
        }

        public override string Name
        {
            get
            {
                return @"&Makra\Uruchom makro...";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.F12;
        }

        public override bool Enabled
        {
            get { return true; }
        }

        public override void Execute()
        {
            //int i = SuperMemo.InsertSubItem();
            //MessageBox.Show(i.ToString());

            //int i = SuperMemo.InsertNewItem();
            //MessageBox.Show(i.ToString());

            ThreadPool.QueueUserWorkItem(OnCallback);
            //using (var dlgInvokeJavaScript = new DlgInvokeJavaScript())
            //{
            //    dlgInvokeJavaScript.ScriptBody = m_PreviousScript;

            //    if (dlgInvokeJavaScript.ShowDialog() == DialogResult.OK)
            //    {
            //        m_PreviousScript = dlgInvokeJavaScript.ScriptBody;
            //        SendKeys.SendWait(dlgInvokeJavaScript.ScriptBody);
            //    }
            //}            
        }

        private void OnCallback(object state)
        {
            SendKeys.SendWait("{Esc}^{Tab}^{Tab}^+{Right}{F7}");
            Thread.Sleep(1000);
            SendKeys.SendWait("{Esc}{Esc}^{Tab}^{Tab}^{End}{Right} ^v{Esc}");
        }
    }
}