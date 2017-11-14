using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TestGitMigrate
{

    public class PvcsArchiveDetail
    {
        public PvcsArchiveDetail(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public string FileName
        {
            get
            {
                int lastSlashIndex = Name.LastIndexOf('\\');
                return Name.Substring(lastSlashIndex+1);
            }
        }

        public string WorkfileRelativePath
        {
            get
            {
                string workfileRelativePath = null;
                int firstSlashIndex = Name.IndexOf('\\');
                if (firstSlashIndex >= 0)
                {
                    // Return the path after the first slash
                    workfileRelativePath = Name.Substring(firstSlashIndex+1);
                }
                return workfileRelativePath;
            }
        }

        public void AddArchiveDetail(string reportDataText)
        {
            string[] archiveDetailItem = reportDataText.Split(new char[] { ' ', ':', '=' }, StringSplitOptions.RemoveEmptyEntries);

            string promotionGroup = archiveDetailItem[0];
            if (string.Compare(promotionGroup, "[NoPromoGroup]", true) == 0)
            {
                // Use the Promotion Group of the higher revision
                if (PvcsArchiveRevisionDetailCollection.Count > 0)
                {
                    promotionGroup =
                        PvcsArchiveRevisionDetailCollection.ElementAt(PvcsArchiveRevisionDetailCollection.Count - 1)
                            .PromotionGroup;
                }
                else
                {
                    promotionGroup = "NoPromoGroup";
                }
            }

            // Find the second colon beyond which is the description
            int colonIndex = reportDataText.IndexOf(':');
            colonIndex = reportDataText.IndexOf(':', colonIndex + 1);
            string description = String.Empty;
            if (colonIndex >= 0)
            {
                if ((colonIndex + 1) < reportDataText.Length)
                    description = reportDataText.Substring(colonIndex + 1).Trim(' ');
                else
                    description = String.Empty;
            }

            string[] descriptionPart = description.Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);

            string revisionNumber = archiveDetailItem[1];
            string issueNumber = String.Empty;
            if (descriptionPart.Length > 0)
            {
                issueNumber = descriptionPart[0].Trim(new char[] { ':' }).ToUpper();
            }
            if (descriptionPart.Length > 1)
            {
                int numeric = 0;
                if (Int32.TryParse(descriptionPart[1], out numeric))
                {
                    issueNumber += descriptionPart[1];
                }
            }
            if (String.IsNullOrEmpty(issueNumber))
            {
                // Console.WriteLine("\"{0}\" has empty Issue Number in \"{1}\"", Name, description);
            }
            string developerId = archiveDetailItem[2];
            PvcsArchiveRevisionDetail pvcsArchiveRevisionDetail =
                new PvcsArchiveRevisionDetail(Name, revisionNumber, promotionGroup, issueNumber, developerId, description);

            PvcsArchiveRevisionDetailCollection.Add(pvcsArchiveRevisionDetail);
        }

        public PvcsArchiveRevisionDetailCollection PvcsArchiveRevisionDetailCollection = new PvcsArchiveRevisionDetailCollection();

        public void Display(int indent)
        {
            Console.WriteLine();
            Console.WriteLine("{0}{1}", PvcsCompleteSystemArchiveDetail.Indent(indent), Name);
            foreach (PvcsArchiveRevisionDetail pvcsArchiveRevisionDetail in PvcsArchiveRevisionDetailCollection)
            {
                pvcsArchiveRevisionDetail.Display(indent + 1, false);
            }
        }

        public bool HasPromotionGroup(string promotionGroup)
        {
            return PvcsArchiveRevisionDetailCollection.HasPromotionGroup(promotionGroup);
        }

        public void CheckDescendents(int indent, string issueNumber, string promotionGroup, SortedSet<string> additionalIssueNumberCollection)
        {
            if (PvcsArchiveRevisionDetailCollection.HasIssueNumber(issueNumber))
            {
                // This Archive has this Issue Number

                Collection<string> displayOutput = new Collection<string>();

                foreach (PvcsArchiveRevisionDetail pvcsArchiveRevisionDetail in PvcsArchiveRevisionDetailCollection)
                {
                    if (String.Compare(promotionGroup, "System_Test", true) == 0)
                    {
                        if ((String.Compare(pvcsArchiveRevisionDetail.PromotionGroup, "User_Test", true) == 0)
                            && (String.Compare(pvcsArchiveRevisionDetail.IssueNumber, issueNumber, true) != 0))
                        {
                            // Display the User_Test details
                            displayOutput.Add(pvcsArchiveRevisionDetail.ToString(indent + 1));
                            if (String.IsNullOrEmpty(pvcsArchiveRevisionDetail.IssueNumber))
                                additionalIssueNumberCollection.Add(pvcsArchiveRevisionDetail.NonEmptyIssueNumber + " : " + pvcsArchiveRevisionDetail.PromotionGroup + " - \"" + pvcsArchiveRevisionDetail.ArchiveName + "\"");
                            else
                                additionalIssueNumberCollection.Add(pvcsArchiveRevisionDetail.NonEmptyIssueNumber + " : " + pvcsArchiveRevisionDetail.PromotionGroup);
                        }
                        else if ((String.Compare(pvcsArchiveRevisionDetail.PromotionGroup, "Pre_Production", true) == 0)
                            && (String.Compare(pvcsArchiveRevisionDetail.IssueNumber, issueNumber, true) != 0))
                        {
                            // Display the Pre_Production details
                            displayOutput.Add(pvcsArchiveRevisionDetail.ToString(indent + 1));
                            if (String.IsNullOrEmpty(pvcsArchiveRevisionDetail.IssueNumber))
                                additionalIssueNumberCollection.Add(pvcsArchiveRevisionDetail.NonEmptyIssueNumber + " : " + pvcsArchiveRevisionDetail.PromotionGroup + " - \"" + pvcsArchiveRevisionDetail.ArchiveName + "\"");
                            else
                                additionalIssueNumberCollection.Add(pvcsArchiveRevisionDetail.NonEmptyIssueNumber + " : " + pvcsArchiveRevisionDetail.PromotionGroup);
                        }
                    }
                }
                if (displayOutput.Count > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("{0}{1} : Has Issue Number {2}", PvcsCompleteSystemArchiveDetail.Indent(indent), Name, issueNumber);
                    foreach (string displayLine in displayOutput)
                    {
                        Console.WriteLine(displayLine);
                    }
                }

                if (!PvcsArchiveRevisionDetailCollection.IsOnlyIssueNumber(issueNumber))
                {
                    // Other Issue Numbers are present in the Archive

                    displayOutput.Clear();

                    foreach (PvcsArchiveRevisionDetail pvcsArchiveRevisionDetail in PvcsArchiveRevisionDetailCollection)
                    {
                        if (String.Compare(promotionGroup, "System_Test", true) == 0)
                        {
                            if (String.Compare(pvcsArchiveRevisionDetail.PromotionGroup, "User_Test", true) == 0)
                            {
                                // Display the User_Test details if another Issue Number is present
                                if (String.Compare(pvcsArchiveRevisionDetail.IssueNumber, issueNumber, true) != 0)
                                {
                                    displayOutput.Add(pvcsArchiveRevisionDetail.ToString(indent + 1));
                                    if (String.IsNullOrEmpty(pvcsArchiveRevisionDetail.IssueNumber))
                                        additionalIssueNumberCollection.Add(pvcsArchiveRevisionDetail.NonEmptyIssueNumber + " : " + pvcsArchiveRevisionDetail.PromotionGroup + " - \"" + pvcsArchiveRevisionDetail.ArchiveName + "\"");
                                    else
                                        additionalIssueNumberCollection.Add(pvcsArchiveRevisionDetail.NonEmptyIssueNumber + " : " + pvcsArchiveRevisionDetail.PromotionGroup);
                                }
                            }
                            else if (String.Compare(pvcsArchiveRevisionDetail.PromotionGroup, "Pre_Production", true) == 0)
                            {
                                // Display the Pre_Production details if another Issue Number is present
                                if (String.Compare(pvcsArchiveRevisionDetail.IssueNumber, issueNumber, true) != 0)
                                {
                                    displayOutput.Add(pvcsArchiveRevisionDetail.ToString(indent + 1));
                                    if (String.IsNullOrEmpty(pvcsArchiveRevisionDetail.IssueNumber))
                                        additionalIssueNumberCollection.Add(pvcsArchiveRevisionDetail.NonEmptyIssueNumber + " : " + pvcsArchiveRevisionDetail.PromotionGroup + " - \"" + pvcsArchiveRevisionDetail.ArchiveName + "\"");
                                    else
                                        additionalIssueNumberCollection.Add(pvcsArchiveRevisionDetail.NonEmptyIssueNumber + " : " + pvcsArchiveRevisionDetail.PromotionGroup);
                                }
                            }
                        }
                    }

                    if (displayOutput.Count > 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("{0}{1} : Has Issue Numbers different from {2}",
                                          PvcsCompleteSystemArchiveDetail.Indent(indent), Name, issueNumber);
                        foreach (string displayLine in displayOutput)
                        {
                            Console.WriteLine(displayLine);
                        }
                    }

                } // Other Issue Numbers are present in the Archive

            } // This Archive has this Issue Number

        } // CheckDescendents

        public void CheckBuriedPromotionGroup(int indent, string promotionGroup)
        {
            PvcsArchiveRevisionDetail highestRevisionWithPromotionGroup = PvcsArchiveRevisionDetailCollection.HighestRevisionWithPromotionGroup(promotionGroup);
            if (highestRevisionWithPromotionGroup != null)
            {
                // A revision with this Promotion Group exists

                // Check all other higher Promotion Groups

                // check up to but not including the specified Promotion Group
                int maximumHierarchyIndex = PvcsCompleteSystemArchiveDetail.PromotionGroupDetailCollection.HierarchyIndex(promotionGroup) - 1;

                for (int hierarchyIndex = PvcsPromotionGroupDetailCollection.DevelopmentHierarchyBaseIndex;
                        hierarchyIndex < maximumHierarchyIndex;
                        ++hierarchyIndex)
                {
                    string higherPromotionGroup = PvcsCompleteSystemArchiveDetail.PromotionGroupDetailCollection.PromotionGroup(hierarchyIndex);
                    PvcsArchiveRevisionDetail higherRevisionWithPromotionGroup = PvcsArchiveRevisionDetailCollection.HighestRevisionWithPromotionGroup(higherPromotionGroup);
                    if (higherRevisionWithPromotionGroup != null)
                    {
                        bool specifiedPromotionGroupOnSameBranch =
                            PvcsArchiveRevisionDetailCollection.RevisionsOnTheSameBranch(
                                highestRevisionWithPromotionGroup.RevisionNumber,
                                higherRevisionWithPromotionGroup.RevisionNumber);

                        if (!specifiedPromotionGroupOnSameBranch)
                        {
                            Console.WriteLine("\"{0}\" has both {1} at r{2} and {3} at r{4}",
                                              highestRevisionWithPromotionGroup.ArchiveName,
                                              highestRevisionWithPromotionGroup.PromotionGroup,
                                              highestRevisionWithPromotionGroup.RevisionNumber,
                                              higherRevisionWithPromotionGroup.PromotionGroup,
                                              higherRevisionWithPromotionGroup.RevisionNumber);
                            Console.WriteLine("{0}Is not on the same branch", PvcsCompleteSystemArchiveDetail.Indent(indent));
                        }
                        else
                        {
                            bool specifiedPromotionGroupAtHigherRevision =
                                PvcsArchiveRevisionDetailCollection.RevisionNumberIsGreater(
                                    highestRevisionWithPromotionGroup.RevisionNumber,
                                    higherRevisionWithPromotionGroup.RevisionNumber);
                            if (!specifiedPromotionGroupAtHigherRevision)
                            {

                                Console.WriteLine("\"{0}\" has both {1} at r{2} and {3} at r{4}",
                                                  highestRevisionWithPromotionGroup.ArchiveName,
                                                  highestRevisionWithPromotionGroup.PromotionGroup,
                                                  highestRevisionWithPromotionGroup.RevisionNumber,
                                                  higherRevisionWithPromotionGroup.PromotionGroup,
                                                  higherRevisionWithPromotionGroup.RevisionNumber);
                                Console.WriteLine("{0}Is not at a higher revision", PvcsCompleteSystemArchiveDetail.Indent(indent));
                            }
                        }
                    }
                }

            } // A revision with this Promotion Group exists
        }

        public void GeneratePromotionEntry(string issueNumber, string promotionGroup)
        {
            if (PvcsArchiveRevisionDetailCollection.HasIssueNumber(issueNumber))
            {
                // This Archive has this Issue Number

                // Find the revision with this Promotion Group
                PvcsArchiveRevisionDetail pvcsArchiveRevisionDetail =
                    PvcsArchiveRevisionDetailCollection.HighestRevisionWithPromotionGroup(promotionGroup);
                if (pvcsArchiveRevisionDetail != null)
                {
                    // This Archive has a revision at this Promotion Group
                    if (pvcsArchiveRevisionDetail.ArchiveName.IndexOf(' ') >= 0)
                        Console.Write("\"{0}\"", pvcsArchiveRevisionDetail.ArchiveName);
                    else
                        Console.Write("{0}", pvcsArchiveRevisionDetail.ArchiveName);
                    Console.WriteLine(" - {0} : {1} = {2} = NoLockers",
                        pvcsArchiveRevisionDetail.PromotionGroup,
                        pvcsArchiveRevisionDetail.RevisionNumber,
                        (String.IsNullOrEmpty(pvcsArchiveRevisionDetail.DeveloperId) ? "Unknown" : pvcsArchiveRevisionDetail.DeveloperId));
                }

            } // This Archive has this Issue Number
        } // GeneratePromotionEntry

        public bool GitAdd(bool debugProgress, int indent , string sourceRootDirectory , string rootWorkingDirectory)
        {
            bool success = false;

            int firstSlashIndex = Name.IndexOf('\\');
            if (firstSlashIndex >= 0)
            {
                // Found start of path

                string sourceFileFullPath = Path.Combine(sourceRootDirectory, Name.Substring(firstSlashIndex + 1));

                if (! File.Exists(sourceFileFullPath))
                {
                    Console.WriteLine("{0}*** Source File \"{1}\" does not exist", PvcsCompleteSystemArchiveDetail.Indent(indent),sourceFileFullPath);
                }
                else
                {
                    // Source File exists

                    string workfileFullPath = Path.Combine(rootWorkingDirectory, Name.Substring(firstSlashIndex + 1));

                    if (debugProgress)
                    {
                        Console.WriteLine("{0}Archive Path \"{1}\" working path resolves to \"{2}\"",
                            PvcsCompleteSystemArchiveDetail.Indent(indent),
                            Name,
                            workfileFullPath);
                    }

                    // Find the last slash to pass to xcopy so that it works with a destination directory
                    int lastSlashIndex = workfileFullPath.LastIndexOf('\\');
                    string workfileDirectory = workfileFullPath.Substring(0,lastSlashIndex+1);

                    string fullWorkfilePathAndFilename = Path.Combine(workfileDirectory, this.FileName);
                    // Determine the length of the filename without the drive specifier
                    firstSlashIndex = fullWorkfilePathAndFilename.IndexOf('\\');
                    string fullRootRelativeWorkfilePathAndFilename = fullWorkfilePathAndFilename.Substring(firstSlashIndex + 1);
                    const int longPathAndFilenameLength = 240;
                    if (fullRootRelativeWorkfilePathAndFilename.Length > longPathAndFilenameLength)
                    {
                        Console.WriteLine("{0}+++ Warning: \"{1}\" workfile name is {2} characters long which is longer than {3} characters",
                            PvcsCompleteSystemArchiveDetail.Indent(indent),
                            fullWorkfilePathAndFilename,
                            fullWorkfilePathAndFilename.Length,
                            longPathAndFilenameLength);
                    }

                    string command = String.Format("xcopy \"{0}\" \"{1}\" /v/f/y", sourceFileFullPath, workfileDirectory);

                    // Copy the file into the working directorBy which will cause the file to be "Staged"
                    if (RunCommand(debugProgress, rootWorkingDirectory, command, false))
                    {
                        DateTime dateTimeNow = DateTime.Now;
                        Console.WriteLine("{0}{1} \"{2}\" -> \"{3}\"",
                            PvcsCompleteSystemArchiveDetail.Indent(indent),
                            dateTimeNow.ToString("yyyy-MM-dd-HH:mm:ss"),
                            sourceFileFullPath,
                            workfileDirectory);

                        success = true;

                        // Add the file to the Repo
                        command = "git add \"" + WorkfileRelativePath + "\"";
                        if (RunCommand(debugProgress, rootWorkingDirectory, command, false))
                        {
                            Console.WriteLine("{0}Succeeded \"{1}\"",
                                    PvcsCompleteSystemArchiveDetail.Indent(indent + 1),
                                    command);
                        }
                        else
                        {
                            success = false;
                            Console.WriteLine("{0}*** Failed \"{1}\"",
                                    PvcsCompleteSystemArchiveDetail.Indent(indent + 1),
                                    command);
                        }
                    }
                    else
                    {
                        DateTime dateTimeNow = DateTime.Now;
                        Console.WriteLine("{0} *** {1} Failed \"{2}\" -> \"{3}\"",
                            PvcsCompleteSystemArchiveDetail.Indent(indent),
                            dateTimeNow.ToString("yyyy-MM-dd-HH:mm:ss"),
                            sourceFileFullPath,
                            workfileDirectory);
                    }

                } // Source File exists

            } // Found start of path

            return success;
        } // GitAdd

        private bool RunCommand(bool debugProgress, string workingDirectory, string command, bool displayStandardOutput)
        {
            bool success = false;

            string commandInterpreter = "cmd.exe";
            string commandInterpreterArguments = String.Format("/c {0}", command );

            ProcessStartInfo processStartInfo = new ProcessStartInfo(commandInterpreter);
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = commandInterpreterArguments;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.WorkingDirectory = workingDirectory;

            // Save the current Working Directory
            string currentWorkingDirectory = Directory.GetCurrentDirectory();

            Process processCommand = new Process();
            processCommand.StartInfo = processStartInfo;

            try
            {
                if (debugProgress)
                {
                    Console.WriteLine("Running \"{0}\" \"{1}\"", commandInterpreter, commandInterpreterArguments);
                }

                processCommand.Start();

                processCommand.StandardInput.WriteLine("\r\n");

                processCommand.WaitForExit();

                success = processCommand.ExitCode == 0;

                if ( ( debugProgress ) || ( ! success ) )
                {
                    Console.WriteLine("--- Running \"{0}\" produced Exit Code {1}", command, processCommand.ExitCode);
                }

                if (!processCommand.StandardOutput.EndOfStream && displayStandardOutput)
                {
                    if (debugProgress)
                    {
                        Console.WriteLine("Executable {0} : \"{1}\" Standard Output:",
                            (success ? "Succeeded" : "Failed"), command);
                    }
                    while (!processCommand.StandardOutput.EndOfStream)
                    {
                        Console.WriteLine("    " + processCommand.StandardOutput.ReadLine());
                    }
                }
                if (!processCommand.StandardError.EndOfStream)
                {
                    Console.WriteLine("Executable {0} : \"{1}\" Standard Error:", 
                        (success ? "Succeeded" : "Failed" ) , command);
                    while (!processCommand.StandardError.EndOfStream)
                    {
                        Console.WriteLine("    " + processCommand.StandardError.ReadLine());
                    }
                }

            }
            catch (Exception eek)
            {
                Console.WriteLine("    *** {0}.{1} : Executing \"{2}\" exception \"{3}\"",
                                  MethodBase.GetCurrentMethod().DeclaringType.Name,
                                  MethodBase.GetCurrentMethod().Name,
                                  command,
                                  eek);
            }

            return success;
        }

    } // PvcsArchiveDetail
}
