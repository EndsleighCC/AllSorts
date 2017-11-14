using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PvcsChangeControl
{
    public class PvcsPromotionGroupDetail
    {
        public PvcsPromotionGroupDetail(string promotionGroupName, int hierarchyIndex)
        {
            PromotionGroupName = promotionGroupName;
            HierarchyIndex = hierarchyIndex;
            PromotionGroupShareName = null;
            GitBranchName = null;

        }
        public PvcsPromotionGroupDetail(string promotionGroupName, int hierarchyIndex, string promotionGroupServerName, string promotionGroupShareName, string gitBranchName)
        {
            PromotionGroupName = promotionGroupName;
            HierarchyIndex = hierarchyIndex;
            PromotionGroupServerName = promotionGroupServerName;
            PromotionGroupShareName = promotionGroupShareName;
            GitBranchName = gitBranchName;
        }

        public string PromotionGroupName { get; private set; }
        public int HierarchyIndex { get; private set; }
        public string PromotionGroupServerName { get; private set; }
        public string PromotionGroupShareName { get; private set; }
        public string PromotionGroupNetworkShareName
        {
            get
            {
                return "\\\\" + PromotionGroupServerName + "\\" + PromotionGroupShareName;
            }
        }
        public string GitBranchName { get; private set; }
    }
}
