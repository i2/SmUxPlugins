using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Plugin;

namespace Diagnostic
{
    public class EditItemSourceInExternalEditor : SuperMemoExtension
    {
        public override int Order
        {
            get { return 42; }
        }

        public override string Name
        {
            get
            {
                return @"Diagnostyka\Edytuj jednostkê w zewnêtrznym edytorze...";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Shift | Keys.Alt | Keys.Control | Keys.F9;
        }

        public override bool Enabled
        {
            get
            {
                return SuperMemo.CurrentCoursePath != null;
            }
        }

        public override void Execute()
        {
            string fullFileName = Path.Combine(Path.Combine(SuperMemo.CurrentCoursePath, "Override"), SuperMemo.CurrentItemFile);
            if (File.Exists(fullFileName))
            {
                Process.Start("notepad.exe", fullFileName);
            }
            else
            {
                MessageBox.Show("Nie uda³o siê odnaleŸæ pliku, prawdopodobnie kurs oryginalny");
            }
        }
    }
}