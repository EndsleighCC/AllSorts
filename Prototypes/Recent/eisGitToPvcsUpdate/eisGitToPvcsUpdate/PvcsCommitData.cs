using System;
using System.Linq;

namespace eisGitToPvcsUpdate
{
    public class PvcsCommitData
    {
        public PvcsCommitData(string pvcsCommitUserId)
        {
            PvcsCommitUserId = pvcsCommitUserId;
        }

        public string PvcsCommitUserId { get; private set; }
        public string Hash { get; set; }
        public string emailAddress { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public string FullChangeDescription
        {
            get
            {
                string fullChangeDescription = "";
                fullChangeDescription = "PVCS Commit User Id: " + PvcsCommitUserId + Environment.NewLine
                                        + "Git Commit Hash: " + Hash + Environment.NewLine
                                        + emailAddress + Environment.NewLine
                                        + DateTime.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine
                                        + Description + Environment.NewLine;
                return fullChangeDescription;
            }
        } // FullChangeDescription

        public PvcsCommitFileDataSortedSet PvcsCommitFileDataCollection = new PvcsCommitFileDataSortedSet();

        public bool Committed { get; private set; }

        public int Commit(string pvcsUserId,
                          BuildSetDetails buildsetDetails,
                          PvcsPromotionGroupData pvcsPromotionGroupData,
                          PvcsPromotionGroupDataSortedSet pvcsPromotionGroupDataSortedSet)
        {
            int error = WindowsErrorDefinition.Success;

            // Make sure there is something to commit
            if ((String.IsNullOrEmpty(PvcsCommitUserId))
                 || (String.IsNullOrEmpty(Hash))
                 || (String.IsNullOrEmpty(emailAddress))
                 || ((DateTime == DateTime.MinValue))
                 || (String.IsNullOrEmpty(Description))
               )
            {
                Console.WriteLine("PvcsCommitData.Commit : Insufficient data to perform a Commit");
            }
            else
            {
                // There is Commit information

                if (!Committed)
                {
                    // This Commit has not been committed already

                    if (PvcsCommitFileDataCollection.Count < 1)
                    {
                        Console.WriteLine("PvcsCommitData.Commit : No files to commit");
                        error = WindowsErrorDefinition.InvalidFunction;
                    }
                    else
                    {
                        // There are files to commit

                        error = PvcsConfiguration.SetPvcsEnvironment(pvcsUserId);
                        if (error == WindowsErrorDefinition.Success)
                        {
                            // Successfully set the PVCS Environment

                            for (int fileIndex = 0;
                                (fileIndex < PvcsCommitFileDataCollection.Count)
                                && (error == WindowsErrorDefinition.Success);
                                ++fileIndex)
                            {
                                error = PvcsCommitFileDataCollection.ElementAt(fileIndex).Commit(
                                                pvcsUserId,
                                                FullChangeDescription,
                                                buildsetDetails,
                                                pvcsPromotionGroupData,
                                                pvcsPromotionGroupDataSortedSet);
                                if (error != WindowsErrorDefinition.Success)
                                {
                                    Console.WriteLine("PvcsCommitData.Commit : File with index {0} = \"{1}\" failed to commit into PVCS",
                                        fileIndex, PvcsCommitFileDataCollection.ElementAt(fileIndex).Name);
                                }
                            } // for fileIndex

                        } // Successfully set the PVCS Environment
                    } // There are files to commit

                    Committed = true;

                } // This Commit has not been committed already
            } // There is Commit information

            return error;
        } // Commit

    } // PvcsCommitData
}
