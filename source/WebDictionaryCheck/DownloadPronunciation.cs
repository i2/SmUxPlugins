using System;
using System.IO;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Configuration;
using PluginSystem.Helpers;
using PluginSystem.Plugin;

namespace WebDictionaries
{
    public class DownloadPronunciation : SuperMemoExtension<WebDictionariesConfiguration>
    {
        private static string m_LastDownloadedFile;

        public static string LastDownloadedFile
        {
            get
            {
                return m_LastDownloadedFile;
            }
        }

        public override int Order
        {
            get { return 53; }
        }

        public override string Name
        {
            get
            {
                return @"&S³owniki\Pobierz nagranie wymowy";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.F8;
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
                string localFileName = DownloadPronunciationFile();

                if (SuperMemo.VisualizerMode == 0)
                {
                    SuperMemo.QuitEditing();
                }

                if (Configuration.PlayDownloadedSoundInPlayer)
                {
                    SuperMemo.Play(localFileName, false);
                }
            }
            catch (Exception ex)
            {
                StatusMessage.ShowError("B³¹d podczas pobierania wymowy z serwera", ex);
            }
        }

        public static string DownloadPronunciationFile()
        {
            if (Configuration.DefaultPronunciationSource >= Configuration.PronunciationSources.Count)
            {
                Configuration.DefaultPronunciationSource = 0;
            }

            var pronunciationSource = Configuration.PronunciationSources[Configuration.DefaultPronunciationSource];
            StatusMessage.Show(MessageType.Info, "£¹czenie siê ze stron¹...");

            string selectedWord = SuperMemo.GetSelectedText().Trim().ToLower();
            string url = String.Format(pronunciationSource.Command, selectedWord);
            string soundFileUrl = pronunciationSource.Site + WebHelper.ExtractTextFromHtmlPage(url, pronunciationSource.TextBeforeSoundSource, pronunciationSource.TextAfterSoundSource, pronunciationSource.UserAgent, pronunciationSource.TimeOut);

            StatusMessage.Show(MessageType.Info, "Pobieranie pliku: " + soundFileUrl);

            m_LastDownloadedFile = Path.GetTempFileName();
            WebHelper.DownloadToFile(soundFileUrl, m_LastDownloadedFile);
            StatusMessage.Remove();

            return m_LastDownloadedFile;
        }
    }
}