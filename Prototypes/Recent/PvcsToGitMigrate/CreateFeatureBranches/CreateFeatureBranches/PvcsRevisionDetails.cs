using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CommandOperations;

namespace CreateFeatureBranches
{
    public enum PvcsRevisionDetailsReadState { MustBeBranchName, MustBeDescription, MustBeRevisionDetails };
    public class PvcsRevisionDetails
    {
        public class PvcsRevisionDetailsUnknownGitBranchNameDeclarationException : ApplicationException
        {
            public PvcsRevisionDetailsUnknownGitBranchNameDeclarationException(string description) : base(description)
            {
            }
        }

        public PvcsRevisionDetails(string pvcsRevisionDetailsLineGitBranchName)
        {
            if (!pvcsRevisionDetailsLineGitBranchName.StartsWith(PvcsGitBranchNameDeclare))
            {
                throw new PvcsRevisionDetailsUnknownGitBranchNameDeclarationException(String.Format("Unknown Git Branch Name Declaration \"{0}\"", pvcsRevisionDetailsLineGitBranchName));
            }
            else
            {
                string tail = pvcsRevisionDetailsLineGitBranchName.Substring(PvcsGitBranchNameDeclare.Length).Trim();
                string[] tailToken = tail.Split(new char[] { ' ', '/' }, StringSplitOptions.RemoveEmptyEntries);

                GitMainBranchName = tailToken[0];
                if (tailToken.Length < 2)
                {
                    // On the main branch
                    GitFeatureBranchName = null;
                }
                else
                {
                    // There is a feature branch name and it must begin with the right word
                    if ((tailToken.Length < 3) || (String.Compare(tailToken[1], _featureBranchNameStart) != 0))
                    {
                        throw new ApplicationException(String.Format("Git Feature Branch Name \"{0}\" is invalid", pvcsRevisionDetailsLineGitBranchName));
                    }
                    else
                    {
                        // There is a proper feature branch
                        GitFeatureBranchName = tailToken[1] + '/' + tailToken[2];
                    }
                } // There is a feature branch name

                Console.WriteLine();
                Console.WriteLine("PvcsRevisionDetails : Processing branch \"{0}\"", GitCurrentBranchName);
            }
        }

        public void SetDescription(string pvcsRevisionDetailsLineDescription)
        {
            if (!pvcsRevisionDetailsLineDescription.StartsWith(_PvcsGitBranchDescriptionDeclare))
            {
                throw new ApplicationException(String.Format("Unknown Git Branch Description Declaration \"{0}\"", pvcsRevisionDetailsLineDescription));
            }
            else
            {
                GitFeatureBranchDescription = pvcsRevisionDetailsLineDescription.Substring(_PvcsGitBranchDescriptionDeclare.Length).Trim();
            }
        }

