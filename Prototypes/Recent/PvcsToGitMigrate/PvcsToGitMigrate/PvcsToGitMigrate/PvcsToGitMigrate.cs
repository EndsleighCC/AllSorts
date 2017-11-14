using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using CommandOperations;
using GitChangeControl;
using PvcsChangeControl;
using DisplayHelper;

namespace PvcsToGitMigrateProgram
{
    class PvcsToGitMigrateProgram
    {
        static bool MigratePvcsArchiveSourcestoGit(PvcsCompleteSystemArchiveDetail pvcsCompleteSystemArchiveDetail,
                                                   SortedSet<string> sortedSetPromotionGroupExclude,
                                                   string rootWorkingDirectory,
                                                   bool useExistingGitRepository)
        {
            bool success = false;
            
            const CommandOperation.DebugProgress debugProgress = CommandOperation.DebugProgress.None;
            const int highLevelIndent = 0;

            PvcsPromotionGroupDetailCollection pvcsPromotionGroupDetailCollection = new PvcsPromotionGroupDetailCollection();

            bool createRepositoryAndPerformInitialCommit = ! useExistingGitRepository;

            // Assume that if using the existing Repository the user knows that the branch structure already exists

            bool importPvcsProduction = ! sortedSetPromotionGroupExclude.Contains(PvcsPromotionGroupDetailCollection.ProductionPromotionGroupName);

            // It only makes sense to import this Promotion Group if all higher Promotion Groups have been imported
            // unless the existing Git Repository already contains those higher Promotion Groups
            bool importPvcsPreProduction = 
                   ( (importPvcsProduction) || ( useExistingGitRepository /* Higher Promotion Group must already be present */ ) )
                && ( ! sortedSetPromotionGroupExclude.Contains(PvcsPromotionGroupDetailCollection.PreProductionPromotionGroupName) );

            // It only makes sense to import this Promotion Group if all higher Promotion Groups have been imported
            // unless the existing Git Repository already contains those higher Promotion Groups
            bool importPvcsUserTest =
                   ( (importPvcsPreProduction) || (useExistingGitRepository /* Higher Promotion Group must already be present */ ) )
                && ( ! sortedSetPromotionGroupExclude.Contains(PvcsPromotionGroupDetailCollection.UserTestPromotionGroupName) );

            // It only makes sense to import this Promotion Group if all higher Promotion Groups have been imported
            // unless the existing Git Repository already contains those higher Promotion Groups
            bool importPvcsSystemTest =
                   ( ( importPvcsUserTest) || (useExistingGitRepository /* Higher Promotion Group must already be present */ ) )
                && ( ! sortedSetPromotionGroupExclude.Contains(PvcsPromotionGroupDetailCollection.SystemTestPromotionGroupName) ) ;

            Console.WriteLine();
            ConsoleDisplay.WritelineWithUnderline("Importing PVCS Archive Revisions for PVCS Promotion Groups into a single Git Repository");

            if (createRepositoryAndPerformInitialCommit)
            {
                success = CreateRepositoryAndPerformInitialCommit(rootWorkingDirectory, highLevelIndent, debugProgress);
            }
            else
            {
                // Indicate success so that subsequent steps execute
                success = true;
            }

            if (success)
            {
                string promotionGroupName = PvcsPromotionGroupDetailCollection.ProductionPromotionGroupName;
                bool importPromotionGroup = importPvcsProduction;
                success = GitOperation.ImportBranchFromPvcs(
                                            pvcsCompleteSystemArchiveDetail,
                                            promotionGroupName,
                                            importPromotionGroup,
                                            pvcsPromotionGroupDetailCollection.PromotionGroupNetworkShareName(promotionGroupName),
                                            pvcsPromotionGroupDetailCollection.GitBranchName(promotionGroupName),
                                            rootWorkingDirectory,
                                            String.Format("Initial PVCS {0} commit to Git {1}",
                                                promotionGroupName,
                                                pvcsPromotionGroupDetailCollection.GitBranchName(promotionGroupName)),
                                            highLevelIndent,
                                            debugProgress);
            }

            if (success)
            {
                string promotionGroupName = PvcsPromotionGroupDetailCollection.PreProductionPromotionGroupName;
                bool importPromotionGroup = importPvcsPreProduction;
                success = GitOperation.ImportBranchFromPvcs(
                                            pvcsCompleteSystemArchiveDetail,
                                            promotionGroupName,
                                            importPromotionGroup,
                                            pvcsPromotionGroupDetailCollection.PromotionGroupNetworkShareName(promotionGroupName),
                                            pvcsPromotionGroupDetailCollection.GitBranchName(promotionGroupName),
                                            rootWorkingDirectory,
                                            String.Format("Initial PVCS {0} commit to Git {1}",
                                                promotionGroupName,
                                                pvcsPromotionGroupDetailCollection.GitBranchName(promotionGroupName)),
                                            highLevelIndent,
                                            debugProgress);
            }

            if (success)
            {
                string promotionGroupName = PvcsPromotionGroupDetailCollection.UserTestPromotionGroupName;
                bool importPromotionGroup = importPvcsUserTest;
                success = GitOperation.ImportBranchFromPvcs(
                                            pvcsCompleteSystemArchiveDetail,
                                            promotionGroupName,
                                            importPromotionGroup,
                                            pvcsPromotionGroupDetailCollection.PromotionGroupNetworkShareName(promotionGroupName),
                                            pvcsPromotionGroupDetailCollection.GitBranchName(promotionGroupName),
                                            rootWorkingDirectory,
                                            String.Format("Initial PVCS {0} commit to Git {1}",
                                                promotionGroupName,
                                                pvcsPromotionGroupDetailCollection.GitBranchName(promotionGroupName)),
                                            highLevelIndent,
                                            debugProgress);
            }

            if (success)
            {
                string promotionGroupName = PvcsPromotionGroupDetailCollection.SystemTestPromotionGroupName;
                bool importPromotionGroup = importPvcsSystemTest;
                success = GitOperation.ImportBranchFromPvcs(
                                            pvcsCompleteSystemArchiveDetail,
                                            promotionGroupName,
                                            importPromotionGroup,
                                            pvcsPromotionGroupDetailCollection.PromotionGroupNetworkShareName(promotionGroupName),
                                            pvcsPromotionGroupDetailCollection.GitBranchName(promotionGroupName),
                                            rootWorkingDirectory,
                                            String.Format("Initial PVCS {0} commit to Git {1}",
                                                promotionGroupName,
                                                pvcsPromotionGroupDetailCollection.GitBranchName(promotionGroupName)),
                                            highLevelIndent,
                                            debugProgress);
            }

            if (success)
            {
                for (int pvcsSystemTestIndex = PvcsPromotionGroupDetailCollection.FirstLowerSystemTestNumber;
                     (success) && (pvcsSystemTestIndex <= PvcsPromotionGroupDetailCollection.LastLowerSystemTestNumber);
                     ++pvcsSystemTestIndex)
                {
                    string pvcsBranchName = String.Format("{0}{1}",
                        PvcsPromotionGroupDetailCollection.SystemTestPromotionGroupName, pvcsSystemTestIndex);
                    string pvcsShareName = pvcsPromotionGroupDetailCollection.PromotionGroupNetworkShareName(pvcsBranchName);
                    string gitBranchName = pvcsPromotionGroupDetailCollection.GitBranchName(pvcsBranchName);
                    string gitBranchCommitComment = String.Format("Initial PVCS {0} commit to Git {1}", pvcsBranchName, gitBranchName);

                    // Ensure that the new Git Branch comes off Git Integration Test
                    if (!GitOperation.CheckedOutBranchIs(pvcsPromotionGroupDetailCollection.GitBranchName(PvcsPromotionGroupDetailCollection.SystemTestPromotionGroupName),
                                                         rootWorkingDirectory)
                       )
                    {
                        // Switch (back) to the Integration Test Branch
                        success = GitOperation.CheckOutBranch(pvcsPromotionGroupDetailCollection.GitBranchName(PvcsPromotionGroupDetailCollection.SystemTestPromotionGroupName),
                                                              rootWorkingDirectory, highLevelIndent, debugProgress);
                    }

                    if (success)
                    {
                        // It only makes sense to import this Promotion Group if all higher Promotion Groups have been imported
                        // unless the existing Git Repository already contains those higher Promotion Groups
                        bool importPvcsLowerSystemTest =
                               ((importPvcsSystemTest) || (useExistingGitRepository /* Higher Promotion Group must already be present */ ))
                            && (!sortedSetPromotionGroupExclude.Contains(pvcsBranchName));
                        success = GitOperation.ImportBranchFromPvcs(pvcsCompleteSystemArchiveDetail,
                                                                    pvcsBranchName,
                                                                    importPvcsLowerSystemTest,
                                                                    pvcsShareName,
                                                                    gitBranchName,
                                                                    rootWorkingDirectory,
                                                                    gitBranchCommitComment,
                                                                    highLevelIndent,
                                                                    debugProgress);
                    }
                } // for pvcsSystemTestIndex
            }

            if (success)
            {
                // Switch to the Integration Test Branch when everything is done
                success = GitOperation.CheckOutBranch(pvcsPromotionGroupDetailCollection.GitBranchName(PvcsPromotionGroupDetailCollection.SystemTestPromotionGroupName),
                                                      rootWorkingDirectory, highLevelIndent,debugProgress);
            }

            return success;

        } // MigratePvcsArchiveSourcestoGit

