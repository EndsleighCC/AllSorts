using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyseDifferences
{
    public class PromotionGroupDetails
    {
        public PromotionGroupDetails(string promotionGroupName, string serverName, string promotionGroupCode)
        {
            PromotionGroupName = promotionGroupName;
            ServerName = serverName;
            PromotionGroupSharePath = @"\\" + serverName + @"\Sys" + promotionGroupCode;
            GitRepoSharePath = @"\\" + serverName + @"\g" + promotionGroupCode;
        }

        public string PromotionGroupName { get; private set; }

        public string ServerName { get; private set; }

        public string PromotionGroupSharePath { get; private set; }
        public string GitRepoSharePath { get; private set; }
    }
}
