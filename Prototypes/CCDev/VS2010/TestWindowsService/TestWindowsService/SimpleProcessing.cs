using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace TestWindowsService
{
    public partial class SimpleProcessing : ServiceBase
    {
        public SimpleProcessing()
        {
            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists("TestWindowsServiceEventSource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                   "TestWindowsServiceEventSource", "TestWindowsServiceEventLog");
            }
            TestWindowsServiceEventLog.Source = "TestWindowsServiceEventSource";
            TestWindowsServiceEventLog.Log = "TestWindowsServiceEventLog";
        }

        private class FileWatcherData
        {
            public FileWatcherData( string pathToWatch)
            {
                PathToWatch = pathToWatch;
            }

            public string PathToWatch { get; private set; }
        }

        private static void FileWatcherThread( object obj )
        {
            FileWatcherData fileWatcherData = (FileWatcherData)obj;

            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(fileWatcherData.PathToWatch);

            fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess
                                             | NotifyFilters.LastWrite
                                             | NotifyFilters.FileName
                                             | NotifyFilters.DirectoryName;
            fileSystemWatcher.Filter = "*";

            fileSystemWatcher.Changed += OnWatchChange;
            fileSystemWatcher.Created += OnWatchChange;
            fileSystemWatcher.Deleted += OnWatchChange;
            fileSystemWatcher.Renamed += OnWatchChange;
        }

        private static void OnWatchChange( object source, FileSystemEventArgs e )
        {
        }

        protected override void OnStart(string[] args)
        {
            TestWindowsServiceEventLog.WriteEntry("TestWindowsService : OnStart executing");

            Thread fileWatcherThread = new Thread(FileWatcherThread);

            fileWatcherThread.Start(new FileWatcherData( @"\\adeoc01\techpickup\TMS\Motor") );
        }

        protected override void OnStop()
        {
            TestWindowsServiceEventLog.WriteEntry("TestWindowsService : OnStop executing");
        }
    }
}
