using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLookup
{
    class Program
    {
        private static int HigherListValue(double listStart, double listIncrement, int value)
        {
            double listHigherPositionDouble = 0.0;

            if (value < listStart)
                // Give the lowest value in the list
                listHigherPositionDouble = listStart;
            else
            {
                // Value is in the list
                double listPositionDouble = (value - listStart) / listIncrement;

                // Calculate the index of the value that is immediately lower in the list
                int listLowerPositionInt = System.Convert.ToInt32(Math.Truncate(listPositionDouble));
                // Calculate the actual list value that is immediately lower
                int listLowerValueInt = System.Convert.ToInt32(listLowerPositionInt * listIncrement + listStart);
                if (listLowerValueInt == value)
                {
                    // The value supplied matches a value in the list
                    listHigherPositionDouble = listLowerValueInt;
                }
                else
                {
                    // The value supplied does not match a value in the list
                    // so choose the next higher list value
                    listHigherPositionDouble = (listLowerPositionInt + 1) * listIncrement + listStart;
                }

            } // Value is in the list

            return System.Convert.ToInt32(listHigherPositionDouble);

        } // HigherListValue

        static void Main(string[] args)
        {
            double listStart = 1000 ;
            double listIncrement = 100;
            string input = null;
            Console.Write("Enter a value to fit into a list starting at {0} with increments of {1} ",listStart,listIncrement);
            while (!String.IsNullOrEmpty(input=Console.ReadLine()))
            {
                int value;
                if (Int32.TryParse(input, out value))
                {
                    Console.WriteLine("Higher List Value of {0} = {1}", value, HigherListValue(listStart, listIncrement, value));
                }
                Console.Write("Enter a value to fit into a list starting at {0} with increments of {1} ", listStart, listIncrement);
            } // while
        }
    }
}
