using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareSourceVolumes
{
    public class PvcsArchiveRevisionDetail : IComparable<PvcsArchiveRevisionDetail>
    {
        public PvcsArchiveRevisionDetail(string archiveName, string promotionGroupName)
        {
            ArchiveName = archiveName;
            PromotionGroupName = promotionGroupName;
        }

        public string ArchiveName { get; private set; }
        public string PromotionGroupName { get; private set; }

        public int CompareTo( PvcsArchiveRevisionDetail pvcsArchiveRevisionDetail)
        {
            int compare = 0;

            compare = String.Compare(this.ArchiveName, pvcsArchiveRevisionDetail.ArchiveName);
            if ( compare == 0 )
            {
                compare = String.Compare(this.PromotionGroupName, pvcsArchiveRevisionDetail.PromotionGroupName);
            }
            return compare;
        }
    }
}
