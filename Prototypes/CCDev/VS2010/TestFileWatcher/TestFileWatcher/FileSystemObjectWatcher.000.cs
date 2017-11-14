using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestFileWatcher
{
    public class FileSystemObjectWatcher : IDisposable
    {
        public class FileSystemObjectDoesNotExist : ApplicationException
        {
            public FileSystemObjectDoesNotExist(string message)
                : base(message)
            {
            }
        }

        public delegate void FileSystemObjectWatcherProcessingDelegate(FileSystemObjectWatcher fileSystemObjectWatcher);

        #region Constructors

        public FileSystemObjectWatcher( string path,
                                        bool includeSubDirectories,
                                        FileSystemObjectWatcherProcessingDelegate fileSystemObjectWatcherProcessingDelegate,
                                        int eventResolutionMilliseconds)
        {
            Path = path;
            IncludeSubDirectories = includeSubDirectories;

            if (File.Exists(Path) || Directory.Exists(Path))
            {
                // Create and start a thread to watch the file system
                _eventResolutionMilliseconds = eventResolutionMilliseconds;
                _fileSystemObjectWatcherProcessingDelegate = fileSystemObjectWatcherProcessingDelegate;
                _threadWatching = new Thread(FileWatchingThread);
                _threadWatching.Start(this);
            }
            else
            {
                throw new FileSystemObjectDoesNotExist( "File or Directory \""+Path+"\" does not exist");
            }
        }

        #endregion Constructors

        #region Public Member Functions

        public void End()
        {
            try
            {
                _protectionMutex.WaitOne();
                if (Running)
                {
                    // Only end the file watching thread if there is one

                    // Tell the File Watching Thread to end
                    _exitEvent.Set();

                    // Wait for the File Watching Thread to end
                    _threadWatching.Join();

                    // Get rid of the reference as the thread has now ended
                    _threadWatching = null;

                } // Only end the file watching thread if there is one
            }
            finally
            {
                _protectionMutex.ReleaseMutex();                
            }
        }

        public void BeginProcessing()
        {
            Thread threadProcessing = new Thread(ProcessingThread);
            threadProcessing.Start(this);
        }

        private static void ProcessingThread( object objFileSystemObjectWatcher )
        {
            FileSystemObjectWatcher fileSystemObjectWatcher = (FileSystemObjectWatcher)objFileSystemObjectWatcher;

            bool fileSystemObjectEventDetected = false;
            bool exitEvent = false;

            while ( (!exitEvent) && (fileSystemObjectWatcher.Running))
            {
                WaitHandle[] waitHandles = new WaitHandle[] { fileSystemObjectWatcher._fileSystemObjectWatchEvent , fileSystemObjectWatcher._exitEvent };

                int waitIndex = WaitHandle.WaitAny(waitHandles, fileSystemObjectWatcher._eventResolutionMilliseconds);
                if (waitIndex != System.Threading.WaitHandle.WaitTimeout)
                {
                    // Not a timeout

                    switch ( waitIndex )
                    {
                        case 0: /* fileSystemObjectWatcher._fileSystemObjectWatchEvent */
                            // File System Object Watcher Event occurred
                            Console.WriteLine("{0} : {1} : Detected a File System Object Event", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
                            fileSystemObjectEventDetected = true;
                            break;
                        case 1: /* fileSystemObjectWatcher._exitEvent */
                            Console.WriteLine("{0} : {1} : Detected an Exit Event", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
                            exitEvent = true;
                            break;
                        default :
                            Console.WriteLine("{0} : {1} : Detected an Unknown Event with WaitIndex {2}", DateTime.Now, Thread.CurrentThread.ManagedThreadId, waitIndex);
                            break;
                    }

                } // Not a timeout
                else
                {
                    // Timeout occurred

                    Console.WriteLine("{0} : {1} : Timeout after {2} ms waiting for File System Object Event",
                                        DateTime.Now,
                                        Thread.CurrentThread.ManagedThreadId ,
                                        fileSystemObjectWatcher._eventResolutionMilliseconds);
                    if (fileSystemObjectEventDetected)
                    {
                        // ProcessingThread this File System Object Event
                        fileSystemObjectEventDetected = false;
                        if (fileSystemObjectWatcher._fileSystemObjectWatcherProcessingDelegate != null)
                        {
                            Console.WriteLine("{0} : {1} : Running task", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
                            fileSystemObjectWatcher._fileSystemObjectWatcherProcessingDelegate(
                                fileSystemObjectWatcher);
                        }
                    } // ProcessingThread this File System Object Event

                } // Timeout occurred

            } // while

            Console.WriteLine("{0} : {1} : ProcessingThread exiting", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
        }

        #endregion Public Member Functions

        #region Public Properties

        public string Path { get; private set; }
        public bool IncludeSubDirectories { get; private set; }

        public bool Running { get { return _threadWatching != null; } }

        /// <summary>
        /// Waits for a File System Object event
        /// </summary>
        /// <param name="timeoutMilliseconds">the time in milliseconds to wait for the event</param>
        /// <returns>true if an event was received else false</returns>
        public bool WaitForEvent( int timeoutMilliseconds )
        {
            return _fileSystemObjectWatchEvent.WaitOne(timeoutMilliseconds);
        }

        #endregion Public Properties

        #region IDisposable interface

        public void Dispose()
        {
            End();
        }

        #endregion

        #region FileWatchingThread

        public static void FileWatchingThread(object objFileSystemObjectWatcher)
        {
            FileSystemObjectWatcher fileSystemObjectWatcher = (FileSystemObjectWatcher)objFileSystemObjectWatcher;

            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(fileSystemObjectWatcher.Path);

            fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess
                                             | NotifyFilters.LastWrite
                                             | NotifyFilters.FileName
                                             | NotifyFilters.DirectoryName;

            fileSystemWatcher.IncludeSubdirectories = fileSystemObjectWatcher.IncludeSubDirectories;
            fileSystemWatcher.Filter = "*";

            fileSystemWatcher.Changed += new FileSystemEventHandler(fileSystemObjectWatcher.OnFileSystemObjectChanged);
            fileSystemWatcher.Created += new FileSystemEventHandler(fileSystemObjectWatcher.OnFileSystemObjectChanged);
            fileSystemWatcher.Deleted += new FileSystemEventHandler(fileSystemObjectWatcher.OnFileSystemObjectChanged);
            fileSystemWatcher.Renamed += new RenamedEventHandler(fileSystemObjectWatcher.OnFileSystemObjectRenamed);

            fileSystemWatcher.EnableRaisingEvents = true;

            Console.WriteLine("{0} : {1} : FileWatchingThread running on \"{2}\" with subdirectories {3}",
                                DateTime.Now ,
                                Thread.CurrentThread.ManagedThreadId ,
                                fileSystemWatcher.Path,
                                (fileSystemWatcher.IncludeSubdirectories?"included":"not included"));

            fileSystemObjectWatcher._exitEvent.WaitOne();

            Console.WriteLine("{0} : {1} : FileWatchingThread exiting", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
        }

        private void OnFileSystemObjectChanged(object source, FileSystemEventArgs e)
        {
            // File is changed, created, or deleted.
            Console.WriteLine("{0} : {1} : File \"{2}\" {3}",
                                DateTime.Now,
                                Thread.CurrentThread.ManagedThreadId ,
                                e.FullPath,
                                e.ChangeType);
            _fileSystemObjectWatchEvent.Set();
        }

        private void OnFileSystemObjectRenamed(object source, RenamedEventArgs e)
        {
            // File is renamed.
            Console.WriteLine("{0} : {1} : File \"{2}\" renamed to \"{3}\"",
                                DateTime.Now,
                                Thread.CurrentThread.ManagedThreadId ,
                                e.OldFullPath,
                                e.FullPath);
            _fileSystemObjectWatchEvent.Set();
        }

        #endregion FileWatchingThread

        #region Private Member Varibles

        private Thread _threadWatching;

        private int _eventResolutionMilliseconds = 0;
        private FileSystemObjectWatcherProcessingDelegate _fileSystemObjectWatcherProcessingDelegate = null;

        private Mutex _protectionMutex = new Mutex();

        private ManualResetEvent _exitEvent = new ManualResetEvent(false);

        private AutoResetEvent _fileSystemObjectWatchEvent = new AutoResetEvent(false);

        #endregion Private Member Variables
    }
}
