using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestAnything
{
    static class StaticClass
    {
        static StaticClass()
        {
            Console.WriteLine("StaticClass default Constructor called");
            internalInt = 4;
        }

        static public int Get()
        {
            return internalInt;
        }

        private static int internalInt;
    }

    class Program
    {
        static void Main(string[] args)
        {
            string pathCombined = Path.Combine("d:\\", "SysST00" , "Endsleigh\\Utility\\BuildLogAnalyser\\BuildLogAnalyser\\bin\\Debug\\BuildLogAnalyser.exe");

            // string pathCombined = Path.Combine("c:\\rubbish\\", "d:\\crap\\crap.txt", "d:\\crapper\\crapper.txt");

            // string pathCombined = Path.Combine("c:\\", "ST00" + "\\");

            // string pathCombined = Path.Combine("/fred", "crap");

            try
            {
                // int yearsAtProperty = System.Convert.ToInt32("9876543210");
                // int yearsAtProperty = System.Convert.ToInt32("x");
                int yearsAtProperty = System.Convert.ToInt32("");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Empty string threw \"{0}\"",ex.ToString());
            }

            Console.WriteLine(pathCombined);

            Console.WriteLine("StaticClass.Get returned {0}", StaticClass.Get());

            int x = 3;
            int y = 4;
            int z = x/y;
        }
    }
}
