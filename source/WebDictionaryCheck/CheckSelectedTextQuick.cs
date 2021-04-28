using System.Windows.Forms;

namespace WebDictionaries
{
    public class CheckSelectedTextQuick : CheckSelectedText
    {
        public override int Order
        {
            get { return 51; }
        }

        public override string Name
        {
            get
            {
                return @"&S�owniki\Sprawd� w ostatnio u�ywanym s�owniku...";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.F6;
        }

        public override void Execute()
        {
            if (!string.IsNullOrEmpty(LastUsedDictionary))
            {
                FireProgram(LastUsedDictionary);        
            }
            else
            {
                base.Execute();
            }
        }
    }
}