using PluginSystem.Configuration;

namespace PluginSystem.Plugin
{
    public abstract class SuperMemoExtension<T> : SuperMemoExtension where T : ModuleConfiguration
    {
        protected static T Configuration
        {
            get
            {
                return ConfigurationManager.GetConfiguration<T>();
            }
        }
    }
}