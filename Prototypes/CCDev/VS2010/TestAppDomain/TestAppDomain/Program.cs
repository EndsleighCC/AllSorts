using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestAppDomain
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Executing in AppDomain \"{0}\" at path \"{1}\"",
                AppDomain.CurrentDomain.FriendlyName,
                AppDomain.CurrentDomain.BaseDirectory); 

       }
    }
}
