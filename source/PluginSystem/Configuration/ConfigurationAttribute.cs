using System;

namespace PluginSystem.Configuration
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationAttribute : Attribute
    {
        public string ConfigurationName { get; set; }


        public ConfigurationAttribute(string configName)
        {
            ConfigurationName = configName;
        }
    }
}