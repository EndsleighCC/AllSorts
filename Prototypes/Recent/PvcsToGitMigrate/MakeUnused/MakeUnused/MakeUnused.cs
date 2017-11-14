using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CommandOperations;

namespace MakeUnusedProgram
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
                if ( ! File.Exists(destinationFilename))
                {
                    File.Move(sourceFileName, destinationFilename);
                }
                else
                {
                    Console.WriteLine("        Destination file already exists - copying with overwrite and delete instead");

                    File.Copy(sourceFileName, destinationFilename, /*overwrite*/ true);

                    // Delete the source file but ensure that the source file is not read-only so that it can be deleted
                    FileAttributes fileAttributes = File.GetAttributes(sourceFileName);
                    if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        // Make the file Read/Write
                        fileAttributes = fileAttributes & ~FileAttributes.ReadOnly;
                        File.SetAttributes(sourceFileName, fileAttributes);
                    }

                    File.Delete(sourceFileName);
                }
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: Moving file \"{0}\" to \"{1}\" = \"{2}\"",
                                    sourceFileName, destinationFilename , ex.ToString());
            }

            return success;
        }

        private static bool RenameDirectory(string sourceDirectoryName)
        {
            bool renamed = false;
            if ( Directory.Exists(sourceDirectoryName))
            {
                // Iterate to find a unique directory name with a digit "extension"
                for (int dirIndex = 0; (!renamed) && (dirIndex < 1000); ++dirIndex)
                {
                    string destinationDirectoryName = String.Format("{0}.{1:000}", sourceDirectoryName, dirIndex);
                    if ( ! Directory.Exists(destinationDirectoryName))
                    {
                        try
                        {
                            Directory.Move(sourceDirectoryName, destinationDirectoryName);
                            renamed = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("    Unable to rename directory \"{0}\" to \"{1}\". Exception \"{2}\"",
                                            sourceDirectoryName,destinationDirectoryName,ex.ToString());
                        }
                    }
                }
                if ( ! renamed )
                {
                    Console.WriteLine("    Unable to find a unique rename for directory \"{0}\"", sourceDirectoryName);
                }
            }
            return renamed;
        }

        private static bool MoveDirectory(string rootDirectory, string directoryNameEntry)
        {
            bool success = false;

            string sourceDirectory = Path.Combine(rootDirectory, directoryNameEntry);

            string destinationParentDirectory = null;

            bool proceedWithMove = true;

            int lastSlashIndex = directoryNameEntry.LastIndexOf('\\');
            if (lastSlashIndex < 1 )
            {
                // Must be the root directory
                destinationParentDirectory = Path.Combine(rootDirectory, _UnusedDirectoryName);
            }
            else
            {
                destinationParentDirectory = Path.Combine(rootDirectory, _UnusedDirectoryName, directoryNameEntry.Substring(0, lastSlashIndex));
            }

            if (!Directory.Exists(destinationParentDirectory))
            {
                try
                {
                    Directory.CreateDirectory(destinationParentDirectory);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("        *** Failed to create directory \"{0}\" : Exception \"{1}\"", destinationParentDirectory, ex.ToString());
                    proceedWithMove = false;
                }
            }

            if ( proceedWithMove )
            {
                string destinationDirectory = Path.Combine(rootDirectory, _UnusedDirectoryName, directoryNameEntry);
                if (!Directory.Exists(destinationDirectory))
                {
                    proceedWithMove = true;
                }
                else
                {
                    // Only proceed if the existing destination can be renamed
                    proceedWithMove = RenameDirectory(destinationDirectory);
                }

                if (!proceedWithMove)
                {
                    Console.WriteLine("    *** Failed to move \"{0}\" because the destination directory \"{1}\" already exists and it could not be renamed",
                                            sourceDirectory, destinationDirectory);
                }
                else
                {
                    Console.WriteLine("    Directory \"{0}\" -> \"{1}\" as \"{2}\"",
                                        sourceDirectory,
                                        destinationParentDirectory,
                                        destinationDirectory);
                    try
                    {
                        Directory.Move(sourceDirectory, destinationDirectory);
                        success = true;
                        //string command = "move \"" + sourceDirectory + "\" \"" + destinationParentDirectory + "\"";
                        //string currentDirectory = Directory.GetCurrentDirectory();
                        //success = CommandOperation.RunCommand(currentDirectory,
                        //                                      command,
                        //                                      CommandOperation.DebugProgress.None,
                        //                                      CommandOperation.CommandOutputDisplayType.StandardErrorOnly);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception: Moving directory \"{0}\" to Parent \"{1}\" to become \"{2}\" = \"{3}\"",
                                            sourceDirectory,
                                            destinationParentDirectory,
                                            destinationDirectory ,
                                            ex.ToString());
                    }
                }

            }

            return success;
        }

        /// <summary>
        /// Make the supplied Promotion Path "Unused" by looping through the Promotion Group Shares in the list
        /// </summary>
        /// <param name="promotionItemRelativePath">The relative pathname within a Promotion Group i.e. without drive specifier</param>
        /// <returns></returns>
        static bool MakeUnused(PromotionGroupDetails promotionGroupDetails, string promotionItemRelativePath )
        {
            // Assume success unless something fails
            bool success = true;

            // Tidy up the entry and, if necessary, convert to a Windows suitable format
            promotionItemRelativePath = promotionItemRelativePath.Replace("/", "\\");
            // No leading or trailing backslashes
            promotionItemRelativePath = promotionItemRelativePath.Trim(new char[] { '\\' });

            string promotionGroupPath = promotionGroupDetails.PromotionGroupSharePath;

            string sourceFullPath = Path.Combine(promotionGroupPath, promotionItemRelativePath);

            if (File.Exists(sourceFullPath))
            {
                bool fileSuccess = MoveFile(promotionGroupPath, promotionItemRelativePath);
                if (!fileSuccess)
                {
                    Console.WriteLine("On Promotion Path {0} move of file \"{1}\" failed",
                                        promotionGroupPath, promotionItemRelativePath);
                }
                success = fileSuccess && success;
            }
            else if (Directory.Exists(sourceFullPath))
            {
                bool dirSuccess = MoveDirectory(promotionGroupPath, promotionItemRelativePath);
                if (!dirSuccess)
                {
                    Console.WriteLine("On Promotion Path {0} move of directory \"{1}\" failed",
                                        promotionGroupPath, promotionItemRelativePath);
                }
                success = dirSuccess && success;
            }
            else
            {
                // A non-existent item still represents a successful "move"
                Console.WriteLine("    Item \"{0}\" does not exist", sourceFullPath);
            }

            return success;
        }

        private class PromotionGroupDetails
        {
            public PromotionGroupDetails(string promotionGroupName, string serverName, string promotionGroupCode)
            {
                PromotionGroupName = promotionGroupName;
                ServerName = serverName;
                PromotionGroupSharePath = @"\\" + serverName + @"\Sys" + promotionGroupCode;
                GitRepoSharePath = @"\\" + serverName + @"\g" + promotionGroupCode;
            }

            public string PromotionGroupName { get; private set; }

            public string ServerName { get; private set; }

            public string PromotionGroupSharePath { get; private set; }
            public string GitRepoSharePath { get; private set; }
        }

        private static SortedDictionary<string, PromotionGroupDetails> _PromotionGroupDetailsSortedDictionary = new SortedDictionary<string, PromotionGroupDetails>
                                {
                                    { "Production" , new PromotionGroupDetails( "Production" , "adebs02" , "PROD" ) } ,
                                    { "Pre_Production" , new PromotionGroupDetails( "Pre_Production" , "adebs02" , "PR00" ) } ,
                                    { "User_Test" , new PromotionGroupDetails( "User_Test" , "adebs02" , "UT00" ) } ,
                                    { "System_Test" , new PromotionGroupDetails( "System_Test" , "adebs04" , "ST00" ) } ,
                                    { "System_Test1" , new PromotionGroupDetails( "System_Test1" , "adebs04" , "ST01" ) } ,
                                    { "System_Test2" , new PromotionGroupDetails( "System_Test2" , "adebs04" , "ST02" ) } ,
                                    { "System_Test3" , new PromotionGroupDetails( "System_Test3" , "adebs04" , "ST03" ) } ,
                                    { "System_Test4" , new PromotionGroupDetails( "System_Test4" , "adebs04" , "ST04" ) } ,
                                    { "System_Test5" , new PromotionGroupDetails( "System_Test5" , "adebs04" , "ST05" ) } ,
                                    { "System_Test6" , new PromotionGroupDetails( "System_Test6" , "adebs04" , "ST06" ) }
                                };

        static int Main(string[] args)
        {
            int error = 1;

            if (args.Length < 2)
            {
                Console.WriteLine();
                Console.WriteLine("Usage: MakeUnused PromotionGroupName ItemRelativePath|@FilenameList {ItemRelativePath|@FilenameList ...}");
            }
            else
            {
                // Sufficient arguments

                string selectedPromotionGroup = args[0];

                if (!_PromotionGroupDetailsSortedDictionary.Keys.Contains(selectedPromotionGroup))
                {
                    Console.WriteLine();
                    Console.WriteLine("Promotion Group \"{0}\" does not exist", selectedPromotionGroup);
                }
                else
                {
                    // Selected Promotion Group exists

                    Console.WriteLine();
                    Console.WriteLine("Selected Promotion Group is {0}", selectedPromotionGroup);

                    PromotionGroupDetails selectedPromotionGroupDetails = _PromotionGroupDetailsSortedDictionary[selectedPromotionGroup];

                    // Assume overall success unless otherwise determined
                    error = 0;

                    for (int argId = 1; argId < args.Length; ++argId)
                    {
                        // Work out whether the supplied argument is the name of a file to be made unused, or, a file containing a list of files
                        if (!args[argId].StartsWith("@"))
                        {
                            // A filename or directory name

                            string promotionItemRelativePath = args[argId].Trim();

                            if (MakeUnused(selectedPromotionGroupDetails, promotionItemRelativePath))
                            {
                                Console.WriteLine();
                                Console.WriteLine("Making unused of \"{0}\" succeeded", promotionItemRelativePath);
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Making unused of \"{0}\" failed", promotionItemRelativePath);
                                error = 1;
                            }

                        } // A filename or directory name
                        else
                        {
                            // A file containing a list of files

                            // Skip the "@" symbol
                            string inputFilename = args[argId].Substring(1);

                            inputFilename = Path.GetFullPath(inputFilename);

                            if (!File.Exists(inputFilename))
                            {
                                Console.WriteLine("File \"{0}\" does not exist", inputFilename);
                            }
                            else
                            {
                                // Input file exists
                                bool success = true;

                                Console.WriteLine();

                                using (StreamReader fileInput = new StreamReader(inputFilename))
                                {
                                    string line = null;
                                    while ((line = fileInput.ReadLine()) != null)
                                    {
                                        string listEntry = line;

                                        // Trim all white space (tabs and spaces)
                                        listEntry = listEntry.Trim();

                                        // Skip lines beginning with a hash character
                                        if ((listEntry.Length > 0) && (listEntry[0] != '#'))
                                        {
                                            success = MakeUnused(selectedPromotionGroupDetails, listEntry) && success;
                                        }

                                    } // while
                                }

                                if (success)
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Making unused from files listed in \"{0}\" succeeded",inputFilename);
                                }
                                else
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Making unused from files listed in \"{0}\" failed", inputFilename);
                                    error = 1;
                                }

                            } // Input file exists

                        } // A file containing a list of files

                    }  // for

                } // Selected Promotion Group exists

            } // Sufficient arguments

            return error;
        }
    }
}
