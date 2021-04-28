using System;
using System.Collections.Generic;
using System.IO;
using PluginSystem.Helpers;

namespace PluginSystem.Configuration
{
    public static class ConfigurationManager
    {
        private static readonly Dictionary<Type, ModuleConfiguration> m_ModuleConfigurations = new Dictionary<Type, ModuleConfiguration>();

        public static T GetConfiguration<T>() where T : ModuleConfiguration
        {
            Type configurationType = typeof(T);
            return (T)GetConfiguration(configurationType);
        }

        private static object GetConfiguration(Type configurationType)
        {
            if (!m_ModuleConfigurations.ContainsKey(configurationType))
            {
                string configurationOutputPath = GetConfigurationOutputPath(configurationType);

                if (File.Exists(configurationOutputPath))
                {
                    m_ModuleConfigurations[configurationType] =
                        (ModuleConfiguration)
                        SerializationHelper.DeserializeFromFile(configurationType, configurationOutputPath);
                }
                else
                {
                    m_ModuleConfigurations[configurationType] = (ModuleConfiguration) Activator.CreateInstance(configurationType);
                    m_ModuleConfigurations[configurationType].CreateDefaults();
                }
            }

            if (m_ModuleConfigurations.ContainsKey(configurationType))
            {
                return m_ModuleConfigurations[configurationType];
            }

            return null;
        }

        public static void SaveConfigurations()
        {
            foreach (var moduleConfigurationPair in m_ModuleConfigurations)
            {
                Type configurationType = moduleConfigurationPair.Value.GetType();
                string outputPath = GetConfigurationOutputPath(configurationType);
                SerializationHelper.SerializeToFile(outputPath, moduleConfigurationPair.Value, configurationType);
            }
        }

        private static string GetConfigurationOutputPath(Type configurationType)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SM UX Plugins");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return Path.ChangeExtension(Path.Combine(path, configurationType.Name), "xml");
        }

        public static IEnumerable<ModuleConfiguration> GetAllConfigurations()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.GetType().IsAbstract && type.IsSubclassOf(typeof(ModuleConfiguration)))
                    {
                        yield return (ModuleConfiguration)GetConfiguration(type);
                    }
                }
            }

            yield break;
        }

        public static void CancelAllChanges()
        {
            m_ModuleConfigurations.Clear();
        }
    }
}