using System.Drawing;
using System.Windows.Forms;
using PluginSystem.Plugin;

namespace PluginSystem.Plugins
{
    public class PluginManager : SuperMemoExtension
    {
        public override string Name
        {
            get { return "&Zarz¹dzanie wtyczkami..."; }
        }

        public override void Execute()
        {
            using (var dlgPluginManagment = new DlgPluginManagment())
            {
                if (dlgPluginManagment.ShowDialog() == DialogResult.OK)
                {
                    SuperMemoInjection.RegisterAllPlugins();        
                }
            }
        }
    }
}