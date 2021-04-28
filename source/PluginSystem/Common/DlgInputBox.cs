using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PluginSystem.Common
{
    public partial class DlgInputBox : BaseForm
    {
        public DlgInputBox()
        {
            InitializeComponent();
        }

        public static bool GetText(string dialogTitle, string label, ref string text)
        {
            using (var dlgInputBox = new DlgInputBox())
            {
                dlgInputBox.Text = dialogTitle;
                dlgInputBox.label1.Text = label;
                dlgInputBox.textBox1.Text = text;
                if (dlgInputBox.ShowDialog() == DialogResult.OK)
                {
                    text = dlgInputBox.textBox1.Text;
                    return true;
                }
            }

            text = null;
            return false;
        }
    }
}
