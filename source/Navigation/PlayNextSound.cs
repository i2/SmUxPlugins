using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using mshtml;
using PluginSystem.API;
using PluginSystem.Plugin;

namespace Navigation
{
    public class PlayNextSound : ClickableElementsNavigation
    {
        public override int Order
        {
            get { return 25; }
        }

        public override string Name
        {
            get
            {
                return @"&Nawigacja\Odtwórz nastêpny dŸwiêk";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.J;
        }

        public override void Execute()
        {
            List<IHTMLElement> clickableElements = GetClickableElements();
            SetElementNumber(1, clickableElements.Count);

            if (m_LastElement >= 0 && m_LastElement < clickableElements.Count)
            {
                IHTMLElement elementToSelect = clickableElements[m_LastElement];
                ClickElement(elementToSelect);
            }
        }
    }

    public abstract class ZoomBase : SuperMemoExtension
    {
        protected static int m_FontSize = 20;

        protected static void SetZoom()
        {
            SuperMemo.AppendToStyleSheet("plugins:zoom", string.Format("div.question {{font-size: {0}px;}} div.answer {{font-size: {0}px;}} div.question input {{font-size: {0}px;}}", m_FontSize));
            SuperMemo.GetWebBrowser().Refresh();
            Thread.Sleep(300);
        }
    }

    public class ZoomIn : ZoomBase
    {
        public override int Order
        {
            get { return 25; }
        }

        public override string Name
        {
            get
            {
                return @"&Wygl¹d\Powiêksz";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Oemplus | Keys.Control;
        }

        public override void Execute()
        {
            m_FontSize++;
            SetZoom();
            return;
       }
    }

    public class ZoomOut : ZoomBase
    {
        public override int Order
        {
            get { return 25; }
        }

        public override string Name
        {
            get
            {
                return @"&Wygl¹d\Zmniejsz";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.OemMinus | Keys.Control;
        }

        public override void Execute()
        {
            m_FontSize--;
            SetZoom();
            return;
        }
    }

}