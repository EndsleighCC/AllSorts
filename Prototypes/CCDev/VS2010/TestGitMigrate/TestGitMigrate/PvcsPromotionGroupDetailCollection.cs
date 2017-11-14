using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TestGitMigrate
{

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
            int hierarchyIndex = -1;
            for (int index = 0; (hierarchyIndex == -1) && (index < this.Count); ++index)
            {
                if (String.Compare(this[index].PromotionGroup, promotionGroup, StringComparison.CurrentCultureIgnoreCase) == 0)
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
}
