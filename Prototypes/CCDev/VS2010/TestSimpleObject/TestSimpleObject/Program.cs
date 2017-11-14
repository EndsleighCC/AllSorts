using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestSimpleObject
{
    class Program
    {

        class SimpleClass
        {
            public SimpleClass(int value)
            {
                _Value = value;
            }

            public int ComparesWith( SimpleClass other )
            {
                int compare = 0;

                // Reference private member of the other object
                if (_Value == other._Value)
                    compare = 0;
                else if (_Value < other._Value )
                    compare = -1;
                else
                    compare = 1;

                return compare;
            }

            private int _Value;

        } // class SimpleClass

        static void Main(string[] args)
        {
            SimpleClass simpleClass1 = new SimpleClass(1);
            SimpleClass simpleClass2 = new SimpleClass(2);

            Console.Write("First ");
            if (simpleClass1.ComparesWith(simpleClass2) == 0)
                Console.Write("is the same as");
            else if (simpleClass1.ComparesWith(simpleClass2) < 0)
                Console.Write("is less than");
            else
                Console.Write("is greater than");
            Console.WriteLine( " second" ) ;

        }
    }
}
