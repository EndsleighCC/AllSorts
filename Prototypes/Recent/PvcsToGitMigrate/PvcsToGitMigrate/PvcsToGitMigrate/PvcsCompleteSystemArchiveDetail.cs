using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CommandOperations;
using GitChangeControl;
using DisplayHelper;

namespace PvcsChangeControl
{
    public class PvcsCompleteSystemArchiveDetail
    {
        public enum PvcsArchiveDetailLevel
        {
            ChangesOnly,
            AllArchives
        }

        public class UnableToOpenFileException : ApplicationException
        {
            public UnableToOpenFileException(string filename) : base("Unable to open file \"" + filename + "\"")
            {
            }
        }

        public PvcsCompleteSystemArchiveDetail(string reportPath , PvcsArchiveDetailLevel pvcsArchiveDetailLevel)
        {
            bool useProdChange = true;

            const string allGroupFilename = "AllGroup.txt";
            const string prodChangeFilename = "ProdChange.txt";

            string prodChangeFilenameWithPath = Path.Combine(reportPath, prodChangeFilename);
            string allGroupFilenameWithPath = Path.Combine(reportPath, allGroupFilename);

            if (useProdChange)
            {
                // Import from ProdChange Report

                if (File.Exists(prodChangeFilenameWithPath))
                {

                    Console.WriteLine();
                    Console.WriteLine("{0} : Adding PVCS Archive revisions that contain changes", DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));

                    try
                    {
                        using (StreamReader fileStream = new StreamReader(prodChangeFilenameWithPath))
                        {
                            string currentArchiveName = null;
                            while (!fileStream.EndOfStream)
                            {
                                string fileLine = fileStream.ReadLine().Trim();

                                string archiveName;
                                string archiveDetailText;

                                ParseArchiveInformation(fileLine, out archiveName, out archiveDetailText);

                                PvcsArchiveDetail pvcsArchiveDetail;

                                if (currentArchiveName == null)
                                {
                                    // No archive name so far
                                    currentArchiveName = archiveName;

                                    pvcsArchiveDetail = new PvcsArchiveDetail(archiveName);
                                    PvcsArchiveDetailCollection.Add(archiveName, pvcsArchiveDetail);

                                }
                                else
                                {
                                    // Existing current archive

                                    if (String.Compare(currentArchiveName, archiveName, /*ignore case*/ true) == 0)
                                    {
                                        // Get the existing Archive Detail
                                        pvcsArchiveDetail = PvcsArchiveDetailCollection[archiveName];
                                    }
                                    else
                                    {
                                        // Different Archive
                                        currentArchiveName = archiveName;

                                        pvcsArchiveDetail = new PvcsArchiveDetail(archiveName);
                                        PvcsArchiveDetailCollection.Add(archiveName, pvcsArchiveDetail);
                                    }

                                } // Existing current archive

                                pvcsArchiveDetail.AddArchiveDetail(archiveDetailText);

                            } // while not end of stream

                        } // using StreamReader

                    }
                    catch (Exception)
                    {
                        throw new UnableToOpenFileException(prodChangeFilename);
                    }

                    Console.WriteLine();
                    Console.WriteLine("{0} : Completed adding PVCS Archives revisions that contain changes", DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));

                }
                else
                {
                    throw new FileNotFoundException(prodChangeFilenameWithPath);
                }

            } // Import from ProdChange Report

