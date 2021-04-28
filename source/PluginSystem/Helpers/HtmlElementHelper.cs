using System.Drawing;
using mshtml;

namespace PluginSystem.Common
{
    public static class HtmlElementHelper
    {
        public static Rectangle GetClientRectangle(this IHTMLElement element)
        {
            var element2 = (IHTMLElement2)element;
            var rectangle = new Rectangle(element2.clientLeft, element2.clientTop, element2.clientWidth, element2.clientHeight);

            if (rectangle.Width == 0)
            {
                rectangle.Width = element.offsetWidth;
            }
            if (rectangle.Height == 0)
            {
                rectangle.Height = element.offsetHeight;
            }

            return rectangle;
        }

        public static Rectangle GetBoundingClientRectangle(this IHTMLElement element)
        {
            var element2 = (IHTMLElement2)element;
            IHTMLRect rect = element2.getBoundingClientRect();
            return new Rectangle(rect.left, rect.top, rect.right - rect.left , rect.bottom = rect.top);
        }

        public static Rectangle GetBrowserDocumentRectagle(this IHTMLElement element)
        {
            Rectangle result = element.GetClientRectangle();
            IHTMLElement e = element;

            while (e != null)
            {
                result.Offset(e.offsetLeft, e.offsetTop);
                e = e.offsetParent;
            }

            var document = (HTMLDocumentClass)element.document;
            var body = (HTMLBodyClass)document.body;
            result.Offset(-body.scrollLeft, -body.scrollTop);
                        
            return result;
        }

        public static bool IsEditable(this IHTMLElement element)
        {
            var element2 = (IHTMLElement3)element;
            return element2.contentEditable == "true";
        }
    }
}