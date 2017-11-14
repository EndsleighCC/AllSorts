using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TestAnything
{
    class Program
    {
        private class Pete
        {
            public Pete(string surname )
            {
                _surname = surname;
            }

            public string Surname { get { return _surname; } private set { _surname = value; } }
            private string _surname;
        }

        static void Main(string[] args)
        {
            Pete pete = new Pete("Bishop");
            Console.WriteLine("Pete {0}",pete.Surname);

            System.Boolean booleanValue = System.Boolean.TrueString ;
        }
    }
}
