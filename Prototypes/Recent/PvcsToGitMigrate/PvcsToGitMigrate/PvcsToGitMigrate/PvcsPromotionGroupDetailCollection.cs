using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GitChangeControl;

namespace PvcsChangeControl
{

    public class PvcsPromotionGroupDetailCollection : Collection<PvcsPromotionGroupDetail>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PvcsPromotionGroupDetailCollection() : base()
        {
            const int Production_HierarchyIndex = 0;
            const int Pre_Production_HierarchyIndex = 1;
            const int User_Test_HierarchyIndex = 2;
            const int Pre_User_Test_HierarchyIndex = 3;
            const int System_Test_HierarchyIndex = 4;
            const int Pre_System_Test_HierarchyIndex = 5;
            const int Development_HierarchyIndex = 6;
            const int Branch_System_Test_HierarchyIndex = Development_HierarchyIndex;
            const int Branch_Pre_System_Test_HierarchyIndex = Branch_System_Test_HierarchyIndex + 1;
            const int Branch_Development_HierarchyIndex = Branch_Pre_System_Test_HierarchyIndex + 1;

            this.Add(new PvcsPromotionGroupDetail(ProductionPromotionGroupName, Production_HierarchyIndex, _productionServerName, "SysPROD", GitOperation.MasterBranchName));

            this.Add(new PvcsPromotionGroupDetail(PreProductionPromotionGroupName, Pre_Production_HierarchyIndex, _productionServerName, "SysPR00", "PreProduction"));
            this.Add(new PvcsPromotionGroupDetail(UserTestPromotionGroupName, User_Test_HierarchyIndex, _productionServerName, "SysUT00", "AcceptanceTest"));
            this.Add(new PvcsPromotionGroupDetail(PreUserTestPromotionGroupName, Pre_User_Test_HierarchyIndex)); // No corresponding Git Branch
            this.Add(new PvcsPromotionGroupDetail(PreUserTestStagingPromotionGroupName, Pre_User_Test_HierarchyIndex)); // No corresponding Git Branch

            this.Add(new PvcsPromotionGroupDetail(SystemTestPromotionGroupName, System_Test_HierarchyIndex, _developmentServerName, "SysST00", "IntegrationTest"));
            this.Add(new PvcsPromotionGroupDetail(PreSystemTestPromotionGroupName, Pre_System_Test_HierarchyIndex)); // No corresponding Git Branch
            this.Add(new PvcsPromotionGroupDetail(PreSystemTestStagingPromotionGroupName, Pre_System_Test_HierarchyIndex)); // No corresponding Git Branch
            this.Add(new PvcsPromotionGroupDetail(DevelopmentPromotionGroupName, Development_HierarchyIndex)); // No corresponding Git Branch

            this.Add(new PvcsPromotionGroupDetail(SystemTest1PromotionGroupName, Branch_System_Test_HierarchyIndex, _developmentServerName, "SysST01", "IntegrationTest1"));
            this.Add(new PvcsPromotionGroupDetail(PreSystemTest1PromotionGroupName, Branch_Pre_System_Test_HierarchyIndex)); // No corresponding Git Branch
            this.Add(new PvcsPromotionGroupDetail(Development1PromotionGroupName, Branch_Development_HierarchyIndex)); // No corresponding Git Branch

            this.Add(new PvcsPromotionGroupDetail(SystemTest2PromotionGroupName, Branch_System_Test_HierarchyIndex, _developmentServerName, "SysST02", "IntegrationTest2"));
            this.Add(new PvcsPromotionGroupDetail(PreSystemTest2PromotionGroupName, Branch_Pre_System_Test_HierarchyIndex)); // No corresponding Git Branch
            this.Add(new PvcsPromotionGroupDetail(Development2PromotionGroupName, Branch_Development_HierarchyIndex)); // No corresponding Git Branch

            this.Add(new PvcsPromotionGroupDetail(SystemTest3PromotionGroupName, Branch_System_Test_HierarchyIndex, _developmentServerName, "SysST03", "IntegrationTest3"));
            this.Add(new PvcsPromotionGroupDetail(PreSystemTest3PromotionGroupName, Branch_Pre_System_Test_HierarchyIndex)); // No corresponding Git Branch
            this.Add(new PvcsPromotionGroupDetail(Development3PromotionGroupName, Branch_Development_HierarchyIndex)); // No corresponding Git Branch

