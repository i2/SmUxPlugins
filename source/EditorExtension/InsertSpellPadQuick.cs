using System;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using PluginSystem.API;
using PluginSystem.Helpers;
using PluginSystem.Plugin;

namespace EditorExtension
{
    public class InsertSpellPadQuick : SuperMemoExtension
    {
        public override string Name
        {
            get
            {
                return @"Inne\Zastosuj kontroler piso&wni";
            }
        }

        public override int Order
        {
            get { return 101; }
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 0 && !string.IsNullOrEmpty(SuperMemo.GetSelectedText()); }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Alt | Keys.F2;
        }

        public override void Execute()
        {
            string selectedText = SuperMemo.GetSelectedText();
            selectedText = selectedText.Replace("\r\n", "|");
            
            XmlNode node = XmlHelper.CreateNode("spellpad");
            node.AddAttribute("correct", selectedText);
            SuperMemo.InsertComponent(node);
        }
    }
}