using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestMonthlyPayments
{
    public static class NumericalOperation
    {
        public const double PencePerPound = 100.0;
        public const double PercentageDivisor = 100.0;

        public static double PennyTruncate(double poundAmount)
        {
            return Math.Truncate(poundAmount * PencePerPound) / PencePerPound;
        }

        public static double PennyRound(double poundAmount)
        {
            // return Math.Truncate(poundAmount * PencePerPound + 0.5*Math.Sign(poundAmount)) / PencePerPound;
            return Math.Round(poundAmount, 2);
        }
    }
}
