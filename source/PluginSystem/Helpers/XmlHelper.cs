using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace PluginSystem.Helpers
{
    public static class XmlHelper
    {
        public static XmlNode CreateNode(string nodeName)
        {
            var xmlDocument = new XmlDocument();
            return xmlDocument.CreateNode(XmlNodeType.Element, nodeName, null); 
        }

        public static void AddAttribute(this XmlNode node, string attributeName, string value)
        {
            XmlAttribute attribute = node.Attributes.Append(node.OwnerDocument.CreateAttribute(attributeName));
            attribute.Value = value;
        }

        public static void SetElementByTagName(XmlDocument xmlDocument, string elementName, string elementValue)
        {
            XmlNodeList qList = xmlDocument.GetElementsByTagName(elementName);
            XmlNode nodeToChange = qList[0];
            if (nodeToChange == null)
            {
                nodeToChange = xmlDocument.CreateNode(XmlNodeType.Element, elementName, null);
                xmlDocument.DocumentElement.AppendChild(nodeToChange);
            }
            nodeToChange.InnerXml = elementValue;
        }

        private static void MarkReplacementsInSubNodes(IDictionary<string, string> toReplace, XmlNode element, bool useRegularExpression, string searchText, string replaceText)
        {
            if (!String.IsNullOrEmpty(element.Value))
            {
                string marker = Guid.NewGuid().ToString();
                string targetText = useRegularExpression ? Regex.Replace(element.Value, searchText, replaceText) : element.Value.Replace(searchText, replaceText);

                element.Value = marker;
                toReplace.Add(marker, targetText);
            }

            foreach (XmlNode xmlElement in element.ChildNodes)
            {
                MarkReplacementsInSubNodes(toReplace, xmlElement, useRegularExpression, searchText, replaceText);
            }
        }

        public static XElement GetXElement(this XmlNode node)
        {
            var xDoc = new XDocument();
            using (XmlWriter xmlWriter = xDoc.CreateWriter())
                node.WriteTo(xmlWriter);

            return xDoc.Root;
        }

        public static XmlNode GetXmlNode(this XElement element)
        {
            using (XmlReader xmlReader = element.CreateReader())
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }

        public static void ReplaceDefinitionElement(XDocument xDocument, string elementName, bool useRegularExpression, string searchText, string replaceText)
        {
            IEnumerable<XNode> xNodes = xDocument.Nodes();

            ReplaceDefinitionElementInNodes(xNodes, elementName, useRegularExpression, searchText, replaceText);
        }

        private static void ReplaceDefinitionElementInNodes(IEnumerable<XNode> xNodes, string elementName, bool useRegularExpression, string searchText, string replaceText)
        {
            foreach (XNode xNode in xNodes)
            {
                var xElement = xNode as XElement;
                if (xElement != null)
                {
                    ReplaceDefinitionElementInNodes(xElement.Nodes(), elementName, useRegularExpression, searchText, replaceText);

                    if (xElement.Name.LocalName == elementName)
                    {
                        string normalizedNodeText = xElement.ToString(SaveOptions.DisableFormatting);
                        string newNode = !useRegularExpression ? normalizedNodeText.Replace(searchText, replaceText) : Regex.Replace(normalizedNodeText, searchText, replaceText);
                        object content = XElement.Parse(newNode);
                        xElement.ReplaceWith(content);
                    }
                }
            }
        }

        public static void ReplaceDefinitionElement(XmlDocument xmlDocument, string elementName, bool useRegularExpression, string searchText, string replaceText)
        {
            XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName(elementName);
            XmlNode nodeToChange = elementsByTagName[0];
            if (nodeToChange == null)
            {
                nodeToChange = xmlDocument.CreateNode(XmlNodeType.Element, elementName, null);
                xmlDocument.DocumentElement.AppendChild(nodeToChange);
            }
            
            if (!String.IsNullOrEmpty(nodeToChange.InnerXml))
            {
                var replacements = new Dictionary<string, string>();
                MarkReplacementsInSubNodes(replacements, nodeToChange, useRegularExpression, searchText, replaceText);

                string s = nodeToChange.InnerXml;

                foreach (var replacement in replacements)
                {
                    s = s.Replace(replacement.Key, replacement.Value);
                }

                nodeToChange.InnerXml = s;
            }
        }
    }
}
