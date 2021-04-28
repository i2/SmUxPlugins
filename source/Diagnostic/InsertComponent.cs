using System;
using System.Windows.Forms;
using System.Xml;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Plugin;

namespace Diagnostic
{
    public class InsertComponent : SuperMemoExtension
    {
        public override int Order
        {
            get { return 49; }
        }

        private string m_PreviousComponent = "<spellpad correct=\"trait\" />";

        public override string Name
        {
            get
            {
                return @"Diagnostyka\Wstaw komponent...";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Shift | Keys.C;
        }

        public override void Execute()
        {
            try
            {
                if (DlgInputBox.GetText("Wstawianie komonentu", "Komponent: ", ref m_PreviousComponent))
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(m_PreviousComponent);

                    foreach (XmlNode childNode in xmlDocument.ChildNodes)
                    {
                        SuperMemo.InsertComponent(childNode);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udane wstawienie komponentu, b³¹d: " + ex);
            }
        }
    }
}