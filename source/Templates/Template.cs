using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using PluginSystem.API;

namespace Templates
{
    public class Template
    {
        private static readonly Regex m_RegExp = new Regex( "\\[.+?\\]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled );

        private List<TemplateFieldInfo> m_FieldsInfo;

        private readonly TreeNode m_Node;

        public int ItemNumber
        {
            get
            {
                return (int) m_Node.Tag;
            }
        }
        
        public Template(TreeNode node)
        {
            m_Node = node;
        }

        public override string ToString()
        {
            return m_Node.Text;
        }

        public IList<TemplateFieldInfo> TagsInfo
        {
            get
            {
                if (m_FieldsInfo == null)
                {
                    m_FieldsInfo = new List<TemplateFieldInfo>();

                    var xmlDocument = SuperMemo.GetItemDefinition(ItemNumber);
                    m_FieldsInfo.AddRange(BuildTagsInfo(xmlDocument.ChildNodes));
                }

                return m_FieldsInfo;
            }
        }

        private static IList<TemplateFieldInfo> BuildTagsInfo(XmlNodeList nodes)
        {
            var result = new List<TemplateFieldInfo>();
            
            if (nodes.Count > 0)
            {
                foreach (XmlNode node in nodes)
                {
                    if (!(node is XmlText) && node.ChildNodes.Count>0)
                    {
                        result.AddRange(BuildTagsInfo(node.ChildNodes));
                    }
                    else
                    {
                        string nodeText = node.InnerText;
                        
                        if (!string.IsNullOrEmpty(nodeText))
                        {
                            result.AddRange(BuildTagsInfo(nodeText));
                        }
                    }
                }
            }
            
            return result;
        }

        private static IList<TemplateFieldInfo> BuildTagsInfo(string s)
        {
            var result = new List<TemplateFieldInfo>();

            var matchCollection = m_RegExp.Matches(s);

            foreach (Match match in matchCollection)
            {
                result.Add( new TemplateFieldInfo(match.Value));
            }

            return result;
        }

        public void ResolveTemplate(XmlDocument xmlDocument, IDictionary<string, string> fields)
        {
            ResolveTemplate(xmlDocument.ChildNodes, new TemplateResolvingData(this, fields));
        }

        private static void ResolveTemplate(XmlNodeList nodes, TemplateResolvingData templateResolvingData)
        {
            if (nodes.Count > 0)
            {
                foreach (XmlNode node in nodes)
                {
                    if (node.HasChildNodes)
                    {
                        ResolveTemplate(node.ChildNodes, templateResolvingData);
                    }
                    
                    if (IsTemplateAwareNode(node))
                    {
                        string nodeText = node.InnerXml;

                        if (!string.IsNullOrEmpty(nodeText))
                        {
                            node.InnerXml = ResolveTemplate(nodeText, templateResolvingData);
                        }
                    }
                }
            }
        }

        private static bool IsTemplateAwareNode(XmlNode node)
        {
            var templateAwareNodes = new List<string>
                                         {
                                             "lesson-title",
                                             "chapter-title",
                                             "question-title",
                                             "question",
                                             "answer",
                                             "template-id",
                                         };
            
            return templateAwareNodes.Contains(node.Name);
        }

        private static string ResolveTemplate(string text, TemplateResolvingData templateResolvingData)
        {
            foreach (var pair in templateResolvingData.Fields)
            {
                string fieldDefinition = pair.Key;
                string fieldValue = pair.Value;

                text = text.Replace(fieldDefinition, fieldValue);

                //replace fields inside strings
                if (fieldDefinition.Contains("\""))
                {
                    fieldDefinition = fieldDefinition.Replace("\"", "&quot;");
                    text = text.Replace(fieldDefinition, fieldValue);
                }
            }

            text = templateResolvingData.RemoveAllBlocks(text);
            
            return text;
        }

        public string GetDefinitionFileName()
        {
            return SuperMemo.GetItemDefinitionFullFileName(ItemNumber);
        }
    }
}