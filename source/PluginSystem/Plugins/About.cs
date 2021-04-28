using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Diagnostic;
using mshtml;
using PluginSystem.API;
using PluginSystem.Configuration;
using PluginSystem.Helpers;
using PluginSystem.Plugin;
using System.IO;

namespace PluginSystem.Plugins
{
    public class About : SuperMemoExtension<PluginSystemConfiguration>, IPageRenderer
    {
        public override string Name
        {
            get { return "SuperMemo UX Plugins - &informacje..."; }
        }

        public override void Execute()
        {
            using (var dlgAboutPlugins = new DlgAboutPlugins())
            {
                dlgAboutPlugins.ShowDialog();
            }
        }

        private static void DisplayPluginsInformation()
        {
            HTMLDocumentClass document = SuperMemo.GetDocument();

            var element = document.getElementById("area5");
            var element2 = document.getElementById("area4");

            if (SuperMemo.VisualizerMode == 2 && element != null && element2 != null && !string.IsNullOrEmpty(element2.innerText) && element2.innerText.StartsWith("SuperMemo UX"))
            {
                if (element.innerHTML.Contains("Plugins"))
                {
                    return;
                }

                if (element.style != null)
                {
                    element.style.height = 50;
                    element.innerHTML += @"</br><b>Plugins</b> wersja " + Assembly.GetExecutingAssembly().GetName().Version;
                }

                if (Configuration.ExtendCourseList)
                {
                    var area0 = document.getElementById("area0");
                    var area02 = document.getElementById("area0_2");
                    
                    if (area0 != null && area02 != null)
                    {
                        area0.style.paddingTop = "10px";
                        area0.style.paddingBottom = "70px";
                        
                        area0.style.top = "100px";
                        area02.style.top = "100px";
                        area0.style.height = "560px";
                        area02.style.height = "64x2px";
                    }
                }

                if (Configuration.ShowAdditionalInformationAboutCourse)
                {
                    AddAdditionalInformation(document);
                }
            }
        }

        private static void AddAdditionalInformation(HTMLDocumentClass document)
        {
            var table = document.getElementsByTagName("table").First<IHTMLElement2>();
            var body = table.getElementsByTagName("tbody").First<IHTMLElement2>();

            int rowNumber = 0;
            foreach (IHTMLElement2 row in body.getElementsByTagName("tr"))
            {
                if (rowNumber == 0)
                {
                    int cellNumber = 0;
                    foreach (IHTMLElement2 cell in row.getElementsByTagName("th"))
                    {
                        if (cellNumber == 4)
                        {
                            IHTMLElement newCell = document.createElement("th");
                            newCell.innerText = "Pozosta³o";

                            cell.insertAdjacentElement("afterEnd", newCell);
                        }
                        cellNumber++;
                    }
                }
                else
                {
                    int cellNumber = 0;
                    foreach (IHTMLElement2 cell in row.getElementsByTagName("td"))
                    {
                        if (cellNumber == 4)
                        {
                            var titleSpan = cell.getElementsByTagName("span").First<IHTMLElement>();
                            
                            string title = titleSpan != null
                                               ? (string) titleSpan.getAttribute("title", 0)
                                               : (string) ((IHTMLElement) cell).getAttribute("title", 0);

                            if (title != null)
                            {
                                string[] numbers = title.Split('/');
                                int doneNumber = int.Parse(numbers[0]);
                                int toBeDone = int.Parse(numbers[1]);

                                IHTMLElement newCell = document.createElement("td");
                                newCell.className = "center";
                                newCell.innerText = string.Format("{0}", toBeDone - doneNumber);
                                cell.insertAdjacentElement("afterEnd", newCell);
                            }
                        }
                        cellNumber++;
                    }
                }
                rowNumber++;
            }

            if (rowNumber > 7)
            {
                var t = document.getElementsByTagName("table").First<IHTMLElement>();
                t.style.fontSize = "18px";
            }
        }

        public void RenderPage()
        {
            //TestLinkify();

            DisplayPluginsInformation();
        }

        private void TestLinkify()
        {
            if ((Control.ModifierKeys & Keys.Control) != 0)
            {
                HTMLDocumentClass document = SuperMemo.GetDocument();
                var element = document.getElementById("question");
                LinkifyElement(document, (IHTMLDOMNode)element);
                var element2 = document.getElementById("answer");
                LinkifyElement(document, (IHTMLDOMNode)element2);
            }
        }

        private void LinkifyElement(HTMLDocumentClass document, IHTMLDOMNode node)
        {
            if (node.hasChildNodes())
            {
                foreach (IHTMLDOMNode child in (IHTMLDOMChildrenCollection) node.childNodes)
                {
                    LinkifyElement(document, child);
                }
            }

            if (node.nodeType == 3)
            {
                var text = (string)node.nodeValue;
                var result = new StringBuilder(text.Length);
                bool first = true;
                var res = new List<IHTMLDOMNode>();
                foreach (var word in text.Split(new[]{' '}))
                {
                    if (!first)
                    {
                        result.Append(" ");
                        res.Add(document.createTextNode(" "));
                    }
                    first = false;

                    if (!string.IsNullOrEmpty(word))
                    {
                        result.AppendFormat(@"<a href=""www.onet.pl"">{0}</a>", word);
                        IHTMLElement htmlElement = document.createElement("a");
                        htmlElement.innerText = word;
                        res.Add((IHTMLDOMNode)htmlElement);
                    }
                }

                foreach (var htmldomNode in res)
                {
                    node.parentNode.insertBefore(htmldomNode, node);
                }
                node.removeNode(true);
            }
        }
    }
}