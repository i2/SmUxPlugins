using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using PluginSystem.Common;
using PluginSystem.Properties;
using PluginSystem.Configuration;

namespace WebDictionaries
{
    public partial class DlgDictionaryCheck : BaseForm
    {
        private static int m_LastSelectedIndex = -1;

        public DlgDictionaryCheck()
        {
            InitializeComponent();
            DictionariesToForm();

            if (m_LastSelectedIndex >= 0 && m_LastSelectedIndex < lbxDictionary.Items.Count)
            {
                lbxDictionary.SelectedIndex = m_LastSelectedIndex;
            }
            else
            {
                if (lbxDictionary.Items.Count > 0)
                {
                    lbxDictionary.SelectedIndex = 0;
                }
            }
        }

        public DictionaryDefinitionCollection Dictionaries
        {
            get
            {
                return ConfigurationManager.GetConfiguration<WebDictionariesConfiguration>().Dictionaries;
            }
        }

        public string CommandText
        {
            get
            {
                var item = lbxDictionary.SelectedItem as DictionaryDefinition;
                if (item != null)
                {
                    return item.Command;
                }

                return null;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            FormToDictionaries();
            m_LastSelectedIndex = lbxDictionary.SelectedIndex;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void DlgDictionaryCheck_Load(object sender, EventArgs e)
        {
            lbxDictionary.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var dlgDictionaryConfiguration = new DlgDictionaryConfiguration(new DictionaryDefinition()))
            {
                if (dlgDictionaryConfiguration.ShowDialog() == DialogResult.OK)
                {
                    lbxDictionary.Items.Add(dlgDictionaryConfiguration.DictionaryDefinition);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var dictionaryDefinition = (DictionaryDefinition)lbxDictionary.SelectedItem;
            var copy = (DictionaryDefinition)dictionaryDefinition.Clone();
            
            using (var dlgDictionaryConfiguration = new DlgDictionaryConfiguration(copy))
            {
                if (dlgDictionaryConfiguration.ShowDialog() == DialogResult.OK)
                {
                    lbxDictionary.Items[lbxDictionary.SelectedIndex] = dlgDictionaryConfiguration.DictionaryDefinition;
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbxDictionary.SelectedItem != null)
            {
                lbxDictionary.Items.Remove(lbxDictionary.SelectedItem);
            }
        }

        private void DictionariesToForm()
        {
            foreach (var dictionary in Dictionaries)
            {
                lbxDictionary.Items.Add(dictionary);
            }
        }

        private void FormToDictionaries()
        {
            Dictionaries.Clear();
            
            foreach (DictionaryDefinition dictionaryDefinition in lbxDictionary.Items)
            {
                Dictionaries.Add(dictionaryDefinition);
            }

            ConfigurationManager.SaveConfigurations();
        }
    }
}
