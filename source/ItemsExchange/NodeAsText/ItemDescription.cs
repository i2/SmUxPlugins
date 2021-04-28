using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Helpers;

namespace ItemsExchange.NodeAsText
{
    public class ItemDescription
    {
        private readonly Dictionary<string, string> m_Media = new Dictionary<string, string>();
        private readonly ImportConfiguration m_ImportConfiguration;

        private string m_Question;
        private string m_Answer;
        private string m_ChapterTitle;
        private string m_LessonTitle;
        private string m_QuestionTitle;
        
        public int Id { get; private set; }
        public int ParentId { get; private set; }
        public string Title { get; private set; }

        private bool HasAnswerSound
        {
            get
            {
                return m_Media.ContainsKey("a.wav") || m_Media.ContainsKey("a.mp3");
            }
        }

        private bool HasQuestionSound
        {
            get
            {
                return m_Media.ContainsKey("q.wav") || m_Media.ContainsKey("q.mp3");
            }
        }

        public ItemDescription(ImportConfiguration importConfiguration,Element element)
        {
            Id = element.Id;
            ParentId = element.ParentId;
            Title = element.Title;
            m_ImportConfiguration = importConfiguration;

            foreach (Component component in element.Components)
            {
                m_ImportConfiguration.AdjustComponent(component);
            }

            PrepareQuestion(element);
            PrepareAnswer(element);
            PrepareLessonTitle(element);
            PrepareChapterTitle(element);
            PrepareQuestionTitle(element);
            PrepareMedia(element);
        }

        private void PrepareQuestionTitle(Element element)
        {
            foreach (Component component in element.Components.Where(component => m_ImportConfiguration.IsQuestionTitleComponent(component)))
            {
                if (component.IsTextType && component.HasText)
                {
                    m_QuestionTitle += GetTextToAppendAsOneLine(m_QuestionTitle, component.XmlText);
                }
            }                        
        }

        private void PrepareChapterTitle(Element element)
        {
            foreach (Component component in element.Components.Where(component => m_ImportConfiguration.IsChapterTitleComponent(component)))
            {
                if (component.IsTextType && component.HasText)
                {
                    m_ChapterTitle += GetTextToAppendAsOneLine(m_ChapterTitle, component.XmlText);
                }
            }                                    
        }

        private void PrepareLessonTitle(Element element)
        {
            foreach (Component component in element.Components.Where(component => m_ImportConfiguration.IsLessonTitleComponent(component)))
            {
                if (component.IsTextType && component.HasText)
                {
                    m_LessonTitle += GetTextToAppendAsOneLine(m_LessonTitle, component.XmlText);
                }
            }                                    
        }

        private void PrepareAnswer(Element element)
        {
            foreach (Component component in element.AnswerComponents.Where(IsNotSpecificComponent))
            {
                var textToAppend = new StringBuilder();

                if (component.IsTextType)
                {
                    if (component.HasText)
                    {
                        string text = component.XmlText;
                        string translation;

                        if (m_ImportConfiguration.Translations != null &&
                            m_ImportConfiguration.Translations.TryGetValue(text, out translation))
                        {
                            const string TEXT_ELEMENT_FORMAT =
                                "<text><sentence>{0}</sentence><translation>{1}</translation></text>";
                            textToAppend.AppendFormat(TEXT_ELEMENT_FORMAT, text, translation);
                        }
                        else
                        {
                            textToAppend.Append(text);
                        }

                        m_Answer += GetTextToAppendAsLine(m_Answer, textToAppend.ToString());
                    }
                }
            }
        }

        private void PrepareQuestion(Element element)
        {
            bool wasMultiChoice = false;

            foreach (Component component in element.QuestionComponents.Where(IsNotSpecificComponent))
            {
                var textToAppend = new StringBuilder();

                if (component.IsTextType)
                {
                    if (component.TestElement != 0)
                    {
                        if (!wasMultiChoice)
                        {
                            var multiChoice = new StringBuilder("<radio>");

                            foreach (Component multiChoiceComponent in element.MultiChoiceComponents)
                            {
                                multiChoice.Append(multiChoiceComponent.TestElement == 1
                                                       ? "<option>"
                                                       : "<option correct=\"true\">");
                                multiChoice.Append(multiChoiceComponent.XmlText);
                                multiChoice.Append("</option>");
                            }

                            multiChoice.Append("</radio>");

                            wasMultiChoice = true;
                            textToAppend.Append(multiChoice);
                        }
                    } 
                    else if (component.HasText)
                    {
                        string text = component.XmlText;
                        string translation;

                        if (m_ImportConfiguration.Translations != null && m_ImportConfiguration.Translations.TryGetValue(text, out translation))
                        {
                            const string TEXT_ELEMENT_FORMAT = "<text><sentence>{0}</sentence><translation>{1}</translation></text>";
                            textToAppend.AppendFormat(TEXT_ELEMENT_FORMAT, text, translation);
                        }
                        else
                        {
                            textToAppend.Append(text);
                        }
                    }
                }

                if (component.IsSpell)
                {
                    textToAppend.AppendFormat("<spellpad correct=\"{0}\" />", component.SpellpadValues);
                }

                if (textToAppend.Length != 0)
                {
                    if (!string.IsNullOrEmpty(m_Question))
                    {
                        m_Question += "<br />\r\n";
                    }
                    
                    m_Question += textToAppend;
                }
            }
        }

