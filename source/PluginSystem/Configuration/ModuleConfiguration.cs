using System;
using PluginSystem.Helpers;

namespace PluginSystem.Configuration
{
    [Serializable]
    public abstract class ModuleConfiguration
    {
        public override string ToString()
        {
            var attribute = ReflectionHelper.GetAttribute<ConfigurationAttribute>(GetType());
            return attribute.ConfigurationName;
        }

        public virtual void CreateDefaults()
        {
            
        }
    }
}