            if (File.Exists(allGroupFilenameWithPath))
            {
                // Import any archives not included in the ProdChange import

                // The ProdChange Report only contains Archives with changes

                Console.WriteLine();
                Console.WriteLine("{0} : Adding revisions for PVCS Archives that contain no changes (Production only)",
                                        DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
                Console.WriteLine();

                try
                {
                    using (StreamReader fileStream = new StreamReader(allGroupFilenameWithPath))
                    {
                        while (!fileStream.EndOfStream)
                        {
                            string fileLine = fileStream.ReadLine().Trim();

                            string archiveName;
                            string archiveDetailText;

                            ParseArchiveInformation(fileLine, out archiveName, out archiveDetailText);

                            if (! PvcsArchiveDetailCollection.ContainsKey(archiveName))
                            {
                                // Add a report entry for an Archive that has no changes
                                if (useProdChange)
                                {
                                    Console.WriteLine("{0}{1} : Adding \"{2}\"",
                                                            ConsoleDisplay.Indent(1),
                                                            archiveName,
                                                            archiveDetailText);
                                }
                                PvcsArchiveDetail pvcsArchiveDetail = new PvcsArchiveDetail(archiveName);
                                pvcsArchiveDetail.AddArchiveDetail(archiveDetailText);
                                PvcsArchiveDetailCollection.Add(archiveName,pvcsArchiveDetail);
                            } // Add a report entry for an Archive that has no changes
                        }
                    }
                }
                catch (Exception)
                {
                    throw new UnableToOpenFileException(allGroupFilenameWithPath);
                }

                Console.WriteLine();
                Console.WriteLine("{0} : Completed adding revisions for PVCS Archives that contain no changes (Production only)",
                                        DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));

            } // Import any archives not included in the ProdChange import
            else
            {
                throw new FileNotFoundException(allGroupFilenameWithPath);
            }

            Console.WriteLine();
            Console.WriteLine("{0} : Adding list of PVCS Source Revisions that represent generated files", DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
            Console.WriteLine();

            // These generated files should really be read from a file but I've put them here

            // Don't add these files as ignored because, although most of them are generated and will represent
            // differences between a Promotion Group Share and a Repository, they should be change controlled
            //_pvcsArchiveIgnoreSet.Add(@"Z:\Endsleigh\AssemblyInfo.cpp");
            //_pvcsArchiveIgnoreSet.Add(@"Z:\Endsleigh\AssemblyInfo.cs");
            //_pvcsArchiveIgnoreSet.Add(@"Z:\Endsleigh\AssemblyInfo.vb");

            _pvcsArchiveIgnoreSet.Add(@"Z:\Endsleigh\Legacy\INETFoundation\Types\BaseEnumerations.cs");
            _pvcsArchiveIgnoreSet.Add(@"Z:\Endsleigh\Legacy\INETFoundation\Types\ClientEnumerations.cs");
            _pvcsArchiveIgnoreSet.Add(@"Z:\Endsleigh\Legacy\INETFoundation\Types\HouseholdEnumerations.cs");
            _pvcsArchiveIgnoreSet.Add(@"Z:\Endsleigh\Legacy\INETFoundation\Types\MotorEnumerations.cs");
            _pvcsArchiveIgnoreSet.Add(@"Z:\Endsleigh\Legacy\INETFoundation\Types\NicheEnumerations.cs");

            // This one is not generated
            //_pvcsArchiveIgnoreSet.Add(@"Z:\Endsleigh\Legacy\INETFoundation\Types\TravelEnumerations.cs");

            if (_pvcsArchiveIgnoreSet.Count < 1 )
            {
                Console.WriteLine("{0} : No PVCS Source Revisions exist in the set of generated files", DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss") ) ;
            }
            else
            {
                foreach (string archiveSetIgnoreEntry in _pvcsArchiveIgnoreSet)
                {
                    Console.WriteLine("{0}{1}", ConsoleDisplay.Indent(1), archiveSetIgnoreEntry);
                }
            }

            Console.WriteLine();
            Console.WriteLine("{0} : Completed adding list of PVCS Source Revisions that represent generated files", DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));

        } // Constructor

        public void Display()
        {
            string heading = String.Format("Change Summary for {0} Archives", PvcsArchiveDetailCollection.Count);
            Console.WriteLine();
            Console.WriteLine(heading);
            Console.WriteLine(new String('~', heading.Length));

            foreach (KeyValuePair<string, PvcsArchiveDetail> pvcsArchiveInfo in PvcsArchiveDetailCollection)
            {
                pvcsArchiveInfo.Value.Display(1);
            }
            Console.WriteLine();
            Console.WriteLine("End of {0}", heading);
        }

