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
    public partial class DlgMultiselectItems<T> : BaseForm where T : class
    {
        public DlgMultiselectItems()
        {
            InitializeComponent();
        }

        public static List<T> MultiselectItems(string dialogTitle, string label, List<T> list, int defaultItem, Func<T, bool> initialCheckStatePredicate)
        {
            using (var dlgInputBox = new DlgMultiselectItems<T>())
            {
                dlgInputBox.Text = dialogTitle;
                dlgInputBox.label1.Text = label;

                foreach (T t in list)
                {
                    int i = dlgInputBox.checkedListBox1.Items.Add(t);
                    dlgInputBox.checkedListBox1.SetItemChecked(i, initialCheckStatePredicate(t));
                }

                if ((defaultItem >= 0) && (defaultItem<dlgInputBox.checkedListBox1.Items.Count))
                {
                    dlgInputBox.checkedListBox1.SelectedIndex = defaultItem;
                }

                if (dlgInputBox.ShowDialog() == DialogResult.OK)
                {
                    return new List<T>(dlgInputBox.SelectedItems);
                }

            }

            return null;
        }

        private IEnumerable<T> SelectedItems
        {
            get
            {
                foreach (T t in checkedListBox1.CheckedItems)
                {
                    yield return t;
                }

                yield break;
            }
        }
    }
}
