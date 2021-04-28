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
    public partial class DlgSelectItem<T> : BaseForm where T : class
    {
        public DlgSelectItem()
        {
            InitializeComponent();
        }

        public static T SelectItem(string dialogTitle, string label, List<T> list, int defaultItem)
        {
            using (var dlgInputBox = new DlgSelectItem<T>())
            {
                dlgInputBox.Text = dialogTitle;
                dlgInputBox.label1.Text = label;

                foreach (T t in list)
                {
                    dlgInputBox.listBox1.Items.Add(t);                    
                }

                if ((defaultItem >= 0) && (defaultItem<dlgInputBox.listBox1.Items.Count))
                {
                    dlgInputBox.listBox1.SelectedIndex = defaultItem;
                }

                if (dlgInputBox.ShowDialog() == DialogResult.OK)
                {
                    return (T)dlgInputBox.listBox1.SelectedItem;
                }

            }

            return null;
        }
    }
}
