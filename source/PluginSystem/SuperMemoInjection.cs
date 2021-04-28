using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AxSHDocVw;
using mshtml;
using PluginSystem.Configuration;
using PluginSystem.Helpers;
using PluginSystem.Plugin;
using PluginSystem.Plugins;
using PluginSystem.Properties;
using PluginSystem.API;
using PluginSystem.Common;
using System.Threading;
using System.Security.AccessControl;
using Diagnostic;

namespace PluginSystem
{
    public class SuperMemoInjection
    {
        private static ToolStripMenuItem m_PluginsMenu;

        private static readonly IList<SuperMemoExtension> m_Plugins = new List<SuperMemoExtension>();

        public static void Install()
        {
            //Debugger.Launch();
            MenuStrip menuStrip = SuperMemo.MainMenuStrip;

            m_PluginsMenu = new ToolStripMenuItem(Resources.SuperMemoInjection_Install_W_tyczki);
            m_PluginsMenu.DropDownOpening += PluginsMenuOnDropDownOpening;
            m_PluginsMenu.DropDownClosed += PluginsMenuOnDropDownClosed;
            menuStrip.Items.Insert(menuStrip.Items.Count - 3, m_PluginsMenu);

            RegisterAllPlugins();
            StatusMessage.InstallMessageFilter();

            AxWebBrowser axWebBrowser = SuperMemo.GetWebBrowser();
            axWebBrowser.DownloadComplete -= WebBrowserDownloadComplete;
            axWebBrowser.DownloadComplete += WebBrowserDownloadComplete;
            NotifyPageRenderers();
            
            TreeView treeView = SuperMemo.GetItemsTreeView();
            treeView.EnabledChanged -= OnItemTreeViewEnabledChanged;
            treeView.EnabledChanged += OnItemTreeViewEnabledChanged;
            
            if (treeView.Enabled)
            {
                NotifyCourseOpened();
            }

            //SuperMemo.MainForm.Deactivate -= MainFormOnDeactivate;
            //SuperMemo.MainForm.Activated -= MainFormOnActivated;
            //SuperMemo.MainForm.Deactivate += MainFormOnDeactivate;
            //SuperMemo.MainForm.Activated += MainFormOnActivated;
        }

        //private static Point? m_LpPoint;

        //private static void MainFormOnActivated(object sender, EventArgs eventArgs)
        //{
        //    if (m_LpPoint != null)
        //    {
        //        Point previousCursorPosition = Cursor.Position;
        //        MouseHelper.PerformLeftButtonMouseClick(m_LpPoint.Value);
        //        Cursor.Position = previousCursorPosition;
        //    }
        //}

        //static void MainFormOnDeactivate(object sender, EventArgs e)
        //{
        //    SuperMemo.GetWebBrowser().ResetCursor();
        //    m_LpPoint = WebBrowserCaretHelper.GetCaretPosition();
        //}

        private static void OnItemTreeViewEnabledChanged(object sender, EventArgs args)
        {
            var treeView = (TreeView)sender;
            
            if (treeView.Enabled)
            {
                NotifyCourseOpened();
            }
        }

        internal static void RegisterAllPlugins()
        {
            ClearPreviouslyInstalledPlugins();

            RegisterPlugin(new PluginManager());
            RegisterPlugin(new EditConfiguration());
            m_PluginsMenu.DropDownItems.Add(new ToolStripSeparator());

            IList<SuperMemoExtension> pluginsToRegister = GetPluginsList();
            foreach (var plugin in pluginsToRegister.OrderBy(extension => extension.Order))
            {
                RegisterPlugin(plugin);
            }

            m_PluginsMenu.DropDownItems.Add(new ToolStripSeparator());
            RegisterPlugin(new About());
        }

        private static void RegisterPlugin(SuperMemoExtension extension)
        {
            string pluginName;
            m_Plugins.Add(extension);
            ToolStripMenuItem destinationMenu = GetDestinationMenu(extension, out pluginName);

            var item = (ToolStripMenuItem)destinationMenu.DropDownItems.Add(pluginName, extension.Image, MenuItemClickEventHandler);
            try
            {
                item.ShortcutKeys = extension.GetShortcut();
            }
            catch (Exception)
            {
                //some shortcuts are not handled by MenuStrip - we will fix it single-handedly
                item.ShortcutKeyDisplayString = extension.GetShortcut().ToString();
            }
            item.Tag = extension;
        }

        private static void ClearPreviouslyInstalledPlugins()
        {
            var toDispose = new List<IDisposable>();
            foreach (IDisposable item in m_PluginsMenu.DropDownItems)
            {
                toDispose.Add(item);
            }

            foreach (IDisposable d in toDispose)
            {
                d.Dispose();    
            }

            m_Plugins.Clear();
        }

        private static void PluginsMenuOnDropDownClosed(object sender, EventArgs e)
        {
            var toolStripMenuItem = (ToolStripMenuItem)sender;
            EnableAllSubitems(toolStripMenuItem);
        }

        private static void WebBrowserDownloadComplete(object sender, EventArgs e)
        {
            NotifyPageRenderers();
        }

