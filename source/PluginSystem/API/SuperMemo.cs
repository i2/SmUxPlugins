using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using AxSHDocVw;
using mshtml;
using PluginSystem.Helpers;

namespace PluginSystem.API
{
    public static partial class SuperMemo
    {
        /// <summary>
        /// VisualizerMode 2 = NotEdit
        /// </summary>
        public static int VisualizerMode
        {
            get
            {
                const string PROPERTY_NAME = "mode";

                return (int)ReflectionHelper.GetPropertyValue(Visualizer, PROPERTY_NAME); ;
            }
        }

        public static int LearnManagerMode
        {
            get
            {
                const string PROPERTY_NAME = "_mode";

                return (int)ReflectionHelper.GetFieldValue(LearnManager, PROPERTY_NAME); ;
            }
        }

        internal static MenuStrip MainMenuStrip
        {
            get
            {
                const string MENU_STRIP_VARIABLE_NAME = "menuStrip";

                Form mainForm = MainForm;
                return ReflectionHelper.GetFieldValue<MenuStrip>(mainForm, MENU_STRIP_VARIABLE_NAME);
            }
        }

        internal static object LearnManager
        {
            get
            {
                const string FIELD_NAME = "mgr";
                return ReflectionHelper.GetFieldValue(MainForm, FIELD_NAME);
            }
        }

        internal static object Storage
        {
            get
            {
                return ReflectionHelper.GetPropertyValue(LearnManager, "storage");
            }
        }

        internal static object Visualizer
        {
            get
            {
                return ReflectionHelper.GetFieldValue(MainForm, "vis");
            }
        }

        public static void GoToPage(int i)
        {
            if (MainForm.InvokeRequired)
            {
                MainForm.Invoke((Action<int>)GoToPage, i);
            }
            else
            {
                ReflectionHelper.InvokeMethod(LearnManager, "gotoPage", i);
            }
        }

        public static HTMLDocumentClass GetDocument()
        {
            return (HTMLDocumentClass)ReflectionHelper.GetPropertyValue(Visualizer, "doc");
        }

        public static void InvokeJavaScript(string striptBody)
        {
            InvokeScript(striptBody, "javascript");
        }

        public static AxWebBrowser GetWebBrowser()
        {
            return (AxWebBrowser)ReflectionHelper.GetFieldValue(Visualizer, "wb");
        }

        public static string GetPageAsHtml()
        {
            object document = GetDocument();
            object documentElement = ReflectionHelper.GetPropertyValue(document, "documentElement");
            var html = (string)ReflectionHelper.GetPropertyValue(documentElement, "innerHTML");

            return html;
        }

        public static void QuitEditing()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            Type type = assembly.GetType("SMW.visualize.interfaces.IVisualizerHost");
            ReflectionHelper.InvokeMethodFromInterface(MainForm, "QuitEditing", type);
        }

        public static void Play(string fileName, bool hiden)
        {
            object o = ReflectionHelper.GetFieldValue(MainForm, "player");
            string methodName = hiden ? "PlayHidden" : "Play";

            Assembly assembly = Assembly.GetEntryAssembly();
            Type type = assembly.GetType("SMW.forms.player.IPlayer");
            ReflectionHelper.InvokeMethodFromInterface(o, methodName, type, fileName);
        }

        public static void PreviewPage(int pageNr)
        {
            object o = ReflectionHelper.GetFieldValue(MainForm, "browser");
            ReflectionHelper.InvokeMethod(o, "PreviewPage", pageNr);
        }

        public static Form GetPreviewPage(int pageNr)
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            Type type = assembly.GetType("SMW.forms.PreviewPageForm");
            Type visualizerType = assembly.GetType("SMW.visualize.Visualizer");

