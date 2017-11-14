using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace TestFileWatcher
{
    public class FileSystemObjectTaskWatcher : IDisposable
    {
        public class FileSystemObjectDoesNotExist : ApplicationException
        {
            public FileSystemObjectDoesNotExist(string message)
                : base(message)
            {
            }
        }

        public class FileSystemObjectTaskDoesNotExist : ApplicationException
        {
            public FileSystemObjectTaskDoesNotExist(string message)
                : base(message)
            {
            }
        }

        #region Constructors

        public FileSystemObjectTaskWatcher( string path,
                                            bool includeSubDirectories,
                                            int eventResolutionMilliseconds,
                                            string taskNameToExecute)
        {
            Path = path;
            IncludeSubDirectories = includeSubDirectories;

            if (File.Exists(Path) || Directory.Exists(Path))
            {
                // Create and start a thread to watch the file system
                _eventResolutionMilliseconds = eventResolutionMilliseconds;

                if ( taskNameToExecute != null)
                {
                    taskNameToExecute = System.IO.Path.Combine(Environment.CurrentDirectory, taskNameToExecute);                    
                    if ( !File.Exists(taskNameToExecute))
                    {
                        throw new FileSystemObjectTaskDoesNotExist(
                            "Task file \"" + taskNameToExecute + "\" does not exist");
                    }
                    _taskNameToExecute = taskNameToExecute;
                }

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

        #region File System Watching Thread

        private static void ProcessingThread( object objFileSystemObjectTaskWatcher )
        {
            FileSystemObjectTaskWatcher fileSystemObjectTaskWatcher = (FileSystemObjectTaskWatcher)objFileSystemObjectTaskWatcher;

            bool fileSystemObjectEventDetected = false;
            bool exitEvent = false;

            while ( (!exitEvent) && (fileSystemObjectTaskWatcher.Running))
            {
                WaitHandle[] waitHandles = new WaitHandle[] { fileSystemObjectTaskWatcher._fileSystemObjectWatchEvent , fileSystemObjectTaskWatcher._exitEvent };

                int waitIndex = WaitHandle.WaitAny(waitHandles, fileSystemObjectTaskWatcher._eventResolutionMilliseconds);
                if (waitIndex != System.Threading.WaitHandle.WaitTimeout)
                {
                    // Not a timeout

                    switch ( waitIndex )
                    {
                        case 0: /* FileSystemObjectTaskWatcher._fileSystemObjectWatchEvent */
                            // File System Object Watcher Event occurred
                            Console.WriteLine("{0} : {1} : Detected a File System Object Event", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
                            fileSystemObjectEventDetected = true;
                            break;
                        case 1: /* FileSystemObjectTaskWatcher._exitEvent */
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
                                        fileSystemObjectTaskWatcher._eventResolutionMilliseconds);
                    if (fileSystemObjectEventDetected)
                    {
                        // ProcessingThread this File System Object Event
                        fileSystemObjectEventDetected = false;
                        if (fileSystemObjectTaskWatcher._taskNameToExecute != null)
                        {
                            Console.WriteLine("{0} : {1} : Running task \"{2}\"",
                                    DateTime.Now, Thread.CurrentThread.ManagedThreadId, fileSystemObjectTaskWatcher._taskNameToExecute);
                            fileSystemObjectTaskWatcher.ExecuteTask();
                        }
                    } // ProcessingThread this File System Object Event

                } // Timeout occurred

            } // while

            Console.WriteLine("{0} : {1} : ProcessingThread exiting", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
        }

        #endregion File System Watching Thread

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

        public static void FileWatchingThread(object objFileSystemObjectTaskWatcher)
        {
            FileSystemObjectTaskWatcher fileSystemObjectTaskWatcher = (FileSystemObjectTaskWatcher)objFileSystemObjectTaskWatcher;

            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(fileSystemObjectTaskWatcher.Path);

            fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess
                                             | NotifyFilters.LastWrite
                                             | NotifyFilters.FileName
                                             | NotifyFilters.DirectoryName;

            fileSystemWatcher.IncludeSubdirectories = fileSystemObjectTaskWatcher.IncludeSubDirectories;
            fileSystemWatcher.Filter = "*";

            fileSystemWatcher.Changed += new FileSystemEventHandler(fileSystemObjectTaskWatcher.OnFileSystemObjectChanged);
            fileSystemWatcher.Created += new FileSystemEventHandler(fileSystemObjectTaskWatcher.OnFileSystemObjectChanged);
            fileSystemWatcher.Deleted += new FileSystemEventHandler(fileSystemObjectTaskWatcher.OnFileSystemObjectChanged);
            fileSystemWatcher.Renamed += new RenamedEventHandler(fileSystemObjectTaskWatcher.OnFileSystemObjectRenamed);

            fileSystemWatcher.EnableRaisingEvents = true;

            Console.WriteLine("{0} : {1} : FileWatchingThread running in \"{2}\" on \"{3}\" with subdirectories {4} and resolution {5} ms",
                                DateTime.Now ,
                                Thread.CurrentThread.ManagedThreadId ,
                                Environment.CurrentDirectory,
                                fileSystemWatcher.Path,
                                (fileSystemWatcher.IncludeSubdirectories?"included":"not included"),
                                fileSystemObjectTaskWatcher._eventResolutionMilliseconds);

            fileSystemObjectTaskWatcher._exitEvent.WaitOne();

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

        #region Private Member Functions

        private void ExecuteTask()
        {
            // Decide what sort of task to run
            FileInfo fileInfoTask = new FileInfo(_taskNameToExecute);

            string programType = null;
            string programToExecute = null;
            string programArguments = null;

            switch (fileInfoTask.Extension.ToLower())
            {
                case ".bat" :
                    programType = "batch file";
                    programToExecute = Environment.GetEnvironmentVariable("COMSPEC");
                    programArguments = String.Format("/c \"{0}\" {1}", _taskNameToExecute, this.Path);
                    break;
                case ".exe" :
                    programType = "executable";
                    programToExecute = _taskNameToExecute;
                    programArguments = this.Path;
                    break;
                default :
                    Console.WriteLine( "Unknown file type for \"{0}\". Assuming it's a program",_taskNameToExecute);
                    programType = "unknown";
                    programToExecute = _taskNameToExecute;
                    programArguments = this.Path;
                    break;
            }

            // Set up a Procees Start Info to control the command execution
            ProcessStartInfo processStartInfo = new ProcessStartInfo(programToExecute);
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = programArguments;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            try
            {
                Console.WriteLine("Running {0} \"{1}\" \"{2}\" in \"{3}\"",
                    programType, programToExecute, programArguments, Environment.CurrentDirectory);

                Process process = new Process();
                process.StartInfo = processStartInfo;

                process.Start();

                process.WaitForExit();

                Console.WriteLine("Running \"{0}\" produced Exit Code {1}", _taskNameToExecute, process.ExitCode);

                if (!process.StandardOutput.EndOfStream)
                {
                    Console.WriteLine("Program \"{0}\" Standard Output:", _taskNameToExecute);
                    while (!process.StandardOutput.EndOfStream)
                    {
                        Console.WriteLine("    " + process.StandardOutput.ReadLine());
                    }
                }
                if (!process.StandardError.EndOfStream)
                {
                    Console.WriteLine("Program \"{0}\" Standard Error:", _taskNameToExecute);
                    while (!process.StandardError.EndOfStream)
                    {
                        Console.WriteLine("    " + process.StandardError.ReadLine());
                    }
                }

            }
            catch (Exception eek)
            {
                Console.WriteLine("    *** {0}.{1} : Executing \"{2}\" generated exception \"{3}\"",
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    _taskNameToExecute,
                    eek.ToString());
            }

        }

        #endregion Private Member Functions

        #region Private Member Variables

        private Thread _threadWatching;

        private int _eventResolutionMilliseconds = 0;

        private string _taskNameToExecute = null;

        private Mutex _protectionMutex = new Mutex();

        private ManualResetEvent _exitEvent = new ManualResetEvent(false);

        private AutoResetEvent _fileSystemObjectWatchEvent = new AutoResetEvent(false);

        #endregion Private Member Variables
    }
}
