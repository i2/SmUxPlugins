using System;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Plugin;

namespace WebDictionaries
{
    public class CheckSelectedText : SuperMemoExtension
    {
        public override int Order
        {
            get { return 50; }
        }

        private static string m_LastUsedDictionary;

        public static string LastUsedDictionary
        {
            get { return m_LastUsedDictionary; }
        }

        public override string Name
        {
            get
            {
                return @"&Słowniki\Sprawdź w słowniku...";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Shift | Keys.F6;
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
            using (var dlgDictionaryCheck = new DlgDictionaryCheck())
            {
                if (dlgDictionaryCheck.ShowDialog() == DialogResult.OK)
                {
                    string commandText = dlgDictionaryCheck.CommandText;
                    FireProgram(commandText);
                }
            }
        }

        protected void FireProgram(string commandText)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                m_LastUsedDictionary = commandText;
                string s = SuperMemo.GetSelectedText();
                string command = string.Format(commandText, s);
                Process.Start(command);
            }
        }
    }
}
