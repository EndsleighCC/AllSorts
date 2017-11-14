using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandOperations;
using PvcsChangeControl;
using DisplayHelper;

namespace GitChangeControl
{
    public static class GitOperation
    {
        public const string MasterBranchName = "master";

        public static bool Init(string rootWorkingDirectory, int indent, CommandOperation.DebugProgress debugProgress)
        {
            bool success = false;
            string command;

            command = "git init";
            success = CommandOperation.RunMonitoredCommand(rootWorkingDirectory,
                                                    command,
                                                    indent,
                                                    debugProgress,
                                                    CommandOperation.CommandOutputDisplayType.StandardOutputOnly,
                                                    null,
                                                    null);

            return success;
        }

        // Perform a simple Git Status
        public static bool GitStatus(string rootWorkingDirectory, int indent, CommandOperation.DebugProgress debugProgress)
        {
            bool success = false;
            string command;

            command = "git status";
            success = CommandOperation.RunMonitoredCommand(  rootWorkingDirectory,
                                                    command,
                                                    indent,
                                                    debugProgress,
                                                    CommandOperation.CommandOutputDisplayType.StandardOutputOnly,
                                                    null,
                                                    null);

            return success;
        } // GitStatus

        // Perform a Git Status and return the standard output and standard error to the caller for subsequent perocessing
        public static bool GitStatus(string rootWorkingDirectory, int indent, CommandOperation.DebugProgress debugProgress,List<string> standardOutput, List<string> errorOutput)
        {
            bool success = false;
            string command = "git status";
            success = CommandOperation.RunMonitoredCommand(  rootWorkingDirectory,
                                                    command,
                                                    indent,
                                                    debugProgress,
                                                    CommandOperation.CommandOutputDisplayType.StandardOutputOnly,
                                                    standardOutput,
                                                    errorOutput);

            return success;
        } // GitStatus

        public static bool CheckedOutBranchIs(string branchName, string rootWorkingDirectory)
        {
            bool isBranch = false;

            bool success = false;

            List<string> standardOutput = new List<string>();

            success = GitStatus(rootWorkingDirectory, 0, CommandOperation.DebugProgress.None , standardOutput, null);
            if (success)
            {
                string[] statusWord = standardOutput[0].Split(new char[] {' '});
                if (statusWord.Length >= 3)
                {
                    isBranch = String.Compare(statusWord[2], branchName) == 0;
                }
            }

            return isBranch;
        }

        public static bool CheckOutBranch(string branchName, string rootWorkingDirectory, int indent, CommandOperation.DebugProgress debugProgress)
        {
            bool success = false;

            string command = "git checkout " + branchName;
            success = CommandOperation.RunMonitoredCommand(rootWorkingDirectory,
                                                    command,
                                                    indent,
                                                    debugProgress,
                                                    CommandOperation.CommandOutputDisplayType.StandardOutputOnly);

            return success;
        }

        public static bool CreateAndSwitchToBranch(string branchName, string rootWorkingDirectory, int indent, CommandOperation.DebugProgress debugProgress)
        {
            bool success = false;

            if (CheckedOutBranchIs(branchName, rootWorkingDirectory))
            {
                // Already on the specified branch
                success = true;
            }
            else
            {
                // Create the branch

                List<string> standardOutput = new List<string>();
                List<string> standardError = new List<string>();

                string command = "git branch " + branchName;
                success = CommandOperation.RunMonitoredCommand(  rootWorkingDirectory,
                                                        command,
                                                        indent,
                                                        debugProgress,
                                                        CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError,
                                                        standardOutput ,
                                                        standardError );
                if ( ! success )
                {
                    // Check that the error was a real error and not just that the branch already existed
                    if (standardOutput.Count >= 1)
                    {
                        if (standardOutput[0].StartsWith("fatal: A branch named"))
                        {
                            Console.WriteLine("{0}The failure of the creation of branch \"{1}\" is not an error because a branch of that name already exists", ConsoleDisplay.Indent(indent), branchName);
                            success = true;
                        }
                    }
                }

                if (success)
                {
                    // Switch to the branch

                    success = CheckOutBranch(branchName, rootWorkingDirectory, indent, debugProgress);

                } // Switch to the branch

            } // Create the branch

            if (success)
            {
                Console.WriteLine("{0}Staging to branch \"{1}\"", ConsoleDisplay.Indent(indent+1),branchName);
            }
            else
            {
                Console.WriteLine("{0}*** Failed to switch to branch \"{1}\"", ConsoleDisplay.Indent(indent+1),branchName);
            }

            return success;

        } // CreateAndSwitchToBranch

