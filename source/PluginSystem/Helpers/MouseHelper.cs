using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Win32;

namespace Diagnostic
{
    public static class MouseHelper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static void PerformLeftButtonMouseClick (Point p)
        {
            Cursor.Position = p;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, p.X, p.Y, 0, 0);            
        }

        public static void PerformRightButtonMouseClick(Point p)
        {
            Cursor.Position = p;
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, p.X, p.Y, 0, 0);
        }
    }

    public static class WebBrowserCaretHelper
    {
        [DllImport("user32.dll")]
        private static extern bool GetCaretPos(out Point lpPoint);

        public static Point? GetCaretPosition()
        {
            Point result;
            GetCaretPos(out result);
            Control control = SuperMemo.GetWebBrowser();
            
            if (control != null && result != Point.Empty) 
            {
                return control.PointToScreen(result);
            }

            return null;
        }
    }
}