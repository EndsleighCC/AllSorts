using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestDateTime
{
    class Program
    {

        static void ShowMonthsBetween( DateTime datetimeBegin, DateTime datetimeEnd )
        {
            Console.WriteLine("Months between {0} and {1} is {2}", datetimeBegin.ToString(), datetimeEnd.ToString(), MotorUtility.MonthCountBetween(datetimeBegin, datetimeEnd));
        }

        static void ShowYearCountBetween( DateTime datetimeBegin, DateTime datetimeEnd )
        {
            Console.WriteLine("Years between {0} and {1} is {2}", datetimeBegin.ToString(), datetimeEnd.ToString(), MotorUtility.YearCountBetween(datetimeBegin, datetimeEnd));
        }

        static void ShowWeekCountBetween(DateTime datetimeBegin, DateTime datetimeEnd)
        {
            Console.WriteLine("Weeks between {0} and {1} is {2}", datetimeBegin.ToString(), datetimeEnd.ToString(), MotorUtility.WeekCountBetween(datetimeBegin, datetimeEnd));
        }

        static void ShowDurationsBetween(DateTime datetimeBegin, DateTime datetimeEnd)
        {
            MotorUtility.LargeTimeSpan largeTimeSpan = new MotorUtility.LargeTimeSpan(datetimeBegin,datetimeEnd);
            Console.WriteLine("Years between {0} and {1} is {2}", datetimeBegin.ToString(), datetimeEnd.ToString(), largeTimeSpan.YearCount);
            Console.WriteLine("Months between {0} and {1} is {2}", datetimeBegin.ToString(), datetimeEnd.ToString(), largeTimeSpan.MonthCount);
            Console.WriteLine("Weeks between {0} and {1} is {2}", datetimeBegin.ToString(), datetimeEnd.ToString(), largeTimeSpan.WeekCount);
        }

        static Mutex protectMutex = new Mutex();

        static void Main(string[] args)
        {
            DateTime datetime = new DateTime();

            datetime = DateTime.Now;

            protectMutex.WaitOne();
            try
            {
                Console.WriteLine("o format = \"{0}\"" , DateTime.Now.ToString("o"));
                Console.WriteLine("HH:mm:ss.fff format =\"{0}\"",DateTime.Now.ToString("HH:mm:ss.fff"));
            }
            finally
            {
                protectMutex.ReleaseMutex();
            }

            ShowMonthsBetween(new DateTime(2012, 1, 1), new DateTime(2012, 1, 1));
            ShowMonthsBetween(new DateTime(2012, 1, 1), new DateTime(2012, 2, 1));
            ShowMonthsBetween(new DateTime(2011, 1, 1), new DateTime(2012, 1, 1));
            ShowMonthsBetween(new DateTime(2011, 12, 31), new DateTime(2012, 1, 1));
            ShowMonthsBetween(new DateTime(2011, 1, 1), new DateTime(2011, 6, 30));
            ShowMonthsBetween(new DateTime(2011, 1, 1), new DateTime(2011, 7, 1));
            ShowMonthsBetween(new DateTime(2011, 1, 1), new DateTime(2012, 4, 16));

            ShowYearCountBetween( new DateTime(1960,01,03),DateTime.Now);
            ShowWeekCountBetween(new DateTime(2013, 04, 01), new DateTime(2013,05,06));

            Console.WriteLine();
            ShowDurationsBetween(new DateTime(2013, 04, 01), new DateTime(2013, 05, 05));
            Console.WriteLine();
            ShowDurationsBetween(new DateTime(2013, 04, 01), new DateTime(2013, 05, 06));
            Console.WriteLine();
            ShowDurationsBetween(new DateTime(2012, 04, 01), new DateTime(2013, 05, 06));
            Console.WriteLine();
            ShowDurationsBetween(new DateTime(2011, 04, 01), new DateTime(2013, 05, 06));

        }
    }
}
