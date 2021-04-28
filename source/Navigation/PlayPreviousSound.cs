using System.Collections.Generic;
using System.Windows.Forms;
using mshtml;

namespace Navigation
{
    public class PlayPreviousSound : ClickableElementsNavigation
    {
        public override int Order
        {
            get { return 26; }
        }

        public override string Name
        {
            get
            {
                return @"&Nawigacja\Odtwórz poprzedni dŸwiêk";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.K;
        }

        public override void Execute()
        {
            List<IHTMLElement> clickableElements = GetClickableElements();
            SetElementNumber(-1, clickableElements.Count);

            if (m_LastElement >= 0 && m_LastElement < clickableElements.Count)
            {
                IHTMLElement elementToSelect = clickableElements[m_LastElement];
                ClickElement(elementToSelect);
            }
        }
    }
}