using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestCountFileLines
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                using (StreamReader file = new StreamReader(args[0]))
                {
                    String line;
                    int lineCount = 0;
                    while ((line = file.ReadLine()) != null)
                    {
                        lineCount += 1;
                    }
                    Console.WriteLine("Line count is {0}",lineCount);
                }
            }
        }
    }
}
