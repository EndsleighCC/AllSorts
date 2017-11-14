using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDateTime
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dateTimeNow = DateTime.Now;
            Console.WriteLine( "{0}" , dateTimeNow);
            Console.WriteLine("{0} {1}", dateTimeNow.ToString("D"), dateTimeNow.ToString("T"));
        }
    }
}
