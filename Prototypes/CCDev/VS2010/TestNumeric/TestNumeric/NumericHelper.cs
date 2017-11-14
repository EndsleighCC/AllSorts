using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestNumeric
{

    public class Enumerations
    {
        /// <summary>
        /// Binary Operations that can be performed on cell data
        /// </summary>
        public enum BinaryOperator
        {
            Set,
            Add,
            Subtract,
            Multiply,
            Divide
        }

        /// <summary>
        /// The Rounding Strategy to be used to get to an integral value
        /// Natural : The nearest "number line" value. For example:
        ///             1.4 -> 1.0
        ///             1.5 -> 2.0
        ///             1.6 -> 2.0
        ///             -1.4 -> -1.0
        ///             -1.5 -> -1.0
        ///             -1.6 -> -2.0
        /// Down : The lowest "number line" integral value contained within the value. For example:
        ///             1.4 -> 1.0
        ///             1.5 -> 1.0
        ///             1.6 -> 1.0
        ///             -1.4 -> -2.0
        ///             -1.5 -> -2.0
        ///             -1.6 -> -2.0
        /// Up : The next highest "number line" integral value above the value. For example:
        ///             1.4 -> 2.0
        ///             1.5 -> 2.0
        ///             1.6 -> 2.0
        ///             -1.4 -> -1.0
        ///             -1.5 -> -1.0
        ///             -1.6 -> -1.0
        /// </summary>
        public enum RoundingStrategy
        {
            None ,
            Natural,
            Down,
            Up
        }

    }

    [Serializable]
    public class NumericHelper
    {

        /// <summary>
        /// Calculates the integer power of the supplied number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static double PowerOf(double number, int power)
        {
            double result = 1.0 ; // Any number (except possibly zero) raised to a power of zero is 1
            if (power != 0)
            {
                int powerAbsolute = Math.Abs( power ) ;
                for (int thisPower = 0; thisPower < powerAbsolute; ++thisPower)
                    result *= number;
                if (power < 0)
                    result = 1.0 / result;
            }
            return result;
        }

        /// <summary>
        /// Perform the required Binary Operation on the supplied value
        /// </summary>
        /// <param name="doubleValue">The value on which to perform the binary operation</param>
        /// <param name="binaryOperator">The Binary Operation to be performed</param>
        /// <param name="rightOperand">The right hand operand</param>
        /// <param name="roundingStrategy">The rounding strategy</param>
        /// <param name="roundAbsolute">Whether or not to round as for the abolute value</param>
        /// <param name="roundAtPowerOf10">The power of 10 at which to perform rounding</param>
        static public double BinaryOperate(double doubleValue,
                                           Enumerations.BinaryOperator binaryOperator,
                                           double rightOperand,
                                           Enumerations.RoundingStrategy roundingStrategy,
                                           bool roundAbsolute,
                                           int roundAtPowerOf10)
        {
            double doubleResult = doubleValue;

            // To conserve precision, where possible, try not to perform any "unity" operation
            switch (binaryOperator)
            {
                case Enumerations.BinaryOperator.Set:
                    doubleResult = rightOperand;
                    break;
                case Enumerations.BinaryOperator.Add:
                    // Do nothing if the right operand is indistinguishable from zero
                    if (!rightOperand.Equals(0.0))
                        doubleResult += rightOperand;
                    break;
                case Enumerations.BinaryOperator.Subtract:
                    // Do nothing if the right operand is indistinguishable from zero
                    if (!rightOperand.Equals(0.0))
                        doubleResult -= rightOperand;
                    break;
                case Enumerations.BinaryOperator.Multiply:
                    // Do nothing if the right operand is indistinguishable from unity
                    if (!rightOperand.Equals(1.0))
                        doubleResult *= rightOperand;
                    break;
                case Enumerations.BinaryOperator.Divide:
                    if (Math.Abs(rightOperand) < DoubleValueEpsilon)
                        MessageBox.Show("Attempt to divide by zero", "Test Numeric");
                        // throw new AttemptToDivideByZeroException(String.Format("NumericHelper.BinaryOperate : An attempt was made to divide {0} by zero", doubleResult));
                    else
                        // Do nothing if the right operand is indistinguishable from unity
                        if (!rightOperand.Equals(1.0))
                            doubleResult /= rightOperand;
                    break;
                default:
                    MessageBox.Show(String.Format( "Binary Operator Enumeration Member {0} not defined",binaryOperator), "Test Numeric"); break ;
                    // throw new BinaryOperatorUnrecognisedException(String.Format("NumericHelper.BinaryOperate : The Binary Operator Enumeration Member of value {0} was not recognised", binaryOperator));
            }

            if (roundingStrategy != Enumerations.RoundingStrategy.None)
            {
                // Perform rounding on the result

                double roundingScaleFactor = 1.0 / PowerOf(10.0, roundAtPowerOf10);

                bool negate = false;

                if ((roundAbsolute) && (doubleResult < 0.0))
                {
                    negate = true;
                    doubleResult = Math.Abs(doubleResult);
                }

                // Scale the number for rounding
                doubleResult *= roundingScaleFactor;

                switch (roundingStrategy)
                {
                    case Enumerations.RoundingStrategy.Down:
                        // For negative numbers the .NET Runtime Truncate and Ceiling functions return the same values
                        // Math.Truncate( -1.5 ) -> -1.0 which is not what is required here
                        // Math.Ceiling( -1.5 ) -> -1.0
                        // So use Math.Floor for both negative and non-negative numbers
                        doubleResult = Math.Floor(doubleResult);
                        break;
                    case Enumerations.RoundingStrategy.Natural:
                        // Produce mid-point rounding that always goes to the nearest scaled integer-like double value
                        // Note that this has the effect of "round absolute" but that appears to be what most people expect!
                        doubleResult = Math.Round(doubleResult);
                        break;
                    case Enumerations.RoundingStrategy.Up:
                        // Use Math.Ceiling for both negative and non-negative numbers
                        doubleResult = Math.Ceiling(doubleResult);
                        break;
                    default:
                        MessageBox.Show(String.Format("Rounding Strategy {0} not defined", roundingStrategy), "Test Numeric");
                        // throw new RoundStrategyUnrecognisedException(String.Format("NumericHelper.BinaryOperate : The Roung Strategy Enumeration Member {0} was not recognised", roundingStrategy));
                        break;
                }

                // Get the number back
                doubleResult /= roundingScaleFactor;

                if (negate)
                    doubleResult = -doubleResult;

            } // Perform rounding on the result

            return doubleResult;
        }

        public const double DoubleValueEpsilon = /* 1e-6 ? */ System.Double.Epsilon;
    }
}
