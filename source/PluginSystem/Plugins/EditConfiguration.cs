using System.Windows.Forms;
using PluginSystem.Common;
using PluginSystem.Configuration;
using PluginSystem.Plugin;

namespace PluginSystem.Plugins
{
    public class EditConfiguration : SuperMemoExtension
    {
        public override string Name
        {
            get { return "&Konfiguruj wtyczki..."; }
        }

        public override void Execute()
        {
            ConfigurationManager.SaveConfigurations();
            var moduleConfigurations = ConfigurationManager.GetAllConfigurations();
            
            using (var configDialog = new CollectionEditor<ModuleConfiguration>(moduleConfigurations, false, false, false))
            {
                configDialog.Text = "Edycja konfiguracji wtyczek";
                
                if (configDialog.ShowDialog() == DialogResult.OK)
                {
                    ConfigurationManager.SaveConfigurations();
                }
                else
                {
                    ConfigurationManager.CancelAllChanges();
                }
            }
        }
    }
}