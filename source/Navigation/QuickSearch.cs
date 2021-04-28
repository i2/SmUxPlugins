using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using PluginSystem.API;

namespace Navigation
{
    public class QuickSearch : SearchBase
    {
        public override int Order
        {
            get { return 24; }
        }

        public override string Name
        {
            get
            {
                return @"&Nawigacja\Wyszukiwanie szybkie...";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.E;
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 1 | SuperMemo.VisualizerMode == 0; }
        }

        public override void Execute()
        {
            PrepareSearchCache();

            if (SuperMemo.VisualizerMode == 0)
            {
                SuperMemo.QuitEditing();
            }

            using (var dlgQuickSearch = new DlgQuickSearch())
            {
                dlgQuickSearch.ShowDialog();
            }
        }
    }

    public class Browse : SearchBase
    {
        public override int Order
        {
            get { return 27; }
        }

        public override string Name
        {
            get
            {
                return @"&Nawigacja\Przegl¹danie...";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Shift | Keys.F5;
        }

        public override bool Enabled
        {
            get { return !string.IsNullOrEmpty(SuperMemo.CurrentCoursePath); }
        }

        public override void Execute()
        {
            var courseStructure = new XmlDocument();
            using (Stream stream = SuperMemo.GetStorageInputStream("course.xml"))
            {
                courseStructure.Load(stream);
            }

            var xmlNamespaceManager = new XmlNamespaceManager(courseStructure.NameTable);
            xmlNamespaceManager.AddNamespace("sm", "http://www.supermemo.net/2006/smux");


            var nodes = courseStructure.SelectNodes("//sm:element", xmlNamespaceManager);

            var dataSet = new DataSet();
            var dataTable = dataSet.Tables.Add("items");
            dataTable.Columns.Add("id", typeof (int));
            dataTable.Columns.Add("name", typeof (string));
            dataTable.Columns.Add("type", typeof (string));

            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    dataTable.Rows.Add(new object[]
                                           {
                                               int.Parse(node.Attributes["id"].Value),
                                               node.Attributes["name"].Value,
                                               node.Attributes["type"].Value
                                           });
                }
            }

            using (var dlgBrowse = new DlgBrowse(dataSet))
            {
                dlgBrowse.ShowDialog();
            }
        }
    }
}