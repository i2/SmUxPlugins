using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Linq;
using PluginSystem.API;
using PluginSystem.Configuration;
using PluginSystem.Helpers;
using EditorExtension.Styles;
using System.Windows.Forms;

namespace EditorExtension.AutoFormat
{
    [Serializable]
    public class AutoFormatRule
    {
        private string m_ReplaceText;
        private string m_StyleToApply;

        [Description("Określa, czy reguła formatowania jest domyślnie aktywna")]
        [DisplayName("Aktywna")]
        [Browsable(true)]
        [Category("Ogólne")]
        public bool Active { get; set; }

        [Description("Nazwa reguły formatującej")]
        [DisplayName("Nazwa")]
        [Browsable(true)]
        [Category("Ogólne")]
        public string Name { get; set; }

        [Description("Określa, czy szukany wzorzec ma postać wyrażenia regularnego")]
        [DisplayName("Wyrażenie regularne")]
        [Browsable(true)]
        [Category("Wyszukiwanie")]
        public bool UseRegularExpressions { get; set; }

        [Description("Szukany wzorzec, do którego zostanie zastosowane formatowanie")]
        [DisplayName("Szukany wzorzec")]
        [Browsable(true)]
        [Category("Wyszukiwanie")]
        public string SearchText { get; set; }

        [Description("Zakres wyszukiwania")]
        [DisplayName("Zakres")]
        [Browsable(true)]
        [Category("Wyszukiwanie")]
        public ItemTextElements ItemElement { get; set; }

        [Description("Nazwa stylu, który zostanie zastosowany do wyszukanego wzorca")]
        [DisplayName("Styl do zastosowania")]
        [Browsable(true)]
        [Category("Formatowanie")]
        public string StyleToApply
        {
            get { return m_StyleToApply; }
            set
            {
                m_StyleToApply = value;
                if (!string.IsNullOrEmpty(m_StyleToApply))
                {
                    ReplaceText = string.Empty;
                }
            }
        }

        [Description("Tekst, który ma zastąpić znaleziony wzorzec")]
        [DisplayName("Tekst do wstawienia")]
        [Browsable(true)]
        [Category("Formatowanie")]
        public string ReplaceText
        {
            get { return m_ReplaceText; }
            set
            {
                m_ReplaceText = value;
                if (!string.IsNullOrEmpty(m_ReplaceText))
                {
                    StyleToApply = string.Empty;
                }
            }
        }

        public AutoFormatRule()
        {
            ItemElement = ItemTextElements.All;
        }

        public void ApplyToItem(int itemNumber)
        {
            try
            {
                if (!Active)
                {
                    throw new InvalidOperationException(string.Format("Rule '{0}' is not active", Name));
                }

                Style styleToApply = ConfigurationManager.GetConfiguration<EditorExtensionsConfiguration>().Styles.GetStyleByName(StyleToApply);
                XDocument xDocument = SuperMemo.GetItemDefinitionAsXDocument(itemNumber);


                string searchText = UseRegularExpressions ? "(?<TextToApplyStyle>" + SearchText + ")" : SearchText;

                string replaceText;
                
                if (ReplaceByText())
                {
                    replaceText = ReplaceText;
                    ApplyToXmlDocument(xDocument, searchText, replaceText);
                }
                else
                {
                    string styleLeft = styleToApply.GetHtmlLeftPart();
                    string styleLeftEscaped = styleLeft.ToRegexpEscapedString();
                    string styleRight = styleToApply.GetHtmlRightPart();
                    string styleRightEscaped = styleRight.ToRegexpEscapedString();
                    replaceText = UseRegularExpressions ? styleLeft + "${TextToApplyStyle}" + styleRight : styleToApply.ToHtml(SearchText);

                    ApplyToXmlDocument(xDocument, searchText, replaceText);

                    //simple protection before multi usage
                    searchText = UseRegularExpressions ? styleLeftEscaped + styleLeftEscaped + "(?<TextToApplyStyle>" + SearchText + ")" + styleRightEscaped + styleRightEscaped : styleToApply.ToHtml(styleToApply.ToHtml(SearchText));
                    ApplyToXmlDocument(xDocument, searchText, replaceText);
                }

                xDocument.Save(SuperMemo.GetItemDefinitionFullFileName(itemNumber));
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format("Wyjątek podczas wykonywania reguły: {0}treść wyjątku: {1}", Name, exception));
            }
        }

        private bool ReplaceByText()
        {
            return !string.IsNullOrEmpty(ReplaceText);
        }

        private void ApplyToXmlDocument(XDocument xDocument, string searchText, string replaceText)
        {
            if ((ItemElement & ItemTextElements.Answer) != 0)
            {
                XmlHelper.ReplaceDefinitionElement(xDocument, "answer", UseRegularExpressions, searchText, replaceText);
            }

            if ((ItemElement & ItemTextElements.ChapterTitle) != 0)
            {
                XmlHelper.ReplaceDefinitionElement(xDocument, "chapter-title", UseRegularExpressions, searchText, replaceText);
            }

            if ((ItemElement & ItemTextElements.LessonTitle) != 0)
            {
                XmlHelper.ReplaceDefinitionElement(xDocument, "lesson-title", UseRegularExpressions, searchText, replaceText);
            }

            if ((ItemElement & ItemTextElements.Question) != 0)
            {
                XmlHelper.ReplaceDefinitionElement(xDocument, "question", UseRegularExpressions, searchText, replaceText);
            }

            if ((ItemElement & ItemTextElements.QuestionTitle) != 0)
            {
                XmlHelper.ReplaceDefinitionElement(xDocument, "question-title", UseRegularExpressions, searchText, replaceText);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}