using System;
using System.Windows.Forms;
using PluginSystem.Common;

namespace Diagnostic
{
    public partial class DlgInvokeJavaScript : BaseForm
    {
        public DlgInvokeJavaScript()
        {
            InitializeComponent();
        }

        public string ScriptBody
        {
            get
            {
                return richTextBox1.Text;
            }
            set
            {
                richTextBox1.Text = value;
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
