using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using PluginSystem.Common;

namespace Navigation
{
    internal static class DatabaseSearchCache
    {
        public const int MAX_SEARCH_RESULT_COUNT = 50;

        private static Dictionary<string, IList<int>> m_Cache = new Dictionary<string, IList<int>>();
        private static readonly Regex m_Regex = new Regex(@"\<.*\>", RegexOptions.Compiled);
        private static FileSystemWatcher m_FileSystemWatcher;
        private static readonly IList<Exception> m_ExceptionList = new List<Exception>();

        internal static bool RemoveTags
        {
            get; set;
        }

        static DatabaseSearchCache()
        {
            RemoveTags = true;
        }

        internal static IList<SearchResult> Find(string text)
        {
            var result = new List<SearchResult>();

            int i = 0;
            foreach (var key in m_Cache.Keys)
            {
                if (key.Contains(text))
                {
                    foreach (var itemNumber in m_Cache[key])
                    {
                        result.Add(new SearchResult(itemNumber, key));
                        i++;
                        if (i >= MAX_SEARCH_RESULT_COUNT)
                        {
                            return result;
                        }
                    }
                }
            }

            return result;
        }

        internal static void Init(string databasePath, IProgress progress)
        {
            const string SEARCH_PATTERN = "item*.xml";
            m_Cache = new Dictionary<string, IList<int>>();

            string[] files = Directory.GetFiles(databasePath, SEARCH_PATTERN);
            int count = files.Length;
            int i = 0;
            foreach (var s in files)
            {
                try
                {
                    progress.ProgressValue = i * 100 / count;
                    progress.Message = "Indeksowanie pliku " + Path.GetFileName(s);
                    i++;
                    IndexFile(s);
                }
                catch (Exception ex)
                {
                    m_ExceptionList.Add(ex);
                }
            }
            
            if (m_FileSystemWatcher != null)
            {
                m_FileSystemWatcher.Dispose();
            }

            m_FileSystemWatcher = new FileSystemWatcher(databasePath, SEARCH_PATTERN);
            m_FileSystemWatcher.Deleted -= FileDeleted;
            m_FileSystemWatcher.Created -= FileCreated;
            m_FileSystemWatcher.Changed -= FileChanged;
            m_FileSystemWatcher.Renamed -= FileRenamed;
            m_FileSystemWatcher.Deleted += FileDeleted;
            m_FileSystemWatcher.Created += FileCreated;
            m_FileSystemWatcher.Changed += FileChanged;
            m_FileSystemWatcher.Renamed += FileRenamed;
            m_FileSystemWatcher.EnableRaisingEvents = true;
        }

        private static void FileRenamed(object sender, RenamedEventArgs e)
        {
            try
            {
                RemoveCacheForFile(e.OldFullPath);
                RemoveCacheForFile(e.FullPath);
                IndexFile(e.FullPath);
            }
            catch (Exception ex)
            {
                m_ExceptionList.Add(ex);
            }
        }

        private static void FileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                RemoveCacheForFile(e.FullPath);
                IndexFile(e.FullPath);
            }
            catch (Exception ex)
            {
                m_ExceptionList.Add(ex);
            }
        }

        private static void FileCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                IndexFile(e.FullPath);
            }
            catch (Exception ex)
            {
                m_ExceptionList.Add(ex);
            }
        }

        private static void FileDeleted(object sender, FileSystemEventArgs e)
        {
            try
            {
                RemoveCacheForFile(e.FullPath);
            }
            catch (Exception ex)
            {
                m_ExceptionList.Add(ex);
            }
        }

        private static void RemoveCacheForFile(string fileName)
        {
            int itemNumber = GetItemNumber(fileName);

            IList<string> keysToDelete = new List<string>();
            
            foreach (KeyValuePair<string, IList<int>> cacheElem in m_Cache)
            {
                if (cacheElem.Value.Contains(itemNumber))
                {
                    if (cacheElem.Value.Count == 1)
                    {
                        keysToDelete.Add(cacheElem.Key);
                    }
                    else
                    {
                        cacheElem.Value.Remove(itemNumber);
                    }
                }
            }

            foreach (string keyToDelete in keysToDelete)
            {
                m_Cache.Remove(keyToDelete);
            }
        }

        private static void IndexFile(string s)
        {
            int itemNumber = GetItemNumber(s);
            var xmlDocument = new XmlDocument();

            xmlDocument.Load(s);
            XmlNodeList qList = xmlDocument.GetElementsByTagName("question");
            IndexNodes(qList, itemNumber);

            XmlNodeList aList = xmlDocument.GetElementsByTagName("answer");
            IndexNodes(aList, itemNumber);
        }

        private static int GetItemNumber(string s)
        {
            string itemNumberAsString = Path.GetFileNameWithoutExtension(s).Replace("item", string.Empty);
            return int.Parse(itemNumberAsString);
        }

        private static void IndexNodes(XmlNodeList qList, int itemNumber)
        {
            foreach (XmlNode node in qList)
            {
                string text = node.InnerXml;
                if (RemoveTags)
                {
                    text = m_Regex.Replace(text, new MatchEvaluator(OnEvaluator));
                }

                if (!string.IsNullOrEmpty(text))
                {
                    if (m_Cache.ContainsKey(text) && !m_Cache[text].Contains(itemNumber))
                    {
                        m_Cache[text].Add(itemNumber);
                    }
                    else
                    {
                        m_Cache.Add(text, new List<int> {itemNumber});
                    }
                }
            }
        }

        private static string OnEvaluator(Match match)
        {
            if (match.Value.StartsWith("<br"))
            {
                return "¶";
            }

            return match.Result(string.Empty);
        }
    }
}