        private int CreateFullDirectoryPathForFile( string fullPathAndFilename )
        {
            int error = WindowsErrorDefinition.Success;

            int posLastSlash = fullPathAndFilename.LastIndexOf('\\');
            if ( posLastSlash >= 0 )
            {
                // There is a path to handle

                // Cope with, for example, "D:\"
                if ( ( posLastSlash != 2 ) || (fullPathAndFilename.Substring(1,2) != ":\\") )
                {
                    // Not a root directory path so try to create it
                    string fullDirectoryPath = fullPathAndFilename.Substring(0, posLastSlash);
                    try
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(fullDirectoryPath);
                        if ( !directoryInfo.Exists )
                        {
                            Console.WriteLine("CreateFullDirectoryPathForFile : Creating \"{0}\"", fullDirectoryPath);
                            directoryInfo = Directory.CreateDirectory(fullDirectoryPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("CreateFullDirectoryPathForFile : Exception trying to create directory \"{0}\" = \"{1}\"",
                            fullDirectoryPath,ex.ToString());
                        error = WindowsErrorDefinition.PathNotFound;
                    }
                }
            }

            return error;
        }

        public int CreateFeatureBranch(string gitRepositoryRootPath, string pvcsSharePath)
        {
            int error = WindowsErrorDefinition.Success;

            if (!_commitAttempted )
            {
                // A commit has not yet been attempted

                _commitAttempted = true;

                bool success = false;

                // Git Checkout the Main Branch
                string command = "git checkout " + GitMainBranchName;
                success = CommandOperation.RunVisibleCommand(gitRepositoryRootPath,
                                                              command,
                                                              1,
                                                              CommandOperation.DebugProgress.Enabled,
                                                              CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError);
                if (!success)
                {
                    Console.WriteLine("CreateFeatureBranch : Unable to checkout main branch \"{0}\"", GitMainBranchName);
                    error = WindowsErrorDefinition.InvalidFunction;
                }
                else
                {
                    if (GitFeatureBranchName != null)
                    {
                        // Create the feature branch
                        command = "git branch " + GitFeatureBranchName;
                        success = CommandOperation.RunVisibleCommand(gitRepositoryRootPath,
                                                                     command,
                                                                     1,
                                                                     CommandOperation.DebugProgress.Enabled,
                                                                     CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError);
                        if (!success)
                        {
                            Console.WriteLine("CreateFeatureBranch : Unable to create feature branch \"{0}\"", GitFeatureBranchName);
                            error = WindowsErrorDefinition.InvalidData;
                        }
                    }
                }

                if (error == WindowsErrorDefinition.Success)
                {
                    // Checkout the feature branch
                    if (GitFeatureBranchName != null)
                    {
                        command = "git checkout " + GitFeatureBranchName;
                        success = CommandOperation.RunVisibleCommand(gitRepositoryRootPath,
                                                                      command,
                                                                      1,
                                                                      CommandOperation.DebugProgress.Enabled,
                                                                      CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError);
                        if (!success)
                        {
                            Console.WriteLine("CreateFeatureBranch : Unable to checkout feature branch \"{0}\"", GitFeatureBranchName);
                            error = WindowsErrorDefinition.InvalidFunction;
                        }
                    }
                } // Checkout the feature branch

                if (error == WindowsErrorDefinition.Success)
                {
                    // Copy each new revision into the right place in the repository directory
                    for (int revisionIndex = 0;
                          (error == WindowsErrorDefinition.Success)
                          && (revisionIndex < PvcsRevisionDetailsLineSortedSet.Count);
                          ++revisionIndex
                        )
                    {
                        string pvcsRevisionDetailsLine = PvcsRevisionDetailsLineSortedSet.ElementAt(revisionIndex);
                        string archiveFilename = null;
                        if (pvcsRevisionDetailsLine.StartsWith("\""))
                        {
                            // Parse for double quote at the end of the filename
                            int doubleQuoteCharPos = pvcsRevisionDetailsLine.Substring(1).IndexOf("\"");
                            archiveFilename = pvcsRevisionDetailsLine.Substring(1, doubleQuoteCharPos);
                        }
                        else
                        {
                            // Parse for the next space which delimits the filename
                            string[] lineElement = pvcsRevisionDetailsLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            archiveFilename = lineElement[0];
                        }

                        // Get the relative path filename skipping the drive letter, the colon and the first slash
                        string relativePathFilename = archiveFilename.Substring(archiveFilename.IndexOf(":") + 2);

                        string sourcePathAndFilename = Path.Combine(pvcsSharePath + "\\", relativePathFilename);
                        string destinationPathAndFilename = Path.Combine(gitRepositoryRootPath + "\\", relativePathFilename);
                        try
                        {
                            // Ensure there is a directory in which to put the file i.e. it might be a file that is not usually copied by the utilities e.g. a DLL
                            error = CreateFullDirectoryPathForFile(destinationPathAndFilename);
                            if ( error == WindowsErrorDefinition.Success)
                            {
                                // Attempt to copy the file
                                File.Copy(sourcePathAndFilename, destinationPathAndFilename,/*overwrite*/true);
                                // Ensure the file is not read only
                                FileAttributes fileAttributes = File.GetAttributes(destinationPathAndFilename);
                                if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                                {
                                    fileAttributes = fileAttributes & ~FileAttributes.ReadOnly;
                                    File.SetAttributes(destinationPathAndFilename, fileAttributes);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("CreateFeatureBranch : Exception during copy and attribute management of \"{0}\" to \"{1}\" generated exception {2}",
                                                sourcePathAndFilename,
                                                destinationPathAndFilename,
                                                ex.ToString());
                            error = WindowsErrorDefinition.GeneralFailure;
                        }

                    } // for revisionIndex

                    if ( error == WindowsErrorDefinition.Success )
                    {
                        // Display a git status before adding the files
                        command = "git status";
                        success = CommandOperation.RunVisibleCommand(gitRepositoryRootPath,
                                                                     command,
                                                                     1,
                                                                     CommandOperation.DebugProgress.Enabled,
                                                                     CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError);
                        if (!success)
                        {
                            Console.WriteLine("CreateFeatureBranch : Unable to perform a \"git status\" on repository \"{0}\"", gitRepositoryRootPath);
                            // Only change the error is there isn't already an error
                            if (error == WindowsErrorDefinition.Success)
                            {
                                error = WindowsErrorDefinition.InvalidFunction;
                            }
                        }
                    }

                } // Copy each new revision into the right place in the repository directory

                if (error == WindowsErrorDefinition.Success)
                {
                    // Stage all the changes
                    command = "git add .";
                    success = CommandOperation.RunVisibleCommand(gitRepositoryRootPath,
                                                                 command,
                                                                 1,
                                                                 CommandOperation.DebugProgress.Enabled,
                                                                 CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError);
                    if (!success)
                    {
                        if (GitFeatureBranchName != null)
                        {
                            Console.WriteLine("CreateFeatureBranch : Unable to stage changes to feature branch \"{0}\"", GitFeatureBranchName);
                        }
                        else
                        {
                            Console.WriteLine("CreateFeatureBranch : Unable to stage changes to main branch \"{0}\"", GitMainBranchName);
                        }
                        error = WindowsErrorDefinition.InvalidFunction;
                    }
                } // Stage all the changes

                if (error == WindowsErrorDefinition.Success)
                {
                    // Commit the changes
                    List<string> stdout = new List<string>();
                    List<string> stderror = new List<string>();
                    command = "git commit -m \"" + GitFeatureTidyBranchDescription + "\"";
                    success = CommandOperation.RunVisibleCommand(gitRepositoryRootPath,
                                                                 command,
                                                                 1,
                                                                 CommandOperation.DebugProgress.Enabled,
                                                                 CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError,
                                                                 stdout,
                                                                 stderror);
                    if (!success)
                    {
                        // Check whether there really was anything to commit
                        bool sourcesToCommit = true;
                        for ( int lineIndex = 0; (sourcesToCommit) && (lineIndex < stdout.Count) ; ++lineIndex)
                        {
                            sourcesToCommit = ! ( stdout[lineIndex].IndexOf("nothing to commit") >= 0 ) ;
                        }
                        if ( sourcesToCommit )
                        {
                            Console.WriteLine("CreateFeatureBranch : Unable to stage changes to feature branch \"{0}\"", GitCurrentBranchName);
                            error = WindowsErrorDefinition.InvalidFunction;
                        }
                        else
                        {
                            Console.WriteLine("CreateFeatureBranch : No source differences were detected for branch \"{0}\" - commit abandoned", GitCurrentBranchName);
                            error = WindowsErrorDefinition.Success;
                        }
                    }
                } // Commit the changes

                // Display a final git status
                command = "git status";
                success = CommandOperation.RunVisibleCommand(gitRepositoryRootPath,
                                                             command,
                                                             1,
                                                             CommandOperation.DebugProgress.Enabled,
                                                             CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError);
                if (!success)
                {
                    Console.WriteLine("CreateFeatureBranch : Unable to perform a final \"git status\" on repository \"{0}\"", gitRepositoryRootPath);
                    // Only change the error is there isn't already an error
                    if (error == WindowsErrorDefinition.Success)
                    {
                        error = WindowsErrorDefinition.InvalidFunction;
                    }
                }

            } // A commit has not yet been attempted

            return error;

        } // CreateFeatureBranch

        public string GitMainBranchName { get; set; }

        public string GitFeatureBranchName { get; set; }
        public string GitFeatureBranchDescription { get; set; }

        public string GitFeatureTidyBranchDescription
        {
            get
            {
                // Try to escape backslashes
                string description = GitFeatureBranchDescription.Replace("\\", "\\\\");
                // Try to escape double quotes
                description = description.Replace("\"", "\\\"");
                return description;
            }
        }

        public string GitCurrentBranchName
        {
            get
            {
                string currentBranchName = null;
                if (GitFeatureBranchName == null)
                {
                    currentBranchName = GitMainBranchName;
                }
                else
                {
                    currentBranchName = GitFeatureBranchName;
                }
                return currentBranchName;
            }
        }

        public string GitFullFeatureBranchName
        {
            get
            {
                return GitMainBranchName + '/' + GitFeatureBranchName;
            }
        }

        public SortedSet<string> PvcsRevisionDetailsLineSortedSet = new SortedSet<string>();

        public const string PvcsGitBranchNameDeclare = "#branch:";
        const string _PvcsGitBranchDescriptionDeclare = "#description:";

        bool _commitAttempted = false;

        const string _featureBranchNameStart = "feature";
    }
}