            this.Add(new PvcsPromotionGroupDetail(SystemTest4PromotionGroupName, Branch_System_Test_HierarchyIndex, _developmentServerName, "SysST04", "IntegrationTest4"));
            this.Add(new PvcsPromotionGroupDetail(PreSystemTest4PromotionGroupName, Branch_Pre_System_Test_HierarchyIndex)); // No corresponding Git Branch
            this.Add(new PvcsPromotionGroupDetail(Development4PromotionGroupName, Branch_Development_HierarchyIndex)); // No corresponding Git Branch

            this.Add(new PvcsPromotionGroupDetail(SystemTest5PromotionGroupName, Branch_System_Test_HierarchyIndex, _developmentServerName, "SysST05", "IntegrationTest5"));
            this.Add(new PvcsPromotionGroupDetail(PreSystemTest5PromotionGroupName, Branch_Pre_System_Test_HierarchyIndex)); // No corresponding Git Branch
            this.Add(new PvcsPromotionGroupDetail(Development5PromotionGroupName, Branch_Development_HierarchyIndex)); // No corresponding Git Branch

            this.Add(new PvcsPromotionGroupDetail(SystemTest6PromotionGroupName, Branch_System_Test_HierarchyIndex, _developmentServerName, "SysST06", "IntegrationTest6"));
            this.Add(new PvcsPromotionGroupDetail(PreSystemTest6PromotionGroupName, Branch_Pre_System_Test_HierarchyIndex)); // No corresponding Git Branch
            this.Add(new PvcsPromotionGroupDetail(Development6PromotionGroupName, Branch_Development_HierarchyIndex)); // No corresponding Git Branch

        } // Constructor

        private const string _productionServerName = "ADEBS02";
        private const string _developmentServerName = "ADEBS04";

        public const string ProductionPromotionGroupName = "Production";

        public const string PreProductionPromotionGroupName = "Pre_Production";
        public const string UserTestPromotionGroupName = "User_Test";
        public const string PreUserTestPromotionGroupName = "Pre_User_Test";
        public const string PreUserTestStagingPromotionGroupName = "Pre_User_Test_Staging";

        public const string SystemTestPromotionGroupName = "System_Test";
        public const string PreSystemTestPromotionGroupName = "Pre_System_Test";
        public const string PreSystemTestStagingPromotionGroupName = "Pre_System_Test_Staging";
        public const string DevelopmentPromotionGroupName = "Development";

        public const string SystemTest1PromotionGroupName = "System_Test1";
        public const string PreSystemTest1PromotionGroupName = "Pre_System_Test1";
        public const string Development1PromotionGroupName = "Development1";

        public const string SystemTest2PromotionGroupName = "System_Test2";
        public const string PreSystemTest2PromotionGroupName = "Pre_System_Test2";
        public const string Development2PromotionGroupName = "Development2";

        public const string SystemTest3PromotionGroupName = "System_Test3";
        public const string PreSystemTest3PromotionGroupName = "Pre_System_Test3";
        public const string Development3PromotionGroupName = "Development3";

        public const string SystemTest4PromotionGroupName = "System_Test4";
        public const string PreSystemTest4PromotionGroupName = "Pre_System_Test4";
        public const string Development4PromotionGroupName = "Development4";

        public const string SystemTest5PromotionGroupName = "System_Test5";
        public const string PreSystemTest5PromotionGroupName = "Pre_System_Test5";
        public const string Development5PromotionGroupName = "Development5";

        public const string SystemTest6PromotionGroupName = "System_Test6";
        public const string PreSystemTest6PromotionGroupName = "Pre_System_Test6";
        public const string Development6PromotionGroupName = "Development6";

        // Base index for Promotion Groups that are *not* Production
        public const int DevelopmentHierarchyBaseIndex = 1;

        public const int FirstLowerSystemTestNumber = 1; // SystemTest1PromotionGroupName
        public const int LastLowerSystemTestNumber = 6; // SystemTest6PromotionGroupName