            return (Form)ReflectionHelper.GetInstanceFromCtor(type, new[] { visualizerType, typeof(int) }, Visualizer, pageNr);
        }

        public static string CurrentCoursePath
        {
            get
            {
                if (LearnManager == null)
                {
                    return null;
                }

                object storage = ReflectionHelper.GetPropertyValue(LearnManager, "storage");

                if (storage == null)
                {
                    return null;
                }

                return (string)ReflectionHelper.GetPropertyValue(storage, "openedPath");
            }
        }

        public static string CurrentItemFile
        {
            get
            {
                object itemBody = ReflectionHelper.GetPropertyValue(LearnManager, "itemBody");
                return (string)ReflectionHelper.InvokeMethod(itemBody, "getXmlName", new Type[] { });
            }
        }

        public static void EditUpdateItemName()
        {
            ReflectionHelper.InvokeMethod(LearnManager, "editAutoPrepareName");
        }

        public static void RefreshCurrentItemPreview()
        {
            TreeView itemsTreeView = GetItemsTreeView();
            TreeNode selectedNode = itemsTreeView.SelectedNode;
            int selectedNodeNumber = (int)selectedNode.Tag;

            TreeNode otherNode = selectedNode.NextNode ?? selectedNode.PrevNode;
            if (otherNode != null)
            {
                GoToPage((int)otherNode.Tag);
                GoToPage(selectedNodeNumber);
            }
        }

        public static ToolStripButton GetMediaAnswerButton()
        {
            return ReflectionHelper.GetFieldValue<ToolStripButton>(MainForm, "sItemAMedia");
        }

        public static ToolStripButton GetMediaQuestionButton()
        {
            return ReflectionHelper.GetFieldValue<ToolStripButton>(MainForm, "sItemQMedia");
        }

        public static ToolStripButton GetInsertMediaButton()
        {
            return ReflectionHelper.GetFieldValue<ToolStripButton>(MainForm, "sInsertSfx");
        }

        public static int InsertNewItem()
        {
            if (MainForm.InvokeRequired)
            {
                return (int)MainForm.Invoke((Func<int>)InsertNewItem);
            }

            ReflectionHelper.InvokeMethod(LearnManager, "editNewInsert", new Type[] { });
            return GetCurentItemNumber();
        }

        public static int AddNewItem()
        {
            if (MainForm.InvokeRequired)
            {
                return (int)MainForm.Invoke((Func<int>)AddNewItem);
            }

            ReflectionHelper.InvokeMethod(LearnManager, "editNewAdd", new Type[] { });
            return GetCurentItemNumber();
        }

        public static int InsertSubItem()
        {
            if (MainForm.InvokeRequired)
            {
                return (int)MainForm.Invoke((Func<int>)InsertSubItem);
            }

            ReflectionHelper.InvokeMethod(LearnManager, "editChangeItemType", 5, true);
            TreeNode parent = GetItemsTreeView().SelectedNode;
            parent.Expand();
            return (int)parent.Nodes[0].Tag;
        }

        public static void SetCurrentItemName(string itemName)
        {
            TreeNode selectedNode = GetItemsTreeView().SelectedNode;
            selectedNode.Text = itemName;
            object o = ReflectionHelper.GetPropertyValue(LearnManager, "itemNode");
            ReflectionHelper.SetFieldValue(o, "_name", itemName);
            object o2 = ReflectionHelper.GetPropertyValue(LearnManager, "course");
            ReflectionHelper.InvokeMethod(o2, "save", new[] { typeof(bool) }, true);
        }

        public static Stream GetStorageInputStream(string name)
        {
            return (Stream)ReflectionHelper.InvokeMethod(Storage, "getInputStream", new object[] { name, true });
        }

        public static void ToggleEditMode()
        {
            var item = (ToolStripMenuItem)MainMenuStrip.Items[3];
            ReflectionHelper.InvokeMethod(item.DropDownItems[4], "OnClick", EventArgs.Empty);
        }

        public static bool GetStorageLocked()
        {
            return (bool)ReflectionHelper.GetPropertyValue(Storage, "locked");
        }

        /// <summary>
        /// Creates component from xmlNode and replace currently selected text with rendered component
        /// </summary>
        /// <param name="xmlNode"></param>
        public static void InsertComponent(XmlNode xmlNode)
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            Type componentType = assembly.GetType("SMW.visualize.components.framework.ComponentFactory");
            object component = ReflectionHelper.InvokeStaticMethod(componentType, "create", new[] {typeof(XmlNode)}, xmlNode);

            object usedComponent = ReflectionHelper.GetPropertyValue(Visualizer, "usedComponents");
            ReflectionHelper.InvokeMethod(usedComponent, "Add", component);

            Type iComponentHostType = assembly.GetType("SMW.visualize.components.framework.IComponentHost");
            var renderedComponent = (string)ReflectionHelper.InvokeMethod(component, "render", new[] { iComponentHostType }, Visualizer);

            ReplaceSelectedTextWithHtml(renderedComponent);
        }

        public static string[] ListFilesForCurrentStorage(string startWith, bool sort)
        {
            return (string[])ReflectionHelper.InvokeMethod(Storage, "listFiles", new[] {typeof(string), typeof(bool)}, startWith, sort);
        }

        public static int UnpackFile(string name, bool throwOnFileExists)
        {
            return (int) ReflectionHelper.InvokeMethod(Storage, "unpackFile", name, throwOnFileExists);
        }

        public static void EditCut()
        {
            ReflectionHelper.InvokeMethod(LearnManager, "editCut");
        }

        public static void EditPaste()
        {
            ReflectionHelper.InvokeMethod(LearnManager, "editPaste");
        }

        public static void GoNext()
        {
            ReflectionHelper.InvokeMethod(LearnManager, "goNext", false);
        }

        public static void GoPrevious()
        {
            ReflectionHelper.InvokeMethod(LearnManager, "goPrev", false);
        }
    }
}
