using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGitMigrate
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
}
