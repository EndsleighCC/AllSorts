using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestDecimal
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal decValue = 3.1234567890123456789012345678m;

            double dblValue1 = System.Convert.ToDouble(decValue);
            double dblValue2 = System.Convert.ToDouble(decValue);
            double dblValue3 = System.Convert.ToDouble(decValue);

            double dblValue45 = 4.5;
            int intDown = System.Convert.ToInt32(dblValue45);

            Console.WriteLine("{0} -> {1}", dblValue45, intDown);

            double dblValue55 = 5.5;
            int intUp = System.Convert.ToInt32(dblValue55);

            Console.WriteLine("{0} -> {1}", dblValue55, intUp);

            decimal decValue2 = 3.1234567890123456789012345678m;
            double dblValue4 = System.Convert.ToDouble(decValue2);

            Console.WriteLine("{0} -> {1}", decValue2, dblValue4);

            decimal decValue3 = 3.1234567890123466789012345678m;
            double dblValue5 = System.Convert.ToDouble(decValue3);

            Console.WriteLine("{0} -> {1}", decValue3, dblValue5);
        }
    }
}
