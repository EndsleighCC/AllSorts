using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PvcsChangeControl
{
    class PvcsArchiveNameSetIgnoreCase : SortedSet<string>
    {
        public PvcsArchiveNameSetIgnoreCase() : base(StringComparer.CurrentCultureIgnoreCase)
        {
        }
    }
}
