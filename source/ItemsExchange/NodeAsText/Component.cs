using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PluginSystem.Helpers;
using PluginSystem.Common;

namespace ItemsExchange.NodeAsText
{
    public class Component
    {
        internal const string BEGIN_LABEL = "Begin Component #";
        private const string END_LABEL = "End Component #";
        private const string TYPE_LABEL = "Type=";
        private const string TEXT_LABEL = "Text=";
        private const string RTF_POS = "RTFPos=";
        private const string RTF_LEN = "RTFLen=";
        private const string RTF_FILE = "RTFFile=";
        private const string TEXT_POS = "TextPos=";
        private const string TEXT_LEN = "TextLen=";
        private const string TEXT_FILE = "TextFile=";
        private const string TEST_LABEL = "TestElement=";
        private const string DISPLAY_AT_LABEL = "DisplayAt=";
        private const string COORDINATES_LABEL = "Cors=(";
        private const string SOUND_FILE_LABEL = "SoundFile=";
        private const string HTML_FILE_LABEL = "HTMFile=";

        public int Id { get; set; }
        public Element Element { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }

        public string XmlText
        {
            get
            {
                return StringHelper.XmlString(Text);
            }
        }

        public int TestElement { get; set; }
        public int DisplayAt { get; set; }
        public Point Location { get; set; }
        public Size Size { get; set; }
        public int RtfPos { get; set; }
        public int RtfLen { get; set; }
        public string RtfFile { get; set; }
        public int TextLen { get; set; }
        public int TextPos { get; set; }
        public string TextFile { get; set; }
        protected static string HtmlFileName { get; set; }
        public string SoundFileName { get; set; }

        public bool IsTextType
        {
            get
            {
                return Type == "Text" || Type == "RTF" || Type == "HTML";
            }
        }

        public bool HasText
        {
            get
            {
                return !string.IsNullOrEmpty(Text);
            }
        }

        public bool IsQuestionComponent
        {
            get
            {
                return (DisplayAt & 0x08) != 0;
            }
        }

        public bool IsAnswerComponent
        {
            get
            {
                return !IsQuestionComponent;
            }
        }

        public bool IsSpell
        {
            get { return Type == "Spell"; }
        }

        public string SpellpadValues
        {
            get
            {
                string xmlText = XmlText;
                string[] values = xmlText.Split('/');
                var result = new StringBuilder(xmlText.Length);
                
                foreach (string value in values)
                {
                    if (result.Length!=0)
                    {
                        result.Append('|');
                    }

                    result.Append(value.Trim().Replace("\"", "&quot;"));
                }

                return result.ToString();
            }
        }

        public bool IsSound
        {
            get
            {
                return Type == "Sound";
            }
        }

        public static Component Deserialize(int id, StreamReader streamReader)
        {
            var component = new Component();

            string line;
            do
            {
                line = streamReader.ReadLine().Trim();

                if (line.StartsWith(TYPE_LABEL))
                {
                    component.Type = line.Substring(TYPE_LABEL.Length);
                }
                else if (line.StartsWith(HTML_FILE_LABEL))
                {
                    HtmlFileName = line.Substring(HTML_FILE_LABEL.Length);
                    using (MemoryStream stream = StreamHelper.FileToMemoryStream(HtmlFileName))
                    {
                        string htmlText = StreamHelper.StreamToString(stream);
                        component.Text = StringHelper.PlainTextToHtml(StringHelper.RemoveHtmlTags(htmlText));
                    }
                }
                else if (line.StartsWith(SOUND_FILE_LABEL))
                {
                    component.SoundFileName = StringHelper.PlainTextToHtml(line.Substring(SOUND_FILE_LABEL.Length));
                }
                else if (line.StartsWith(TEXT_LABEL))
                {
                    component.Text = StringHelper.PlainTextToHtml(line.Substring(TEXT_LABEL.Length));
                }
                else if (line.StartsWith(RTF_FILE))
                {
                    component.RtfFile = line.Substring(RTF_FILE.Length);
                }
                else if (line.StartsWith(RTF_POS))
                {
                    component.RtfPos = int.Parse(line.Substring(RTF_POS.Length));
                }
                else if (line.StartsWith(RTF_LEN))
                {
                    component.RtfLen = int.Parse(line.Substring(RTF_LEN.Length));

                    if (File.Exists(component.RtfFile))
                    {
                        using (MemoryStream stream = StreamHelper.FileToMemoryStream(component.RtfFile, component.RtfPos - 1, component.RtfLen))
                        {
                            using (var rtb = new RichTextBox())
                            {
                                rtb.LoadFile(stream, RichTextBoxStreamType.RichText);
                                component.Text = StringHelper.PlainTextToHtml(rtb.Text);
                            }
                        }
                    }
                    else
                    {
                        Logger.AddToLog("<p>Brak pliku: " + component.RtfFile + ", zaimportowane dane mog¹ byæ niekompletne</p>");
                    }
                }
                else if (line.StartsWith(TEXT_FILE))
                {
                    component.TextFile = line.Substring(TEXT_FILE.Length);
                }
                else if (line.StartsWith(TEXT_POS))
                {
                    component.TextPos = int.Parse(line.Substring(TEXT_POS.Length));
                }
                else if (line.StartsWith(TEXT_LEN))
                {
                    component.TextLen = int.Parse(line.Substring(TEXT_LEN.Length));
                    if (File.Exists(component.TextFile))
                    {
                        using (MemoryStream stream = StreamHelper.FileToMemoryStream(component.TextFile, component.TextPos - 1, component.TextLen - 6))
                        {
                            string text = StreamHelper.StreamToString(stream, Encoding.Default);
                            component.Text = StringHelper.PlainTextToHtml(text);
                        }
                    }
                    else
                    {
                        Logger.AddToLog("<p>Brak pliku: " + component.TextFile + ", zaimportowane dane mog¹ byæ niekompletne</p>");
                    }
                }
                else if (line.StartsWith(TEST_LABEL))
                {
                    component.TestElement = int.Parse(line.Substring(TEST_LABEL.Length));
                }
                else if (line.StartsWith(DISPLAY_AT_LABEL))
                {
                    component.DisplayAt = int.Parse(line.Substring(DISPLAY_AT_LABEL.Length));
                }
                else if (line.StartsWith(COORDINATES_LABEL))
                {
                    string s = line.Substring(COORDINATES_LABEL.Length);
                    string s2 = line.Substring(COORDINATES_LABEL.Length, s.Length - 1);
                    string[] coors = s2.Split(',');
                    component.Location = new Point(int.Parse(coors[0]), int.Parse(coors[1]));
                    component.Size = new Size(int.Parse(coors[2]), int.Parse(coors[3]));
                }
            } while (!streamReader.EndOfStream && !line.Equals(END_LABEL + id));
            
            return component;
        }
    }
}