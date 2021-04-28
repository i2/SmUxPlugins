using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using PluginSystem.Common;

namespace Templates
{
    public partial class DlgImportQAWithTemplate : BaseForm
    {
        readonly IDictionary<string, Control> m_FieldEditors = new Dictionary<string, Control>();

        public DlgImportQAWithTemplate()
        {
            InitializeComponent();

            TreeNode node = TemplateBaseItem.GetTemplatesNode();

            foreach (TreeNode treeNode in node.Nodes)
            {
                cbxTemplates.Items.Add(new Template(treeNode));
            }

            if (cbxTemplates.Items.Count > 0)
            {
                cbxTemplates.SelectedIndex = 0;
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

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
