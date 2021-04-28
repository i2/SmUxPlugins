using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Plugin;
using System.Collections.Generic;

namespace Diagnostic
{
    public class OpenLockedBaseForEdit : SuperMemoExtension
    {
        public override int Order
        {
            get { return 102; }
        }

        public override string Name
        {
            get
            {
                return @"Inne\Tryb edycji (dla bazy komercyjnej)";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Shift | Keys.F9;
        }

        public override bool Enabled
        {
            get
            {
                return SuperMemo.GetStorageLocked();
            }
        }

        public override void Execute()
        {
            SuperMemo.ToggleEditMode();
        }
    }

    public class BrowseStorageContent : SuperMemoExtension
    {
        public override int Order
        {
            get { return 103; }
        }

        public override string Name
        {
            get
            {
                return @"Diagnostyka\Przegl¹daj zawartoœæ SmPak...";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Shift | Keys.F11;
        }

        public override bool Enabled
        {
            get
            {
                return CurrentState.IsCourseLoaded;
            }
        }

        public override void Execute()
        {
            string[] storage = SuperMemo.ListFilesForCurrentStorage(string.Empty, true);
            var files = new List<string>(storage);

            string selectedFile = DlgSelectItem<string>.SelectItem("Pliki archiwum SmPack", "Wybierz plik", files, 0);
            if (!string.IsNullOrEmpty(selectedFile))
            {
                SuperMemo.UnpackFile(selectedFile, false);
            }
        }
    }
}