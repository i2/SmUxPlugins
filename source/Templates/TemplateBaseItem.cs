using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Helpers;
using PluginSystem.Plugin;

namespace Templates
{
    public abstract class TemplateBaseItem : SuperMemoExtension
    {
        public override bool Enabled
        {
            get
            {
                return CurrentState.IsCourseLoaded && CurrentState.IsInEditMode;
            }
        }

        protected static bool AreTemplatesInstalled()
        {
            return GetTemplatesNode() != null;
        }

        private static void ExtractItemFromResource(bool insert, string resourceName, string itemName)
        {
            int itemNumber = insert ? SuperMemo.InsertSubItem() : SuperMemo.AddNewItem();
            ExtractResourceFile(resourceName, SuperMemo.GetItemDefinitionFullFileName(itemNumber));
            SuperMemo.GoToPage(itemNumber);
            SuperMemo.SetCurrentItemName(itemName);
        }

        private static void ExtractResourceFile(string resourceName, string outputFileName)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                StreamHelper.WriteStreamToFile(stream, outputFileName);
            }
        }

        protected static void InstallDefaultTemplatesInCourse()
        {
            GoToLastItemInMainChapter();

            const string RESOURCE_NAME0 = "Templates.SampleTemplates.TemplateChapter.xml";
            const string ITEM_NAME0 = "Szablony";
            ExtractItemFromResource(false, RESOURCE_NAME0, ITEM_NAME0);

            int currentItem = SuperMemo.GetCurentItemNumber();

            const string RESOURCE_NAME1 = "Templates.SampleTemplates.Template1.xml";
            const string ITEM_NAME1 = "Jednostka z wymow¹";
            ExtractItemFromResource(true, RESOURCE_NAME1, ITEM_NAME1);

            const string RESOURCE_NAME2 = "Templates.SampleTemplates.Template2.xml";
            const string ITEM_NAME2 = "Jednostka z wymow¹ i przyk³adami";
            ExtractItemFromResource(false, RESOURCE_NAME2, ITEM_NAME2);

            const string RESOURCE_NAME3 = "Templates.SampleTemplates.Template3.xml";
            const string ITEM_NAME3 = "Jednostka z wymow¹,objaœnieniem i przyk³adami";
            ExtractItemFromResource(false, RESOURCE_NAME3, ITEM_NAME3);

            SuperMemo.GoToPage(currentItem);
        }

        protected static bool IsTemplateNode(TreeNode node)
        {
            return node.Text.Contains("Szablony");
        }

        public static TreeNode GetTemplatesNode()
        {
            TreeView tv = SuperMemo.GetItemsTreeView();

            foreach (TreeNode node in tv.Nodes)
            {
                if (IsTemplateNode(node))
                {
                    return node;
                }
            }

            return null;
        }

        private static void GoToLastItemInMainChapter()
        {
            TreeView view = SuperMemo.GetItemsTreeView();
            int count = view.Nodes.Count;

            if (count > 0)
            {
                TreeNode node = view.Nodes[count - 1];
                var itemNumber = (int) node.Tag;
                SuperMemo.GoToPage(itemNumber);
            }
        }

        public static void PrepareFields(Template template, TableLayoutPanel tableLayoutPanel, IDictionary<string, Control> fieldEditors)
        {
            fieldEditors.Clear();

            tableLayoutPanel.Controls.Clear();
            tableLayoutPanel.RowStyles.Clear();
            
            IList<TemplateFieldInfo> infos = template.TagsInfo;

            foreach (TemplateFieldInfo templateFieldInfo in infos)
            {
                if (templateFieldInfo.IsField)
                {
                    Control fieldLabel = templateFieldInfo.GetFieldLabel();
                    tableLayoutPanel.Controls.Add(fieldLabel);

                    Control fieldEditor = templateFieldInfo.GetFieldEditor();
                    fieldEditors[templateFieldInfo.OriginalTag] = fieldEditor;

                    tableLayoutPanel.Controls.Add(fieldEditor);
                    
                    Control fieldButton = templateFieldInfo.GetFieldButton();
                    tableLayoutPanel.Controls.Add(fieldButton);
                }
            }

            var label3 = new Label();
            tableLayoutPanel.Controls.Add(label3);
        }
    }
}