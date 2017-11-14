using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGitMigrate
{
    class Program
    {
        private static void GitAdd( PvcsCompleteSystemArchiveDetail pvcsCompleteSystemArchiveDetail,
                                    string promotionGroup,
                                    string sourcePath ,
                                    string destinationPath )
        {

            Console.WriteLine();
            Console.WriteLine("Add Source Revisions for \"{0}\"",promotionGroup);
            if (!pvcsCompleteSystemArchiveDetail.GitAdd(promotionGroup, 0, sourcePath, destinationPath))
            {
                Console.WriteLine("*** Not all \"{0}\" Source Revisions were added",promotionGroup);
            }
        }

        private static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                string reportPath = args[0];
                string selectedIssueNumber = null;

                if (args.Length > 1)
                    selectedIssueNumber = args[1];

                PvcsCompleteSystemArchiveDetail pvcsCompleteSystemArchiveDetail = new PvcsCompleteSystemArchiveDetail(
                    reportPath,
                    PvcsCompleteSystemArchiveDetail.PvcsArchiveDetailLevel.ChangesOnly);

                pvcsCompleteSystemArchiveDetail.Display();

                if (selectedIssueNumber != null)
                {
                    // An Issue Number has been supplied

                    SortedSet<string> additionalIssueNumberCollection = new SortedSet<string>();

                    pvcsCompleteSystemArchiveDetail.CheckDescendents(selectedIssueNumber, "System_Test",
                        additionalIssueNumberCollection);

                    if (additionalIssueNumberCollection.Count == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("*** No Issue Numbers in addtion to {0} were found", selectedIssueNumber);
                    }
                    else
                    {
                        string heading = String.Format("Issue Numbers Found in Addition to {0}", selectedIssueNumber);
                        Console.WriteLine();
                        Console.WriteLine(heading);
                        Console.WriteLine(new string('~', heading.Length));
                        foreach (string issueNumber in additionalIssueNumberCollection)
                        {
                            Console.WriteLine("{0}{1}", PvcsCompleteSystemArchiveDetail.Indent(1), issueNumber);
                        }
                    }

                    string promotionGroup = "System_Test";
                    Console.WriteLine();
                    string promotionHeading = String.Format("Promotion List for Issue Number {0} at {1}",
                        selectedIssueNumber, promotionGroup);
                    Console.WriteLine(promotionHeading);
                    Console.WriteLine(new string('~', promotionHeading.Length));
                    pvcsCompleteSystemArchiveDetail.GeneratePromotionList(selectedIssueNumber, promotionGroup);

                    pvcsCompleteSystemArchiveDetail.CheckBuriedPromotionGroup("System_Test");

                } // An Issue Number has been supplied

                GitAdd(pvcsCompleteSystemArchiveDetail, "Production", "\\\\ADEBS02\\SysPR00\\", "d:\\Repos\\HeritageTest");
                GitAdd(pvcsCompleteSystemArchiveDetail, "Pre_Production", "\\\\ADEBS02\\SysPR00\\","d:\\Repos\\HeritageTest");
                // GitAdd(pvcsCompleteSystemArchiveDetail, "User_Test", "\\\\ADEBS02\\SysUT00\\", "d:\\Repos\\Heritage");

            } // Main
        }
    }
}
