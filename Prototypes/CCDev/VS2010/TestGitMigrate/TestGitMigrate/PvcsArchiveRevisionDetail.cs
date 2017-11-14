using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGitMigrate
{
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
            if (!displayArchiveName)
                Console.WriteLine("{0}{1} {2} {3} {4:6} {5}", PvcsCompleteSystemArchiveDetail.Indent(indent), RevisionNumber, PromotionGroup, DeveloperId, (String.IsNullOrEmpty(IssueNumber) ? "\"\"" : IssueNumber), Description);
            else
                Console.WriteLine("{0}{1} {2} {3} {4} {5:6} {6}", PvcsCompleteSystemArchiveDetail.Indent(indent), ArchiveName, RevisionNumber, PromotionGroup, DeveloperId, (String.IsNullOrEmpty(IssueNumber) ? "\"\"" : IssueNumber), Description);
        }

        public string ToString(int indent)
        {
            return String.Format("{0}{1} {2} {3} {4:6} \"{5}\"", PvcsCompleteSystemArchiveDetail.Indent(indent), RevisionNumber, PromotionGroup, DeveloperId, (String.IsNullOrEmpty(IssueNumber) ? "\"\"" : IssueNumber), Description);
        }

    } // PvcsArchiveRevisionDetail
}