        public static bool Add(string rootWorkingDirectory, string sourceRelativePath, int indent, CommandOperation.DebugProgress debugProgress)
        {
            bool success = false;
            // Add the file to the Repo
            string command = "git add -f \"" + sourceRelativePath + "\"";
            success = CommandOperation.RunMonitoredCommand(  rootWorkingDirectory,
                                                    command,
                                                    indent,
                                                    debugProgress,
                                                    CommandOperation.CommandOutputDisplayType.StandardOutputOnly);

            return success;
        } // Add

        public static bool Commit(  string description, 
                                    string rootWorkingDirectory,
                                    int indent,
                                    CommandOperation.DebugProgress debugProgress)
        {
            bool success = false;

            List<string> standardOutput = new List<string>();
            // Add the file to the Repo
            string command = "git commit -m \"" + description + "\"";
            success = CommandOperation.RunMonitoredCommand(  rootWorkingDirectory,
                                                    command,
                                                    indent,
                                                    debugProgress,
                                                    CommandOperation.CommandOutputDisplayType.StandardOutputOnly,
                                                    standardOutput,
                                                    null);
            if (! success)
            {
                // Try to work out what went wrong
                if (standardOutput.Count >= 2)
                {
                    if (    (standardOutput[1].IndexOf("nothing to commit, working directory clean") > -1)
                         || (standardOutput[1].IndexOf("nothing to commit, working tree clean") > -1)
                       )
                    {
                        Console.WriteLine("{0}The failure of the commit is not an error because there were no sources to commit", ConsoleDisplay.Indent(indent));
                        success = true;
                    }
                }

                if (!success)
                {
                    Console.WriteLine("Unexpected commit failure");
                }

            } // Try to work out what went wrong

            return success;
        } // Commit

        /// <summary>
        /// Import all the sources associated with a PVCS Promotion Group into a single git branch
        /// </summary>
        /// <param name="pvcsCompleteSystemArchiveDetail">Complete set of PVCS Archive Details with Promotion Group and revision details</param>
        /// <param name="promotionGroup">The particular Promotion Group to be imported</param>
        /// <param name="performImport">Whether or not to actually perform the import (true) or just create the branch (false)</param>
        /// <param name="promotionGroupSourceVolume">The location of the PVCS source revisions that correspond to the Promotion Group</param>
        /// <param name="branchName">The name of the Git Branch into which to perform the import</param>
        /// <param name="rootWorkingDirectory">The full path to the directory containing the Git Repository including working tree</param>
        /// <param name="commitMessage">The message to be placed on the Git Branch Commit</param>
        /// <param name="indent">The indent value of output</param>
        /// <param name="debugProgress">The progress debug output details</param>
        /// <returns></returns>
        public static bool ImportBranchFromPvcs(PvcsCompleteSystemArchiveDetail pvcsCompleteSystemArchiveDetail,
                                                string promotionGroup ,
                                                bool performImport ,
                                                string promotionGroupSourceVolume ,
                                                string branchName ,
                                                string rootWorkingDirectory,
                                                string commitMessage ,
                                                int indent,
                                                CommandOperation.DebugProgress debugProgress)
        {
            bool success = false;

            Console.WriteLine();
            Console.WriteLine("Importing PVCS \"{0}\" Source Revisions from \"{1}\" to Repository \"{2}\" branch \"{3}\"",
                promotionGroup,promotionGroupSourceVolume,rootWorkingDirectory,branchName);

            // Check that the working directory has/is a Git Repository
            success = GitOperation.GitStatus(rootWorkingDirectory, indent, debugProgress);

            if (    success
                 && (success = GitOperation.CreateAndSwitchToBranch(branchName, rootWorkingDirectory, indent, debugProgress))
               )
            {
                // On the appropriate Git Repository branch

                if ( performImport )
                {
                    Console.WriteLine();
                    Console.WriteLine("Adding Source Revisions for PVCS Promotion Group \"{0}\" to \"{1}\"", promotionGroup, branchName);
                    if (pvcsCompleteSystemArchiveDetail.GitAdd(promotionGroup,
                                                               indent,
                                                               promotionGroupSourceVolume,
                                                               rootWorkingDirectory)
                       )
                    {
                        success = GitOperation.Commit(commitMessage, rootWorkingDirectory, indent, debugProgress);
                        if (success)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Committed Source Revisions for PVCS Promotion Group \"{0}\" to \"{1}\"", promotionGroup,branchName);
                        }
                    }
                    else
                    {
                        success = false;
                        Console.WriteLine("*** Not all \"{0}\" Source Revisions were added to branch \"{1}\"",
                            promotionGroup, branchName);
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Skipping import of PVCS Promotion Group \"{0}\" to \"{1}\"", promotionGroup, branchName);
                }

            } // On the appropriate Git Repository branch

            return success;

        } // ImportBranchFromPvcs

    } // GitOperation

} // PvcsToGitMigrate
