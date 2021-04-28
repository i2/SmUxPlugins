using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AxSHDocVw;
using Diagnostic;
using mshtml;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Plugin;
using System;
using PluginSystem.Helpers;
using System.IO;

namespace Navigation
{
    public abstract class ClickableElementsNavigation : SuperMemoExtension
    {
        protected static int m_LastElement = -1;
        private static int m_LastItemNumber;

        public override bool Enabled
        {
            get
            {
                //TO DO: we should be more precise and detect if focus is spellPad
                bool noSpellPadInQuestionPart = !SuperMemo.GetPageAsHtml().Contains("spInputQuestion");

                if (CurrentState.IsCourseLoaded &&
                    (CurrentState.InLearningModeInAnswerPart ||
                     ((CurrentState.InLearningModeInQuestionPart && noSpellPadInQuestionPart))))
                {
                    return true;
                }

                return false;
            }
        }

        protected static void SetElementNumber(int delta, int clickableElementsCount)
        {
            if (m_LastItemNumber != SuperMemo.GetCurentItemNumber())
            {
                m_LastItemNumber = SuperMemo.GetCurentItemNumber();
                m_LastElement = -1;
            }

            m_LastElement += delta;

            if (clickableElementsCount != 0)
            {
                m_LastElement = (m_LastElement + clickableElementsCount) % clickableElementsCount;
            }
            else
            {
                m_LastElement = -1;
            }
        }

        protected static void ClickElement(IHTMLElement element)
        {
            Rectangle rectangle = element.GetBrowserDocumentRectagle();
            AxWebBrowser browser = SuperMemo.GetWebBrowser();
            Point screen = browser.PointToScreen(rectangle.GetMiddlePoint());

            //Point previousPosition = Cursor.Position;
            MouseHelper.PerformLeftButtonMouseClick(screen);
            //Cursor.Position = previousPosition;
        }

        protected static List<IHTMLElement> GetClickableElements()
        {
            var clickableElements = new List<IHTMLElement>();
            foreach (IHTMLElement element in SuperMemo.GetDocument().getElementsByTagName("img"))
            {
                var className = element.className;

                if (!string.IsNullOrEmpty(className) && className.StartsWith("sfx"))
                {
                    clickableElements.Add(element);
                }
            }
            return clickableElements;
        }
    }
}