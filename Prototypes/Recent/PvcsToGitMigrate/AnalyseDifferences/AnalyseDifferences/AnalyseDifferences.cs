using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Numerics;

namespace AnalyseDifferences
{
    class Program
    {
        private static FilenameAndHashValues GenerateFileHashes(PromotionGroupDetails promotionGroupDetails, string filenameListEntry)
        {
            string promotionGroupFilePath = Path.Combine(promotionGroupDetails.PromotionGroupSharePath, filenameListEntry);
            string repositoryFilePath = Path.Combine(promotionGroupDetails.GitRepoSharePath, filenameListEntry);

            return new FilenameAndHashValues(filenameListEntry,
                                             new NonEolHash(promotionGroupFilePath).Generate(),
                                             new NonEolHash(repositoryFilePath).Generate());
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
                Console.WriteLine("Usage: AnalyseDifferences PromotionGroup InputFilename {InputFilename...}");
            }
            else
            {
                // At least two program arguments

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
                    Console.WriteLine();

                    PromotionGroupDetails selectedPromotionGroupDetails = _PromotionGroupDetailsSortedDictionary[selectedPromotionGroup];

                    // Assume overall success unless otherwise determined
                    error = 0;

                    // Start after the Promotion Group specification
                    for (int argId = 1; argId < args.Length; ++argId)
                    {
                        string inputFilename = args[argId];

                        inputFilename = Path.GetFullPath(inputFilename);

                        if (!File.Exists(inputFilename))
                        {
                            Console.WriteLine();
                            Console.WriteLine("File \"{0}\" does not exist", inputFilename);
                        }
                        else
                        {
                            // Input file exists

                            // Assume success for this Promotion Group and change the error if it isn't
                            int fileDifferenceCount = 0;

                            using (StreamReader fileInput = new StreamReader(inputFilename))
                            {
                                string line = null;
                                int lineNumber = 0;
                                bool linesProcessed = false;
                                bool finished = false;
                                while ((!finished) && (line = fileInput.ReadLine()) != null)
                                {
                                    string listEntry = line;
                                    lineNumber += 1;

                                    // Trim all white space (tabs and spaces)
                                    listEntry = listEntry.Trim();

                                    // Skip lines beginning with a hash character
                                    if ( listEntry.Length == 0 )
                                    {
                                        if (linesProcessed)
                                        {
                                            // Encountering an empty line after having processed non-blank filenames means the task is complete
                                            finished = true;
                                        }
                                    }
                                    else if (listEntry[0] != '#')
                                    {
                                        // Valid list entry and List Entry is not commented out

                                        // Tidy up the entry and convert to a Windows suitable format
                                        listEntry = listEntry.Replace("/", "\\");
                                        // No leading or trailing backslashes
                                        listEntry = listEntry.Trim(new char[] { '\\' });

                                        // Input line is of the format "{whitespace}gitstatus:{whitespace}filename

                                        // Locate the filename which must follow a colon
                                        int posColon = listEntry.IndexOf(':');
                                        if (posColon < 0)
                                        {
                                            if (linesProcessed )
                                            {
                                                // Lines have been previously processed so unexpectedly a filename has not been specified
                                                Console.WriteLine();
                                                Console.WriteLine("\"{0}\"({1}) : Expected a file specification but none was present",
                                                                        inputFilename, lineNumber);
                                                Console.WriteLine();
                                            }
                                        }
                                        else
                                        {
                                            // A file specification appears to be present

                                            // Guard against trailing colons at the end of the line
                                            if ( (posColon+1) < listEntry.Length )
                                            {
                                                // A filename really does appear to be present
                                                linesProcessed = true;

                                                // Parse out the "git status" action
                                                string[] listEntryItem = listEntry.Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);

                                                string gitStatusAction = listEntryItem[0];

                                                // Parse out the filename allowing for filenames with spaces and remove leading and trailing whitespace
                                                string gitRepoFilename = listEntry.Substring(posColon + 1).Trim();

                                                // Only compare if there is a file to compare
                                                if (String.Compare(gitStatusAction, "deleted", true /* ignore case */) == 0)
                                                {
                                                    Console.WriteLine("Promotion Group file \"{0}\" has been deleted relative to the Git Repository", gitRepoFilename);
                                                    // The Promotion Group Directory and the Repository Working Directory should contain the same files
                                                    fileDifferenceCount += 1;
                                                }
                                                else
                                                {
                                                    // Compare the files in each Promotion Group Volume and the corresponding Git Repository Working Directory

                                                    string sourceName = Path.Combine(selectedPromotionGroupDetails.PromotionGroupSharePath, gitRepoFilename);
                                                    if (File.Exists(sourceName))
                                                    {
                                                        // Use a hash to attempt to decide whether the file contents are actually different
                                                        FilenameAndHashValues filenameAndHashValues = GenerateFileHashes(selectedPromotionGroupDetails, gitRepoFilename);
                                                        if (filenameAndHashValues.PromotionGroupFileHash != filenameAndHashValues.RepositoryFileHash)
                                                        {
                                                            // The file hashes are different
                                                            // However, if the files are Solutions perhaps they are actually semantically equivalent
                                                            bool thisFileIsTheSame = false;
                                                            FileInfo fileInfo = new FileInfo(sourceName);
                                                            if (String.Compare(fileInfo.Extension, ".sln", true /*ignore case*/) == 0)
                                                            {
                                                                bool exactlyEquivalent = false;
                                                                bool semanticallyEquivalent = false;
                                                                AnalyseDifferences.CompareSolutions.Compare(gitRepoFilename,
                                                                                                            selectedPromotionGroupDetails.GitRepoSharePath,
                                                                                                            selectedPromotionGroupDetails.PromotionGroupSharePath,
                                                                                                            ref exactlyEquivalent,
                                                                                                            ref semanticallyEquivalent);
                                                                // It is sufficient that the Solutions "mean" the same thing rather than "be" exactly the same
                                                                thisFileIsTheSame = semanticallyEquivalent;
                                                                Console.WriteLine("Solution comparison for \"{0}\" indicates semantic {1}",
                                                                                    gitRepoFilename,
                                                                                    (thisFileIsTheSame ? "equivalence" : "difference"));
                                                            }
                                                            if (!thisFileIsTheSame)
                                                            {
                                                                fileDifferenceCount += 1;
                                                                Console.WriteLine("Files for \"{0}\" appear to be different", filenameAndHashValues.Filename);

                                                            }
                                                        } // The file hashes are different
                                                        else
                                                        {
                                                            //Console.WriteLine("Files for \"{0}\" appear to be the same. Hash = {1}",
                                                            //                    filenameAndHashValues.Filename, filenameAndHashValues.PromotionGroupFileHash);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Promotion Group file \"{0}\" has been deleted relative to the Git Repository", gitRepoFilename);
                                                        // The Promotion Group Directory and the Repository Working Directory should contain the same files
                                                        fileDifferenceCount += 1;
                                                    }

                                                } // Compare the files in each Promotion Group Volume and the corresponding Git Repository Working Directory

                                            } // A filename really does appear to be present

                                        } // A file specification appears to be present

                                    } // Valid list entry and List Entry is not commented out

                                } // while not end-of-file

                            } // using StreamReader

                            if (fileDifferenceCount == 0)
                            {
                                Console.WriteLine();
                                Console.WriteLine("No differences were detected between the Promotion Group Volume \"{0}\" and the corresponding Git Repository \"{1}\"",
                                    selectedPromotionGroupDetails.PromotionGroupSharePath, selectedPromotionGroupDetails.GitRepoSharePath);
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Number of differences detected between the Promotion Group Volume \"{0}\" and the corresponding Git Repository \"{1}\" was {2}",
                                    selectedPromotionGroupDetails.PromotionGroupSharePath, selectedPromotionGroupDetails.GitRepoSharePath,fileDifferenceCount);
                                error = 1;
                            }

                        } // Selected Promotion Group exists

                    } // for argId

                } // Input file exists

            } // At least two program arguments

            return error;
        }
    }
}
