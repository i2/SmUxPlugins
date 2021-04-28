using System;
using System.Media;
using System.Windows.Forms;
using EditorExtension.Styles;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Helpers;
using PluginSystem.Plugin;

namespace WebDictionaries
{
    public class GetIPATranscription : SuperMemoExtension<WebDictionariesConfiguration>
    {
        private string m_Pronunciation;

        public override int Order
        {
            get { return 52; }
        }

        public override string Name
        {
            get
            {
                return @"&S³owniki\Pobierz zapis wymowy (IPA)";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.F7;
        }

        public override bool Enabled
        {
            get
            {
                return !string.IsNullOrEmpty(SuperMemo.GetSelectedText());
            }
        }

        public override void Execute()
        {
            try
            {
                DlgProgress.Execute("Pobieranie wymowy", LookUp);

                if (Configuration.SimplifyIPA)
                {
                    m_Pronunciation = m_Pronunciation.Replace(new string((char)720, 1), ":");
                }

                StyleCollection currentStyles = ManageStyles.Styles;
                if (currentStyles != null)
                {
                    if (!string.IsNullOrEmpty(Configuration.PronunciationStyle))
                    {
                        Style pronunciationStyle = currentStyles.Find(style => style.Name.Equals(Configuration.PronunciationStyle));
                        if (pronunciationStyle != null)
                        {
                            m_Pronunciation = pronunciationStyle.ToHtml(m_Pronunciation);
                        }
                    }
                }

                Clipboard.SetText(m_Pronunciation, TextDataFormat.UnicodeText);
                StatusMessage.Show(MessageType.Info, "Wymowa zosta³a skopiowana do schowka: " + m_Pronunciation);
                SystemSounds.Beep.Play();
            }
            catch(Exception ex)
            {
                StatusMessage.ShowError("B³¹d podczas pobierania wymowy z serwera", ex);
            }
        }

        private void LookUp(IProgress progress)
        {
            progress.Undefined = true;
            const string COMMAND = "http://www.macmillandictionary.com/dictionary/british/{0}";

            const string BEFORE = "<span class=\"SEP\" context=\"PRON-before\">\u00A0\u002F</span>";
            const string AFTER = @"<span class=""SEP"" context=""PRON-after"">/</span>";

            progress.Message = "£¹czenie siê ze stron¹...";

            string selectedWord = SuperMemo.GetSelectedText().Trim().ToLower();
            string url = String.Format(COMMAND, selectedWord);

            m_Pronunciation = WebHelper.ExtractTextFromHtmlPage(url, BEFORE, AFTER);
            m_Pronunciation = string.Format("/{0}/", m_Pronunciation);
        }
    }
}