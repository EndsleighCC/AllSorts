using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace TestCheckPromotionDescendents
{
    public class PvcsAllArchiveDetail
    {

        public class PvcsPromotionGroupDetail
        {
            public PvcsPromotionGroupDetail(string promotionGroup, int hierarchyIndex)
            {
                PromotionGroup = promotionGroup;
                HierarchyIndex = hierarchyIndex;
            }

            public string PromotionGroup { get; private set; }
            public int HierarchyIndex { get; private set; }
        }

        public class PvcsPromotionGroupDetailCollection : Collection<PvcsPromotionGroupDetail>
        {
            public PvcsPromotionGroupDetailCollection()
                : base()
            {
                this.Add(new PvcsPromotionGroupDetail("Production", 0));
                this.Add(new PvcsPromotionGroupDetail("Pre_Production", 1));
                this.Add(new PvcsPromotionGroupDetail("User_Test", 2));
                this.Add(new PvcsPromotionGroupDetail("Pre_User_Test", 3));

                this.Add(new PvcsPromotionGroupDetail("System_Test", 4));
                this.Add(new PvcsPromotionGroupDetail("Pre_System_Test", 5));
                this.Add(new PvcsPromotionGroupDetail("Development", 6));

                this.Add(new PvcsPromotionGroupDetail("System_Test1", 6));
                this.Add(new PvcsPromotionGroupDetail("Pre_System_Test1", 7));
                this.Add(new PvcsPromotionGroupDetail("Development1", 8));

                this.Add(new PvcsPromotionGroupDetail("System_Test2", 6));
                this.Add(new PvcsPromotionGroupDetail("Pre_System_Test2", 7));
                this.Add(new PvcsPromotionGroupDetail("Development2", 8));

                this.Add(new PvcsPromotionGroupDetail("System_Test3", 6));
                this.Add(new PvcsPromotionGroupDetail("Pre_System_Test3", 7));
                this.Add(new PvcsPromotionGroupDetail("Development3", 8));

                this.Add(new PvcsPromotionGroupDetail("System_Test4", 6));
                this.Add(new PvcsPromotionGroupDetail("Pre_System_Test4", 7));
                this.Add(new PvcsPromotionGroupDetail("Development4", 8));

                this.Add(new PvcsPromotionGroupDetail("System_Test5", 6));
                this.Add(new PvcsPromotionGroupDetail("Pre_System_Test5", 7));
                this.Add(new PvcsPromotionGroupDetail("Development5", 8));

                this.Add(new PvcsPromotionGroupDetail("System_Test6", 6));
                this.Add(new PvcsPromotionGroupDetail("Pre_System_Test6", 7));
                this.Add(new PvcsPromotionGroupDetail("Development6", 8));
            }

            // Indices that are not Production
            public const int DevelopmentHierarchyBaseIndex = 1;

            /// <summary>
            /// Returns the Promotion Hierarchy Index of any Development Promotion Group including Production
            /// </summary>
            /// <param name="promotionGroup"></param>
            /// <returns></returns>
            public int HierarchyIndex(string promotionGroup)
            {
                int hierarchyIndex = -1 ;
                for (int index = 0; (hierarchyIndex==-1) && (index < this.Count); ++index)
                {
                    if ( String.Compare( this[index].PromotionGroup , promotionGroup , StringComparison.CurrentCultureIgnoreCase ) == 0 )
                    {
                        hierarchyIndex = this[index].HierarchyIndex;
                    }
                }
                return hierarchyIndex;
            }

            /// <summary>
            /// Returns the Promotion Hierarchy Index of Development Promotion Groups i.e. not Production
            /// </summary>
            /// <param name="promotionGroup"></param>
            /// <returns></returns>
            public int DevelopmentHierarchyIndex(string promotionGroup)
            {
                int hierarchyIndex = -1;
                for (int index = 1; (hierarchyIndex == -1) && (index < this.Count); ++index)
                {
                    if (String.Compare(this[index].PromotionGroup, promotionGroup, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        hierarchyIndex = this[index].HierarchyIndex;
                    }
                }
                return hierarchyIndex;
            }

            public string PromotionGroup(int hierarchyIndex)
            {
                string promotionGroup = null;

                for (int index = 0; (promotionGroup == null) && (index < this.Count); ++index)
                {
                    if (hierarchyIndex == this[index].HierarchyIndex)
                        promotionGroup = this[index].PromotionGroup;
                }

                return promotionGroup;
            }

            public bool PromotionGroupIsHigher(string first, string second)
            {
                return HierarchyIndex(first) > HierarchyIndex(second);
            }
        }

        public PvcsAllArchiveDetail(string filename)
        {
            if (File.Exists(filename))
            {
                using (StreamReader fileStream = new StreamReader(filename))
                {
                    string currentArchiveName = null;
                    while (!fileStream.EndOfStream)
                    {
                        string fileLine = fileStream.ReadLine().Trim();

                        string archiveName;
                        string archiveDetailText;
                        if (fileLine[0] != '"')
                        {
                            // Spaces in the filename

                            string[] archiveDetailPart = fileLine.Split(new char[] {' '},
                                                                        StringSplitOptions.RemoveEmptyEntries);

                            archiveName = archiveDetailPart[0];
                            archiveDetailText = fileLine.Substring(archiveName.Length).TrimStart(new char [] { ' ' , '-'});

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

                            // The Archive Details follow the last double quote
                            archiveDetailText = fileLine.Substring(archiveName.Length + 2).TrimStart(new char[] { ' ', '-' });

                        } // No spaces in the filename

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

                            if (String.Compare(currentArchiveName, archiveName, /*ignore case*/true) == 0)
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

        } // Default Constructor

        public class PvcsArchiveRevisionDetail
        {
            public PvcsArchiveRevisionDetail(string archiveName, string revisionNumber, string promotionGroup, string issueNumber, string developerId, string description)
            {
                ArchiveName = archiveName;
                RevisionNumber = revisionNumber;
                PromotionGroup = promotionGroup;
                if (issueNumber.IndexOfAny("0123456789".ToCharArray()) >= 0)
                {
                    // Issue Number has numbers so it might be a real Issue Number
                    IssueNumber = issueNumber;
                }
                else
                {
                    IssueNumber = "";
                }
                DeveloperId = developerId;
                Description = description;
            }

            public string ArchiveName { get; private set; }
            public string RevisionNumber { get; private set; }
            public string PromotionGroup { get; private set; }
            public string IssueNumber { get; private set; }
            public string DeveloperId { get; private set; }
            public string Description { get; private set; }

            public string NonEmptyIssueNumber
            {
                get
                {
                    string issueNumber;
                    if (!String.IsNullOrEmpty(IssueNumber))
                        issueNumber = IssueNumber.ToUpper();
                    else
                    {
                        issueNumber = "\"\"";
                    }
                    return issueNumber;
                }
            }

            public void Display(int indent, bool displayArchiveName)
            {
                if (!displayArchiveName )
                    Console.WriteLine("{0}{1} {2} {3} {4:6} {5}", PvcsAllArchiveDetail.Indent(indent), RevisionNumber, PromotionGroup, DeveloperId, (String.IsNullOrEmpty(IssueNumber) ? "\"\"" : IssueNumber), Description);
                else
                    Console.WriteLine("{0}{1} {2} {3} {4} {5:6} {6}", PvcsAllArchiveDetail.Indent(indent), ArchiveName, RevisionNumber, PromotionGroup, DeveloperId, (String.IsNullOrEmpty(IssueNumber) ? "\"\"" : IssueNumber), Description);
            }

            public string ToString( int indent )
            {
                return String.Format("{0}{1} {2} {3} {4:6} \"{5}\"", PvcsAllArchiveDetail.Indent(indent), RevisionNumber, PromotionGroup, DeveloperId, (String.IsNullOrEmpty(IssueNumber) ? "\"\"" : IssueNumber), Description);
            }

        } // PvcsArchiveRevisionDetail

        public class PvcsArchiveRevisionDetailCollection : Collection<PvcsArchiveRevisionDetail>
        {
            public PvcsArchiveRevisionDetailCollection() : base()
            {
            }

            public bool HasIssueNumber(string issueNumber)
            {
                bool hasIssueNumber = false;

                for (int revisionIndex = 0; (!hasIssueNumber) && (revisionIndex < Count); ++revisionIndex)
                {
                    if (String.Compare(this[revisionIndex].IssueNumber, issueNumber, true) == 0)
                        // Found this Issue Number
                        hasIssueNumber = true;
                }

                return hasIssueNumber;
            }

            public bool IsOnlyIssueNumber( string issueNumber )
            {
                bool isOnlyIssueNumber = true;

                for (int revisionIndex = 0; (isOnlyIssueNumber) && (revisionIndex < Count); ++revisionIndex)
                {
                    if (String.Compare(this[revisionIndex].IssueNumber, issueNumber, true) != 0)
                        // A different Issue Number
                        isOnlyIssueNumber = false;
                }

                return isOnlyIssueNumber;
            }

            public bool HasPromotionGroup(string promotionGroup )
            {
                bool hasPromotionGroup = false;

                for (int revisionIndex = 0; (!hasPromotionGroup) && (revisionIndex < Count); ++revisionIndex)
                {
                    if (String.Compare(this[revisionIndex].PromotionGroup, promotionGroup, true) != 0)
                        // A different Issue Number
                        hasPromotionGroup = true;
                }

                return hasPromotionGroup;
            }

            public bool RevisionsOnTheSameBranch( string first, string second )
            {
                bool onSameBranch = true;

                string[] revisionPartFirst = first.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                string[] revisionPartSecond = second.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                if (revisionPartFirst.Length != revisionPartSecond.Length)
                    onSameBranch = false;
                else
                {
                    // Revision Numbers have the same count of version number parts
                    // Check Revision Number parts up to but not including the last part
                    // since that is effectively the Revision Number on the Branch
                    bool finished = false;
                    for (int part = 0; (!finished) && (part < (revisionPartFirst.Length-1)); ++part)
                    {
                        int secondPart;
                        if (!int.TryParse(revisionPartSecond[part], out secondPart))
                        {
                            Console.WriteLine("Second Revision Part {0} = {1} is unknown", part, revisionPartSecond[part]);
                        }
                        else
                        {
                            int firstPart;
                            if (!int.TryParse(revisionPartFirst[part], out firstPart))
                            {
                                Console.WriteLine("First Revision Part {0} = {1} is unknown", part, revisionPartFirst[part]);
                            }
                            else
                            {
                                // Stop at the first non-equal part (highest to lowest significance)
                                if (firstPart != secondPart)
                                {
                                    onSameBranch = false;
                                    finished = true;
                                }
                            }
                        }
                    }

                } // Revision Numbers have the same count of version number parts

                return onSameBranch;
            }

            public bool RevisionNumberIsGreater( string first , string second)
            {
                string[] revisionPartFirst = first.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                string[] revisionPartSecond = second.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                bool firstIsGreater = false;
                bool finished = false;

                for (int part = 0; (!finished) && (part < Math.Min(revisionPartSecond.Length, revisionPartFirst.Length)); ++part)
                {
                    int secondPart;
                    if (!int.TryParse(revisionPartSecond[part], out secondPart))
                    {
                        Console.WriteLine("Second Revision Part {0} = {1} is unknown", part, revisionPartSecond[part]);
                    }
                    else
                    {
                        int firstPart;
                        if (!int.TryParse(revisionPartFirst[part], out firstPart))
                        {
                            Console.WriteLine("First Revision Part {0} = {1} is unknown", part, revisionPartFirst[part]);
                        }
                        else
                        {
                            // Stop at the first non-equal part (highest to lowest significance)
                            if (firstPart != secondPart)
                            {
                                firstIsGreater = firstPart > secondPart;
                                finished = true;
                            }
                        }
                    }
                }
                return firstIsGreater;
            }

            public PvcsArchiveRevisionDetail HighestRevisionWithPromotionGroup(string promotionGroup)
            {
                PvcsArchiveRevisionDetail pvcsArchiveRevisionDetail = null;

                for (int revisionIndex = 0; revisionIndex < Count; ++revisionIndex)
                {
                    if (String.Compare(this[revisionIndex].PromotionGroup, promotionGroup, true) == 0)
                    {
                        // Examine the revision detail

                        if (pvcsArchiveRevisionDetail == null )
                            // First one found
                            pvcsArchiveRevisionDetail = this[revisionIndex];
                        else
                        {
                            // A revision was found earlier

                            string [] revisionPartPrevious = pvcsArchiveRevisionDetail.RevisionNumber.Split(new char [] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                            string[] revisionPartThis = this[revisionIndex].RevisionNumber.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                            bool thisIsGreater = false;
                            bool finished = false;

                            for (int part = 0; (!finished) && (part < Math.Min(revisionPartPrevious.Length, revisionPartThis.Length)); ++part)
                            {
                                int previousPart;
                                if (!int.TryParse(revisionPartPrevious[part], out previousPart))
                                {
                                    Console.WriteLine("Previous Revision Part {0} = {1} is unknown", part, revisionPartPrevious[part]);
                                }
                                else
                                {
                                    int thisPart;
                                    if (!int.TryParse(revisionPartThis[part], out thisPart))
                                    {
                                        Console.WriteLine("Previous Revision Part {0} = {1} is unknown", part, revisionPartThis[part]);
                                    }
                                    else
                                    {
                                        // Stop at the first non-equal part (highest to lowest significance)
                                        if (thisPart != previousPart)
                                        {
                                            thisIsGreater = thisPart > previousPart;
                                            finished = true;
                                        }
                                    }
                                    
                                }

                                if (thisIsGreater)
                                    // This revision is higher
                                    pvcsArchiveRevisionDetail = this[revisionIndex];
                            }

                        } // A revision was found earlier

                    } // Examine the revision detail
                }

                return pvcsArchiveRevisionDetail;
            }

            public PvcsArchiveRevisionDetail RevisionWithIssueNumber(string issueNumber)
            {
                PvcsArchiveRevisionDetail pvcsArchiveRevisionDetail = null;

                for (int revisionIndex = 0; (pvcsArchiveRevisionDetail == null ) && (revisionIndex < Count); ++revisionIndex)
                {
                    if (String.Compare(this[revisionIndex].IssueNumber, issueNumber, true) != 0)
                        // Return the revision detail
                        pvcsArchiveRevisionDetail = this[revisionIndex];
                }

                return pvcsArchiveRevisionDetail;
            }
        }

        public class PvcsArchiveDetail
        {
            public PvcsArchiveDetail(string name )
            {
                Name = name;
            }

            public string Name { get; private set; }

            public void AddArchiveDetail( string reportDataText )
            {
                string [] archiveDetailItem = reportDataText.Split(new char[] {' ',':','='},StringSplitOptions.RemoveEmptyEntries);

                string promotionGroup = archiveDetailItem[0];
                if ( string.Compare(promotionGroup,"[NoPromoGroup]",true)==0)
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
                colonIndex = reportDataText.IndexOf(':', colonIndex+1);
                string description;
                if ((colonIndex + 1) < reportDataText.Length)
                    description = reportDataText.Substring(colonIndex + 1).Trim(' ');
                else
                    description = "";

                string[] descriptionPart = description.Split(new char[] {' ','-'},StringSplitOptions.RemoveEmptyEntries);

                string revisionNumber = archiveDetailItem[1];
                string issueNumber = descriptionPart[0].Trim(new char[] {':'}).ToUpper();
                if (descriptionPart.Length > 1)
                {
                    int numeric = 0;
                    if (Int32.TryParse(descriptionPart[1], out numeric))
                    {
                        issueNumber += descriptionPart[1];
                    }
                }
                if ( String.IsNullOrEmpty(issueNumber))
                    Console.WriteLine( "\"{0}\" has empty Issue Number in \"{1}\"",Name,description);
                string developerId = archiveDetailItem[2];
                PvcsArchiveRevisionDetail pvcsArchiveRevisionDetail =
                    new PvcsArchiveRevisionDetail(Name, revisionNumber, promotionGroup, issueNumber, developerId, description);

                PvcsArchiveRevisionDetailCollection.Add(pvcsArchiveRevisionDetail);
            }

            public PvcsArchiveRevisionDetailCollection PvcsArchiveRevisionDetailCollection = new PvcsArchiveRevisionDetailCollection();

            public void Display(int indent)
            {
                Console.WriteLine();
                Console.WriteLine("{0}{1}", PvcsAllArchiveDetail.Indent(indent), Name);
                foreach (PvcsArchiveRevisionDetail pvcsArchiveRevisionDetail in PvcsArchiveRevisionDetailCollection)
                {
                    pvcsArchiveRevisionDetail.Display(indent + 1,false);
                }
            }

            public void CheckDescendents( int indent, string issueNumber , string promotionGroup, SortedSet<string> additionalIssueNumberCollection )
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
                                &&(String.Compare(pvcsArchiveRevisionDetail.IssueNumber,issueNumber,true)!=0))
                            {
                                // Display the User_Test details
                                displayOutput.Add(pvcsArchiveRevisionDetail.ToString(indent + 1));
                                if (String.IsNullOrEmpty(pvcsArchiveRevisionDetail.IssueNumber))
                                    additionalIssueNumberCollection.Add(pvcsArchiveRevisionDetail.NonEmptyIssueNumber + " : " + pvcsArchiveRevisionDetail.PromotionGroup + " - \"" + pvcsArchiveRevisionDetail.ArchiveName + "\"");
                                else
                                    additionalIssueNumberCollection.Add(pvcsArchiveRevisionDetail.NonEmptyIssueNumber + " : " + pvcsArchiveRevisionDetail.PromotionGroup);
                            }
                            else if ((String.Compare(pvcsArchiveRevisionDetail.PromotionGroup, "Pre_Production", true) ==0)
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
                        Console.WriteLine("{0}{1} : Has Issue Number {2}", PvcsAllArchiveDetail.Indent(indent), Name, issueNumber);
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
                                              PvcsAllArchiveDetail.Indent(indent), Name, issueNumber);
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
                    int maximumHierarchyIndex = PromotionGroupDetailCollection.HierarchyIndex(promotionGroup) - 1;

                    for (int hierarchyIndex = PvcsPromotionGroupDetailCollection.DevelopmentHierarchyBaseIndex;
                            hierarchyIndex < maximumHierarchyIndex;
                            ++hierarchyIndex)
                    {
                        string higherPromotionGroup = PromotionGroupDetailCollection.PromotionGroup(hierarchyIndex);
                        PvcsArchiveRevisionDetail higherRevisionWithPromotionGroup = PvcsArchiveRevisionDetailCollection.HighestRevisionWithPromotionGroup(higherPromotionGroup);
                        if ( higherRevisionWithPromotionGroup != null )
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
                                Console.WriteLine("{0}Is not on the same branch",Indent(indent));
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
                                    Console.WriteLine("{0}Is not at a higher revision", Indent(indent));
                                }
                            }
                        }
                    }

                } // A revision with this Promotion Group exists
            }

            public void GeneratePromotionEntry( string issueNumber , string promotionGroup )
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
                        if (pvcsArchiveRevisionDetail.ArchiveName.IndexOf(' ')>=0)
                            Console.Write("\"{0}\"", pvcsArchiveRevisionDetail.ArchiveName);
                        else
                            Console.Write("{0}", pvcsArchiveRevisionDetail.ArchiveName);
                        Console.WriteLine(" - {0} : {1} = {2} = NoLockers",
                            pvcsArchiveRevisionDetail.PromotionGroup,
                            pvcsArchiveRevisionDetail.RevisionNumber,
                            (String.IsNullOrEmpty(pvcsArchiveRevisionDetail.DeveloperId)?"Unknown":pvcsArchiveRevisionDetail.DeveloperId));
                    }

                } // This Archive has this Issue Number
            }

        } // PvcsArchiveDetail

        public class PvcsArchiveDetailCollectionType : SortedDictionary<string,PvcsArchiveDetail>
        {
            public PvcsArchiveDetailCollectionType() : base(StringComparer.CurrentCultureIgnoreCase)
            {
            }
        }

        public PvcsArchiveDetailCollectionType PvcsArchiveDetailCollection = new PvcsArchiveDetailCollectionType();

        public void Display()
        {
            string heading = String.Format("Change Summary");
            Console.WriteLine();
            Console.WriteLine(heading);
            Console.WriteLine(new String('~', heading.Length));

            foreach (KeyValuePair<string, PvcsArchiveDetail> pvcsArchiveInfo in PvcsArchiveDetailCollection)
            {
                pvcsArchiveInfo.Value.Display(1);
            }
        }

        public void CheckDescendents(string issueNumber, string promotionGroup , SortedSet<string> additionalIssueNumberCollection )
        {
            string heading = String.Format("Descendent Check for Issue Number {0} and {1}",issueNumber,promotionGroup);

            Console.WriteLine();
            Console.WriteLine(heading);
            Console.WriteLine(new String('~',heading.Length));

            foreach (KeyValuePair<string, PvcsArchiveDetail> pvcsArchiveInfo in PvcsArchiveDetailCollection)
            {
                pvcsArchiveInfo.Value.CheckDescendents(1,issueNumber, promotionGroup, additionalIssueNumberCollection );
            }
        }

        public void CheckBuriedPromotionGroup( string promotionGroup )
        {
            string heading = String.Format("Buried Promotion Group Report {0}", promotionGroup);

            Console.WriteLine();
            Console.WriteLine(heading);
            Console.WriteLine(new String('~', heading.Length));

            foreach (KeyValuePair<string, PvcsArchiveDetail> pvcsArchiveInfo in PvcsArchiveDetailCollection)
            {
                pvcsArchiveInfo.Value.CheckBuriedPromotionGroup(1,promotionGroup);
            }
        }

        public static string Indent(int indent)
        {
            return new String(' ',indent*4);
        }

        public void GeneratePromotionList( string issueNumber , string promotionGroup )
        {
            foreach ( KeyValuePair<string, PvcsArchiveDetail> pvcsArchiveInfo in PvcsArchiveDetailCollection)
            {
                pvcsArchiveInfo.Value.GeneratePromotionEntry(issueNumber, promotionGroup);
            }
        }

        public static PvcsPromotionGroupDetailCollection PromotionGroupDetailCollection = new PvcsPromotionGroupDetailCollection();

    } // PvcsAllArchiveDetail

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                string reportName = args[0];
                string selectedIssueNumber = args[1];

                PvcsAllArchiveDetail pvcsAllArchiveDetail = new PvcsAllArchiveDetail(reportName);

                pvcsAllArchiveDetail.Display();

                SortedSet<string> additionalIssueNumberCollection = new SortedSet<string>();

                pvcsAllArchiveDetail.CheckDescendents(selectedIssueNumber, "System_Test", additionalIssueNumberCollection);

                if (additionalIssueNumberCollection.Count == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("*** No Issue Numbers in addtion to {0} were found",selectedIssueNumber);
                }
                else
                {
                    string heading = String.Format("Issue Numbers Found in Addition to {0}",selectedIssueNumber);
                    Console.WriteLine();
                    Console.WriteLine(heading);
                    Console.WriteLine(new string('~', heading.Length));
                    foreach (string issueNumber in additionalIssueNumberCollection)
                    {
                        Console.WriteLine("{0}{1}", PvcsAllArchiveDetail.Indent(1), issueNumber);
                    }
                }

                string promotionGroup = "System_Test";
                Console.WriteLine();
                string promotionHeading = String.Format("Promotion List for Issue Number {0} at {1}", selectedIssueNumber, promotionGroup);
                Console.WriteLine(promotionHeading);
                Console.WriteLine(new string('~', promotionHeading.Length));
                pvcsAllArchiveDetail.GeneratePromotionList(selectedIssueNumber, promotionGroup);

                pvcsAllArchiveDetail.CheckBuriedPromotionGroup("System_Test");
            }

        } // Main
    }
}
