using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGitMigrate
{
    public class PvcsArchiveDetailCollectionType : SortedDictionary<string,PvcsArchiveDetail>
    {
        public PvcsArchiveDetailCollectionType() : base(StringComparer.CurrentCultureIgnoreCase)
        {
        }
    }
}
