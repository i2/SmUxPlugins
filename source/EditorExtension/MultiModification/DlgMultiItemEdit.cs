using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;

namespace EditorExtension
{
    public partial class DlgMultiItemEdit : BaseForm
    {
        private readonly TreeView m_View;

        public TreeView TreeView
        {
            get { return treeView1; }
            set { treeView1 = value; }
        }

        public string Lesson
        {
            get
            {
                return chbLesson.Checked ? txbLesson.Text : null;
            }
            set
            {
                txbLesson.Text = value;
                chbLesson.Checked = !string.IsNullOrEmpty(txbLesson.Text);
            }
        }

        public string Chapter
        {
            get
            {
                return chbChapter.Checked ? txbChapter.Text : null;
            }
            set
            {
                txbChapter.Text = value;
                chbChapter.Checked = !string.IsNullOrEmpty(txbChapter.Text);
            }
        }

        public string Command
        {
            get
            {
                return chbCommand.Checked ? txbCommand.Text : null;
            }
            set
            {
                txbCommand.Text = value;
                chbCommand.Checked = !string.IsNullOrEmpty(txbCommand.Text);
            }
        }

        public int? TemplateNumber
        {
            get
            {
                int result;

                if (chbTemplate.Checked && int.TryParse(txbTemplate.Text, out result))
                {
                    return result;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    txbCommand.Text = value.ToString();
                }
                else
                {
                    txbCommand.Text = string.Empty;                           
                }
                chbTemplate.Checked = !string.IsNullOrEmpty(txbTemplate.Text);
            }
        }

        public string SearchText
        {
            get
            {
                if (chbReplaceInAnswer.Checked || chbReplaceInQuestion.Checked || chbReplaceInChapterTitle.Checked || chbReplaceInLessonTitle.Checked || chbReplaceInQuestionTitle.Checked)
                {
                    return txbSearchText.Text;
                }

                return null;
            }
            set
            {
                txbSearchText.Text = value;
            }
        }

        public string ReplaceText
        {
            get
            {
                if (chbReplaceInAnswer.Checked || chbReplaceInQuestion.Checked || chbReplaceInChapterTitle.Checked || chbReplaceInLessonTitle.Checked || chbReplaceInQuestionTitle.Checked)
                {
                    return txbReplaceText.Text;
                }

                return null;
            }
            set
            {
                txbReplaceText.Text = value;
            }
        }

        public bool ReplaceInAnswer
        {
            get
            {
                return chbReplaceInAnswer.Checked;
            }
            set
            {
                chbReplaceInAnswer.Checked = value;
            }
        }

        public bool ReplaceInQuestion
        {
            get
            {
                return chbReplaceInQuestion.Checked;
            }
            set
            {
                chbReplaceInQuestion.Checked = value;
            }
        }

        public bool ReplaceInQuestionTitle
        {
            get
            {
                return chbReplaceInQuestionTitle.Checked;
            }
            set
            {
                chbReplaceInQuestionTitle.Checked = value;
            }
        }

        public bool ReplaceInLessonTitle
        {
            get
            {
                return chbReplaceInLessonTitle.Checked;
            }
            set
            {
                chbReplaceInLessonTitle.Checked = value;
            }
        }

        public bool ReplaceInChapterTitle
        {
            get
            {
                return chbReplaceInChapterTitle.Checked;
            }
            set
            {
                chbReplaceInChapterTitle.Checked = value;
            }
        }

        public bool UseRegularExpressions
        {
            get
            {
                return chbUseRegularExpressions.Checked;   
            }
            set
            {
                chbUseRegularExpressions.Checked = value;
            }
        }

        public bool ItemNameBasedOnQuestion
        {
            get
            {
                return rbtAsQuestion.Checked;
            }
            set
            {
                rbtAsQuestion.Checked = value;
            }
        }

        public string ItemNameAsValue
        {
            get
            {
                if (!rbtAsValue.Checked)
                {
                    return null;
                }

                return txbTreeItemName.Text;
            }
            set
            {
                txbTreeItemName.Text = value;
                rbtAsValue.Checked = !string.IsNullOrEmpty(txbTreeItemName.Text);
            }
        }

        public DlgMultiItemEdit()
        {
            int originalPosition = SuperMemo.GetCurentItemNumber();

            m_View = SuperMemo.GetItemsTreeView();

            InitializeComponent();
            treeView1.Nodes.Clear();
            m_View.BeginUpdate();
            m_View.ExpandAll();
            CopyNodes(m_View.Nodes, treeView1.Nodes);
            m_View.CollapseAll();
            m_View.EndUpdate();

            SuperMemo.GoToPage(originalPosition);

            RefreshControlStates();

            if (m_View.SelectedNode != null)
            {
                treeView1.SelectedNode = GetNodeForItem(originalPosition, treeView1);

                if (treeView1.SelectedNode != null)
                {
                    treeView1.SelectedNode.EnsureVisible();
                    treeView1.SelectedNode.Checked = true;
                    CheckAllChildNodes(treeView1.SelectedNode, treeView1.SelectedNode.Checked);
                }
            }
        }

