using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Helpers;
using PluginSystem.Plugin;

namespace EditorExtension
{
    public class MultiEdit : SuperMemoExtension
    {
        private IList<int> m_SelectedItems;
        private string m_Lesson = null;
        private string m_Chapter;
        private string m_Command;
        private bool m_ItemNameBasedOnQuestion;
        private string m_ItemNameAsValue;
        private int? m_TemplateNumber;
        private string m_SearchText;
        private string m_ReplaceText;
        private bool m_ReplaceInAnswer;
        private bool m_ReplaceInQuestion;
        private bool m_ReplaceInQuestionTitle;
        private bool m_ReplaceInLessonTitle;
        private bool m_ReplaceInChapterTitle;
        private bool m_UseRegularExpressions;
        private static MultiEdit m_LastInstance;

        public static MultiEdit LastInstance
        {
            get { return m_LastInstance; }
        }

        public MultiEdit()
        {
            m_LastInstance = this;
        }

        public override int Order
        {
            get { return 5; }
        }

        public override string Name
        {
            get
            {
                return @"&Edycja zbiorowa\Wykonaj edycjê zbiorow¹...";
            }
        }

        public override bool Enabled
        {
            get
            {
                return SuperMemo.GetItemsTreeView() != null && !string.IsNullOrEmpty(SuperMemo.CurrentCoursePath) && SuperMemo.VisualizerMode == 1;
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Alt | Keys.M;
        }

        public override void Execute()
        {
            using (var dlgMultiItemEdit = new DlgMultiItemEdit())
            {
                dlgMultiItemEdit.Lesson = m_Lesson;
                dlgMultiItemEdit.Chapter = m_Chapter;
                dlgMultiItemEdit.Command = m_Command;
                dlgMultiItemEdit.TemplateNumber = m_TemplateNumber;
                dlgMultiItemEdit.ItemNameBasedOnQuestion = m_ItemNameBasedOnQuestion;
                dlgMultiItemEdit.ItemNameAsValue = m_ItemNameAsValue;

                dlgMultiItemEdit.SearchText = m_SearchText;
                dlgMultiItemEdit.ReplaceText = m_ReplaceText;
                dlgMultiItemEdit.ReplaceInAnswer = m_ReplaceInAnswer;
                dlgMultiItemEdit.ReplaceInQuestion = m_ReplaceInQuestion;
                dlgMultiItemEdit.ReplaceInQuestionTitle = m_ReplaceInQuestionTitle;
                dlgMultiItemEdit.ReplaceInLessonTitle = m_ReplaceInLessonTitle;
                dlgMultiItemEdit.ReplaceInChapterTitle = m_ReplaceInChapterTitle;
                dlgMultiItemEdit.UseRegularExpressions = m_UseRegularExpressions;

                if (dlgMultiItemEdit.ShowDialog() == DialogResult.OK)
                {
                    m_SelectedItems = dlgMultiItemEdit.GetSelectedItems();
                    m_Lesson = dlgMultiItemEdit.Lesson;
                    m_Chapter = dlgMultiItemEdit.Chapter;
                    m_Command = dlgMultiItemEdit.Command;
                    m_TemplateNumber = dlgMultiItemEdit.TemplateNumber;
                    m_ItemNameBasedOnQuestion = dlgMultiItemEdit.ItemNameBasedOnQuestion;
                    m_ItemNameAsValue = dlgMultiItemEdit.ItemNameAsValue;

                    m_SearchText = dlgMultiItemEdit.SearchText;
                    m_ReplaceText = dlgMultiItemEdit.ReplaceText;
                    m_ReplaceInAnswer = dlgMultiItemEdit.ReplaceInAnswer;
                    m_ReplaceInQuestion = dlgMultiItemEdit.ReplaceInQuestion;
                    m_ReplaceInQuestionTitle = dlgMultiItemEdit.ReplaceInQuestionTitle;
                    m_ReplaceInLessonTitle = dlgMultiItemEdit.ReplaceInLessonTitle;
                    m_ReplaceInChapterTitle = dlgMultiItemEdit.ReplaceInChapterTitle;
                    m_UseRegularExpressions = dlgMultiItemEdit.UseRegularExpressions;
                    
                    if (m_SelectedItems.Count>0)
                    {
                        DlgProgress.Execute("Edycja zbiorowa jednostek", MultiEditProcess);
                        //force refresh
                        SuperMemo.GoToPage(m_SelectedItems[m_SelectedItems.Count-1]);
                    }
                }
            }
        }

        internal void MultiEditProcess(IProgress progress)
        {
            int i = 0;
            foreach (int itemNumber in m_SelectedItems)
            {
                i++;
                if (progress.Cancel)
                {
                    break;
                }

                progress.Message = string.Format("Modyfikacja jednostki o ID {0}", itemNumber);
                progress.ProgressValue = 100*i/m_SelectedItems.Count; 
                    
                XmlDocument xmlDocument = SuperMemo.GetItemDefinition(itemNumber);

                SetElementValues(xmlDocument);
                ReplaceTextForItem(xmlDocument);

                xmlDocument.Save(SuperMemo.GetItemDefinitionFullFileName(itemNumber));

                SetItemNodeName(itemNumber, xmlDocument);
            }
        }

        private void SetElementValues(XmlDocument xmlDocument)
        {
            if (!string.IsNullOrEmpty(m_Lesson))
            {
                XmlHelper.SetElementByTagName(xmlDocument, "lesson-title", m_Lesson);
            }

            if (!string.IsNullOrEmpty(m_Chapter))
            {
                XmlHelper.SetElementByTagName(xmlDocument, "chapter-title", m_Chapter);
            }
                
            if (!string.IsNullOrEmpty(m_Command))
            {
                XmlHelper.SetElementByTagName(xmlDocument, "question-title", m_Command);
            }

            if (m_TemplateNumber.HasValue)
            {
                XmlHelper.SetElementByTagName(xmlDocument, "template-id", m_TemplateNumber.Value.ToString());
            }
        }

        private void ReplaceTextForItem(XmlDocument xmlDocument)
        {
            if (!string.IsNullOrEmpty(m_SearchText))
            {
                if (m_ReplaceInAnswer)
                {
                    XmlHelper.ReplaceDefinitionElement(xmlDocument, "answer", m_UseRegularExpressions, m_SearchText, m_ReplaceText);
                }

                if (m_ReplaceInChapterTitle)
                {
                    XmlHelper.ReplaceDefinitionElement(xmlDocument, "chapter-title", m_UseRegularExpressions, m_SearchText, m_ReplaceText);
                }

                if (m_ReplaceInLessonTitle)
                {
                    XmlHelper.ReplaceDefinitionElement(xmlDocument, "lesson-title", m_UseRegularExpressions, m_SearchText, m_ReplaceText);
                }

                if (m_ReplaceInQuestion)
                {
                    XmlHelper.ReplaceDefinitionElement(xmlDocument, "question", m_UseRegularExpressions, m_SearchText, m_ReplaceText);
                }

                if (m_ReplaceInQuestionTitle)
                {
                    XmlHelper.ReplaceDefinitionElement(xmlDocument, "question-title", m_UseRegularExpressions, m_SearchText, m_ReplaceText);
                }
            }
        }

        private void SetItemNodeName(int itemNumber, XmlDocument xmlDocument)
        {
            if (m_ItemNameBasedOnQuestion || m_ItemNameAsValue != null)
            {
                SuperMemo.GoToPage(itemNumber);
                if (m_ItemNameBasedOnQuestion)
                {
                    XmlNodeList qList = xmlDocument.GetElementsByTagName("question");
                    if (qList.Count > 0)
                    {
                        XmlNode questionNode = qList[0];
                        string newName = questionNode.InnerText;
                        StringHelper.RemoveFromText(@"< /br>", string.Empty);
                        if (!string.IsNullOrEmpty(newName))
                        {
                            SuperMemo.SetCurrentItemName(newName.Substring(0, Math.Min(30, newName.Length)));
                        }
                    }
                }

                if (!string.IsNullOrEmpty(m_ItemNameAsValue))
                {
                    SuperMemo.SetCurrentItemName(m_ItemNameAsValue);
                }
            }
        }

        public void SetSelectedItems(int number)
        {
            if (m_SelectedItems == null)
            {
                m_SelectedItems = new List<int>();
            }
            else
            {
                m_SelectedItems.Clear();
            }

            m_SelectedItems.Add(number);
        }
    }
}