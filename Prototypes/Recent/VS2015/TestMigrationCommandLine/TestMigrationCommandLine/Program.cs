using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMigrationCommandLine
{
    class Program
    {

        private class PvcsCompleteSystemArchiveDetail
        {
            public enum PvcsArchiveDetailLevel
            {
                ChangesOnly,
                AllArchives
            }

            public PvcsCompleteSystemArchiveDetail(string fullReportPath, PvcsArchiveDetailLevel pvcsArchiveDetailLevel)
            {

            }

            public void Display()
            {
            }

            public void GeneratePromotionGroupLists()
            {

            }

        } // PvcsCompleteSystemArchiveDetail

        private const int _errorSuccess = 0;
        private const int _errorFileNotFound = 1;
        private const int _errorFileUnableToBeOpened = 2;
        private const int _errorGeneralError = 3;
        private const int _errorMigrationFailed = 4;
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

        private static int Main(string[] args)
        {
            int error = _errorSuccess;

            if (args.Length >= 1)
            {
                // The first argument must be a directory path to the PVCS Archive Reports
                string fullReportPath = IdentifyFullPromotionGroupReportPath(args[0]);
                string selectedIssueNumber = null;
                string gitRepositoryDirectoryPath = null;

                if (args.Length == 1)
                {
                    // Assume a Git Repository import from PVCS
                    gitRepositoryDirectoryPath = "d:\\Repos\\Heritage";
                    Console.WriteLine("Defaulting to import from PVCS to a Git Repository located in \"{0}\"", gitRepositoryDirectoryPath);
                }
                else
                {
                    // More than one argument

                    try
                    {
                        FileAttributes fileAttributes = File.GetAttributes(args[1]);
                        if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            // It's an existing directory so assume a Git Repository import from PVCS
                            // into a Repository in that directory
                            gitRepositoryDirectoryPath = args[1];
                            Console.WriteLine("Importing from PVCS to a Git Repository located in \"{0}\"", gitRepositoryDirectoryPath);
                        }
                        else
                        {
                            // Assume it's an issue number
                            selectedIssueNumber = args[1];
                            Console.WriteLine("Analysing the status of Issue Number \"{0}\"", selectedIssueNumber);
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        // Assume it's an issue number
                        selectedIssueNumber = args[1];
                        Console.WriteLine("Analysing the status of Issue Number \"{0}\"", selectedIssueNumber);
                    }

                } // More than one argument

                Console.WriteLine();
                Console.WriteLine("Promotion Group Report Directory is \"{0}\"", fullReportPath);

                try
                {

                    PvcsCompleteSystemArchiveDetail pvcsCompleteSystemArchiveDetail = new PvcsCompleteSystemArchiveDetail(
                        fullReportPath,
                        PvcsCompleteSystemArchiveDetail.PvcsArchiveDetailLevel.AllArchives);

                    pvcsCompleteSystemArchiveDetail.Display();
                    pvcsCompleteSystemArchiveDetail.GeneratePromotionGroupLists();

                    if (gitRepositoryDirectoryPath != null)
                    {
                        // Git Repository import from PVCS
                        error = PerformPVCStoGitMigration(pvcsCompleteSystemArchiveDetail, gitRepositoryDirectoryPath);
                    }
                    else
                    {
                        // An Issue Number has been supplied
                        error = PerformIssueAnalysis(pvcsCompleteSystemArchiveDetail, selectedIssueNumber);
                    }

                }
                catch (FileNotFoundException exFileNotFound)
                {
                    Console.WriteLine();
                    Console.WriteLine("File \"{0}\" was not found", exFileNotFound.Message);
                    error = _errorFileNotFound;
                }
                catch (PvcsCompleteSystemArchiveDetail.UnableToOpenFileException exFileCantOpen)
                {
                    Console.WriteLine();
                    Console.WriteLine("Unable to open file \"{0}\"", exFileCantOpen.Message);
                    error = _errorFileUnableToBeOpened;
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine("Exception : \"{0}\"", ex.ToString());
                    error = _errorGeneralError;
                }

            } // There are arguments

            return error;

        } // Main
    }
}