        private void PrepareMedia(Element element)
        {
            foreach (Component component in element.SoundComponents)
            {
                if (component.IsAnswerComponent && !HasAnswerSound)
                {
                    string extension = Path.GetExtension(component.SoundFileName);
                    m_Media.Add("a" + extension, component.SoundFileName);
                } 
                else if (component.IsQuestionComponent && !HasQuestionSound)
                {
                    string extension = Path.GetExtension(component.SoundFileName);
                    m_Media.Add("q" + extension, component.SoundFileName);
                }
            }
        }

        public override string ToString()
        {
            return string.Format("Id: {0}\r\nParentId: {1}\r\nQuestion: {2}\r\nAnswer: {3}", Id, ParentId, m_Question, m_Answer);
        }

        public void SaveToFile(string fileName)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            SetNodeValue(xmlDocument, m_Question, "question");
            SetNodeValue(xmlDocument, m_Answer, "answer");
            SetNodeValue(xmlDocument, m_LessonTitle, "lesson-title");
            SetNodeValue(xmlDocument, m_ChapterTitle, "chapter-title");
            SetNodeValue(xmlDocument, m_QuestionTitle, "question-title");

            if (HasAnswerSound)
            {
                SetNodeValue(xmlDocument, "true", "answer-audio");
            }

            if (HasQuestionSound)
            {
                SetNodeValue(xmlDocument, "true", "question-audio");
            }

            xmlDocument.Save(fileName);

            foreach (KeyValuePair<string, string> keyValuePair in m_Media)
            {
                if (!Directory.Exists(SuperMemo.CurrentCourseMediaPath))
                {
                    Directory.CreateDirectory(SuperMemo.CurrentCourseMediaPath);
                }
                
                string mediaType = keyValuePair.Key;
                string itemNumber = Path.GetFileNameWithoutExtension(fileName).Replace("item", string.Empty);
                string mediaFileName = Path.Combine(SuperMemo.CurrentCourseMediaPath, itemNumber + mediaType);
                string sourceFileName = keyValuePair.Value;

                if (File.Exists(mediaFileName))
                {
                    File.Delete(mediaFileName);
                }
                
                if (File.Exists(sourceFileName))
                {
                    File.Copy(sourceFileName, mediaFileName);
                }
                else
                {
                    string warning = string.Format("<p>Nie znaleziono pliku: {0}</p>", sourceFileName);
                    Logger.AddToLog(warning);
                }
            }
        }

        private static void SetNodeValue(XmlDocument xmlDocument, string value, string nodeName)
        {
            if (!string.IsNullOrEmpty(value))
            {
                XmlNodeList qList = xmlDocument.GetElementsByTagName(nodeName);
                XmlNode questionNode = qList[0];
                if (questionNode == null)
                {
                    questionNode = xmlDocument.CreateNode(XmlNodeType.Element, nodeName, null);
                    if (xmlDocument.DocumentElement == null)
                    {
                        throw new InvalidDataException();    
                    }
                    xmlDocument.DocumentElement.AppendChild(questionNode);
                }
                questionNode.InnerXml = value;
            }
        }

        private bool IsNotSpecificComponent(Component component)
        {
            return !m_ImportConfiguration.IsChapterTitleComponent(component) &&
                   !m_ImportConfiguration.IsQuestionTitleComponent(component) &&
                   !m_ImportConfiguration.IsLessonTitleComponent(component);
        }

        private static string GetTextToAppendAsLine(string currentValue, string value)
        {
            string result = string.Empty;
            if (value.Length != 0)
            {
                if (!string.IsNullOrEmpty(currentValue))
                {
                    result += "<br />\r\n";
                }

                result += value;
            }
            
            return result;
        }

        private static string GetTextToAppendAsOneLine(string currentValue, string value)
        {
            string result = string.Empty;
            if (value.Length != 0)
            {
                if (!string.IsNullOrEmpty(currentValue))
                {
                    result += ", ";
                }

                result += value;
            }

            return result;
        }
    }
}