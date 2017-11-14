using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text.RegularExpressions;

namespace CompareSourceVolumes
{
    partial class Program
    {
        private static string IdentifyFullPromotionGroupReportPath(string reportPath)
        {
            string fullReportPath = null;
            // Regular Expression that matches a Report Directory Name
            const string reportDirectoryName = "[0-9]-[0-9][0-9]-[0-9][0-9]-[0-9][0-9]";
            // The supplied path may be a Report Directory already
            DirectoryInfo reportPathDirectoryInfo = new DirectoryInfo(reportPath);
            if (!reportPathDirectoryInfo.Exists)
            {
                // Supplied directory does not exist

                // Just return the supplied value
                fullReportPath = reportPath;

            } // Supplied directory does not exist
            else
            {
                // Supplied directory exists

                if (Regex.IsMatch(reportPathDirectoryInfo.Name, reportDirectoryName))
                {
                    // The Report Directory has been explicitly specified
                    fullReportPath = reportPath;
                }
                else
                {
                    // Not a Report Directory but should be the parent directory of all Report Directories

                    DirectoryInfo[] reportDirectories = new DirectoryInfo(reportPath).GetDirectories();

                    DirectoryInfo highestReportDirectory = null;

                    for (int index = 0; index < reportDirectories.Length; ++index)
                    {
                        if (Regex.IsMatch(reportDirectories[index].Name, reportDirectoryName))
                        {
                            // It must be a Report Directory
                            if (highestReportDirectory == null)
                            {
                                highestReportDirectory = reportDirectories[index];
                            }
                            else
                            {
                                // Check whether it's higher
                                if (String.Compare(reportDirectories[index].Name, highestReportDirectory.Name) > 0)
                                {
                                    // If it is then replace the current one
                                    highestReportDirectory = reportDirectories[index];
                                    fullReportPath = highestReportDirectory.FullName;
                                } // Check whether it's higher
                            }
                        } // It must be a Report Directory

                    } // for

                } // Not a Report Directory but should be the parent directory of all Report Directories

            } // Supplied directory exists
            return fullReportPath;
        } // IdentifyFullPromotionGroupReportPath

        static int Main(string[] args)
        {
            int error = 0;

            if (args.Length < 3)
            {
                Console.WriteLine();
                Console.WriteLine("CompareSourceVolumes reportPath rootPath1 rooPath2");
            }
            else
            {
                // Sufficient parameters supplied

                string pvcsPromotionGroupReportPath = args[0].Trim();
                string rootPath1 = args[1].Trim();
                string rootPath2 = args[2].Trim();

                string fullPvcsReportPath = null;

                if (!Directory.Exists(pvcsPromotionGroupReportPath))
                {
                    Console.WriteLine("PVCS Promotion Group Report Path \"{0}\" does not exist", pvcsPromotionGroupReportPath);
                    error = 1;
                }
                else
                {
                    fullPvcsReportPath = IdentifyFullPromotionGroupReportPath(pvcsPromotionGroupReportPath);
                }

                if (!Directory.Exists(rootPath1) )
                {
                    Console.WriteLine("Root Path \"{0}\" does not exist", rootPath1);
                    error = 1;
                }

                if (!Directory.Exists(rootPath2) )
                {
                    Console.WriteLine("Root Path \"{0}\" does not exist", rootPath2);
                    error = 1;
                }

                if ( error == 0 )
                {
                    // All information is available

                    Console.WriteLine();
                    Console.WriteLine("PVCS Promotion Group Report directory is \"{0}\"", fullPvcsReportPath);
                    Console.WriteLine("Root Path 1 = \"{0}\"", rootPath1);
                    Console.WriteLine("Root Path 2 = \"{0}\"", rootPath2);

                    PvcsArchiveRevisionDetailCollection pvcsArchiveRevisionDetailCollection
                        = new PvcsArchiveRevisionDetailCollection(fullPvcsReportPath);

                    // Remove any trailing backslashes
                    rootPath1 = rootPath1.TrimEnd('\\');
                    rootPath2 = rootPath2.TrimEnd('\\');

                    SortedSet<string> comparedArchiveNames = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);
                    int compareCount = 0;
                    int differenceCount = 0;

                    Console.WriteLine();
                    foreach (PvcsArchiveRevisionDetail pvcsArchiveRevisionDetail in pvcsArchiveRevisionDetailCollection)
                    {
                        if ( ! comparedArchiveNames.Contains(pvcsArchiveRevisionDetail.ArchiveName ))
                        {
                            // Not previously compared
                            compareCount += 1;

                            Console.WriteLine("{0} : {1:00000} : \"{2}\"",
                                DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"),
                                compareCount,
                                pvcsArchiveRevisionDetail.ArchiveName);

                            comparedArchiveNames.Add(pvcsArchiveRevisionDetail.ArchiveName);

                            FileInfo fileInfo = new FileInfo(pvcsArchiveRevisionDetail.ArchiveName);

                            string relativeArchivePath = pvcsArchiveRevisionDetail.ArchiveName.Substring(3);

                            string pathAndFilename1 = Path.Combine( rootPath1 , relativeArchivePath) ;
                            string pathAndFilename2 = Path.Combine(rootPath2, relativeArchivePath);

                            // Compare Solution Files not as just a blob
                            if ( String.Compare( fileInfo.Extension , ".sln", true /*ignore case*/) == 0 )
                            {
                                // Compare as solutions
                                bool exactlyEquivalent = false;
                                bool semanticallyEquivalent = false ;
                                CompareSolutions.Compare(relativeArchivePath, rootPath1, rootPath2, ref exactlyEquivalent, ref semanticallyEquivalent);

                                if ( semanticallyEquivalent )
                                {
                                    // Console.WriteLine("Solution \"{0}\" in \"{1}\" and \"{2}\" are semantically equivalent", relativeArchivePath, rootPath1, rootPath2);
                                }
                                else
                                {
                                    differenceCount += 1;
                                    Console.WriteLine("Solution \"{0}\" in \"{1}\" and \"{2}\" are not semantically equivalent", relativeArchivePath, rootPath1, rootPath2);
                                }

                            } // Compare as solutions
                            else
                            {
                                // Compare as simple files

                                BigInteger fileHash1 = new NonEolHash(pathAndFilename1).Generate();
                                BigInteger fileHash2 = new NonEolHash(pathAndFilename2).Generate();

                                if ( fileHash1 == fileHash2 )
                                {
                                    // Console.WriteLine("Files \"{0}\" in \"{1}\" and \"{2}\" are identical", relativeArchivePath, rootPath1, rootPath2);
                                }
                                else
                                {
                                    differenceCount += 1;
                                    Console.WriteLine("Files \"{0}\" in \"{1}\" and \"{2}\" are different", relativeArchivePath, rootPath1, rootPath2);
                                }

                            } // Compare as simple files

                        } // Not previously compared

                    } // foreach

                    Console.WriteLine();
                    Console.WriteLine("{0} : Total count of files compared was {1}. Total count of differences detected was {2}",
                                            DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss") ,
                                            compareCount,
                                            differenceCount);

                    if (differenceCount > 0)
                    {
                        // Return differences as an error
                        error = 2;
                    }

                } // All information is available

            } // Sufficient parameters supplied

            return error;
        } // Main
    }
}
