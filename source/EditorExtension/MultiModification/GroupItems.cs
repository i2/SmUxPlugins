using System;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Plugin;

namespace EditorExtension.MultiModification
{
    public class GroupItems : SuperMemoExtension
    {
        public override int Order
        {
            get { return 5; }
        }

        public override string Name
        {
            get
            {
                return @"&Edycja zbiorowa\Grupuj...";
            }
        }

        public override bool Enabled
        {
            get
            {
                return SuperMemo.GetItemsTreeView() != null && !string.IsNullOrEmpty(SuperMemo.CurrentCoursePath) && SuperMemo.VisualizerMode == 1;
            }
        }

        public override bool Active
        {
            get { return false; }
        }

        public override void Execute()
        {
            int insertSubItem = SuperMemo.InsertSubItem();

            SuperMemo.GoToPage(insertSubItem);
            SuperMemo.GoNext();
            SuperMemo.EditCut();
            SuperMemo.GoPrevious();
            SuperMemo.EditPaste();
        }
    }
}