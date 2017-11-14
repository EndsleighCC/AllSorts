using System;
using System.Collections.Generic;
using System.Runtime.Serialization ;

namespace TestADGroup
{
    public class Collections
    {
        [Serializable]
        public class CaseIgnoringSortedSetType : SortedSet<string>
        {
            // Construct empty
            public CaseIgnoringSortedSetType()
                : base(StringComparer.CurrentCultureIgnoreCase)
            {
            }

            // Construct and copy from another set
            public CaseIgnoringSortedSetType(CaseIgnoringSortedSetType existingSet)
                : base(existingSet, StringComparer.CurrentCultureIgnoreCase)
            {
            }
            protected CaseIgnoringSortedSetType(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
            }
        }

        [Serializable]
        public class StringStringIgnoreCaseSortedDictionaryType : SortedDictionary<string, string>
        {
            public StringStringIgnoreCaseSortedDictionaryType()
                : base(StringComparer.CurrentCultureIgnoreCase)
            {
            }
        }
    }

} // namespace Endsleigh.Legacy.TMS.Types
