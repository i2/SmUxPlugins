using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using PluginSystem.Common;

namespace Templates
{
    public partial class DlgAddTemplateItem : BaseForm
    {
        readonly IDictionary<string, Control> m_FieldEditors = new Dictionary<string, Control>();

        public DlgAddTemplateItem()
        {
            InitializeComponent();

            TreeNode node = TemplateBaseItem.GetTemplatesNode();
            node.Expand();

            foreach (TreeNode treeNode in node.Nodes)
            {
                cbxTemplates.Items.Add(new Template(treeNode));
            }

            if (cbxTemplates.Items.Count > 0)
            {
                cbxTemplates.SelectedIndex = 0;
            }
        }

        public Stream ResultStream
        {
            get
            {
                var currentTemplate = (Template)cbxTemplates.SelectedItem;
                
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(currentTemplate.GetDefinitionFileName());

                currentTemplate.ResolveTemplate(xmlDocument, GetFieldValues());

                var result = new MemoryStream();
                xmlDocument.Save(result);
                result.Seek(0, SeekOrigin.Begin);
                
                return result;
            }
        }

        private IDictionary<string, string> GetFieldValues()
        {
            var result = new Dictionary<string, string>();

            foreach (var pair in m_FieldEditors)
            {
                string fieldValue = pair.Value.Text;
                fieldValue = fieldValue.Replace("\r\n", "<br />");
                fieldValue = fieldValue.Replace("\n", "<br />");

                result.Add(pair.Key, fieldValue);
            }

            return result;
        }

        private void RememberHistoricalFields()
        {
            foreach (var pair in m_FieldEditors)
            {
                var comboBox = pair.Value as ComboBox;

                if (comboBox != null)
                {
                    var templateFieldInfo = new TemplateFieldInfo(pair.Key);

                    string value = comboBox.Text;

                    string fieldName = templateFieldInfo.FieldName;

                    if (!AddTemplateBasedItem.FieldsHistory.ContainsKey(fieldName))
                    {
                        AddTemplateBasedItem.FieldsHistory[fieldName] = new SerializableList<string>();
                    }

                    List<string> historyList = AddTemplateBasedItem.FieldsHistory[fieldName];

                    if (historyList.Contains(value))
                    {
                        historyList.Remove(value);
                    }

                    historyList.Insert(0, value);

                    if (historyList.Count > 8)
                    {
                        historyList.RemoveAt(historyList.Count -1);
                    }
                }
            }
        }

        private void cbxTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            var template = (Template)cbxTemplates.SelectedItem;

            TemplateBaseItem.PrepareFields(template, tableLayoutPanel1, m_FieldEditors);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                string message = errorProvider1.GetError(control);
                if (!string.IsNullOrEmpty(message))
                {
                    MessageBox.Show("Nie poprawne wartości: " + message);
                    DialogResult = DialogResult.None;
                    return;
                }
            }

            RememberHistoricalFields();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