        private const int _unknownHierarchyIndex = -1;

        /// <summary>
        /// Indicates whether the supplied name is a valid Promotion Group Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true if the supplied name is a valid Promotion Group Name, else, false</returns>
        public bool PromotionGroupNameIsValid(string name)
        {
            return HierarchyIndex(name) != _unknownHierarchyIndex;
        }

        /// <summary>
        /// Returns the Promotion Hierarchy Index of any Development Promotion Group including Production
        /// </summary>
        /// <param name="promotionGroup"></param>
        /// <returns></returns>
        public int HierarchyIndex(string promotionGroup)
        {
            int hierarchyIndex = _unknownHierarchyIndex;
            for (int index = 0; (hierarchyIndex == _unknownHierarchyIndex) && (index < this.Count); ++index)
            {
                if (String.Compare(this[index].PromotionGroupName, promotionGroup, StringComparison.CurrentCultureIgnoreCase) == 0)
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
            int hierarchyIndex = _unknownHierarchyIndex;
            for (int index = 1; (hierarchyIndex == _unknownHierarchyIndex) && (index < this.Count); ++index)
            {
                if (String.Compare(this[index].PromotionGroupName, promotionGroup, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    hierarchyIndex = this[index].HierarchyIndex;
                }
            }
            return hierarchyIndex;
        }

        /// <summary>
        /// Returns the name of the PVCS Promotion Group corresponding to the supplied PVCS Promotion Group Hierarchy Index
        /// </summary>
        /// <param name="hierarchyIndex"></param>
        /// <returns></returns>
        public string PromotionGroupName(int hierarchyIndex)
        {
            string promotionGroupName = null;

            for (int index = 0; (promotionGroupName == null) && (index < this.Count); ++index)
            {
                if (hierarchyIndex == this[index].HierarchyIndex)
                {
                    promotionGroupName = this[index].PromotionGroupName;
                }
            }

            return promotionGroupName;
        }

        public string PromotionGroupServerName(string promotionGroupName)
        {
            string promotionGroupServerName = null;

            for (int index = 0; (promotionGroupServerName == null) && (index < this.Count); ++index)
            {
                if (String.Compare(this[index].PromotionGroupName, promotionGroupName, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    promotionGroupServerName = this[index].PromotionGroupShareName;
                }
            }

            return promotionGroupServerName;
        }

        public string PromotionGroupShareName(string promotionGroupName)
        {
            string promotionGroupShareName = null;

            for (int index = 0; (promotionGroupShareName == null) && (index < this.Count); ++index)
            {
                if (String.Compare(this[index].PromotionGroupName, promotionGroupName, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    promotionGroupShareName = this[index].PromotionGroupShareName;
                }
            }

            return promotionGroupShareName;
        }

        public string PromotionGroupNetworkShareName(string promotionGroupName)
        {
            string promotionGroupNetworkShareName = null;

            for (int index = 0; (promotionGroupNetworkShareName == null) && (index < this.Count); ++index)
            {
                if (String.Compare(this[index].PromotionGroupName, promotionGroupName, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    promotionGroupNetworkShareName = this[index].PromotionGroupNetworkShareName;
                }
            }

            return promotionGroupNetworkShareName;
        }

        /// <summary>
        /// Returns the Git Branch Name corresponding to the supplied PVCS Promotion Group Name
        /// </summary>
        /// <param name="promotionGroupName"></param>
        /// <returns></returns>
        public string GitBranchName(string promotionGroupName)
        {
            string gitBranchName = null;

            for (int index = 0; (gitBranchName == null) && (index < this.Count); ++index)
            {
                if (String.Compare(this[index].PromotionGroupName, promotionGroupName, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    gitBranchName = this[index].GitBranchName;
                }
            }

            return gitBranchName;
        }

        /// <summary>
        /// Returns whether the PVCS Promotion Group Hierarchy Index of the first supplied
        /// PVCS Promotion Group name is greater than that of the second
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public bool PromotionGroupIsHigher(string first, string second)
        {
            return HierarchyIndex(first) > HierarchyIndex(second);
        }
    }
}
