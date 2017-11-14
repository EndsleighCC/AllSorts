using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CommandOperations;
using GitChangeControl;
using DisplayHelper;

namespace PvcsChangeControl
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

        private string CaseSensitivePathOfFile(string filename)
        {
            string caseSensitivePathOfFile = null;

            FileInfo fileInfo = new FileInfo(filename);
            if (!fileInfo.Exists)
            {
                // Default to returning the name supplied
                caseSensitivePathOfFile = filename;
            }
            else
            {
                // The file exists

                string[] filenamePart = filename.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                if (filenamePart.Length > 0)
                {
                    // The filename has at least one part

                    caseSensitivePathOfFile = string.Empty;

                    string caseInsensitivePathOfFile = string.Empty;
                    int firstPartIndex = 0;

                    if (filename.Substring(0, 2) == "\\\\")
                    {
                        // Skip the UNC name
                        caseInsensitivePathOfFile = "\\\\" + filenamePart[0] + "\\" + filenamePart[1];
                        firstPartIndex = 2;
                        caseSensitivePathOfFile = "\\\\" + filenamePart[0] + "\\" + filenamePart[1];
                    } // Skip the UNC name
                    else if (filename[1] == ':')
                    {
                        // Skip the drive specifier
                        firstPartIndex = 1;
                        caseInsensitivePathOfFile = filenamePart[0];
                        caseSensitivePathOfFile = filenamePart[0];
                    } // Skip the drive specifier
                    else
                    {
                        // Must be a relative path
                        firstPartIndex = 0;
                    }

                    // Progress down the tree acquiring the case sensitive name of each directory and eventually the file
                    for (int filenamePartIndex = firstPartIndex;
                        filenamePartIndex < filenamePart.Length;
                        ++filenamePartIndex)
                    {

                        if (String.IsNullOrEmpty(caseInsensitivePathOfFile))
                        {
                            caseInsensitivePathOfFile = filenamePart[filenamePartIndex];
                        }
                        else
                        {
                            caseInsensitivePathOfFile += "\\" + filenamePart[filenamePartIndex];
                        }

                        string parentDirectory = Directory.GetParent(caseInsensitivePathOfFile).FullName;

                        string[] name;

                        FileAttributes fileAttributes = File.GetAttributes(caseInsensitivePathOfFile);
                        if (fileAttributes.HasFlag(FileAttributes.Directory))
                        {
                            // Console.WriteLine("{0} is a directory", caseInsensitivePathOfFile);
                            // Match the name in the parent directory to acquire the case sensitive name
                            name = Directory.GetDirectories(parentDirectory, filenamePart[filenamePartIndex]);
                        }
                        else
                        {
                            // Console.WriteLine("{0} is a file", caseInsensitivePathOfFile);
                            // Match the name in the parent directory to acquire the case sensitive name
                            name = Directory.GetFiles(parentDirectory, filenamePart[filenamePartIndex]);
                        }

                        // Only the last part of the path actually has its case preserved
                        string[] caseSensitivePart = name[0].Split(new char[] { '\\' });
                        caseSensitivePathOfFile += "\\" + caseSensitivePart[caseSensitivePart.Length - 1];

                    } // for

                } // The filename has at least one part

            } // The file exists

            return caseSensitivePathOfFile;

        } // CaseSensitivePathOfFile

        public string WorkfileRelativePath( string rootWorkingDirectory )
        {
            string workfileRelativePath = null;
            int firstSlashIndex = Name.IndexOf('\\');
            if (firstSlashIndex >= 0)
            {
                // Return the path after the first slash
                workfileRelativePath = Name.Substring(firstSlashIndex+1);
                string workfileFullPath = Path.Combine(rootWorkingDirectory, workfileRelativePath);
                workfileFullPath = CaseSensitivePathOfFile(workfileFullPath);
                int startOfRelativePath = 0;
                if (workfileFullPath[rootWorkingDirectory.Length] != '\\')
                {
                    startOfRelativePath = rootWorkingDirectory.Length;
                }
                else
                {
                    startOfRelativePath = rootWorkingDirectory.Length+1;
                }
                workfileRelativePath = workfileFullPath.Substring(startOfRelativePath);
            }
            return workfileRelativePath;
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
            Console.WriteLine("{0}{1}", ConsoleDisplay.Indent(indent), Name);
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
                    Console.WriteLine("{0}{1} : Has Issue Number {2}", ConsoleDisplay.Indent(indent), Name, issueNumber);
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
                                          ConsoleDisplay.Indent(indent), Name, issueNumber);
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

                // Check up to but not including the specified Promotion Group
                int maximumHierarchyIndex = PvcsCompleteSystemArchiveDetail.PromotionGroupDetailCollection.HierarchyIndex(promotionGroup) - 1;

                for (int hierarchyIndex = PvcsPromotionGroupDetailCollection.DevelopmentHierarchyBaseIndex;
                        hierarchyIndex < maximumHierarchyIndex;
                        ++hierarchyIndex)
                {
                    string higherPromotionGroup = PvcsCompleteSystemArchiveDetail.PromotionGroupDetailCollection.PromotionGroupName(hierarchyIndex);
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
                            Console.WriteLine("{0}Is not on the same branch", ConsoleDisplay.Indent(indent));
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
                                Console.WriteLine("{0}Is not at a higher revision", ConsoleDisplay.Indent(indent));
                            }
                        }
                    }
                }

            } // A revision with this Promotion Group exists
        }

        public void GeneratePromotionGroupEntryForIssue(string issueNumber, string promotionGroup)
        {
            if (PvcsArchiveRevisionDetailCollection.HasIssueNumber(issueNumber))
            {
                // This Archive has this Issue Number

                GeneratePromotionGroupEntry(promotionGroup);

            } // This Archive has this Issue Number
        } // GeneratePromotionGroupEntryForIssue

        public string GeneratePromotionGroupEntry(string promotionGroup)
        {
            string entry = null;

            if (PvcsArchiveRevisionDetailCollection.HasPromotionGroup(promotionGroup))
            {
                // Find the revision with this Promotion Group
                PvcsArchiveRevisionDetail pvcsArchiveRevisionDetail =
                    PvcsArchiveRevisionDetailCollection.HighestRevisionWithPromotionGroup(promotionGroup);
                if (pvcsArchiveRevisionDetail != null)
                {
                    // This Archive has a revision at this Promotion Group
                    if (pvcsArchiveRevisionDetail.ArchiveName.IndexOf(' ') >= 0)
                    {
                        entry = String.Format("\"{0}\"", pvcsArchiveRevisionDetail.ArchiveName);
                    }
                    else
                    {
                        entry = String.Format("{0}", pvcsArchiveRevisionDetail.ArchiveName);
                    }
                    entry += String.Format(" - {0} : {1} = {2} = NoLockers",
                        pvcsArchiveRevisionDetail.PromotionGroup,
                        pvcsArchiveRevisionDetail.RevisionNumber,
                        (String.IsNullOrEmpty(pvcsArchiveRevisionDetail.DeveloperId) ? "Unknown" : pvcsArchiveRevisionDetail.DeveloperId));
                }
            }
            return entry ;
        }

        public bool GitAdd(string sourceRootDirectory , string rootWorkingDirectory, int indent, CommandOperation.DebugProgress debugProgress)
        {
            bool success = false;

            // PVCS Archive Names start with the Archive Drive which ends at the first slash
            int firstSlashIndex = Name.IndexOf('\\');
            if (firstSlashIndex >= 0)
            {
                // Found start of relative path
                string relativePathAndFilename = Name.Substring(firstSlashIndex + 1);

                string sourceFileFullPath = Path.Combine(sourceRootDirectory, relativePathAndFilename);

                if (! File.Exists(sourceFileFullPath))
                {
                    Console.WriteLine("{0}*** Source File \"{1}\" does not exist", ConsoleDisplay.Indent(indent),sourceFileFullPath);
                }
                else
                {
                    // Source File exists

                    string workfileFullPath = Path.Combine(rootWorkingDirectory, relativePathAndFilename);

                    if (debugProgress == CommandOperation.DebugProgress.Enabled)
                    {
                        Console.WriteLine("{0}Archive Path \"{1}\" working path resolves to \"{2}\"",
                            ConsoleDisplay.Indent(indent),
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
                            ConsoleDisplay.Indent(indent),
                            fullWorkfilePathAndFilename,
                            fullWorkfilePathAndFilename.Length,
                            longPathAndFilenameLength);
                    }

                    string command = String.Format("xcopy \"{0}\" \"{1}\" /v/f/y", sourceFileFullPath, workfileDirectory);

                    // Copy the file into the working directorBy which will cause the file to be "Staged"
                    if (CommandOperation.RunCommand(rootWorkingDirectory, command, debugProgress,CommandOperation.CommandOutputDisplayType.None))
                    {
                        DateTime dateTimeNow = DateTime.Now;
                        Console.WriteLine("{0}{1} \"{2}\" -> \"{3}\"",
                            ConsoleDisplay.Indent(indent),
                            dateTimeNow.ToString("yyyy-MM-dd-HH:mm:ss"),
                            sourceFileFullPath,
                            workfileDirectory);

                        // Add the file to the Repo
                        success = GitOperation.Add(rootWorkingDirectory, WorkfileRelativePath(rootWorkingDirectory), indent, debugProgress);
                    }
                    else
                    {
                        DateTime dateTimeNow = DateTime.Now;
                        Console.WriteLine("{0} *** {1} Failed \"{2}\" -> \"{3}\"",
                            ConsoleDisplay.Indent(indent),
                            dateTimeNow.ToString("yyyy-MM-dd-HH:mm:ss"),
                            sourceFileFullPath,
                            workfileDirectory);
                    }

                } // Source File exists

            } // Found start of relative path

            return success;
        } // GitAdd

    } // PvcsArchiveDetail
}
