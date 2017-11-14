using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PvcsChangeControl
{
    public class PvcsArchiveDetailCollectionType : SortedDictionary< string /* Archive Name */ , PvcsArchiveDetail>
    {
        public PvcsArchiveDetailCollectionType() : base(StringComparer.CurrentCultureIgnoreCase)
        {
        }
    }
}
