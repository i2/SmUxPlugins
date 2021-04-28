using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EditorExtension.AutoFormat;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Plugin;

namespace EditorExtension
{
    public class ApplyAutoFormat : SuperMemoExtension<EditorExtensionsConfiguration>
    {
        public override int Order
        {
            get { return 6; }
        }

        public override string Name
        {
            get
            {
                return @"&Edycja zbiorowa\Zastosuj formatowanie automatyczne...";
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
            return Keys.Control | Keys.F1;
        }

        public override void Execute()
        {
            var autoFormatRules = Configuration.AutoFormatRules;
            List<AutoFormatRule> selectedRules = DlgMultiselectItems<AutoFormatRule>.MultiselectItems(
                    "Wybór reguł autoformatowania",
                    "Wybierz reguły autoformatowania do zastosowania",
                    autoFormatRules, 0, rule => rule.Active);

            if (selectedRules != null)
            {
                foreach (var autoFormatRule in selectedRules)
                {
                    autoFormatRule.ApplyToItem(SuperMemo.GetCurentItemNumber());
                }
                
                SuperMemo.RefreshCurrentItemPreview();
            }
        }
    }
}