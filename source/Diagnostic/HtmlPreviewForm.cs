using System;
using System.Drawing;
using System.Windows.Forms;
using AxSHDocVw;
using mshtml;
using PluginSystem.API;
using PluginSystem.Common;

namespace Diagnostic
{
    public partial class HtmlPreviewForm : BaseForm
    {
        private readonly HtmlPreview m_HtmlPreview;
        private readonly IHTMLElement m_Element;
        
        protected internal bool SynchronizingContent
        {
            get; set;
        }
    
        public HtmlPreviewForm(HtmlPreview htmlPreview, IHTMLElement element) : this()
        {
            m_HtmlPreview = htmlPreview;
            m_Element = element;
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            TopMost = true;

            StartPosition = FormStartPosition.Manual;

            CalculateWindowPosition(element);
            ElementToPreview();
        }

        private void ElementToPreview()
        {
            if (!SynchronizingContent)
            {
                using (new ValueRestorer<HtmlPreviewForm, bool>(this, (form, b) => form.SynchronizingContent = b, form => form.SynchronizingContent))
                {
                    SynchronizingContent = true;
                    txbHtmlPreview.Text = m_Element.innerHTML;
                }
            }
        }

        private void PreviewToElement()
        {
            if (!SynchronizingContent)
            {
                using (new ValueRestorer<HtmlPreviewForm, bool>(this, (form, b) => form.SynchronizingContent = b, form => form.SynchronizingContent))
                {
                    SynchronizingContent = true;
                    m_Element.innerHTML = txbHtmlPreview.Text;
                }
            }
        }

        public HtmlPreviewForm()
        {
            InitializeComponent();
        }

        private void CalculateWindowPosition(IHTMLElement element)
        {
            AxWebBrowser browser = SuperMemo.GetWebBrowser();
            Rectangle rectangle = element.GetBrowserDocumentRectagle();
            int leftPosition = browser.Left + rectangle.Left;
            int topPosition = browser.Top + rectangle.Bottom;
            Point point = browser.Parent.PointToScreen(new Point(leftPosition, topPosition));
            SetBounds(point.X, point.Y + Height - ClientSize.Height, rectangle.Width, Math.Max(Math.Min(rectangle.Height, 200), 400));
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        private void TxbHtmlPreviewTextChanged(object sender, EventArgs e)
        {
            PreviewToElement();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_Element != HtmlPreview.GetFirstEditableElement())
            {
                m_HtmlPreview.ToggleHtmlPreview();
            }
            else
            {
                if (!Focused && !ContainsFocus)
                {
                    ElementToPreview();
                }
            }
        }

        private void HtmlPreviewForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                m_HtmlPreview.ToggleHtmlPreview();
            }
        }
    }
}