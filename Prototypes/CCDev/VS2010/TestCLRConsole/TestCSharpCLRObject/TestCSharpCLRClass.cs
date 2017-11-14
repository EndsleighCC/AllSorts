using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCSharpCLRObject
{
    public class TestCSharpCLRClass
    {
        public TestCSharpCLRClass()
        {
        }

        public string HelloString()
        {
            return "Hello from the C# Method";
        }

        public string HelloStringProperty { get { return "Hello from the C# Property"; } }

        public static string GetStaticString() { return "Static String from non-static Class"  ; }
    }
}
