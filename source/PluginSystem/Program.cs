using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;
using System.Windows.Forms;
using PluginSystem.Win32;

[assembly: System.Security.AllowPartiallyTrustedCallersAttribute()]

namespace PluginSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //InstallByReflection();
            //return;

            Process superMemoProcess = GetSuperMemoProcess();

            if (superMemoProcess == null)
            {
                superMemoProcess = TryToStartSuperMemo();
                Thread.Sleep(4000);
            }

            if (superMemoProcess == null)
            {
                MessageBox.Show("Najpierw uruchom program SuperMemoUX");
                return;
            }

            IntPtr superMemoHandle = superMemoProcess.MainWindowHandle;
            ManagedInjector.Injector.Inject(superMemoHandle, Assembly.GetEntryAssembly(), typeof(SuperMemoInjection).FullName, "Install");
        }

        private static Process TryToStartSuperMemo()
        {
            string currentDirectory = Environment.CurrentDirectory;

            const string PLUGINS_FOLDER = @"\Plugins";

            if (!string.IsNullOrEmpty(currentDirectory) && currentDirectory.EndsWith(PLUGINS_FOLDER))
            {
                Environment.CurrentDirectory = currentDirectory.Substring(0, currentDirectory.Length - PLUGINS_FOLDER.Length);
            }
            
            const int MAX_ATTEMPTS = 15;

            if (!File.Exists("supermemo.exe"))
            {
                return null;
            }

            Process.Start(@"supermemo.exe");

            int i = 0;
            Process superMemoProcess;

            do
            {
                Thread.Sleep(200);
                superMemoProcess = GetSuperMemoProcess();

            } while (superMemoProcess == null && i++ < MAX_ATTEMPTS);

            return superMemoProcess;
        }

        private static Process GetSuperMemoProcess()
        {
            const string SUPER_MEMO_PROCESS_NAME = "supermemo";

            Process[] processes = Process.GetProcessesByName(SUPER_MEMO_PROCESS_NAME);
            
            if (processes.Length == 0)
            {
                return null;
            }

            return processes[0];
        }

        public static void InstallByReflection()
        {
            try
            {
                const string APPDOMAIN_MANAGER_ASM = "APPDOMAIN_MANAGER_ASM";
                const string APPDOMAIN_MANAGER_TYPE = "APPDOMAIN_MANAGER_TYPE";

                string assemblyQualifiedName = typeof (PluginSystemAppDomainManager).Assembly.FullName;
                string typeName = typeof (PluginSystemAppDomainManager).FullName;

                if (Environment.GetEnvironmentVariable(APPDOMAIN_MANAGER_ASM) != assemblyQualifiedName ||
                    Environment.GetEnvironmentVariable(APPDOMAIN_MANAGER_TYPE) != typeName)
                {
                    Environment.SetEnvironmentVariable(APPDOMAIN_MANAGER_ASM, assemblyQualifiedName);
                    Environment.SetEnvironmentVariable(APPDOMAIN_MANAGER_TYPE, typeName);

                    //Environment.CurrentDirectory = Environment.CurrentDirectory + @"\Plugins";
                    Process.Start(Assembly.GetExecutingAssembly().Location);
                }
                else
                {
                    Process.Start(Assembly.GetExecutingAssembly().Location);
                    var assembly = Assembly.LoadFrom("supermemo.exe");
                    var type = assembly.GetType("SMW.Program");
                    var methodInfo = type.GetMethod("Main",
                                                    BindingFlags.NonPublic | BindingFlags.Static |
                                                    BindingFlags.InvokeMethod);
                    var parameters = new string[] {};
                    methodInfo.Invoke(null, new object[] {parameters});
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }

    [Guid("F4D15099-3407-4A7E-A607-DEA440CF3891")]
    [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    [SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    public class PluginSystemAppDomainManager : AppDomainManager
    {
    }
}