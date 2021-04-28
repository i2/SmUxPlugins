using System;
using System.Windows.Forms;
using PluginSystem.API;

namespace ItemsExchange.SmPack
{
    [Serializable]
    public class Item
    {
        private int m_ItemNumber;
        private readonly ItemCollection m_Subitems = new ItemCollection();

        [NonSerialized]
        private readonly TreeNode m_TreeNode;

        public TreeNode TreeNode
        {
            get { return m_TreeNode; }
        }

        public ItemCollection Subitems
        {
            get { return m_Subitems; }
        }

        public int ItemNumber
        {
            get
            {
                return m_ItemNumber;
            }
            set
            {
                m_ItemNumber = value;
            }
        }

        public int ImportedItemNumber { get; set; }

        public string NodeName { get; set; }

        public Item()
        {
        }

        private Item(TreeNode treeNode, int itemNumber)
        {
            m_TreeNode = treeNode;
            m_ItemNumber = itemNumber;
            NodeName = treeNode.Text;

            BuildSubItems(treeNode.Nodes, m_Subitems);
        }

        public static ItemCollection GetSelectedItems(TreeNodeCollection source)
        {
            var result = new ItemCollection();

            foreach (TreeNode node in source)
            {
                if (node.Checked && (node.Parent == null || !node.Parent.Checked))
                {
                    result.Add(new Item(node, (int)node.Tag));
                }

                if (node.Nodes.Count > 0)
                {
                    result.AddRange(GetSelectedItems(node.Nodes));
                }
            }

            return result;
        }

        private static void BuildSubItems(TreeNodeCollection nodeCollection, ItemCollection subitems)
        {
            foreach (TreeNode node in nodeCollection)
            {
                if (node.Checked)
                {
                    subitems.Add(new Item(node, (int)node.Tag));
                }
            }
        }

        public string GetFileName()
        {
            int itemNumber = m_ItemNumber;

            return SuperMemo.ItemNumberToDefinitionFileName(itemNumber);
        }
    }
}