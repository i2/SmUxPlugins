using System.Collections.Generic;
using System;

namespace PluginSystem.Common
{
    public partial class CollectionEditor<T> : BaseForm
    {
        public CollectionEditor(IEnumerable<T> collection, bool workWithCopy) : this(collection, workWithCopy, true, true)
        {
        }

        public CollectionEditor(IEnumerable<T> collection, bool workWithCopy, bool enableAdding, bool enableRemoving)
        {
            InitializeComponent();

            foreach (T t in collection)
            {
                T clone = workWithCopy ? (T) ((ICloneable) t).Clone() : t;
                listBox1.Items.Add(clone);
            }

            if (listBox1.Items.Count > 0)
            {
                listBox1.SelectedItem = listBox1.Items[0];
            }

            btnAdd.Visible = enableAdding;
            btnRemove.Visible = enableRemoving;

            if (!enableAdding && !enableRemoving)
            {
                listBox1.Top = propertyGrid1.Top;
                listBox1.Height = propertyGrid1.Height;
            }
        }

        public IList<T> Result
        {
            get
            {
                var result = new List<T>();

                foreach (var item in listBox1.Items)
                {
                    result.Add((T)item);
                }

                return result;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var item = Activator.CreateInstance(typeof(T));
            listBox1.SelectedItem = item;
            listBox1.Items.Add(item);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            while (listBox1.SelectedItems.Count>0)
            {
                listBox1.Items.Remove(listBox1.SelectedItems[0]);                
            }
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            SetSelectionInPropertyGrid();
        }

        private void SetSelectionInPropertyGrid()
        {
            var selected = new object[listBox1.SelectedItems.Count];
            listBox1.SelectedItems.CopyTo(selected, 0);
            propertyGrid1.SelectedObjects = selected;
        }
    }
}