using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using PluginSystem.Helpers;

namespace ItemsExchange.NodeAsText
{
    public static class ImportAsText
    {
        public static IList<Element> LoadElements(string fileName)
        {
            var result = new List<Element>();

            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                var streamReader = new StreamReader(fileStream, Encoding.Default);
                do
                {
                    string line = streamReader.ReadLine();

                    if (line.StartsWith(Element.ELEMENT_BEGIN_LABEL))
                    {
                        int id = int.Parse(line.Substring(Element.ELEMENT_BEGIN_LABEL.Length));
                        result.Add(Element.Deserialize(id, streamReader));
                    }

                } while (!streamReader.EndOfStream);

            }

            return result;
        }

        public static Dictionary<string, string> LoadTranslations(string fileName)
        {
            var result = new Dictionary<string, string>();

            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                var streamReader = new StreamReader(fileStream, Encoding.Default);
                
                do
                {
                    string line = streamReader.ReadLine();

                    const string SEPARATOR1 = "*****";

                    if (line.StartsWith(SEPARATOR1))
                    {
                        string key = string.Empty;
                        string value = string.Empty;

                        line = string.Empty;
                        do
                        {
                            if (!string.IsNullOrEmpty(key))
                            {
                                key += StringHelper.XmlString("<br />");
                            }
                            key += StringHelper.XmlString(line);
                            line = streamReader.ReadLine();
                        } while (!line.StartsWith(SEPARATOR1));

                        line = string.Empty;
                        do
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                value += StringHelper.XmlString("<br />");
                            }
                            value += StringHelper.XmlString(line);
                            line = streamReader.ReadLine();
                        } while (!line.StartsWith(SEPARATOR1));
                        
                        line = streamReader.ReadLine();
                        if (string.IsNullOrEmpty(line))
                        {
                            result[key] = value;
                        }
                        else
                        {
                            throw new DataException("Nie poprawny format pliku z t³umaczeniami");
                        }
                    }
                } while (!streamReader.EndOfStream);
            }

            return result;
        }

        internal static int ComponentComparison(Component x, Component y)
        {
            if (x.Location.Y == y.Location.Y)
            {
                return x.Location.X.CompareTo(y.Location.X);
            }

            return x.Location.Y.CompareTo(y.Location.Y);
        }

    }
}