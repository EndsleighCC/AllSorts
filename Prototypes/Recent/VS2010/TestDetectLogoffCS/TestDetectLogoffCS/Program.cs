using System;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Threading;
using Microsoft.Win32;

public class TestDetectLogoffCS
{
    private static AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

    [DllImport("kernel32.dll")]
    static extern int GetCurrentThreadId();

    private static void Main()
    {
        string simpleUserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        string sidUserId = System.Security.Principal.WindowsIdentity.GetCurrent().User.ToString();
        string envUserId = Environment.UserName;
        Console.WriteLine("Logged on user is \"{0}\"", simpleUserId);

        SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);
        SystemEvents.SessionEnded += new SessionEndedEventHandler(SystemEvents_SessionEnded);
        SystemEvents.EventsThreadShutdown += new EventHandler(SystemEvents_EventsThreadShutdown);

        // For demonstration purposes, this application sits idle waiting for events
        Console.WriteLine("Managed Thread Id = {0}, Native Thread Id {1} : This application is waiting for system events",
            Thread.CurrentThread.ManagedThreadId, GetCurrentThreadId());
        // Console.WriteLine("Press <Enter> to terminate this application");
        // Console.ReadLine();
        _autoResetEvent.WaitOne();
        Console.WriteLine("Managed Thread Id = {0}, Native Thread Id {1} : System Events have occurred - exiting 1",
            Thread.CurrentThread.ManagedThreadId, GetCurrentThreadId());
        // Ensure everything that has been written has been flushed to the output
        Console.Out.Flush();
        _autoResetEvent.WaitOne();
        Console.WriteLine("Managed Thread Id = {0}, Native Thread Id {1} : System Events have occurred - exiting 2",
            Thread.CurrentThread.ManagedThreadId, GetCurrentThreadId());
        // Ensure everything that has been written has been flushed to the output
        Console.Out.Flush();
        if (_autoResetEvent.WaitOne(1000))
        {
            Console.WriteLine("Managed Thread Id = {0}, Native Thread Id {1} : System Events have occurred - exiting 3",
                Thread.CurrentThread.ManagedThreadId, GetCurrentThreadId());
        }
        else
        {
            Console.WriteLine("Managed Thread Id = {0}, Native Thread Id {1} : System Events have occurred with timeout - exiting 3",
                Thread.CurrentThread.ManagedThreadId, GetCurrentThreadId());
        }
        // Ensure everything that has been written has been flushed to the output
        Console.Out.Flush();

        // Get user id into class member
        while (true)
        {
            // Look for toolbar
            // Get phone extension
            // Put phone extension in REDIS with userid as key
            // Wait for toolbar to exit
            // Delete entry in REDIS
        }
    }

    // This method is called when the User is logging off
    private static void SystemEvents_SessionEnding(object sender, EventArgs e)
    {
        // Remove from REDIS item with key userid
        Console.WriteLine("Managed Thread Id = {0}, Native Thread Id {1} : The User is logging off",
            Thread.CurrentThread.ManagedThreadId, GetCurrentThreadId());
        Console.Out.Flush();
        _autoResetEvent.Set();
    }

    // This method is called when the User has logged off
    private static void SystemEvents_SessionEnded(object sender, EventArgs e)
    {
        Console.WriteLine("Managed Thread Id = {0}, Native Thread Id {1} : The User has logged off",
            Thread.CurrentThread.ManagedThreadId,GetCurrentThreadId());
        Console.Out.Flush();
        _autoResetEvent.Set();
    }

    private static void SystemEvents_EventsThreadShutdown(object sender, EventArgs e)
    {
        Console.WriteLine("Managed Thread Id = {0}, Native Thread Id {1} : The system is shutting down", 
            Thread.CurrentThread.ManagedThreadId, GetCurrentThreadId());
        Console.Out.Flush();
        _autoResetEvent.Set();
    }

}
