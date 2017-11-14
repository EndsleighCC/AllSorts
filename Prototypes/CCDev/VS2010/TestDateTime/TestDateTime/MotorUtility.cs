using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestDateTime
{
    public class MotorUtility
    {

        /// <summary>
        /// Class to calculate Time Spans that are larger than days.
        /// The .NET class TimeSpan does not cope with anything larger than days.
        /// </summary>
        public class LargeTimeSpan
        {
            public LargeTimeSpan(DateTime begin, DateTime end)
            {
                Begin = begin;
                End = end;

                MonthCount = MotorUtility.MonthCountBetween(begin, end);
                YearCount = MonthCount / 12 /* months in year */;
                WeekCount = MotorUtility.WeekCountBetween(begin, end);
            }

            public DateTime Begin { get; private set; }
            public DateTime End { get; private set; }

            public int YearCount { get; private set; }
            public int MonthCount { get; private set; }
            public int WeekCount { get; private set; }

        }

        /// <summary>
        /// Determines the whole number of weeks between the supplied dates
        /// </summary>
        /// <param name="beginDateTime">The beginning of the time period</param>
        /// <param name="endDateTime">The end of the time period</param>
        /// <returns>The whole number of weeks contained within the time period</returns>
        public static int WeekCountBetween(DateTime beginDateTime, DateTime endDateTime)
        {
            int weekCount = 0;

            // Ensure that the date subtraction will always generate a positive result
            if (endDateTime < beginDateTime)
            {
                DateTime buffer = beginDateTime;
                beginDateTime = endDateTime;
                endDateTime = buffer;
            }

            TimeSpan timeSpan = endDateTime - beginDateTime;

            // There are always 7 days in a whole week
            weekCount = timeSpan.Days / 7;

            return weekCount;
        }

        /// <summary>
        /// Determines the whole number of months between the supplied dates
        /// </summary>
        /// <param name="beginDateTime">The beginning of the time period</param>
        /// <param name="endDateTime">The end of the time period</param>
        /// <returns>The whole number of months contained within the time period</returns>
        public static int MonthCountBetween(DateTime beginDateTime, DateTime endDateTime)
        {
            int monthCount = 0;

            // Ensure that the date subtraction will always generate a positive result
            if (endDateTime < beginDateTime)
            {
                DateTime buffer = beginDateTime;
                beginDateTime = endDateTime;
                endDateTime = buffer;
            }

            DateTime dateTimeIntermediate = beginDateTime;

            // Add the whole number of days in the month taking into account leap years if necessary
            while ((dateTimeIntermediate = dateTimeIntermediate.AddMonths(1)) <= endDateTime)
            {
                monthCount += 1;
            }

            return monthCount;
        }

        /// <summary>
        /// Determines the whole number of years between the supplied dates
        /// </summary>
        /// <param name="beginDateTime">The beginning of the time period</param>
        /// <param name="endDateTime">The end of the time period</param>
        /// <returns>The whole number of years contained within the time period</returns>
        public static int YearCountBetween(DateTime beginDateTime, DateTime endDateTime)
        {
            int yearCount = 0;

            // Ensure that the date subtraction will always generate a positive result
            if (endDateTime < beginDateTime)
            {
                DateTime buffer = beginDateTime;
                beginDateTime = endDateTime;
                endDateTime = buffer;
            }

            DateTime dateTimeIntermediate = beginDateTime;

            // Add the whole number of days in a year taking into account leap years when necessary
            while ((dateTimeIntermediate = dateTimeIntermediate.AddMonths(12)) <= endDateTime)
            {
                yearCount += 1;
            }

            return yearCount;
        }
    }
}
