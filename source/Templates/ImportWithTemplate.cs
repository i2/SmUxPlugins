using System.Windows.Forms;

namespace Templates
{
    public class ImportWithTemplate : TemplateBaseItem
    {
        public override bool Active
        {
            get
            {
                return false;
            }
        }

        public override string Name
        {
            get { return @"Szablony\Importuj z pliku Q&A..."; }
        }

        public override void Execute()
        {
            if (!AreTemplatesInstalled())
            {
                if (
                    MessageBox.Show(
                        "Wykryto, ¿e w danym kursie nie ma jeszcze zdefiniowanych szablonów. \nCzy chcesz teraz zdefiniowaæ przyk³adowe szablony?",
                        "Pytanie", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    InstallDefaultTemplatesInCourse();
                }
            }
            else
            {
                using (var dlgAddTemplateItem = new DlgImportQAWithTemplate())
                {
                    if (dlgAddTemplateItem.ShowDialog() == DialogResult.OK)
                    {
                    
                    }
                }
            }
        }
    }
}