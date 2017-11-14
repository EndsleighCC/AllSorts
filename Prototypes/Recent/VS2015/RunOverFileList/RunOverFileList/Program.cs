using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CommandOperations;

namespace RunOverFileList
{
    class Program
    {
        static bool RunOverFileList( string command, string streamName, StreamReader fileListStream)
        {
            bool success = true;

            Console.WriteLine();
            Console.WriteLine("Executing command \"{0}\" over files contained in \"{1}\"", command, streamName);
            Console.WriteLine();

            int lineNumber = 0;
            string filename = null;
            while ((filename = fileListStream.ReadLine()) != null)
            {
                lineNumber += 1;
                if (!filename.StartsWith("#"))
                {
                    // Not a comment line

                    filename = filename.Trim(new char[] { ' ', '\"' });

                    if (!File.Exists(filename))
                    {
                        Console.WriteLine("File \"{0}\" does not exist", filename);
                    }
                    else
                    {
                        string fullCommand = String.Format("{0} \"{1}\"", command, Path.GetFullPath(filename));

                        string currentDirectory = Directory.GetCurrentDirectory();

                        bool commandSuccess = CommandOperation.RunVisibleCommand(currentDirectory,
                                                                                 fullCommand,
                                                                                 1,
                                                                                 CommandOperation.DebugProgress.Enabled,
                                                                                 CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError);
                        if (!commandSuccess)
                        {
                            Console.WriteLine("Command \"{0}\" failed", fullCommand);
                        }
                        success = commandSuccess && success;
                    }

                } // Not a comment line
            } // while

            return success;

        } // RunOverListOfFiles

        static int Main(string[] args)
        {
            int error = 0;
            if ( args.Length < 1)
            {
                Console.WriteLine("RunOverFileList Command {FileContainingListOfPathedFilenames}");
            }
            else
            {
                string command = args[0];
                string filenameContainingFileList = null;

                StreamReader fileListStreamReader = null;

                if ( args.Length < 2 )
                {
                    // Assume the filename list is coming from Standard Input
                    filenameContainingFileList = "Standard Input";
                    fileListStreamReader = new StreamReader(Console.OpenStandardInput());
                } // Assume the filename list is coming from Standard Input
                else
                {
                    // The filename list is coming from the supplied file

                    filenameContainingFileList = filenameContainingFileList = Path.GetFullPath(args[1].Trim(new char[] { ' ', '\"' }));

                    if (!File.Exists(filenameContainingFileList))
                    {
                        Console.WriteLine("File \"{0}\" does not exist", filenameContainingFileList);
                    }
                    else
                    {
                        // Filename list file exists
                        fileListStreamReader = new StreamReader(filenameContainingFileList);
                    } // Filename list file exists

                } // The filename list is coming from the supplied file

                if ( fileListStreamReader != null )
                {
                    // File is open
                    if (RunOverFileList(command, filenameContainingFileList, fileListStreamReader))
                    {
                        error = 0;
                    }
                    else
                    {
                        error = 1;
                    }
                } // File is open
            }

            return error;
        }
    }
}
