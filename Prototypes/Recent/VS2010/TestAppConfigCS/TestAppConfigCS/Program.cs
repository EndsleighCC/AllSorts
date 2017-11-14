using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace TestAppConfigCS
{
    class Program
    {
        static void Main(string[] args)
        {
            string key = "BuildSetFullFilePath";
            string path = ConfigurationManager.AppSettings[key];
            if (path == null)
            {
                Console.WriteLine("Unable to find \"{0}\"",key);

            }
            else
            {
                Console.WriteLine("\"{0}\" = \"{1}\"", key , path);
            }

        }
    }
}
