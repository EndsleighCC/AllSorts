using System;
using System.Collections.Generic;
using System.IO;
using CommandOperations;


namespace MakeUnused
{
    class Program
    {
        private const string _UnusedDirectoryName = "Unused";

        private static bool MoveFile(string rootDirectory , string filenameEntry)
        {
            bool success = false;

            string filenameEntryDirectory = filenameEntry.Substring(0, filenameEntry.LastIndexOf('\\'));

            string sourceFileDirectory = Path.Combine(rootDirectory , filenameEntryDirectory);
            string sourceFileName = Path.Combine(rootDirectory, filenameEntry);

            FileInfo fileInfo = new FileInfo(sourceFileName);

            string destinationDirectory = Path.Combine(rootDirectory, _UnusedDirectoryName, filenameEntryDirectory);
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }
            string destinationFilename = Path.Combine(destinationDirectory, fileInfo.Name);
            Console.WriteLine("    File \"{0}\" -> \"{1}\"", sourceFileName, destinationFilename);
            try
            {
                File.Move(sourceFileName, destinationFilename);
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: Moving file \"{0}\" to \"{1}\" = \"{2}\"",
                                    sourceFileName, destinationFilename , ex.ToString());
            }

            return success;
        }

        private static bool MoveDirectory( string rootDirectory , string directoryNameEntry)
        {
            bool success = false;

            string sourceDirectory = Path.Combine(rootDirectory, directoryNameEntry);

            string destinationDirectory = Path.Combine(rootDirectory, _UnusedDirectoryName, directoryNameEntry.Substring(0, directoryNameEntry.LastIndexOf('\\')));

            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            Console.WriteLine("    Directory \"{0}\" -> \"{1}\"", sourceDirectory, destinationDirectory);

            try
            {
                string command = "move \"" + sourceDirectory + "\" \"" + destinationDirectory + "\"";
                // Directory.Move(sourceDirectory, destinationDirectory);
                string currentDirectory = Directory.GetCurrentDirectory();
                success = CommandOperation.RunCommand(currentDirectory,
                                                      command,
                                                      CommandOperation.DebugProgress.None,
                                                      CommandOperation.CommandOutputDisplayType.StandardErrorOnly);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: Moving directory \"{0}\" to \"{1}\" = \"{2}\"",
                                    sourceDirectory, destinationDirectory, ex.ToString());
            }

            return success;
        }

        private static List<string> _PromotionGroupPathList = new List<string> { // @"\\adebs02\SysPROD",
                                                                                 // @"\\adebs02\SysPR00",
                                                                                 // @"\\adebs02\SysUT00",
                                                                                 // @"\\adebs04\SysST00",
                                                                                 @"\\adebs04\SysST02" };

        static int Main(string[] args)
        {
            int error = 0;

            if (args.Length >= 1)
            {
                string inputFilename = args[0];

                inputFilename = Path.GetFullPath(inputFilename);

                if (!File.Exists(inputFilename))
                {
                    Console.WriteLine("File \"{0}\" does not exist", inputFilename);
                }
                else
                {
                    bool success = true;

                    string line = null;
                    StreamReader fileInput = new StreamReader(inputFilename);
                    while ((line = fileInput.ReadLine()) != null)
                    {
                        string listEntry = line;

                        // Trim all white space (tabs and spaces)
                        listEntry = listEntry.Trim();

                        if ( ( listEntry.Length > 0 ) && ( listEntry[0] != '#' ) )
                        {
                            // Tidy up the entry and convert to a Windows suitable format
                            listEntry = listEntry.Replace("/", "\\");
                            // No leading or trailing backslashes
                            listEntry = listEntry.Trim(new char[] { '\\' });

                            foreach (string promotionGroupPath in _PromotionGroupPathList)
                            {
                                string sourceName = Path.Combine(promotionGroupPath, listEntry);

                                if (File.Exists(sourceName))
                                {
                                    bool fileSuccess = MoveFile(promotionGroupPath, listEntry);
                                    if (!fileSuccess)
                                    {
                                        Console.WriteLine("On Promotion Path {0} move of file \"{1}\" failed" ,
                                                            promotionGroupPath,listEntry);
                                    }
                                    success = fileSuccess && success;
                                }
                                else if (Directory.Exists(sourceName))
                                {
                                    bool dirSuccess = MoveDirectory(promotionGroupPath, listEntry);
                                    if (!dirSuccess)
                                    {
                                        Console.WriteLine("On Promotion Path {0} move of directory \"{1}\" failed",
                                                            promotionGroupPath, listEntry);
                                    }
                                    success = dirSuccess && success;
                                }
                                else
                                {
                                    Console.WriteLine("    Item \"{0}\" does not exist", sourceName);
                                }

                            }

                        }

                    } // while

                    if (success)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Making unused succeeded");
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Making unused failed. Review previous output.");
                    }

                }
            }

            return error;
        }
    }
}
