using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;
using PluginSystem.API;
using PluginSystem.Plugins;
using PluginSystem.Win32;
using System.Diagnostics;
using Diagnostic;
using System.Threading;

namespace PluginSystem.Common
{
    internal class MessageFilter : IMessageFilter
    {
        public bool PreFilterMessage(ref Message m)
        {
            if (
                m.Msg == (int)Msgs.WM_KEYDOWN ||
                m.Msg == (int)Msgs.WM_SYSKEYDOWN ||
                m.Msg ==  (int)Msgs.WM_LBUTTONDOWN ||
                m.Msg ==  (int)Msgs.WM_RBUTTONDOWN ||
                m.Msg ==  (int)Msgs.WM_MBUTTONDOWN)
            {
                StatusMessage.Remove();

                if (SuperMemo.MainForm.ContainsFocus)
                {
                    if (m.Msg == (int) Msgs.WM_KEYDOWN || m.Msg == (int) Msgs.WM_SYSKEYDOWN)
                    {
                        Keys keyData = ((Keys) ((int) ((long) m.WParam))) | Control.ModifierKeys;

                        try
                        {

                            if (SuperMemoInjection.ExecuteShortcut(keyData))
                            {
                                return true;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }

            //if (m.Msg == (int)Msgs.WM_LBUTTONUP)
            //{
            //    Point currentPosition = Cursor.Position;
            //    Control control = SuperMemo.MainForm.GetChildAtPoint(currentPosition);

            //    if (control != null && control.GetType().Name == "AxWebBrowser")
            //    {
            //        MouseHelper.PerformRightButtonMouseClick(currentPosition);
            //    }
            //}
            
            return false;           
        }
    }
}