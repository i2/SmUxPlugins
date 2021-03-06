using System.Drawing;
using System.Windows.Forms;
using PluginSystem.Plugin;

namespace PluginSystem.Plugins
{
    public class PluginManager : SuperMemoExtension
    {
        public override string Name
        {
            get { return "&Zarządzanie wtyczkami..."; }
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