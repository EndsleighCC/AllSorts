using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TmsSectorDeduplicate
{
    [Serializable]
    public class SectorNotPresentException : ApplicationException
    {
        public SectorNotPresentException(string errorMessage)
            : base(errorMessage)
        {
        }
    }

    [Serializable]
    public class DuplicateSectorNumberException : ApplicationException
    {
        public DuplicateSectorNumberException(string errorMessage)
            : base(errorMessage)
        {
        }
    }

    [Serializable]
    public class SectorContainerWriteException : ApplicationException
    {
        public SectorContainerWriteException(string errorMessage, Exception eek)
            : base(errorMessage, eek)
        {
        }
    }

}
