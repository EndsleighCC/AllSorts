using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace TestFileWatcher
{
    class Program
    {

        static void Main(string[] args)
        {
            if ( args.Count() > 0 )
            {
                string directoryToWatch = args[0];
                bool includeSubDirectories = false;
                int eventResolutionMilliseconds = 10000;
                string taskNameToExecute = null;
                if (args.Count() > 1)
                    includeSubDirectories = (args[1].ToLower() == "/s" ? true : false);
                if (args.Count() > 2)
                {
                    try
                    {
                        eventResolutionMilliseconds = System.Convert.ToInt32(args[2]);
                    }
                    catch (Exception)
                    {
                    }
                    Console.WriteLine( "Event Resolution is {0} milliseconds",eventResolutionMilliseconds);
                }

                if ( args.Count() > 3 )
                {
                    taskNameToExecute = args[3];
                    Console.WriteLine("Task to execute is \"{0}\"",taskNameToExecute);
                }

                try
                {
                    using (FileSystemObjectTaskWatcher fileSystemObjectTaskWatcher = new FileSystemObjectTaskWatcher(
                                                                                        directoryToWatch,
                                                                                        includeSubDirectories,
                                                                                        eventResolutionMilliseconds,
                                                                                        taskNameToExecute))
                    {
                        // Wait for the user to quit the program

                        fileSystemObjectTaskWatcher.BeginProcessing();

                        Console.WriteLine("Press \'q\' to quit File Watching");
                        int response = 0;
                        do
                        {
                            response = Console.Read();
                        } while (response != 'q');
                    }

                }
                catch (Exception eek)
                {
                    Console.WriteLine("Exception: \"{0}\"",eek.ToString());
                }
            }
        }
    }
}
