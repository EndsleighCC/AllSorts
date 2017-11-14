using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TmsSectorDeduplicate
{
    public class Chassis
    {
        public Chassis()
        {
            FullReadPath = null;
            FullWritePath = null;
            RiskName = null;
        }

        public Chassis(string fullReadPath , string fullWritePath, string riskName)
        {
            FullReadPath = fullReadPath;
            FullWritePath = fullWritePath;
            RiskName = riskName;
        }

        public string FullReadPath;
        public string FullWritePath;
        public string RiskName;

        public StreamWriter DiagnosticStream(string diagnosticItem, string filename, string backFilename)
        {
            StreamWriter diagnosticsStreamWriter = null;

            string assemblyLocationFile = Assembly.GetAssembly(typeof(Chassis)).Location;
            // Get the path to the assembly
            string assemblyLocationPath = assemblyLocationFile.Substring(0, assemblyLocationFile.LastIndexOf(@"\"));

            try
            {
                diagnosticsStreamWriter = FileHelper.GetStreamWriter(assemblyLocationPath, filename,
                                                                        backFilename);
            }
            catch (Exception)
            {
                // If something dreadful happens (concurrent open for write?) ensure that the diagnostic stream is null
                Console.WriteLine(String.Format("DiagnosticStream : Unable to open Diagnostic Stream Writer \"{0}\"",
                                                Path.Combine(assemblyLocationPath, filename)));
                diagnosticsStreamWriter = null;
            }

            return diagnosticsStreamWriter;

        }

        public static class Logger
        {
            public static void ErrorException( string message , Exception eek )
            {

            }
        }
    }
}
