using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;

namespace ItemsExchange
{
    public partial class DlgExportItems : BaseForm
    {
        private readonly TreeView m_View;

        public TreeView TreeView
        {
            get { return treeView1; }
            set { treeView1 = value; }
        }

        public DlgExportItems(TreeView view)
        {
            m_View = view;
            InitializeComponent();
            treeView1.Nodes.Clear();
            m_View.ExpandAll();
            CopyNodes(m_View.Nodes, treeView1.Nodes);
            m_View.CollapseAll();
        }

        private static void CopyNodes(TreeNodeCollection source, TreeNodeCollection destination)
        {
            foreach (TreeNode sourceNode in source)
            {
                var destinationNode = (TreeNode) sourceNode.Clone();
                destinationNode.Nodes.Clear();
                destinationNode.ContextMenuStrip = null;

                destination.Add(destinationNode);
                
                if (sourceNode.Nodes.Count > 0)
                {
                    CopyNodes(sourceNode.Nodes, destinationNode.Nodes);  
                }
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    CheckAllChildNodes(e.Node, e.Node.Checked);
                }
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ShowPreview();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            ShowPreview();

        }

        private static void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private void ShowPreview()
        {
            if (treeView1 != null && treeView1.SelectedNode != null)
            {
                int itemNumber = (int)treeView1.SelectedNode.Tag;
                SuperMemo.PreviewPage(itemNumber);
            }
        }
    }
}