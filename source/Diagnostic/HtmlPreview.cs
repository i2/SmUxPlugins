using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AxSHDocVw;
using mshtml;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Plugin;

namespace Diagnostic
{
    public class HtmlPreview : SuperMemoExtension
    {
        private HtmlPreviewForm m_HtmlPreviewForm;

        public override int Order
        {
            get { return 44; }
        }

        public override string Name
        {
            get
            {
                return @"Diagnostyka\Podgl¹d html...";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Alt | Keys.F9;
        }

        public override void Execute()
        {
            ToggleHtmlPreview();
        }

        internal void ToggleHtmlPreview()
        {
            if (m_HtmlPreviewForm != null)
            {
                m_HtmlPreviewForm.Dispose();
                m_HtmlPreviewForm = null;
            }
            else
            {
                IHTMLElement element = GetFirstEditableElement();

                if (element != null)
                {
                    m_HtmlPreviewForm = new HtmlPreviewForm(this, element);
                    m_HtmlPreviewForm.Show();                    
                }
                else
                {
                    StatusMessage.Show(MessageType.Info, "Nie ma aktywnego pola edycyjnego");
                }
            }
        }

        internal static IHTMLElement GetFirstEditableElement()
        {
            HTMLDocumentClass document = SuperMemo.GetDocument();

            foreach (IHTMLElement element in document.all)
            {
                bool editable = element.IsEditable();
                if (editable)
                {
                    return element;
                }
            }

            return null;
        }
    }
}