        private TreeNode GetNodeForItem(int itemNumber, TreeView treeView)
        {
            return GetNodeForItem(itemNumber, treeView.Nodes);
        }

        private TreeNode GetNodeForItem(int number, TreeNodeCollection collection)
        {
            foreach (TreeNode node in collection)
            {
                if ((int)node.Tag == number)
                {
                    return node;
                }

                if (node.Nodes.Count>0)
                {
                    TreeNode item = GetNodeForItem(number, node.Nodes);
                    if (item != null)
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        public IList<int> GetSelectedItems()
        {
            var result = new List<int>();

            result.AddRange(GetSelectedNodes(treeView1.Nodes));

            return result;
        }

        private static IList<int> GetSelectedNodes(TreeNodeCollection nodes)
        {
            var result = new List<int>();


            foreach (TreeNode node in nodes)
            {
                if (node.Checked)
                {
                    result.Add((int)node.Tag);                    
                }
                if (node.Nodes.Count>0)
                {
                    result.AddRange(GetSelectedNodes(node.Nodes));
                }
            }

            return result;
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

        private void RefreshControlStates()
        {
            txbCommand.Enabled = chbCommand.Checked;
            txbLesson.Enabled = chbLesson.Checked;
            txbChapter.Enabled = chbChapter.Checked;
            txbTreeItemName.Enabled = rbtAsValue.Checked;
            txbTemplate.Enabled = chbTemplate.Checked;

            txbSearchText.Enabled = chbReplaceInAnswer.Checked || chbReplaceInQuestion.Checked ||
                                    chbReplaceInChapterTitle.Checked || chbReplaceInLessonTitle.Checked ||
                                    chbReplaceInQuestionTitle.Checked;

            txbReplaceText.Enabled = chbReplaceInAnswer.Checked || chbReplaceInQuestion.Checked ||
                                     chbReplaceInChapterTitle.Checked || chbReplaceInLessonTitle.Checked ||
                                     chbReplaceInQuestionTitle.Checked;
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

        private void chbCommand_CheckedChanged(object sender, EventArgs e)
        {
            RefreshControlStates();

            if (txbCommand.Enabled)
            {
                txbCommand.Focus();
            }
        }

        private void chbChapter_CheckedChanged(object sender, EventArgs e)
        {
            RefreshControlStates();

            if (txbChapter.Enabled)
            {
                txbChapter.Focus();
            }
        }

        private void chbLesson_CheckedChanged(object sender, EventArgs e)
        {
            RefreshControlStates();

            if (txbLesson.Enabled)
            {
                txbLesson.Focus();
            }
        }

        private void chbTemplate_CheckedChanged(object sender, EventArgs e)
        {
            RefreshControlStates();

            if (txbTemplate.Enabled)
            {
                txbTemplate.Focus();
            }
        }

        private void rdbLeaveAsIs_CheckedChanged(object sender, EventArgs e)
        {
            RefreshControlStates();
        }

        private void rbtAsQuestion_CheckedChanged(object sender, EventArgs e)
        {
            RefreshControlStates();
        }

        private void rbtAsValue_CheckedChanged(object sender, EventArgs e)
        {
            RefreshControlStates();

            if (txbTreeItemName.Enabled)
            {
                txbTreeItemName.Focus();
            }
        }

        private void chbReplaceInQuestion_CheckedChanged(object sender, EventArgs e)
        {
            RefreshControlStates();

            if (txbSearchText.Enabled)
            {
                txbSearchText.Focus();
            }
        }

        private void chbReplaceInLessonTitle_CheckedChanged(object sender, EventArgs e)
        {
            RefreshControlStates();

            if (txbSearchText.Enabled)
            {
                txbSearchText.Focus();
            }
        }

        private void chbReplaceInQuestionTitle_CheckedChanged(object sender, EventArgs e)
        {
            RefreshControlStates();

            if (txbSearchText.Enabled)
            {
                txbSearchText.Focus();
            }
        }

        private void chbReplaceInAnswer_CheckedChanged(object sender, EventArgs e)
        {
            RefreshControlStates();

            if (txbSearchText.Enabled)
            {
                txbSearchText.Focus();
            }
        }

        private void chbReplaceInChapterTitle_CheckedChanged(object sender, EventArgs e)
        {
            RefreshControlStates();

            if (txbSearchText.Enabled)
            {
                txbSearchText.Focus();
            }
        }
    }
}