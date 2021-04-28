using System.Windows.Forms;

namespace PluginSystem.Helpers
{
    public static class ToolStripHelper
    {
        public static ToolStripItem FindExactly(this ToolStripItemCollection items, string text)
        {
            foreach (ToolStripItem item in items)
            {
                if (item.Text == text)
                {
                    return item;
                }
            }

            return null;
        }
    }
}