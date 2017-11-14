using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestFindUniqueFilenames
{
    class Program
    {
        static void Main(string[] args)
        {
            if ( args.Length >= 2 )
            {
                string issueNumber = args[0].Trim();
                string requiredPromotionGroup = args[1].Trim();
                string filename = args[2].Trim();

                if ( File.Exists( filename ))
                {
                    SortedSet< string > filenameSet = new SortedSet<string>();
                    SortedDictionary<string,string> archivePromoteDictionary = new SortedDictionary<string, string>();

                    using (StreamReader fileStream = new StreamReader(filename) )
                    {
                        string currentArchiveName = null;
                        string currentPromotionGroup = null;
                        while ( !fileStream.EndOfStream)
                        {
                            string fileLine = fileStream.ReadLine().Trim();

                            if ((issueNumber == "*") || (fileLine.IndexOf(issueNumber) >= 0))
                            {
                                // Only process the Archive if the Issue Number is the same or any Issue Number will do

                                string archiveName;
                                string archivePromotionGroup;
                                if (fileLine[0] != '"')
                                {
                                    // Spaces in the filename

                                    string[] archiveDetailPart = fileLine.Split(new char[] {' '},
                                                                                StringSplitOptions.RemoveEmptyEntries);

                                    archiveName = archiveDetailPart[0];
                                    archivePromotionGroup = archiveDetailPart[2];

                                } // Spaces in the filename
                                else
                                {
                                    // No spaces in the filename

                                    int endDoubleQuoteIndex = fileLine.IndexOf('"', 1);

                                    archiveName = fileLine.Substring(1, endDoubleQuoteIndex - 1);

                                    string[] archiveDetailPart =
                                        fileLine.Substring(endDoubleQuoteIndex + 1).Split(new char[] {' '},
                                                                                          StringSplitOptions.
                                                                                              RemoveEmptyEntries);

                                    archivePromotionGroup = archiveDetailPart[1];

                                } // No spaces in the filename

                                if (currentArchiveName == null)
                                {
                                    // No archive name so far
                                    currentArchiveName = archiveName;
                                    currentPromotionGroup = archivePromotionGroup;
                                }
                                else
                                {
                                    // Existing current archive

                                    if (String.Compare(currentArchiveName, archiveName, /*ignore case*/true) != 0)
                                    {
                                        // Change Archive
                                        currentArchiveName = archiveName;
                                        currentPromotionGroup = archivePromotionGroup;
                                    }

                                } // Existing current archive

                                // Allow for this Archive Entry to have further revisions above it
                                if ((String.Compare(archivePromotionGroup,requiredPromotionGroup) == 0)
                                    || (String.Compare(archivePromotionGroup, "[NoPromoGroup]") == 0)
                                        &&(String.Compare(currentPromotionGroup,requiredPromotionGroup)==0))
                                {
                                    if (!filenameSet.Contains(archiveName))
                                    {
                                        if (String.Compare(archivePromotionGroup, "[NoPromoGroup]") != 0)
                                        {
                                            // This change is not "buried" in this archive
                                            filenameSet.Add(archiveName);
                                            archivePromoteDictionary.Add(archiveName, fileLine);
                                        }
                                        else
                                        {
                                            filenameSet.Add(archiveName);
                                            archivePromoteDictionary.Add(archiveName, "# Buried : " + fileLine);
                                        }
                                    }
                                }

                            } // Only process the Archive if the Issue Number is the same or any Issue Number will do

                        }
                    }

                    Console.WriteLine("Unique Archive Names at {0} for Issue Number {1} is {2}", requiredPromotionGroup, issueNumber, filenameSet.Count);
                    foreach( string filenameEntry in filenameSet)
                    {
                        Console.WriteLine("    {0}",filenameEntry);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Promotion List from {0} contains {1} entries:", requiredPromotionGroup, archivePromoteDictionary.Count);
                    foreach ( KeyValuePair<string,string> archiveEntry in archivePromoteDictionary)
                    {
                        Console.WriteLine("    {0}",archiveEntry.Value);
                    }
                }
            }
        }
    }
}
