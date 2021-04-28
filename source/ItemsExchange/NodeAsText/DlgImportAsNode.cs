using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PluginSystem.Common;

namespace ItemsExchange.NodeAsText
{
    public partial class DlgImportAsNode : BaseForm
    {
        public DlgImportAsNode()
        {
            InitializeComponent();
        }

        public string NodeAsTextFileName
        {
            get { return txbNodeAsTextFile.Text; }
        }

        public string TranslationFileName
        {
            get { return txbTranslationFile.Text; }
        }

        private void btnNodeAsTextFile_Click(object sender, EventArgs e)
        {
            txbNodeAsTextFile.Text = GetFileName() ?? txbNodeAsTextFile.Text;
        }

        private void btnTranslationFile_Click(object sender, EventArgs e)
        {
            txbTranslationFile.Text = GetFileName() ?? txbTranslationFile.Text;
        }

        private static string GetFileName()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "NodeAsText|*.txt";
                dialog.AddExtension = true;
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;                
                }
            }

            return null;
        }
    }
}
