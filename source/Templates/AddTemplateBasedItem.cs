using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Helpers;
using PluginSystem.Plugin;

namespace Templates
{
    public class AddTemplateBasedItem : TemplateBaseItem, ICourseOpenNotification
    {
        #region Private fields

        private static FieldsHistory m_FieldsHistory = new FieldsHistory();

        #endregion
        #region Properties

        public static FieldsHistory FieldsHistory
        {
            get { return m_FieldsHistory; }
        }

        public override string Name
        {
            get { return @"Szablony\Dodaj jednostkę wg szablonu..."; }
        }

        #endregion
        #region Overrides

        public override void Execute()
        {
            if (!AreTemplatesInstalled())
            {
                if (
                    MessageBox.Show(
                        "Wykryto, że w danym kursie nie ma jeszcze zdefiniowanych szablonów. \nCzy chcesz teraz zdefiniować przykładowe szablony?",
                        "Pytanie", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    InstallDefaultTemplatesInCourse();
                }
            }
            else
            {
                LoadFieldsHistory();

                using (var dlgAddTemplateItem = new DlgAddTemplateItem())
                {
                    if (dlgAddTemplateItem.ShowDialog() == DialogResult.OK)
                    {
                        int previousPosition = SuperMemo.GetCurentItemNumber();
                        int newItemNumber = SuperMemo.AddNewItem();
                        string newItemFile = SuperMemo.GetItemDefinitionFullFileName(newItemNumber);
                        StreamHelper.WriteStreamToFile(dlgAddTemplateItem.ResultStream, newItemFile);

                        SuperMemo.GoToPage(previousPosition);
                        SuperMemo.GoToPage(newItemNumber);
                        SuperMemo.EditUpdateItemName();
                    }
                }

                SaveFieldsHistory();
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.T | Keys.Control;
        }

        #endregion
        #region Public static methods

        #endregion
        #region Private methods

        private static string GetFieldsHistoryFileName()
        {
            return Path.Combine(SuperMemo.CurrentCourseOverridePath, "TemplatesFieldsHistory.xml");
        }

        private static void LoadFieldsHistory()
        {
            string fieldsHistoryFileName = GetFieldsHistoryFileName();

            if (File.Exists(fieldsHistoryFileName))
            {
                m_FieldsHistory = SerializationHelper.DeserializeFromFile<FieldsHistory>(fieldsHistoryFileName);
            }
        }

        private static void SaveFieldsHistory()
        {
            string fieldsHistoryFileName = GetFieldsHistoryFileName();

            SerializationHelper.SerializeToFile(fieldsHistoryFileName, m_FieldsHistory);
        }

        #endregion
        #region ICourseOpenNotification Members

        public void CourseOpened()
        {
            TreeNode node = GetTemplatesNode();
            if (node != null)
            {
                TreeView tv = SuperMemo.GetItemsTreeView();

                node.ImageIndex = 0;
                node.SelectedImageIndex = 1;
                node.NodeFont = new Font(tv.Font, FontStyle.Italic);
            }
        }

        #endregion
    }
}