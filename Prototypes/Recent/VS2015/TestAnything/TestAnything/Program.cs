using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;

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

        static void TestPete()
        {
            Pete pete = new Pete("Bishop");
            Console.WriteLine("Pete {0}", pete.Surname);
        }

        static void TestDirectoryRename()
        {
            string sourceDirectoryName = @"D:\Repos\Personal\corc1\VS2015\TestAnything\TestAnything\Crap";

            bool renamed = false;
            for (int dirIndex = 0 ; ( ! renamed ) && ( dirIndex < 1000 ) ; ++dirIndex)
            {
                string destinationDirectoryName = String.Format("{0}.{1:000}", sourceDirectoryName, dirIndex);
                if ( ! Directory.Exists(destinationDirectoryName))
                {
                    Console.WriteLine("Renaming \"{0}\" to \"{1}\"", sourceDirectoryName, destinationDirectoryName);
                    Directory.Move(sourceDirectoryName, destinationDirectoryName);
                    renamed = true;
                }
            }
        }

        static void TestDecimal()
        {
            decimal decimalValue = 1;

            try
            {
                decimal decimalMax = decimal.MaxValue;

                Console.WriteLine("Maximum Decimal value is {0} with {1} digits", decimalMax,decimal.MaxValue.ToString().Length);

                while (true)
                {
                    decimalValue = decimalValue * 10;
                    Console.WriteLine("Higher decimal value is {0}", decimalValue);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception is {0}", ex.ToString());
            }
        }

        static void TestDateTime()
        {
            Console.WriteLine("DataTime is {0}", DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.fff"));
        }

        static void TestElapsed()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Thread.Sleep(2516);
            TimeSpan totalElapsedTime = stopWatch.Elapsed;
            Console.WriteLine("Total time taken was {0:00}:{1:00}:{2:00}.{3:000}",
                                    totalElapsedTime.Hours,
                                    totalElapsedTime.Minutes,
                                    totalElapsedTime.Seconds,
                                    totalElapsedTime.Milliseconds);

        }

        static void TestExpression()
        {
            double repayment = (((23000 - 21000) * 0.09) + ((23000 - 21000) * 0.06)) / 12;
            Console.WriteLine("Repayment is {0}", repayment);
        }

        static void TestSplit()
        {
            string expression = "@";
            string[] splitExpression = expression.Split(new char[] { '@' } , StringSplitOptions.None);
            Console.WriteLine("Expression \"{0}\" produces {1} split components", expression, splitExpression.Length);

            expression = "fred@bunnies";
            splitExpression = expression.Split(new char[] { '@' }, StringSplitOptions.None);
            Console.WriteLine("Expression \"{0}\" produces {1} split components", expression, splitExpression.Length);

            expression = "@fred@bunnies";
            splitExpression = expression.Split(new char[] { '@' }, StringSplitOptions.None);
            Console.WriteLine("Expression \"{0}\" produces {1} split components", expression, splitExpression.Length);
        }

        static string PrefixSimpleXPath(string prefix, string xPath)
        {
            StringBuilder sbFullyPrefixedXPath = new StringBuilder();

            const char xPathDelimiter = '/';

            if (!xPath.Contains(xPathDelimiter))
            {
                // The supplied XPath does not contain any XPath path delimiters

                if (!String.IsNullOrEmpty(xPath))
                {
                    sbFullyPrefixedXPath.Append(prefix);
                    sbFullyPrefixedXPath.Append(":");
                    sbFullyPrefixedXPath.Append(xPath);
                }

            } // The supplied XPath does not contain any XPath path delimiters
            else
            {
                // The supplied XPath contains XPath path delimiters

                char currentToken = '\0';
                char previousToken = '\0';
                for (int charIndex = 0; charIndex < xPath.Length; ++charIndex)
                {
                    previousToken = currentToken;
                    currentToken = xPath[charIndex];
                    if (currentToken == xPathDelimiter)
                    {
                        // Add just the token (delimiter)
                        sbFullyPrefixedXPath.Append(currentToken);
                    }
                    else
                    {
                        // Not an XPath delimiter

                        if ((previousToken == xPathDelimiter) || (charIndex == 0))
                        {
                            // Prefix non-delimiter tokens that:
                            // - Are preceded by a delimiter,
                            // or
                            // - Are at the start of the string
                            sbFullyPrefixedXPath.Append(prefix);
                            sbFullyPrefixedXPath.Append(":");
                            sbFullyPrefixedXPath.Append(currentToken);
                        }
                        else
                        {
                            // Add just the token
                            sbFullyPrefixedXPath.Append(currentToken);
                        }

                    } // Not an XPath delimiter
                } // for charIndex
            } // The supplied XPath contains XPath path delimiters

            return sbFullyPrefixedXPath.ToString();
        } // PrefixSimpleXPath

        static void ShowStringArray(string [] stringArray)
        {
            for (int index = 0; index < stringArray.Length; ++index)
            {
                Console.WriteLine("    {0} = \"{1}\"", index, stringArray[index]);
            }
        }

        static void TestPrefixSimpleXPath()
        {
            string xPath = "/";
            string xPathExpression = PrefixSimpleXPath("namespace", xPath);
            Console.WriteLine("Prefixed XPath expression of \"{0}\" = \"{1}\"", xPath, xPathExpression);

            xPath = "//fred";
            xPathExpression = PrefixSimpleXPath("namespace", xPath);
            Console.WriteLine("Prefixed XPath expression of \"{0}\" = \"{1}\"", xPath, xPathExpression);

            xPath = "//fred/smith//gwen//nichols";
            xPathExpression = PrefixSimpleXPath("namespace", xPath);
            Console.WriteLine("Prefixed XPath expression of \"{0}\" = \"{1}\"", xPath, xPathExpression);

            xPath = "fred/smith//gwen//nichols";
            xPathExpression = PrefixSimpleXPath("namespace", xPath);
            Console.WriteLine("Prefixed XPath expression of \"{0}\" = \"{1}\"", xPath, xPathExpression);
        }

        static void Main(string[] args)
        {
            // TestDirectoryRename();
            // TestDecimal();
            // TestDateTime();
            // TestElapsed();
            // TestExpression();
            // TestSplit();
            TestPrefixSimpleXPath();
        }
    }
}
