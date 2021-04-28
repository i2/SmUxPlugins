using System.Windows.Forms;
using PluginSystem.API;

namespace PluginSystem.Common
{
    public class BaseForm : Form
    {
        public BaseForm()
        {
            MinimizeBox = false;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            if (SuperMemo.MainForm != null)
            {
                Icon = SuperMemo.MainForm.Icon;
            }

            ShowInTaskbar = false;
            Owner = SuperMemo.MainForm;
        }
    }
}