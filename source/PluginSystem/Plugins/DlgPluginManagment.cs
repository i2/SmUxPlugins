using System;
using System.Windows.Forms;
using PluginSystem.Common;
using PluginSystem.Properties;
using System.IO;
using PluginSystem.API;
using PluginSystem.Configuration;

namespace PluginSystem.Plugins
{
    public partial class DlgPluginManagment : BaseForm
    {
        public DlgPluginManagment()
        {
            InitializeComponent();
            
            lbxPluginsList.Items.Clear();

            string[] plugins = ConfigurationManager.GetConfiguration<PluginSystemConfiguration>().Plugins.Split(';');
            foreach (var plugin in plugins)
            {
                lbxPluginsList.Items.Add(plugin);                
            }
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            ConfigurationManager.GetConfiguration<PluginSystemConfiguration>().Plugins = string.Empty;
            foreach (string plugin in lbxPluginsList.Items)
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.GetConfiguration<PluginSystemConfiguration>().Plugins))
                {
                    ConfigurationManager.GetConfiguration<PluginSystemConfiguration>().Plugins += ";";
                }
                ConfigurationManager.GetConfiguration<PluginSystemConfiguration>().Plugins += plugin;
            }
            ConfigurationManager.SaveConfigurations();
            Close();
            
            DialogResult = DialogResult.OK;
        }

        private void btnAddPlugin_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Path.Combine(Environment.CurrentDirectory, @"Plugins");
                openFileDialog.Filter = "Pliki Dll |*.dll";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = openFileDialog.FileName;
                    if (Path.GetDirectoryName(fileName).Equals(Environment.CurrentDirectory))
                    {
                        fileName = Path.GetFileName(fileName);
                    }
                    lbxPluginsList.Items.Add(fileName);
                }
            }
        }

        private void btnRemovePlugin_Click(object sender, EventArgs e)
        {
            if (lbxPluginsList.SelectedItem != null)
            {
                lbxPluginsList.Items.Remove(lbxPluginsList.SelectedItem);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}