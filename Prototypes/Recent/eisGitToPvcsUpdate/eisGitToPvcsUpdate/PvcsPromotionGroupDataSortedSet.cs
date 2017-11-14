using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace eisGitToPvcsUpdate
{
    public class PvcsPromotionGroupDataSortedSet : SortedSet<PvcsPromotionGroupData>
    {
        public PvcsPromotionGroupDataSortedSet()
            : base()
        {
            // Name, HierarchyIndex, IsCandidate, LowerNonCandidate, LowerPromotionGroup, NextHigher, DirectCheckIn
            this.Add(new PvcsPromotionGroupData("Development", 0, false, null, null, "Pre_System_Test", null));
            this.Add(new PvcsPromotionGroupData("Development1", 0, false, null, null, "Pre_System_Test1", null));
            this.Add(new PvcsPromotionGroupData("Development2", 0, false, null, null, "Pre_System_Test2", null));
            this.Add(new PvcsPromotionGroupData("Development3", 0, false, null, null, "Pre_System_Test3", null));
            this.Add(new PvcsPromotionGroupData("Development4", 0, false, null, null, "Pre_System_Test4", null));
            this.Add(new PvcsPromotionGroupData("Development5", 0, false, null, null, "Pre_System_Test5", null));
            this.Add(new PvcsPromotionGroupData("Development6", 0, false, null, null, "Pre_System_Test6", null));
            this.Add(new PvcsPromotionGroupData("Pre_System_Test_Staging", 0, false, null, null, "System_Test", null));
            this.Add(new PvcsPromotionGroupData("Pre_System_Test", 1, true, "Development", "Development", "System_Test", "Development"));
            this.Add(new PvcsPromotionGroupData("Pre_System_Test1", 1, true, "Development1", "Development1", "System_Test1", "Development1"));
            this.Add(new PvcsPromotionGroupData("Pre_System_Test2", 1, true, "Development2", "Development2", "System_Test2", "Development2"));
            this.Add(new PvcsPromotionGroupData("Pre_System_Test3", 1, true, "Development3", "Development3", "System_Test3", "Development3"));
            this.Add(new PvcsPromotionGroupData("Pre_System_Test4", 1, true, "Development4", "Development4", "System_Test4", "Development4"));
            this.Add(new PvcsPromotionGroupData("Pre_System_Test5", 1, true, "Development5", "Development5", "System_Test5", "Development5"));
            this.Add(new PvcsPromotionGroupData("Pre_System_Test6", 1, true, "Development6", "Development6", "System_Test6", "Development6"));
            this.Add(new PvcsPromotionGroupData("System_Test", 2, false, "Development", "Pre_System_Test", "User_Test", "Pre_System_Test_Staging"));
            this.Add(new PvcsPromotionGroupData("System_Test1", 2, false, "Development1", "Pre_System_Test1", "Pre_System_Test", "Development1"));
            this.Add(new PvcsPromotionGroupData("System_Test2", 2, false, "Development2", "Pre_System_Test2", "Pre_System_Test", "Development2"));
            this.Add(new PvcsPromotionGroupData("System_Test3", 2, false, "Development3", "Pre_System_Test3", "Pre_System_Test", "Development3"));
            this.Add(new PvcsPromotionGroupData("System_Test4", 2, false, "Development4", "Pre_System_Test4", "Pre_System_Test", "Development4"));
            this.Add(new PvcsPromotionGroupData("System_Test5", 2, false, "Development5", "Pre_System_Test5", "Pre_System_Test", "Development5"));
            this.Add(new PvcsPromotionGroupData("System_Test6", 2, false, "Development6", "Pre_System_Test6", "Pre_System_Test", "Development6"));
            this.Add(new PvcsPromotionGroupData("Pre_User_Test_Staging", 0, false, null, null, "User_Test", null));
            this.Add(new PvcsPromotionGroupData("Pre_User_Test", 3, true, "Development", "System_Test", "User_Test", "Pre_User_Test_Staging"));
            this.Add(new PvcsPromotionGroupData("User_Test", 4, false, "Pre_User_Test_Staging", "Pre_User_Test", "Pre_Production", "Pre_User_Test_Staging"));
            this.Add(new PvcsPromotionGroupData("Pre_Production", 5, false, "User_Test", "User_Test", "Production", "Pre_Release"));
            this.Add(new PvcsPromotionGroupData("Production", 6, true, "Pre_Release", "Pre_Production", null, null));
        }

        public PvcsPromotionGroupData GetPromotionGroupData(string name)
        {
            PvcsPromotionGroupData pvcsPromotionGroupData = null;

            try
            {
                pvcsPromotionGroupData = this.First(p => p.Name == name);
            }
            catch (InvalidOperationException)
            {
                // Nothing that matches so null will be returned
            }
            catch (Exception ex)
            {
                Console.WriteLine("PvcsPromotionGroupDataSortedSet.GetPromotionGroupData : Unexpected exception = {0}", ex.ToString());
            }

            return pvcsPromotionGroupData;
        } // GetPromotionGroupData

        private enum ReadingState { Reading, ProcessingCommitAttributes, ProcessingCommitFilenames } ;

        public int UpdatePvcsForPromotionGroup(string pvcsUserId, BuildSetDetails buildsetDetails, string gitLogReportFilenameWithPath)
        {
            int error = 0;
            PvcsPromotionGroupData promotionGroupData = GetPromotionGroupData(buildsetDetails.SecondaryIdentifier);
            if (promotionGroupData == null)
            {
                Console.WriteLine("PvcsPromotionGroupDataSortedSet.UpdatePvcsForPromotionGroup : Promotion Group \"{0}\" is not known", buildsetDetails.SecondaryIdentifier);
                error = WindowsErrorDefinition.BadEnvironment;
            }
            else
            {
                // Promotion Group is known

                if (!File.Exists(gitLogReportFilenameWithPath))
                {
                    Console.WriteLine("PvcsPromotionGroupDataSortedSet.UpdatePvcsForPromotionGroup : git log report file \"{0}\" does not exist", gitLogReportFilenameWithPath);
                    error = WindowsErrorDefinition.FileNotFound;
                }
                else
                {
                    // git log report file exists

                    Console.WriteLine("Current directory is \"{0}\"",Directory.GetCurrentDirectory());

                    using (StreamReader gitLogFileStream = new StreamReader(gitLogReportFilenameWithPath))
                    {
                        string gitLogLine = null;
                        int lineNumber = 0;
                        int commitCount = 0;
                        int commitAttributeCount = 0;
                        PvcsCommitData pvcsCommitData = null; // One object reused for each commit

                        ReadingState readingState = ReadingState.Reading;

                        while (((gitLogLine = gitLogFileStream.ReadLine()) != null) && (error == WindowsErrorDefinition.Success))
                        {
                            lineNumber += 1;
                            Console.WriteLine("{0,-2} : {1}", lineNumber.ToString("###"), gitLogLine);
                            if (!gitLogLine.StartsWith("#"))
                            {
                                // Not a commented out line
                                switch (readingState)
                                {
                                    case ReadingState.Reading:
                                        // Check for entering a processing state
                                        if (gitLogLine == "+++")
                                        {
                                            readingState = ReadingState.ProcessingCommitAttributes;
                                            commitCount += 1;
                                            commitAttributeCount = 0;
                                            // Replace any existing commit data
                                            pvcsCommitData = new PvcsCommitData(pvcsUserId);
                                        }
                                        break;
                                    case ReadingState.ProcessingCommitAttributes:
                                        // Check for a exiting a commit attributs processing state
                                        if (gitLogLine == "---")
                                        {
                                            readingState = ReadingState.ProcessingCommitFilenames;
                                        }
                                        else
                                        {
                                            // Process the attribute

                                            switch (commitAttributeCount)
                                            {
                                                case 0:
                                                    // Must be the Commit Hash
                                                    pvcsCommitData.Hash = gitLogLine;
                                                    commitAttributeCount += 1;
                                                    break;
                                                case 1:
                                                    // Must be the email address
                                                    pvcsCommitData.emailAddress = gitLogLine;
                                                    commitAttributeCount += 1;
                                                    break;
                                                case 2:
                                                    {
                                                        // Must be the Commit Data/Time
                                                        try
                                                        {
                                                            pvcsCommitData.DateTime = System.Convert.ToDateTime(gitLogLine);
                                                        }
                                                        catch (Exception)
                                                        {
                                                            pvcsCommitData.DateTime = DateTime.Now;
                                                            Console.WriteLine("{0} : Line Number {1} : Unable to convert \"{2}\" to a DateTime, Defaulting to \"Now\" = {3}",
                                                                gitLogReportFilenameWithPath, lineNumber, gitLogLine, pvcsCommitData.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                                        }
                                                        commitAttributeCount += 1;
                                                        break;
                                                    }
                                                case 3:
                                                    // Cope with multi-line descriptions
                                                    if (String.IsNullOrEmpty(pvcsCommitData.Description))
                                                    {
                                                        commitAttributeCount += 1;
                                                    }
                                                    if (!(String.IsNullOrEmpty(gitLogLine)) && (!String.IsNullOrWhiteSpace(gitLogLine)))
                                                    {
                                                        if (String.IsNullOrEmpty(pvcsCommitData.Description))
                                                        {
                                                            pvcsCommitData.Description = gitLogLine;
                                                        }
                                                        else
                                                        {
                                                            // Separate multiple lines with a space
                                                            pvcsCommitData.Description += " " + gitLogLine;
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    Console.WriteLine("PvcsPromotionGroupDataSortedSet.UpdatePvcsForPromotionGroup : Unexpected Commit Attribute Index {0}", commitAttributeCount);
                                                    break;
                                            }

                                        } // Process the attribute
                                        break;
                                    case ReadingState.ProcessingCommitFilenames:
                                        if (gitLogLine.Length == 0)
                                        {
                                            // Nothing to do
                                        }
                                        else if (gitLogLine == "+++")
                                        {
                                            // Found the next commit entry so process this one

                                            error = pvcsCommitData.Commit(pvcsUserId,
                                                                          buildsetDetails,
                                                                          promotionGroupData,
                                                                          this);

                                            if (error == WindowsErrorDefinition.Success)
                                            {
                                                // Go back to processing commit attributes
                                                readingState = ReadingState.ProcessingCommitAttributes;
                                                commitCount += 1;
                                                commitAttributeCount = 0;
                                                pvcsCommitData = new PvcsCommitData(pvcsUserId);
                                            } // Go back to processing commit attributes

                                        } // Found the next commit entry so process this one
                                        else
                                        {
                                            // Process the filename
                                            string commitstatus = new string(gitLogLine.ToCharArray(), 0, 1);
                                            string filename = gitLogLine.Substring(1).Trim();
                                            // Replace all forward slashes with back slashes
                                            filename = filename.Replace("/", "\\");
                                            PvcsCommitFileData pvcsCommitFileData = new PvcsCommitFileData(filename, commitstatus);
                                            pvcsCommitData.PvcsCommitFileDataCollection.Add(pvcsCommitFileData);

                                        } // Process the filename
                                        break;
                                    default:
                                        Console.WriteLine("Reading \"{0}\" : Unexpected Reading State = \"{1}\"", gitLogReportFilenameWithPath, readingState.ToString());
                                        break;
                                } // switch
                            } // Not a commented out line
                        } // while

                        if (error == WindowsErrorDefinition.Success)
                        {
                            // Ensure it was committed
                            error = pvcsCommitData.Commit(pvcsUserId,
                                                          buildsetDetails,
                                                          promotionGroupData,
                                                          this);
                        }

                        if (error == WindowsErrorDefinition.Success)
                        {
                            Console.WriteLine("PvcsPromotionGroupDataSortedSet.UpdatePvcsForPromotionGroup : Successfully processed {0} PVCS Commits", commitCount);
                        }
                        else
                        {
                            Console.WriteLine("PvcsPromotionGroupDataSortedSet.UpdatePvcsForPromotionGroup : Processed {0} PVCS Commits with error {1}", commitCount, error);
                        }

                    } // using

                } // git log report file exists

            } // Promotion Group is known
            return error;
        } // UpdatePvcsForPromotionGroup

    } // PvcsPromotionGroupDataSortedSet
}
