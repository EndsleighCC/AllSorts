using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestGitMigrate
{
    public class PvcsCompleteSystemArchiveDetail
    {
        public enum PvcsArchiveDetailLevel
        {
            ChangesOnly,
            AllArchives
        }

        public PvcsCompleteSystemArchiveDetail(string reportPath , PvcsArchiveDetailLevel pvcsArchiveDetailLevel)
        {
            const string allGroupFilename = "AllGroup.txt";
            const string prodChangeFilename = "ProdChange.txt";

            string prodChangeFilenameWithPath = Path.Combine(reportPath, prodChangeFilename);
            string allGroupFilenameWithPath = Path.Combine(reportPath, allGroupFilename);
            if (File.Exists(prodChangeFilenameWithPath))
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
                            PvcsArchiveDetailCollection.Add(archiveName,pvcsArchiveDetail);

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

            if (File.Exists(allGroupFilenameWithPath))
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
                            PvcsArchiveDetail pvcsArchiveDetail = new PvcsArchiveDetail(archiveName);
                            pvcsArchiveDetail.AddArchiveDetail(archiveDetailText);
                            PvcsArchiveDetailCollection.Add(archiveName,pvcsArchiveDetail);
                        } // Add a report entry for an Archive that has no changes
                    }
                }
            }

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

        public static string Indent(int indent)
        {
            return new String(' ', indent * 4);
        }

        public void GeneratePromotionList(string issueNumber, string promotionGroup)
        {
            foreach (KeyValuePair<string, PvcsArchiveDetail> pvcsArchiveInfo in PvcsArchiveDetailCollection)
            {
                pvcsArchiveInfo.Value.GeneratePromotionEntry(issueNumber, promotionGroup);
            }
        }

        public bool GitAdd(string promotionGroup , int indent,string sourceRootDirectory, string rootWorkingDirectory)
        {
            bool success = true;

            Console.WriteLine();
            Console.WriteLine("{0}Adding revisions for {1} from {2} PVCS Archives",
                Indent(indent), promotionGroup, PvcsArchiveDetailCollection.Count);
            Console.WriteLine();
            int promotionGroupTotalRevisionCount = 0;
            int promotionGroupRevisionAddCount = 0;
            foreach (KeyValuePair<string, PvcsArchiveDetail> pvcsArchiveInfo in PvcsArchiveDetailCollection)
            {
                if (pvcsArchiveInfo.Value.HasPromotionGroup(promotionGroup))
                {
                    promotionGroupTotalRevisionCount += 1;
                    success = pvcsArchiveInfo.Value.GitAdd(false, indent + 1, sourceRootDirectory, rootWorkingDirectory) && success ;
                    if (success)
                    {
                        promotionGroupRevisionAddCount += 1;
                    }
                    else
                    {
                        Console.WriteLine("{0}*** Failed to add \"{1}\" to the Repository",
                                Indent(indent+1) ,
                                pvcsArchiveInfo.Value.Name
                            );
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("{0}Successfully added {1} revisions out of {2} revisions for {3} from {4} PVCS Archives",
                Indent(indent),
                promotionGroupRevisionAddCount,
                promotionGroupTotalRevisionCount,
                promotionGroup,
                PvcsArchiveDetailCollection.Count);
            if (promotionGroupRevisionAddCount != promotionGroupTotalRevisionCount)
            {
                Console.WriteLine("{0}*** Failed to add {1} revisions for {2} from {3} revisions",
                    Indent(indent),
                    (promotionGroupTotalRevisionCount - promotionGroupRevisionAddCount),
                    promotionGroup,
                    promotionGroupTotalRevisionCount);
            }

            return success;
        }

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

    } // PvcsCompleteSystemArchiveDetail
}
