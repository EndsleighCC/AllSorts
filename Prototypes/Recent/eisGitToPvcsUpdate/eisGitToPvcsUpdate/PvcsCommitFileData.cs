using System;
using System.Collections.Generic;
using System.IO;
using eisGitToPvcsUpdate;

namespace eisGitToPvcsUpdate
{
    public class PvcsCommitFileData : IComparable<PvcsCommitFileData>
    {
        public PvcsCommitFileData(string name, string commitStatus)
        {
            Name = name;
            CommitStatus = commitStatus;
            // A commit file spec does not have a drive specifier and must be a Windows-style filename
            // Put the root specifier on the drive specifier and not at the start of the second argument
            // otherwise Path.Combine will generate a root path that excludes the drive specifier
            PvcsArchiveNameAndPath = Path.Combine(PvcsConfiguration.PvcsArchiveDriveSpec + "\\", Name);
        }

        public string Name { get; private set; }
        public string CommitStatus { get; private set; }

        public string PvcsArchiveNameAndPath { get; private set; }

        public int CompareTo(PvcsCommitFileData pvcsCommitFileData)
        {
            int compareValue = String.Compare(this.Name, pvcsCommitFileData.Name);

            return compareValue;
        }

        public int Commit(string pvcsUserId,
                          string changeDescription,
                          BuildSetDetails buildSetDetails,
                          PvcsPromotionGroupData pvcsPromotionGroupData,
                          PvcsPromotionGroupDataSortedSet pvcsPromotionGroupDataSortedSet)
        {
            int error = WindowsErrorDefinition.Success;

            // Check whether the PVCS Archive exists
            if (!File.Exists(PvcsArchiveNameAndPath))
            {
                Console.WriteLine("PvcsCommitFileData.Commit : PVCS Archive \"{0}\" does not exist", PvcsArchiveNameAndPath);
                error = WindowsErrorDefinition.FileNotFound;
            }
            else
            {
                // PVCS Archive exists

                Console.WriteLine("PvcsCommitFileData.Commit : Committing \"{0}\"", Name);

                // Identify all the Promotion Groups in the PVCS Archive
                SortedSet<string> promotionGroupSortedSet = new SortedSet<string>();
                error = PvcsCommandOperation.GetArchivePromotionGroupNames(PvcsArchiveNameAndPath, ref promotionGroupSortedSet);
                if (error != WindowsErrorDefinition.Success)
                {
                    Console.WriteLine("PvcsCommitFileData.Commit({0}) : Failed to locate any Promotion Groups within the PVCS Archive", PvcsArchiveNameAndPath);
                }
                else
                {
                    // Got the Promotion Groups within the Archive or there aren't any

                    if ((promotionGroupSortedSet.Count > 0)
                         && (promotionGroupSortedSet.Contains(pvcsPromotionGroupData.LowerSourcePromotionGroupName))
                       )
                    {
                        // There are Promotion Groups and the lower Promotion Group exists

                        error = PvcsCommandOperation.Promote(PvcsArchiveNameAndPath,
                                                             pvcsPromotionGroupData.LowerSourcePromotionGroupName);

                    } // There are Promotion Groups and the lower Promotion Group exists
                    else
                    {
                        // Try to Check In the new source via the Direct Checkin Promotion Group

                        // The mechanism of CheckIn is slightly different for an empty Archive

                        bool archiveIsEmpty = false;
                        error = PvcsCommandOperation.ArchiveIsEmpty(PvcsArchiveNameAndPath, out archiveIsEmpty);
                        if ((error == WindowsErrorDefinition.Success)
                             && (!archiveIsEmpty)
                           )
                        {
                            // Lock the Archive at the Direct CheckIn Promotion Group

                            if (pvcsPromotionGroupData.DirectCheckInPromotionGroupName == null)
                            {
                                Console.WriteLine("PvcsCommitFileData.Commit : Promotion Group \"{0}\" has no Direct CheckIn Promotion Group",
                                    pvcsPromotionGroupData.Name);
                                error = WindowsErrorDefinition.PathNotFound;
                            }
                            else
                            {
                                error = PvcsCommandOperation.Lock(PvcsArchiveNameAndPath,
                                                                  pvcsPromotionGroupData.DirectCheckInPromotionGroupName);
                            }

                        } // Lock the Archive at the Direct CheckIn Promotion Group

                        if (error == WindowsErrorDefinition.Success)
                        {
                            // Archive is ready for CheckIn

                            // Build the full path and filename to the source that has changed
                            string sourcePathAndFilename = Path.Combine(buildSetDetails.BuildSetRootPath, Name);

                            if ( archiveIsEmpty )
                            {
                                // Checking In to an empty Archive requires assigning the correct Promotion Group
                                error = PvcsCommandOperation.CheckIntoEmptyArchive(pvcsUserId,
                                                                                  changeDescription,
                                                                                  pvcsPromotionGroupData.
                                                                                          DirectCheckInPromotionGroupName,
                                                                                  PvcsArchiveNameAndPath,
                                                                                  sourcePathAndFilename);
                            }
                            else
                            {
                                // A non-empty Archive will have had a revision locked at the necessary Promotion Group
                                error = PvcsCommandOperation.CheckIntoNonEmptyArchive(pvcsUserId,
                                                                                      changeDescription,
                                                                                      PvcsArchiveNameAndPath,
                                                                                      sourcePathAndFilename);
                            }

                            if (error == WindowsErrorDefinition.Success)
                            {
                                // Source has been CheckedIn

                                // Promote to end up at the required Promotion Group

                                // "DevelopmentN" Promotion Groups have IsCandidate "Pre_System_TestN"
                                // Promotion Groups above them so there would need to be two Promotions
                                // to get to the necessary Promotion Group
                                if (pvcsPromotionGroupData.DirectCheckInPromotionGroupName.StartsWith("Development"))
                                {
                                    // There will need to be two Promotions

                                    // Promote from the Direct CheckIn Promotion Group
                                    error = PvcsCommandOperation.Promote(PvcsArchiveNameAndPath,
                                                                         pvcsPromotionGroupData.
                                                                             DirectCheckInPromotionGroupName);
                                    if (error == WindowsErrorDefinition.Success)
                                    {
                                        // Now Promote from the Group above the Direct CheckIn Promotion Group

                                        // Get the data for the Direct CheckIn Promotion Group
                                        PvcsPromotionGroupData pvcsPromotionGroupDataDirectCheckIn =
                                            pvcsPromotionGroupDataSortedSet.GetPromotionGroupData(
                                                pvcsPromotionGroupData.DirectCheckInPromotionGroupName);

                                        if (pvcsPromotionGroupDataDirectCheckIn == null)
                                        {
                                            Console.WriteLine(
                                                "PvcsCommitFileData.Commit : Unable to find the details for the"
                                                + " Promotion Group that is higher than \"{0}\"",
                                                pvcsPromotionGroupData.DirectCheckInPromotionGroupName);
                                            error = WindowsErrorDefinition.PathNotFound;
                                        }
                                        else
                                        {
                                            // Promote from the Group above the Direct CheckIn Promotion Group
                                            error = PvcsCommandOperation.Promote(PvcsArchiveNameAndPath,
                                                                                 pvcsPromotionGroupDataDirectCheckIn.NextHigherPromotionGroupName);
                                        }
                                    } // Now Promote from the Group above the Direct CheckIn Promotion Group

                                } // There will need to be two Promotions
                                else
                                {
                                    // There will need to be one Promotion only

                                    // Promote from the Direct CheckIn Promotion Group
                                    error = PvcsCommandOperation.Promote(PvcsArchiveNameAndPath,
                                                                         pvcsPromotionGroupData.
                                                                             DirectCheckInPromotionGroupName);

                                } // There will need to be one Promotion only

                            } // Source has been CheckedIn

                        } // Archive is ready for CheckIn

                    } // Try to Check In the new source via the Direct Checkin Promotion Group

                } // Got the Promotion Groups within the Archive or there aren't any

            } // PVCS Archive exists

            return error;
        } // Commit

    } // PvcsCommitFileData
}
