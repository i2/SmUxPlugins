using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Plugin;

namespace EditorExtension
{
    public class RepeatLastMultiEdit : SuperMemoExtension
    {
        public override int Order
        {
            get { return 5; }
        }

        public override string Name
        {
            get
            {
                return @"&Edycja zbiorowa\Zastosuj ostatni¹ edycjê zbiorow¹...";
            }
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
            return Keys.Control | Keys.F3;
        }

        public override void Execute()
        {
            int curentItemNumber = SuperMemo.GetCurentItemNumber();

            if (MessageBox.Show("Czy powtórzyæ ostatni¹ edycjê zbiorow¹ dla bie¿¹cej jednostki?", "Pytanie", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                MultiEdit.LastInstance.SetSelectedItems(curentItemNumber);
                DlgProgress.Execute("Edycja zbiorowa jednostek", MultiEdit.LastInstance.MultiEditProcess);
                SuperMemo.RefreshCurrentItemPreview();
            }
        }
    }
}