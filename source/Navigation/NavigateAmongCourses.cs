using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AxSHDocVw;
using Diagnostic;
using mshtml;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Plugin;

namespace Navigation
{
    //public class NavigateAmongCourses : SuperMemoExtension
    //{
    //    private int i = 0;
    //    private static IHTMLElement m_LastElement = null;
    //    private static object m_LastElementOriginalBackgroundColor = null;

    //    public override bool Enabled
    //    {
    //        get
    //        {
    //            return true;
    //        }
    //    }

    //    public override int Order
    //    {
    //        get { return 25; }
    //    }

    //    public override string Name
    //    {
    //        get
    //        {
    //            return @"&Nawigacja\Odtwórz nastêpny dŸwiêk";
    //        }
    //    }

    //    public override Keys GetShortcut()
    //    {
    //        return Keys.Down;
    //    }

    //    private static void GoToElement(IHTMLElement element)
    //    {
    //        if (m_LastElement != null)
    //        {
    //            m_LastElement.style.backgroundColor = m_LastElementOriginalBackgroundColor;
    //        }

    //        Rectangle rectagle = element.GetBrowserDocumentRectagle();
    //        AxWebBrowser browser = SuperMemo.GetWebBrowser();
    //        Point screen = browser.PointToScreen(rectagle.GetMiddlePoint());

    //        Cursor.Position = screen;
    //        m_LastElement = element;
    //        m_LastElementOriginalBackgroundColor = element.style.backgroundColor;
    //        //element.style.backgroundColor = "red";
    //        //element.click();
    //    }

    //    private static List<IHTMLElement> GetClickableElements()
    //    {
    //        var clickableElements = new List<IHTMLElement>();
            
    //        foreach (IHTMLElement element in SuperMemo.GetDocument().getElementsByTagName("a"))
    //        {
    //            var className = element.className;

    //            if (!string.IsNullOrEmpty(className) && className.StartsWith("plan"))
    //            {
    //                clickableElements.Add(element);
    //            }
    //        }

    //        foreach (IHTMLElement element in SuperMemo.GetDocument().getElementsByTagName("div"))
    //        {
    //            var className = element.className;

    //            if (!string.IsNullOrEmpty(className) && className.StartsWith("sfx"))
    //            {
    //                clickableElements.Add(element);
    //            }
    //        }

    //        return clickableElements;
    //    }
 
    //    public override void Execute()
    //    {
    //        List<IHTMLElement> clickableElements = GetClickableElements();
    //        IHTMLElement element = DeterminNextElement(Keys.Down, clickableElements);
    //        GoToElement(element);
    //        //SetElementNumber(1, clickableElements.Count);

    //        //if (m_LastElement >= 0 && m_LastElement < clickableElements.Count)
    //        //{
    //        //    IHTMLElement elementToSelect = clickableElements[m_LastElement];
    //        //}
    //    }

    //    private IHTMLElement DeterminNextElement(Keys key, IList<IHTMLElement> clickableElements)
    //    {
    //        if (key == Keys.Down)
    //        {
    //            i++;
    //            return clickableElements[i++ % clickableElements.Count];
    //        }

    //        return clickableElements[i++ % clickableElements.Count];
    //    }
    //}
}