        static bool MigratePvcsArchiveSourcestoGit(PvcsCompleteSystemArchiveDetail pvcsCompleteSystemArchiveDetail,
                                                   string singlePromotionGroupName,
                                                   string rootWorkingDirectory)
        {
            bool success = false;

            const CommandOperation.DebugProgress debugProgress = CommandOperation.DebugProgress.None;
            const int highLevelIndent = 0;

            PvcsPromotionGroupDetailCollection pvcsPromotionGroupDetailCollection = new PvcsPromotionGroupDetailCollection();
            // ============================================================================================================
            // For this to work, the Repository must have the branch from which this Promotion Group is to come checked out
            // ============================================================================================================
            success = GitOperation.ImportBranchFromPvcs(pvcsCompleteSystemArchiveDetail,
                                        singlePromotionGroupName,
                                        true,
                                        pvcsPromotionGroupDetailCollection.PromotionGroupNetworkShareName(singlePromotionGroupName),
                                        pvcsPromotionGroupDetailCollection.GitBranchName(singlePromotionGroupName),
                                        rootWorkingDirectory,
                                        String.Format("Initial PVCS {0} commit to Git {1}",
                                            singlePromotionGroupName,
                                            pvcsPromotionGroupDetailCollection.GitBranchName(singlePromotionGroupName)),
                                        highLevelIndent,
                                        debugProgress);

            return success;
        } // MigratePvcsArchiveSourcestoGit

