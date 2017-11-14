using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisplayHelper
{
    public static class ConsoleDisplay
    {
        public static string Indent(int indent)
        {
            return new String(' ', indent * 4);
        }

        public static void WritelineWithUnderline(string s)
        {
            Console.WriteLine(s);
            Console.WriteLine(new string('~', s.Length));
        }
    }
}
