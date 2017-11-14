using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.ComponentModel;

namespace TestAdminFunctions
{
    class Program
    {

        [DllImport("user32.dll",SetLastError = true)]
        public static extern int ExitWindowsEx(int uFlags, int dwReason);

        // Function represented as "Flags"
        private const int EWX_HYBRID_SHUTDOWN = 0x00400000;
        private const int EWX_LOGOFF = 0;
        private const int EWX_POWEROFF = 0x00000008;
        private const int EWX_REBOOT = 0x00000002;
        private const int EWX_RESTARTAPPS = 0x00000040;
        private const int EWX_SHUTDOWN = 0x00000001;

        // Bitwise modifiers EWX constants
        private const int EWX_FORCE = 0x00000004;
        private const int EWX_FORCEIFHUNG = 0x00000010;

        // Major Reason Codes
        private const int SHTDN_REASON_MAJOR_APPLICATION = 0x00040000;
        private const int SHTDN_REASON_MAJOR_HARDWARE = 0x00010000;
        private const int SHTDN_REASON_MAJOR_LEGACY_API = 0x00070000;
        private const int SHTDN_REASON_MAJOR_OPERATINGSYSTEM = 0x00020000;
        private const int SHTDN_REASON_MAJOR_OTHER = 0x00000000;
        private const int SHTDN_REASON_MAJOR_POWER = 0x00060000;
        private const int SHTDN_REASON_MAJOR_SOFTWARE = 0x00030000;
        private const int SHTDN_REASON_MAJOR_SYSTEM = 0x00050000;

        // Lots of "minor" Reason Codes

        private static void ObjectModelAdmin(string option)
        {
            switch (option.ToLower())
            {
                case "reboot":
                    // Ensure that the current thread has the "Shutdown" Privilege if it is allowed
                    AddPrivilege(ShutdownPrivilege);
                    if (ExitWindowsEx(EWX_REBOOT | EWX_FORCE, SHTDN_REASON_MAJOR_OTHER) != 0)
                        Console.WriteLine("Reboot has been initiated");
                    else
                    {
                        string error = new Win32Exception().Message;
                        Console.WriteLine("Reboot has failed with error \"{0}\"", error);
                    }
                    break;
                case "logoff":
                    if (ExitWindowsEx(EWX_LOGOFF | EWX_FORCE, SHTDN_REASON_MAJOR_OTHER) != 0)
                        Console.WriteLine("Logoff has been initiated");
                    else
                    {
                        string error = new Win32Exception().Message;
                        Console.WriteLine("Logoff has failed with error \"{0}\"", error);
                    }
                    break;
                default:
                    Console.WriteLine("Uknown admin functions \"{0}\"", option);
                    break;
            }
        }

        private static void CommandLineShutdown()
        {
            string command = "shutdown";
            string parameters = "/r /f /t 0";

            Console.WriteLine("Executing: \"{0} {1}\"", command, parameters);
            Process.Start(command, parameters);
        }

        private const int PrivilegeEnabled = 0x00000002;
        private const int TokenQuery = 0x00000008;
        private const int AdjustPrivileges = 0x00000020;
        private const string ShutdownPrivilege = "SeShutdownPrivilege";

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokenPrivileges
        {
            public int PrivilegeCount;
            public long Luid;
            public int Attributes; // Just one
        }

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern int OpenProcessToken(IntPtr processHandle, int desiredAccess, ref IntPtr tokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern int LookupPrivilegeValue(string systemName, string name, ref long luid);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern int AdjustTokenPrivileges(IntPtr tokenHandle, bool disableAllPrivileges, ref TokenPrivileges newState, int bufferLength, IntPtr previousState, IntPtr length);

        private static void AddPrivilege(string privilegeName)
        {
            IntPtr currentProcess = GetCurrentProcess();
            IntPtr tokenHandle = IntPtr.Zero;
            int result = OpenProcessToken(currentProcess, AdjustPrivileges | TokenQuery, ref tokenHandle);
            if (result == 0) throw new Win32Exception(Marshal.GetLastWin32Error());
            TokenPrivileges tokenPrivileges;
            tokenPrivileges.PrivilegeCount = 1;
            tokenPrivileges.Luid = 0;
            tokenPrivileges.Attributes = PrivilegeEnabled;
            result = LookupPrivilegeValue(null, privilegeName, ref tokenPrivileges.Luid);
            if (result == 0) throw new Win32Exception(Marshal.GetLastWin32Error());
            result = AdjustTokenPrivileges(tokenHandle, false, ref tokenPrivileges, 0, IntPtr.Zero, IntPtr.Zero);
            if (result == 0) throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        private static void ShowProcesses()
        {
            string processName = "notepad";

            Process process1 = Process.Start(processName, "");
            // Process.Start(processName, "");

            bool completed = process1.WaitForExit(5000);

            if (completed)
                Console.WriteLine("Process \"{0}\" completed",processName);
            else
                Console.WriteLine("Process \"{0}\" did not complete", processName);

            Process[] processes = Process.GetProcessesByName("notepad");

            Console.WriteLine("Count of processes with name \"{0}\" is {1}",processName,processes.Count());

            foreach (Process process in processes)
            {
                Console.WriteLine("Process \"{0}\" with PID {1}",process.ProcessName,process.Id);
                process.Kill();
                // Wait until it is "gone"
                process.WaitForExit();
            }
        }

        static void Main(string[] args)
        {
            if (args.Count() > 0)
            {
                ShowProcesses();
                ObjectModelAdmin(args[0]);
            }
        }
    }
}
