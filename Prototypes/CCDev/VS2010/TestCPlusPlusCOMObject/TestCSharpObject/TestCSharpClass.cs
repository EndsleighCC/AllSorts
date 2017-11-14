using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCSharpObject
{
    public class TestCSharpClass
    {
        public TestCSharpClass()
        {
        }

        public void Output(string message)
        {
            Console.WriteLine("TestCSharpClass.Output : {0}", message);
        }

        public void OutputDiagnostics(INETBaseLib.IDiagnostics diagnostics, string message)
        {
            Console.WriteLine("TestCSharpClass.OutputDiagnostics : Calling IDiagnostics.Trace with \"{0}\"", message);
            diagnostics.Trace(message);
            Console.WriteLine("TestCSharpClass.OutputDiagnostics : Back from calling IDiagnostics.Trace with \"{0}\"", message);
        }

        public void ReferenceSchemeResult(INETBaseLib.ISchemeResult schemeResult, string schemeName)
        {
            Console.WriteLine("TestCSharpClass.OutputDiagnostics : Calling ISchemeResult.SchemeName with \"{0}\"", schemeName);
            schemeResult.SchemeName = schemeName;
            Console.WriteLine("TestCSharpClass.OutputDiagnostics : Back from calling ISchemeResult.SchemeName with \"{0}\"", schemeName);
        }
    }
}
