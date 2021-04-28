using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ItemsExchange.SmPack;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Helpers;
using PluginSystem.Plugin;

namespace ItemsExchange
{
    public class ExportItems : SuperMemoExtension
    {
        private string m_Name;
        private TreeNodeCollection m_Nodes;

        public override string Name
        {
            get { return @"&Jednostki\&Eksportuj jednostki..."; }
        }

        public override bool Enabled
        {
            get
            {
                return SuperMemo.GetItemsTreeView() != null && !string.IsNullOrEmpty(SuperMemo.CurrentCoursePath) && SuperMemo.VisualizerMode == 1;
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.None;
        }

        public override void Execute()
        {
            using (var dlgExportItems = new DlgExportItems(SuperMemo.GetItemsTreeView()))
            {
                if (dlgExportItems.ShowDialog() == DialogResult.OK)
                {
                    using (var saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "SmUxPack|*.SmUxPack";
                        saveFileDialog.AddExtension = true;

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            m_Name = saveFileDialog.FileName;
                            const string EXTENSION = ".SmUxPack";
                            if (!m_Name.EndsWith(EXTENSION))
                            {
                                m_Name += EXTENSION;
                            }

                            m_Nodes = dlgExportItems.TreeView.Nodes;
                            DlgProgress.Execute("Eksport jednostek", ExportSelecteditems);
                        }
                    }                 
   
                    StatusMessage.ShowInfo("Eksport zakończony poprawnie");
                }
            }
        }

        private static string GetItemFileName(int id)
        {
            return (string.Format("item{0}.xml", GetItemAsString(id)));
        }

        private static string GetItemAsString(int id)
        {
            return id <= 0x1869f ? id.ToString("00000") : id.ToString("0000000");
        }

        private void ExportSelecteditems(IProgress progress)
        {
            ExportStructureInfo(progress);
            ExportSelectedItems(progress);
        }

        private void ExportSelectedItems(IProgress progress)
        {
            var itemFiles = new List<string>();
            var multimediaFiles = new List<string>();
            PrepareExportFileList(m_Nodes, itemFiles, multimediaFiles);
            ZipHelper.AddFilesToZip(progress, m_Name, "Override", itemFiles);
            ZipHelper.AddFilesToZip(progress, m_Name, @"Override\Multimedia", multimediaFiles);
        }

        private void ExportStructureInfo(IProgress progress)
        {
            string temporaryPath = Path.GetTempPath();
            string structureFileName = Path.Combine(temporaryPath, @"Structure.xml");
            using (var stream = new FileStream(structureFileName, FileMode.OpenOrCreate))
            {
                ItemCollection items = Item.GetSelectedItems(m_Nodes);                
                SerializationHelper.Serialize(stream, items);
            }
            ZipHelper.AddFilesToZip(progress, m_Name, "General", new List<string> { structureFileName });
            File.Delete(structureFileName);
        }

        private static void PrepareExportFileList(TreeNodeCollection nodeCollection, List<string> itemFiles, List<string> multimediaFiles)
        {
            foreach (TreeNode node in nodeCollection)
            {
                if (node.Checked)
                {
                    var itemNumber = (int)node.Tag;
                    string itemFileName = GetItemFileName(itemNumber);
                    string itemFile = Path.Combine(SuperMemo.CurrentCourseOverridePath, itemFileName);
                    itemFiles.Add(itemFile);

                    if (Directory.Exists(SuperMemo.CurrentCourseMediaPath))
                    {
                        foreach (string fileName in Directory.GetFiles(SuperMemo.CurrentCourseMediaPath, "*" + GetItemAsString(itemNumber) + "*.*"))
                        {
                            multimediaFiles.Add(fileName);
                        }
                    }
                }

                if (node.Nodes.Count > 0)
                {
                    PrepareExportFileList(node.Nodes, itemFiles, multimediaFiles);
                }
            }
        }
    }
}
