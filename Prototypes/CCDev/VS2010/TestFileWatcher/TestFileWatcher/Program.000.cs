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

        private void ExecuteBatchFile(string batchFullFilename, FileSystemObjectWatcher fileSystemObjectWatcher)
        {

            string commandInterpreter = Environment.GetEnvironmentVariable("COMSPEC");

            // Pass the following parameters to the batch file
            //  Project Directory
            //  Project Filename
            //  Configuration
            //  Configuration Path
            string batchFileArguments = String.Format("/c \"{0}\" {1}",
                                                      batchFullFilename,
                                                      fileSystemObjectWatcher.Path);

            // Set up a Procees Start Info to control the command execution
            ProcessStartInfo batchFileProcessStartInfo = new ProcessStartInfo(commandInterpreter);
            batchFileProcessStartInfo.UseShellExecute = false;
            batchFileProcessStartInfo.Arguments = batchFileArguments;
            // batchFileProcessStartInfo.RedirectStandardInput = true;
            batchFileProcessStartInfo.RedirectStandardOutput = true;
            batchFileProcessStartInfo.RedirectStandardError = true;

            try
            {
                Console.WriteLine("Running \"{0}\" \"{1}\" in \"{2}\"", commandInterpreter, batchFileArguments, Environment.CurrentDirectory);

                Process batchFileProcess = new Process();
                batchFileProcess.StartInfo = batchFileProcessStartInfo;

                batchFileProcess.Start();

                batchFileProcess.WaitForExit();

                Console.WriteLine("Running \"{0}\" produced Exit Code {1}", batchFullFilename, batchFileProcess.ExitCode);

                if (!batchFileProcess.StandardOutput.EndOfStream)
                {
                    Console.WriteLine("Batch File \"{0}\" Standard Output:", batchFullFilename);
                    while (!batchFileProcess.StandardOutput.EndOfStream)
                    {
                        Console.WriteLine("    " + batchFileProcess.StandardOutput.ReadLine());
                    }
                }
                if (!batchFileProcess.StandardError.EndOfStream)
                {
                    Console.WriteLine("Batch File \"{0}\" Standard Error:", batchFullFilename);
                    while (!batchFileProcess.StandardError.EndOfStream)
                    {
                        Console.WriteLine("    " + batchFileProcess.StandardError.ReadLine());
                    }
                }

            }
            catch (Exception eek)
            {
                Console.WriteLine("    *** {0}.{1} : Executing \"{2}\" generated exception \"{3}\"",
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    batchFullFilename,
                    eek.ToString());
            }

        }

        private void ProcessFileSystemEvent( FileSystemObjectWatcher fileSystemObjectWatcher )
        {
            Console.WriteLine("{0} : {1} , ProcessFileSystemEvent running", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
            ExecuteBatchFile("fred.bat", fileSystemObjectWatcher);
        }

        static void Main(string[] args)
        {
            if ( args.Count() > 0 )
            {
                string directoryToWatch = args[0];
                bool includeSubDirectories = false;
                int eventResolutionMilliseconds = 10000;
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

                try
                {
                    using (FileSystemObjectWatcher fileSystemObjectWatcher = new FileSystemObjectWatcher(
                                                                                        directoryToWatch,
                                                                                        includeSubDirectories,
                                                                                        ProcessFileSystemEvent,
                                                                                        eventResolutionMilliseconds))
                    {
                        // Wait for the user to quit the program

                        fileSystemObjectWatcher.BeginProcessing();

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
