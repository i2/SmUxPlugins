using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ItemsExchange.SmPack;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Helpers;
using PluginSystem.Plugin;

namespace ItemsExchange
{
    public class ImportItems : SuperMemoExtension
    {
        private int m_ItemsToImport;
        private int m_ImportedItems;
        private string m_ZipFilename;
        private int m_RootItemNumber;

        public override string Name
        {
            get { return @"&Jednostki\&Importuj jednostki..."; }
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
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "SmUxPack|*.SmUxPack";
                openFileDialog.AddExtension = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    m_ZipFilename = openFileDialog.FileName;
                    const string EXTENSION = ".SmUxPack";
                    if (!m_ZipFilename.EndsWith(EXTENSION))
                    {
                        m_ZipFilename += EXTENSION;
                    }

                    m_RootItemNumber = SuperMemo.GetCurentItemNumber();
                    DlgProgress.Execute("Importowanie jednostek", PerformImportItems);

                    SuperMemo.GoToPage(m_RootItemNumber);
                    StatusMessage.ShowInfo("Import zakoñczony poprawnie");
                }
            }
        }

        private void PerformImportItems(IProgress progress)
        {
            ItemCollection structureInfo = GetStructureInfo(m_ZipFilename);
            m_ItemsToImport = structureInfo.GetAllItemsCount();
            m_ImportedItems = 0;

            IList<string> archiveFileList = ZipHelper.GetArchiveFileList(m_ZipFilename);

            IDictionary<string, string> dictionary = CreateItemStructure(progress, m_RootItemNumber, structureInfo, archiveFileList);

            if (!Directory.Exists(SuperMemo.CurrentCourseMediaPath))
            {
                Directory.CreateDirectory(SuperMemo.CurrentCourseMediaPath);
            }

            ZipHelper.ExtractFiles(progress, m_ZipFilename, dictionary);
        }

        private IDictionary<string, string> CreateItemStructure(IProgress progress, int rootItemNumber, IEnumerable<Item> itemList, IEnumerable<string> archiveFileList)
        {
            int position = rootItemNumber;
            Item firstItem = itemList.LastOrDefault();
            IDictionary<string, string> result = new Dictionary<string, string>();

            foreach (Item item in itemList.Reverse())
            {
                EnsureCurrentPosition(position);
                position = (item == firstItem) ? SuperMemo.InsertSubItem() : SuperMemo.InsertNewItem();
                item.ImportedItemNumber = position;
                EnsureCurrentPosition(position);
                SuperMemo.SetCurrentItemName(item.NodeName);
                
                m_ImportedItems++;
                progress.Message = string.Format("Importowanie jednostki: ({0}/{1})", m_ImportedItems, m_ItemsToImport);
                progress.ProgressValue = 100*m_ImportedItems/m_ItemsToImport;

                if (item.Subitems.Count>0)
                {
                    IDictionary<string, string> dictionary = CreateItemStructure(progress, position, item.Subitems, archiveFileList);
                    foreach (KeyValuePair<string, string> keyValuePair in dictionary)
                    {
                        result.Add(keyValuePair);
                    }
                }

                EnsureCurrentPosition(position);
                string itemFileName = item.GetFileName();

                string archiveFileName = @"Override\" + itemFileName;
                string destinationFileName = SuperMemo.CurrentItemFullPathFileName;
                result.Add(archiveFileName, destinationFileName);

                string itemSignature = itemFileName.Replace("item", string.Empty).Replace(".xml", string.Empty);
                
                foreach (string archiveFile in archiveFileList)
                {
                    if (archiveFile.StartsWith(@"Override\Multimedia\" + itemSignature, StringComparison.InvariantCultureIgnoreCase))
                    {
                        string newItemSignature = SuperMemo.CurrentItemFileWithoutExtension.Replace("item", string.Empty);
                        string destinationFile = Path.Combine(SuperMemo.CurrentCourseMediaPath, Path.GetFileName(archiveFile).Replace(itemSignature, newItemSignature));
                        result.Add(archiveFile, destinationFile);
                    }
                }
            }

            return result;
        }

        private static void EnsureCurrentPosition(int position)
        {
            if (SuperMemo.GetCurentItemNumber() != position)
            {
                SuperMemo.GoToPage(position);
            }            
        }

        private static ItemCollection GetStructureInfo(string name)
        {
            string structureFileName = Path.GetTempFileName();
            ZipHelper.ExtractFile(name, @"General\Structure.xml", structureFileName);
            var structureInfo = SerializationHelper.DeserializeFromFile<ItemCollection>(structureFileName);
            File.Delete(structureFileName);
            
            return structureInfo;
        }
    }
}