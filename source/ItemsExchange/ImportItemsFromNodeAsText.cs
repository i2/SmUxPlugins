using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ItemsExchange.NodeAsText;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Plugin;

namespace ItemsExchange
{
    public class ImportItemsFromNodeAsText : SuperMemoExtension
    {
        private string m_FileName;
        private string m_TranslationFileName;

        public override string Name
        {
            get { return @"&Jednostki\Importuj jednostki z formatu &NodeAsText..."; }
        }

        public override bool Enabled
        {
            get
            {
                return SuperMemo.GetItemsTreeView() != null && !string.IsNullOrEmpty(SuperMemo.CurrentCoursePath) && SuperMemo.VisualizerMode == 1;
            }
        }

        public override void Execute()
        {
            using (var dialog = new DlgImportAsNode())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    m_FileName = dialog.NodeAsTextFileName;
                    m_TranslationFileName = dialog.TranslationFileName;

                    if (!string.IsNullOrEmpty(m_FileName) && File.Exists(m_FileName))
                    {
                        Logger.CreateNewLog("Import jednostek z pliku");
                        Logger.AddToLog(string.Format("<p>Import danych z pliku: {0}</p>", m_FileName));
                        Logger.AddToLog(string.Format("<p>Plik translation file: {0}</p>", m_TranslationFileName));
                        DlgProgress.Execute("Importowanie danych", Import);

                        Logger.ShowLogResult();
                    }
                    else
                    {
                        MessageBox.Show("Nie podano nazwy instniej¹cego pliku do importu");
                    }
                }
            }
        }

        private void Import(IProgress progress)
        {
            var importConfiguration = new ImportConfiguration();
            
            if (!string.IsNullOrEmpty(m_TranslationFileName) && File.Exists(m_TranslationFileName))
            {
                importConfiguration.Translations = ImportAsText.LoadTranslations(m_TranslationFileName);
            }
            
            IList<Element> elements = ImportAsText.LoadElements(m_FileName);

            var originalIdToImported = new Dictionary<int, int>();
            int importParentId = SuperMemo.GetCurentItemNumber();

            int i = 0;
            foreach (Element element in elements)
            {
                i++;

                if (progress.Cancel)
                {
                    break;    
                }

                progress.ProgressValue = 100*i/elements.Count;
                progress.Message = string.Format("Import jednostki {0} z {1}", i, elements.Count);

                var itemDescription = new ItemDescription(importConfiguration, element);
                
                if (originalIdToImported.ContainsKey(itemDescription.ParentId))
                {
                    SuperMemo.GoToPage(originalIdToImported[itemDescription.ParentId]);
                }
                else
                {
                    SuperMemo.GoToPage(importParentId);
                }

                bool insert = true;
                TreeView view = SuperMemo.GetItemsTreeView();
                TreeNode selectedNode = view.SelectedNode;

                if (selectedNode.Nodes.Count>0)
                {
                    SuperMemo.GoToPage((int)selectedNode.Nodes[selectedNode.Nodes.Count - 1].Tag);
                    insert = false;
                }

                int newItemId = insert ? SuperMemo.InsertSubItem() : SuperMemo.AddNewItem();
                originalIdToImported[itemDescription.Id] = newItemId;
                
                itemDescription.SaveToFile(SuperMemo.GetItemDefinitionFullFileName(newItemId));
                SuperMemo.GoToPage(newItemId);
                SuperMemo.SetCurrentItemName(itemDescription.Title);
            }
        }
    }
}