        public void CheckDescendents(string issueNumber, string promotionGroup, SortedSet<string> additionalIssueNumberCollection)
        {
            string heading = String.Format("Descendent Check for Issue Number {0} and {1}", issueNumber, promotionGroup);

            Console.WriteLine();
            Console.WriteLine(heading);
            Console.WriteLine(new String('~', heading.Length));

            foreach (KeyValuePair<string, PvcsArchiveDetail> pvcsArchiveInfo in PvcsArchiveDetailCollection)
            {
                pvcsArchiveInfo.Value.CheckDescendents(1, issueNumber, promotionGroup, additionalIssueNumberCollection);
            }
        }

        public void CheckBuriedPromotionGroup(string promotionGroup)
        {
            string heading = String.Format("Buried Promotion Group Report {0}", promotionGroup);

            Console.WriteLine();
            Console.WriteLine(heading);
            Console.WriteLine(new String('~', heading.Length));

            foreach (KeyValuePair<string, PvcsArchiveDetail> pvcsArchiveInfo in PvcsArchiveDetailCollection)
            {
                pvcsArchiveInfo.Value.CheckBuriedPromotionGroup(1, promotionGroup);
            }
        }

        public void GeneratePromotionListForIssue(string issueNumber, string promotionGroup)
        {
            foreach (KeyValuePair<string, PvcsArchiveDetail> pvcsArchiveInfo in PvcsArchiveDetailCollection)
            {
                pvcsArchiveInfo.Value.GeneratePromotionGroupEntryForIssue(issueNumber, promotionGroup);
            }
        }

        public void GeneratePromotionGroupList(int indent , string promotionGroup)
        {
            Console.WriteLine();
            Console.WriteLine("{0}{1}", ConsoleDisplay.Indent(indent), promotionGroup);
            Console.WriteLine();
            int linecount = 0;
            foreach (KeyValuePair<string, PvcsArchiveDetail> pvcsArchiveInfo in PvcsArchiveDetailCollection)
            {
                string entry = pvcsArchiveInfo.Value.GeneratePromotionGroupEntry(promotionGroup);
                if (entry != null)
                {
                    Console.WriteLine("{0}{1}", ConsoleDisplay.Indent(indent+1) , entry);
                    linecount += 1;
                }
            }
            if (linecount == 0)
            {
                Console.WriteLine("{0}None",ConsoleDisplay.Indent(indent+1));
            }
        }

        public void GeneratePromotionGroupLists()
        {
            Console.WriteLine();
            ConsoleDisplay.WritelineWithUnderline("Change Summary by Promotion Group");
            int indent = 1;
            GeneratePromotionGroupList(indent, PvcsPromotionGroupDetailCollection.PreProductionPromotionGroupName);
            GeneratePromotionGroupList(indent, PvcsPromotionGroupDetailCollection.UserTestPromotionGroupName);
            GeneratePromotionGroupList(indent, PvcsPromotionGroupDetailCollection.SystemTestPromotionGroupName);
            GeneratePromotionGroupList(indent, PvcsPromotionGroupDetailCollection.SystemTest1PromotionGroupName);
            GeneratePromotionGroupList(indent, PvcsPromotionGroupDetailCollection.SystemTest2PromotionGroupName);
            GeneratePromotionGroupList(indent, PvcsPromotionGroupDetailCollection.SystemTest3PromotionGroupName);
            GeneratePromotionGroupList(indent, PvcsPromotionGroupDetailCollection.SystemTest4PromotionGroupName);
            GeneratePromotionGroupList(indent, PvcsPromotionGroupDetailCollection.SystemTest5PromotionGroupName);
            GeneratePromotionGroupList(indent, PvcsPromotionGroupDetailCollection.SystemTest6PromotionGroupName);
        }