        private static void NotifyPageRenderers()
        {
            foreach (var extension in m_Plugins)
            {
                var renderer = extension as IPageRenderer;
                if (renderer!= null)
                {
                    renderer.RenderPage();
                }
            }
        }

        private static void NotifyCourseOpened()
        {
            foreach (var extension in m_Plugins)
            {
                var renderer = extension as ICourseOpenNotification;
                if (renderer != null)
                {
                    renderer.CourseOpened();
                }
            }
        }

        private static IList<SuperMemoExtension> GetPluginsList()
        {
            IList<SuperMemoExtension> pluginsToRegister = new List<SuperMemoExtension>();

            if (string.IsNullOrEmpty(ConfigurationManager.GetConfiguration<PluginSystemConfiguration>().Plugins))
            {
                const string STARTING_PLUGIN_LIST = @"Bookmarks.dll;Diagnostic.dll;EditorExtension.dll;ItemsExchange.dll;Navigation.dll;WebDictionaries.dll;Templates.dll;MediaFiles.dll";
                ConfigurationManager.GetConfiguration<PluginSystemConfiguration>().Plugins = STARTING_PLUGIN_LIST;
            }

            string[] plugins = ConfigurationManager.GetConfiguration<PluginSystemConfiguration>().Plugins.Split(';');

            foreach (var plugin in plugins)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(@"Plugins\" + plugin);

                    if (assembly.GetName().Version != Assembly.GetExecutingAssembly().GetName().Version)
                    {
                        string message = string.Format("Uwaga, niezgodnoœæ wersji pliku {0} - aplikacja mo¿e nie pracowaæ prawid³owo", assembly.GetName().Name);
                        MessageBox.Show(message);
                    }
                    Type[] types = assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (type.IsSubclassOf(typeof (SuperMemoExtension)) && !type.IsAbstract)
                        {
                            var instance = (SuperMemoExtension) ReflectionHelper.GetInstanceFromDefaultCtor(type);
                            if (instance.Active)
                            {
                                pluginsToRegister.Add(instance);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    throw;
                }
            }
            return pluginsToRegister;
        }

        private static ToolStripMenuItem GetDestinationMenu(SuperMemoExtension extension, out string menuItemName)
        {
            ToolStripMenuItem result = m_PluginsMenu;
            string[] menus = extension.Name.Split('\\');
            menuItemName = menus[menus.Length - 1];
            
            for (int i = 0; i < menus.Length - 1; i++)
            {
                var submenu = (ToolStripMenuItem)result.DropDownItems.FindExactly(menus[i]);
                result = submenu ?? (ToolStripMenuItem)result.DropDownItems.Add(menus[i]);
            }

            return result;
        }

        private static void MenuItemClickEventHandler(object sender, EventArgs args)
        {
            var toolStripMenuItem = (ToolStripMenuItem) sender;
            var plugin = (SuperMemoExtension)toolStripMenuItem.Tag;
            if (plugin.Enabled)
            {
                try
                {
                    plugin.Execute();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                string message = string.Format("Opcja {0} nie jest aktywna w bie¿¹cym kontekœcie", toolStripMenuItem.Text);
                StatusMessage.ShowInfo(message);
            }
        }

        private static void PluginsMenuOnDropDownOpening(object sender, EventArgs e)
        {
            var toolStripMenuItem = (ToolStripMenuItem)sender;
            CalculateEnabledItems(toolStripMenuItem);
        }

        private static void CalculateEnabledItems(ToolStripDropDownItem toolStripMenuItem)
        {
            foreach (var item in toolStripMenuItem.DropDownItems)
            {
                var menuItem = item as ToolStripMenuItem;
                if (menuItem != null)
                {
                    var plugin = menuItem.Tag as SuperMemoExtension;
                    
                    if (plugin != null)
                    {
                        try
                        {
                            menuItem.Enabled = plugin.Enabled;

                        }
                        catch (Exception)
                        {
                            menuItem.Enabled = false;
                            const string TEXT = "(Wyst¹pi³ b³¹d)";

                            if (!menuItem.Text.EndsWith(TEXT))
                            {
                                menuItem.Text += TEXT;
                            }
                        }
                    }
                    
                    if (menuItem.DropDownItems.Count > 0)
                    {
                        CalculateEnabledItems(menuItem);
                    }
                }
            }
        }

        private static void EnableAllSubitems(ToolStripDropDownItem toolStripMenuItem)
        {
            foreach (var item in toolStripMenuItem.DropDownItems)
            {
                var menuItem = item as ToolStripMenuItem;
                if (menuItem != null)
                {
                    menuItem.Enabled = true;
                    if (menuItem.DropDownItems.Count > 0)
                    {
                        EnableAllSubitems(menuItem);
                    }
                }
            }
        }

        internal static bool ExecuteShortcut(Keys keyData)
        {
            foreach (SuperMemoExtension extension in m_Plugins)
            {
                if (extension.GetShortcut() == keyData)
                {
                    if (extension.Enabled)
                    {
                        extension.Execute();
                        return true;
                    }
                }
            }

            return false;
        }
    }
}