        static bool CreateRepositoryAndPerformInitialCommit(string rootWorkingDirectory,
                                                            int indent,
                                                            CommandOperation.DebugProgress debugProgress)
        {
            bool success = false;

            DirectoryInfo directoryInfo = new DirectoryInfo(rootWorkingDirectory);
            if ( ! directoryInfo.Exists)
            {
                Console.WriteLine();
                Console.WriteLine("{0}Local Git Repository Directory \"{1}\" does not exist",
                                    ConsoleDisplay.Indent(indent),rootWorkingDirectory);
                success = false;
            }
            else
            {
                // Root working directory exists

                Console.WriteLine();
                Console.WriteLine("{0}Local Git Repository Directory is \"{1}\"",
                                    ConsoleDisplay.Indent(indent),rootWorkingDirectory);
            
                // Create the Git Repository
                success = GitOperation.Init(rootWorkingDirectory, indent, debugProgress);
                if (success)
                {
                    // Repository was initialised successfully

                    const string readMeFilename = "readme.txt";
                    const string initialCommitMessage = "Initial commit to create Branch master";

                    string readMeFileFullPathAndFilename = Path.Combine(rootWorkingDirectory, readMeFilename);

                    try
                    {
                        using (System.IO.StreamWriter readMeFile = new System.IO.StreamWriter(readMeFileFullPathAndFilename, true))
                        {
                            string[] readMeLines = { "Heritage Source Git Repository",
                                                     "Contains all the sources imported from the PVCS Change Control System" };
                            foreach (string readMeLine in readMeLines)
                            {
                                readMeFile.WriteLine(readMeLine);
                            }

                            DateTime datetimeNow = DateTime.Now;
                            readMeFile.WriteLine("Git Repository created {0} {1}", datetimeNow.ToString("D"), datetimeNow.ToString("T"));
                        }
                        success = true;
                    }
                    catch (Exception eek)
                    {
                        success = false;
                        Console.WriteLine("Exception writing file \"{0}\" = {1}", readMeFileFullPathAndFilename, eek.ToString());
                    }

                    if (success)
                    {
                        // Add the file to the Repo
                        success = GitOperation.Add(rootWorkingDirectory, readMeFilename, indent, debugProgress);
                        if (success)
                        {
                            success = GitOperation.Commit(initialCommitMessage, rootWorkingDirectory, indent, debugProgress);
                            if (success)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Committed initial file \"{0}\"", readMeFileFullPathAndFilename);
                            }
                        }
                    }

                } // Repository was initialised successfully

            } // Root working directory exists

