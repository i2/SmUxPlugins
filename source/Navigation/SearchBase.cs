using System;
using System.IO;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Plugin;

namespace Navigation
{
    public abstract class SearchBase : SuperMemoExtension
    {
        private string m_CacheBuildForPath;
        private string m_OverridePath;

        protected void PrepareSearchCache()
        {
            m_OverridePath = Path.Combine(SuperMemo.CurrentCoursePath, "Override");
            if (m_CacheBuildForPath != m_OverridePath)
            {
                DlgProgress.Execute("Budowanie cache'a", ProgressActionDelegate);
            }
        }

        private void ProgressActionDelegate(IProgress progress)
        {
            DatabaseSearchCache.Init(m_OverridePath, progress);
            m_CacheBuildForPath = m_OverridePath;            
        }
    }
}