        public bool GitAdd(string promotionGroup , int indent,string sourceRootDirectory, string rootWorkingDirectory)
        {
            bool overallSuccess = true;

            Console.WriteLine();
            Console.WriteLine("{0}Adding revisions for PVCS Promotion Group \"{1}\" from {2} PVCS Archive sources in \"{3}\" to Git Repository \"{4}\"",
                                ConsoleDisplay.Indent(indent),
                                promotionGroup,
                                PvcsArchiveDetailCollection.Count,
                                sourceRootDirectory ,
                                rootWorkingDirectory);
            Console.WriteLine();
            int promotionGroupTotalRevisionCount = 0;
            int promotionGroupRevisionAddCount = 0;
            foreach (KeyValuePair<string, PvcsArchiveDetail> pvcsArchiveInfo in PvcsArchiveDetailCollection)
            {
                if (_pvcsArchiveIgnoreSet.Contains(pvcsArchiveInfo.Key))
                {
                    // PVCS Archive should be ignored

                    Console.WriteLine("{0}{1} * Ignoring \"{2}\"",
                        ConsoleDisplay.Indent(indent+1),
                        DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"),
                        pvcsArchiveInfo.Key);

                } // PVCS Archive should be ignored
                else
                {
                    // PVCS Archive should NOT be ignored

                    if (pvcsArchiveInfo.Value.HasPromotionGroup(promotionGroup))
                    {
                        promotionGroupTotalRevisionCount += 1;
                        bool individualSuccess = pvcsArchiveInfo.Value.GitAdd(
                            sourceRootDirectory,
                            rootWorkingDirectory,
                            indent + 1,
                            CommandOperation.DebugProgress.None);
                        if (individualSuccess)
                        {
                            promotionGroupRevisionAddCount += 1;
                        }
                        else
                        {
                            overallSuccess = false;
                            Console.WriteLine("{0}*** Failed to add \"{1}\" to the Repository",
                                ConsoleDisplay.Indent(indent + 1),
                                pvcsArchiveInfo.Value.Name);
                        }
                    }
                } // PVCS Archive should NOT be ignored
            } // foreach

            Console.WriteLine();
            Console.WriteLine("{0}Successfully added {1} revisions out of {2} revisions for PVCS Promotion Group \"{3}\" from {4} PVCS Archive sources in \"{5}\" to Git Repository \"{6}\"",
                                ConsoleDisplay.Indent(indent),
                                promotionGroupRevisionAddCount,
                                promotionGroupTotalRevisionCount,
                                promotionGroup,
                                PvcsArchiveDetailCollection.Count,
                                sourceRootDirectory,
                                rootWorkingDirectory);

            if (promotionGroupRevisionAddCount != promotionGroupTotalRevisionCount)
            {
                Console.WriteLine("{0}*** Failed to add {1} revisions for {2} from {3} revisions",
                    ConsoleDisplay.Indent(indent),
                    (promotionGroupTotalRevisionCount - promotionGroupRevisionAddCount),
                    promotionGroup,
                    promotionGroupTotalRevisionCount);
            }

            return overallSuccess;

        } // GitAdd

        public PvcsArchiveDetailCollectionType PvcsArchiveDetailCollection = new PvcsArchiveDetailCollectionType();

        public static PvcsPromotionGroupDetailCollection PromotionGroupDetailCollection = new PvcsPromotionGroupDetailCollection();

        private void ParseArchiveInformation(string fileLine, out string archiveName, out string archiveDetailText)
        {
            if (fileLine[0] != '"')
            {
                // Spaces in the filename

                string[] archiveDetailPart = fileLine.Split(new char[] { ' ' },
                                                            StringSplitOptions.RemoveEmptyEntries);

                archiveName = archiveDetailPart[0];
                archiveDetailText = fileLine.Substring(archiveName.Length).TrimStart(new char[] { ' ', '-' });

            } // Spaces in the filename
            else
            {
                // No spaces in the filename

                int endDoubleQuoteIndex = fileLine.IndexOf('"', 1);

                archiveName = fileLine.Substring(1, endDoubleQuoteIndex - 1);

                string[] archiveDetailPart =
                    fileLine.Substring(endDoubleQuoteIndex + 1).Split(new char[] { ' ' },
                                                                        StringSplitOptions.
                                                                            RemoveEmptyEntries);

                // The Archive Details follow the last double quote
                archiveDetailText = fileLine.Substring(archiveName.Length + 2).TrimStart(new char[] { ' ', '-' });

            } // No spaces in the filename

        } // ParseArchiveInformation

        private PvcsArchiveNameSetIgnoreCase _pvcsArchiveIgnoreSet = new PvcsArchiveNameSetIgnoreCase(); 

    } // PvcsCompleteSystemArchiveDetail
}
