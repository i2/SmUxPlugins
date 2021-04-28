using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PluginSystem.Common;

namespace WebDictionaries
{
    public partial class DlgDictionaryConfiguration : BaseForm
    {
        private readonly DictionaryDefinition m_DictionaryDefinition;

        public DictionaryDefinition DictionaryDefinition
        {
            get 
            {
                return m_DictionaryDefinition;
            }
        }

        public DlgDictionaryConfiguration()
        {
            InitializeComponent();
        }

        public DlgDictionaryConfiguration(DictionaryDefinition dictionaryDefinition) : this()
        {
            m_DictionaryDefinition = dictionaryDefinition;
            txtName.Text = m_DictionaryDefinition.Name;
            txtCommand.Text = m_DictionaryDefinition.Command;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            m_DictionaryDefinition.Name = txtName.Text;
            m_DictionaryDefinition.Command = txtCommand.Text;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }
    }
}
