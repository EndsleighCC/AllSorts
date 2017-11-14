using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitXmlDocument
{
    static class ErrorDefinitions
    {
        public const int Success = 0;
        public const int InsufficientCommandLineParameters = 1;
        public const int FileNotFound = 2;
        public const int UnexpectedFailure = 3;
        public const int FailureProcessingXML = 4;
        public const int NoQuotesDetected = 5;
    }
}
