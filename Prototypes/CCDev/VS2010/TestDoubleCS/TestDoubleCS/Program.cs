using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestDoubleCS
{
    class Program
    {

        public static class MathHelper
        {
            public static double Max(params double[] values)
            {
                return Enumerable.Max(values);
            }
        }

        static void Main(string[] args)
        {
            double firstDouble = -5.0;
            double secondDouble = -5.0;
            double thirdDouble = -5.0;

            double theMax = Math.Max(firstDouble, Math.Max( secondDouble, thirdDouble));

            Console.WriteLine("Maximum is {0}", theMax);

            double theHelperMax = MathHelper.Max(firstDouble, secondDouble, thirdDouble);

            Console.WriteLine("Maximum is {0}", theHelperMax);

            const double THREE_MONTH_TERM = (365/4);

            Console.WriteLine("THREE_MONTH_TERM = {0}", THREE_MONTH_TERM);
        }
    }
}
