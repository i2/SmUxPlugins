using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using mshtml;
using PluginSystem.Helpers;

namespace PluginSystem.API
{
    public static partial class SuperMemo
    {
        public static void InvokeScript(string scriptBody, string scriptType)
        {
            HTMLWindow2Class window = GetHtmlWindow();
            window.execScript(scriptBody, scriptType);
        }

        public static Form MainForm
        {
            get
            {
                if (Application.OpenForms.Count <= 0)
                {
                    return null;
                }

                return Application.OpenForms[0];
            }
        }

        public static HTMLWindow2Class GetHtmlWindow()
        {
            HTMLDocumentClass document = GetDocument();
            return (HTMLWindow2Class)document.parentWindow;
        }

        public static string CurrentItemFileWithoutExtension
        {
            get
            {
                string currentItemFile = CurrentItemFile;

                if (currentItemFile == null)
                {
                    return null;
                }

                return Path.GetFileNameWithoutExtension(currentItemFile);
            }
        }

        public static string CurrentCourseMediaPath
        {
            get
            {
                return Path.Combine(CurrentCoursePath, @"Override\Media");
            }
        }

        public static string CurrentCourseOverridePath
        {
            get
            {
                string mediaPath = Path.Combine(CurrentCoursePath, @"Override");
                return mediaPath;
            }
        }

        public static string GetSelectedText()
        {
            HTMLDocumentClass document = GetDocument();
            IHTMLSelectionObject selection = document.selection;
            var range = (IHTMLTxtRange)selection.createRange();
            return range.text;
        }

        public static string GetSelectedHtml()
        {
            HTMLDocumentClass document = GetDocument();
            IHTMLSelectionObject selection = document.selection;
            var range = (IHTMLTxtRange)selection.createRange();
            return range.htmlText;
        }

        public static void ReplaceSelectedText(string text)
        {
            HTMLDocumentClass document = GetDocument();
            IHTMLSelectionObject selection = document.selection;
            var range = (IHTMLTxtRange)selection.createRange();
            range.text = text;
        }

        public static void ReplaceSelectedTextWithHtml(string htmlText)
        {
            HTMLDocumentClass document = GetDocument();
            IHTMLSelectionObject selection = document.selection;
            var range = (IHTMLTxtRange)selection.createRange();
            range.pasteHTML(htmlText);
        }

        public static void SelectionExecCommand(string command, string parameter)
        {
            HTMLDocumentClass document = GetDocument();
            IHTMLSelectionObject selection = document.selection;
            var range = (IHTMLTxtRange)selection.createRange();
            range.execCommand(command, false, parameter);
        }

        public static void ExecCommand(string command, bool p1, object p2)
        {
            var document = GetDocument();
            document.execCommand(command, p1, p2);
        }

        public static TreeView GetItemsTreeView()
        {
            if (MainForm == null)
            {
                return null;
            }

            foreach (Control control in MainForm.Controls)
            {
                var view = control as TreeView;
                
                if (view != null)
                {
                    return view;
                }
            }

            return null;
        }

        public static TreeView GetOperationTreeView()
        {
            if (MainForm == null)
            {
                return null;
            }

            TreeView itemsTreeView = GetItemsTreeView();
            var toSearch = new List<Control>();
            toSearch.Add(MainForm);

            do
            {
                foreach (Control control in toSearch[0].Controls)
                {
                    var view = control as TreeView;

                    if (view != null && view != itemsTreeView)
                    {
                        return view;
                    }

                    if (control.Controls.Count>0)
                    {
                        toSearch.Add(control);
                    }
                }

                toSearch.RemoveAt(0);

            } while (toSearch.Count > 0);
            
            return null;
        }

        public static int GetCurentItemNumber()
        {
            if (MainForm.InvokeRequired)
            {
                return (int)MainForm.Invoke((Func<int>)GetCurentItemNumber);
            }

            ReflectionHelper.InvokeMethod(LearnManager, "lazyTimer_Tick", new[] { typeof(object), typeof(EventArgs) }, null, EventArgs.Empty);

            TreeView view = GetItemsTreeView();
            if (view == null)
            {
                return -1;
            }

            TreeNode node = view.SelectedNode;
            if (node == null)
            {
                return -1;
            }

            if (!(node.Tag is int))
            {
                return -1;
            }

            return (int)node.Tag;
        }
    
        public static string CurrentItemFullPathFileName
        {
            get
            {
                return Path.Combine(CurrentCourseOverridePath, CurrentItemFile);
            }
        }

        public static string ItemNumberToDefinitionFileName(int itemNumber)
        {
            const string FORMAT = "item{0}.xml";
            string numberFormat = itemNumber <= 99999 ? "00000" : "0000000";
            return String.Format(FORMAT, itemNumber.ToString(numberFormat));
        }

        public static string GetItemDefinitionFullFileName(int templatesDirectoryItemNumber)
        {
            return Path.Combine(CurrentCourseOverridePath ,ItemNumberToDefinitionFileName(templatesDirectoryItemNumber));
        }

        public static XmlDocument GetItemDefinition(int itemNumber)
        {
            string itemDefinitionFullFileName = GetItemDefinitionFullFileName(itemNumber);
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(itemDefinitionFullFileName);
            return xmlDocument;
        }

        public static XDocument GetItemDefinitionAsXDocument(int itemNumber)
        {
            string itemDefinitionFullFileName = GetItemDefinitionFullFileName(itemNumber);
            return XDocument.Load(itemDefinitionFullFileName);
        }

        public static string GetFirstFreeMediaFileName(string extension)
        {
            for (char c ='b'; c<='p'; c++)
            {
                string fileName = String.Format("{0}{1}" + extension, CurrentItemFileWithoutExtension.Replace("item", String.Empty), c);
                string fullFileName = Path.Combine(CurrentCourseMediaPath, fileName);

                if (!File.Exists(fullFileName))
                {
                    return fileName;
                }
            }

            return null;
        }

        public static string GetStyleSheetFileName()
        {
            var htmlDocument = SuperMemo.GetDocument();
            var element = (IHTMLElement2)htmlDocument.body.parentElement;

            var htmlElement = element.getElementsByTagName("BASE").First<IHTMLElement>();
            var baseDirectory = (string)htmlElement.getAttribute("href", 0);

            var htmlElement2 = element.getElementsByTagName("LINK").First<IHTMLElement>();
            var cssStylesFileName = (string)htmlElement2.getAttribute("href", 0);

            return Path.Combine(baseDirectory, cssStylesFileName);
        }

        public static void AppendToStyleSheet(string id, string text)
        {
            var beginMarker = string.Format("{0}/*{1}(begin)*/", Environment.NewLine, id);
            var endMarker = string.Format("{0}/*{1}(end)*/", Environment.NewLine, id);
            
            var styleSheetFileName = GetStyleSheetFileName();
            
            string contentAsString = StringHelper.FileToString(styleSheetFileName);
            var content = new StringBuilder(contentAsString);

            int beginMarkerPos = contentAsString.IndexOf(beginMarker);
            int endMarkerPos = contentAsString.IndexOf(endMarker);

            if (beginMarkerPos != -1 && endMarkerPos != -1)
            {
                int position = beginMarkerPos + beginMarker.Length;
                content.Remove(position, endMarkerPos - position);
                content.Insert(position, Environment.NewLine + text);
            }
            else
            {
                content.Append(beginMarker);
                content.Append(Environment.NewLine + text);
                content.Append(endMarker);
            }

            StringHelper.StringToFile(content.ToString(), styleSheetFileName);
        }
    }
}