using System;

namespace eisGitToPvcsUpdate
{
    public class PvcsPromotionGroupData : IComparable<PvcsPromotionGroupData>
    {
        public PvcsPromotionGroupData( string name,
                                       int hierarchyIndex,
                                       bool isCandidate ,
                                       string lowerNonCandidatePromotionGroupName ,
                                       string lowerSourcePromotionGroup ,
                                       string nextHigherPromotionGroupName,
                                       string directCheckInPromotionGroupName)
        {
            Name = name;
            HierarchyIndex = hierarchyIndex;
            IsCandidate = isCandidate;
            LowerNonCandidatePromotionGroupName = lowerNonCandidatePromotionGroupName;
            LowerSourcePromotionGroupName = lowerSourcePromotionGroup;
            NextHigherPromotionGroupName = nextHigherPromotionGroupName;
            DirectCheckInPromotionGroupName = directCheckInPromotionGroupName;
        }

        public string Name { get; private set; }
        public int HierarchyIndex { get; private set; }
        public bool IsCandidate { get; private set; }
        public string LowerNonCandidatePromotionGroupName { get; private set; }
        public string LowerSourcePromotionGroupName { get; private set; }
        public string NextHigherPromotionGroupName { get; private set; }
        public string DirectCheckInPromotionGroupName { get; private set; }

        public bool CheckInable { get { return (DirectCheckInPromotionGroupName == null) && (HierarchyIndex == 0); } }

        public int CompareTo(PvcsPromotionGroupData pvcsPromotionGroupData)
        {
            int compareValue = 0;

            compareValue = this.HierarchyIndex - pvcsPromotionGroupData.HierarchyIndex;
            if (compareValue == 0)
            {
                if (String.Compare(pvcsPromotionGroupData.Name, Name) > 0)
                {
                    compareValue = -1;
                }
                else
                {
                    compareValue = 1;
                }
            }

            return compareValue;
        }
    } // PvcsPromotionGroupData
}