            return success;

        } // CreateRepositoryAndPerformInitialCommit

        private static void DisplayPvcsSourcesForIssue(PvcsCompleteSystemArchiveDetail pvcsCompleteSystemArchiveDetail, string selectedIssueNumber)
        {
            SortedSet<string> additionalIssueNumberCollection = new SortedSet<string>();

            pvcsCompleteSystemArchiveDetail.CheckDescendents(selectedIssueNumber, "System_Test",
                additionalIssueNumberCollection);

            if (additionalIssueNumberCollection.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("*** No Issue Numbers in addtion to {0} were found", selectedIssueNumber);
            }
            else
            {
                string heading = String.Format("Issue Numbers Found in Addition to {0}", selectedIssueNumber);
                Console.WriteLine();
                ConsoleDisplay.WritelineWithUnderline(heading);
                foreach (string issueNumber in additionalIssueNumberCollection)
                {
                    Console.WriteLine("{0}{1}", ConsoleDisplay.Indent(1), issueNumber);
                }
            }

            string promotionGroup = "System_Test";
            Console.WriteLine();
            string promotionHeading = String.Format("Promotion List for Issue Number {0} at {1}",
                selectedIssueNumber, promotionGroup);
            ConsoleDisplay.WritelineWithUnderline(promotionHeading);
            pvcsCompleteSystemArchiveDetail.GeneratePromotionListForIssue(selectedIssueNumber, promotionGroup);

            // Assume already at User_Test. Bad but good enough for now (forever?)
            pvcsCompleteSystemArchiveDetail.CheckBuriedPromotionGroup("System_Test");
            
        } // DisplayPvcsSourcesForIssue

        private static string IdentifyFullPromotionGroupReportPath( string reportPath )
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

        private const int _errorSuccess = 0;
        private const int _errorFileNotFound = 1;
        private const int _errorFileUnableToBeOpened = 2;
        private const int _errorGeneralError = 3;
        private const int _errorMigrationFailed = 4;

        private static int PerformPVCStoGitMigration(PvcsCompleteSystemArchiveDetail pvcsCompleteSystemArchiveDetail,
                                                     SortedSet<string> sortedSetPromotionGroupExclude ,
                                                     string gitRepositoryDirectoryPath,
                                                     bool useExistingGitRepository)
        {
            int error = _errorSuccess;

            bool migrationSuccess = false;

            // Start timing the migration into Git from PVCS
            Stopwatch stopwatchMigration = new Stopwatch();

            stopwatchMigration.Start();

            try
            {
                migrationSuccess = MigratePvcsArchiveSourcestoGit(pvcsCompleteSystemArchiveDetail,
                                                                  sortedSetPromotionGroupExclude,
                                                                  gitRepositoryDirectoryPath,
                                                                  useExistingGitRepository);

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

            // Stop timing
            stopwatchMigration.Stop();

            // Get the elapsed time as a TimeSpan value.
            TimeSpan timespanMigration = stopwatchMigration.Elapsed;
            // Format and display the TimeSpan value. 
            string elapsedTimeMigration = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                                timespanMigration.Hours,
                                                timespanMigration.Minutes,
                                                timespanMigration.Seconds,
                                                timespanMigration.Milliseconds / 10);
            if (migrationSuccess)
            {
                Console.WriteLine();
                Console.WriteLine("Migration from PVCS to Git is complete. Elapsed time {0}", elapsedTimeMigration);
                error = _errorSuccess;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Migration from PVCS to Git failed. Elapsed time {0}", elapsedTimeMigration);
                error = _errorMigrationFailed;
            }

            return error ;
        } // PerformPVCStoGitMigration

        private static int PerformPVCStoGitMigration(PvcsCompleteSystemArchiveDetail pvcsCompleteSystemArchiveDetail,
                                                     string singlePromotionGroupName,
                                                     string gitRepositoryDirectoryPath)
        {
            int error = _errorSuccess;

            bool migrationSuccess = false;

            // Start timing the migration into Git from PVCS
            Stopwatch stopwatchMigration = new Stopwatch();

            stopwatchMigration.Start();

            try
            {
                migrationSuccess = MigratePvcsArchiveSourcestoGit(pvcsCompleteSystemArchiveDetail,
                                                                  singlePromotionGroupName,
                                                                  gitRepositoryDirectoryPath);

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

            // Stop timing
            stopwatchMigration.Stop();

            // Get the elapsed time as a TimeSpan value.
            TimeSpan timespanMigration = stopwatchMigration.Elapsed;
            // Format and display the TimeSpan value. 
            string elapsedTimeMigration = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                                timespanMigration.Hours,
                                                timespanMigration.Minutes,
                                                timespanMigration.Seconds,
                                                timespanMigration.Milliseconds / 10);
            if (migrationSuccess)
            {
                Console.WriteLine();
                Console.WriteLine("Partial migration from PVCS to Git is complete. Elapsed time {0}", elapsedTimeMigration);
                error = _errorSuccess;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Partial migration from PVCS to Git failed. Elapsed time {0}", elapsedTimeMigration);
                error = _errorMigrationFailed;
            }

            return error;
        } // PerformPVCStoGitMigration


        private static int PerformIssueAnalysis(PvcsCompleteSystemArchiveDetail pvcsCompleteSystemArchiveDetail, string selectedIssueNumber)
        {
            DisplayPvcsSourcesForIssue(pvcsCompleteSystemArchiveDetail, selectedIssueNumber);
            return _errorSuccess;
        }

        private static SortedSet<string> _sortedSetPromotionGroupExclude = new SortedSet<string>();

        private static int Main(string[] args)
        {
            int error = _errorSuccess;

            string fullReportPath = null; // Mandatory
            string gitRepositoryDirectoryPath = null; // Optional - defaulted
            bool useExistingGitRepository = false;
            string importSinglePromotionGroupName = null;

            foreach ( string argValue in args)
            {
                if ( argValue.StartsWith("/"))
                {
                    // Value is a switch

                    switch (argValue.ToLower()[1])
                    {
                        case 'e':
                            Console.WriteLine("Use existing Git Repository selected");
                            useExistingGitRepository = true;
                            break;
                        case 'i':
                            if ( ! useExistingGitRepository )
                            {
                                Console.WriteLine("A single Promotion Group import cannot be performed unless using an existing Git Repository");
                            }
                            else
                            {
                                // /x:PromotionGroupName
                                // 0123...
                                string possiblePromotionGroupName = argValue.Substring(3);
                                if (new PvcsPromotionGroupDetailCollection().PromotionGroupNameIsValid(possiblePromotionGroupName))
                                {
                                    importSinglePromotionGroupName = possiblePromotionGroupName;
                                    Console.WriteLine("PVCS Import includes only Promotion Group \"{0}\"", importSinglePromotionGroupName);
                                }
                            }
                            break;
                        case 'x':
                            {
                                // /x:PromotionGroupName
                                // 0123...
                                string possiblePromotionGroupName = argValue.Substring(3);
                                if (new PvcsPromotionGroupDetailCollection().PromotionGroupNameIsValid(possiblePromotionGroupName))
                                {
                                    if (!_sortedSetPromotionGroupExclude.Contains(possiblePromotionGroupName))
                                    {
                                        _sortedSetPromotionGroupExclude.Add(possiblePromotionGroupName);
                                        Console.WriteLine("PVCS Import excludes Promotion Group \"{0}\" and all lower Promotion Groups", possiblePromotionGroupName);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Promotion Group Name \"{0}\" is invalid)", possiblePromotionGroupName);
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("Unknown switch \"{0}\"", argValue);
                            break;
                    } // switch

                } // Value is a switch
                else
                {
                    if ( fullReportPath == null )
                    {
                        fullReportPath = IdentifyFullPromotionGroupReportPath(argValue);
                        Console.WriteLine("Promotion Group Report Directory is \"{0}\"", fullReportPath);
                    }
                    else if ( gitRepositoryDirectoryPath == null )
                    {
                        gitRepositoryDirectoryPath = argValue;
                        Console.WriteLine("Importing from PVCS to a Git Repository located in \"{0}\"", gitRepositoryDirectoryPath);
                    }
                    else
                    {
                        Console.WriteLine("Unexpected argument \"{0}\"", argValue);
                    }
                }
            }

            if (fullReportPath == null)
            {
                Console.WriteLine();
                Console.WriteLine("The PVCS Archive Report Path is mandatory but was not supplied");
            }
            else
            {
                // PVCS Archive Report path was supplied

                if (gitRepositoryDirectoryPath == null)
                {
                    // Assume a Git Repository import from PVCS
                    gitRepositoryDirectoryPath = "d:\\Repos\\Heritage";
                    Console.WriteLine("Defaulting to import from PVCS to a Git Repository located in \"{0}\"", gitRepositoryDirectoryPath);
                }

                Console.WriteLine();
                Console.WriteLine("Promotion Group Report Directory is \"{0}\"",fullReportPath) ;

                try
                {

                    PvcsCompleteSystemArchiveDetail pvcsCompleteSystemArchiveDetail = new PvcsCompleteSystemArchiveDetail(
                        fullReportPath,
                        PvcsCompleteSystemArchiveDetail.PvcsArchiveDetailLevel.AllArchives);

                    pvcsCompleteSystemArchiveDetail.Display();
                    pvcsCompleteSystemArchiveDetail.GeneratePromotionGroupLists();

                    if (importSinglePromotionGroupName == null )
                    {
                        // Git Repository import from PVCS
                        error = PerformPVCStoGitMigration(pvcsCompleteSystemArchiveDetail,
                                                          _sortedSetPromotionGroupExclude,
                                                          gitRepositoryDirectoryPath,
                                                          useExistingGitRepository);
                    }
                    else
                    {
                        error = PerformPVCStoGitMigration(pvcsCompleteSystemArchiveDetail,
                                                          importSinglePromotionGroupName,
                                                          gitRepositoryDirectoryPath);
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

            } // PVCS Archive Report path was supplied

            return error;

        } // Main

    } // Program

} // PvcsToGitMigrate
