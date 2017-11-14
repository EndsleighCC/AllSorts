using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running as {0}.{1}",
                MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
        }
    }
}
