using System.IO;
using PluginSystem.Plugin;
using System.Windows.Forms;

namespace MediaFiles
{
    public abstract class RecordMediaFileBase : SuperMemoExtension
    {
        public override void Execute()
        {
            using (var dlgRecording = new DlgRecording())
            {
                if (dlgRecording.ShowDialog() == DialogResult.OK)
                {
                    AttachRecordedFile(dlgRecording.OutputFileName, dlgRecording.Extension);
                }
            }
        }

        protected abstract void AttachRecordedFile(string fileName, string mediaExtension);

        protected static bool FileNotExistOrShouldOverwrite(string destinationFile)
        {
            if (!File.Exists(destinationFile))
            {
                return true;
            }
            
            if (MessageBox.Show("Plik z nagraniem pytania ju¿ istnieje. Nadpisaæ? ", "Pytanie", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.Delete(destinationFile);
                return true;
            }
            
            return false;
        }
    }
}