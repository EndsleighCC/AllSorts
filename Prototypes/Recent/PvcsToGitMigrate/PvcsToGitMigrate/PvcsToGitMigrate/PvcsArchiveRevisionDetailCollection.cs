using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PvcsChangeControl
{

    public class PvcsArchiveRevisionDetailCollection : Collection<PvcsArchiveRevisionDetail>
    {
        public PvcsArchiveRevisionDetailCollection()
            : base()
        {
        }

        public bool HasIssueNumber(string issueNumber)
        {
            bool hasIssueNumber = false;

            for (int revisionIndex = 0; (!hasIssueNumber) && (revisionIndex < Count); ++revisionIndex)
            {
                if (String.Compare(this[revisionIndex].IssueNumber, issueNumber, true) == 0)
                {
                    // Found this Issue Number
                    hasIssueNumber = true;
                }
            }

            return hasIssueNumber;
        }

        public bool IsOnlyIssueNumber(string issueNumber)
        {
            bool isOnlyIssueNumber = true;

            for (int revisionIndex = 0; (isOnlyIssueNumber) && (revisionIndex < Count); ++revisionIndex)
            {
                if (String.Compare(this[revisionIndex].IssueNumber, issueNumber, true) != 0)
                {
                    // A different Issue Number
                    isOnlyIssueNumber = false;
                }
            }

            return isOnlyIssueNumber;
        }

        public bool HasPromotionGroup(string promotionGroup)
        {
            bool hasPromotionGroup = false;

            for (int revisionIndex = 0; (!hasPromotionGroup) && (revisionIndex < Count); ++revisionIndex)
            {
                if (String.Compare(this[revisionIndex].PromotionGroup, promotionGroup, true) == 0)
                {
                    // Matching Promotion Group
                    hasPromotionGroup = true;
                }
            }

            return hasPromotionGroup;
        }

        public bool RevisionsOnTheSameBranch(string first, string second)
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
                for (int part = 0; (!finished) && (part < (revisionPartFirst.Length - 1)); ++part)
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

        public bool RevisionNumberIsGreater(string first, string second)
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

                    if (pvcsArchiveRevisionDetail == null)
                        // First one found
                        pvcsArchiveRevisionDetail = this[revisionIndex];
                    else
                    {
                        // A revision was found earlier

                        string[] revisionPartPrevious = pvcsArchiveRevisionDetail.RevisionNumber.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
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

            for (int revisionIndex = 0; (pvcsArchiveRevisionDetail == null) && (revisionIndex < Count); ++revisionIndex)
            {
                if (String.Compare(this[revisionIndex].IssueNumber, issueNumber, true) != 0)
                    // Return the revision detail
                    pvcsArchiveRevisionDetail = this[revisionIndex];
            }

            return pvcsArchiveRevisionDetail;
        }
    }
}
