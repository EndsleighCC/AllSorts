using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindText
{
    class Program
    {
        private static void ScanFile( string filename , ref int foundCount )
        {

        }

        private static void ProcessCommandLine(string [] args,
                                               ref bool excludeEmbedded ,
                                               ref string ignoreLinesBeginningWith ,
                                               ref bool negateSearch ,
                                               ref bool quietOperation ,
                                               List<string> filenameList)
        {
            for ( int argIndex = 0; argIndex < args.Length; ++argIndex )
            {
                string argument = args[argIndex];

                if ( argument.StartsWith("/"))
                {
                    // A switch

                    string switchCharacter = argument.Substring(1, 1).ToLower();

                    switch (switchCharacter)
                    {
                        case "e":
                            if ( ! quietOperation )
                            {
                                excludeEmbedded = true;
                                Console.Error.WriteLine("Embedded text will not be considered a match");
                            }
                            break;
                        case "i":
                            if (!quietOperation)
                            {
                                ignoreLinesBeginningWith = null;
                                Console.Error.WriteLine("Ignore Lines Beginning With is not implemented");
                            }
                            break;
                        case "n":
                            if (!quietOperation)
                            {
                                negateSearch = true;
                                Console.Error.WriteLine("Lines NOT containing the search string will be displayed");
                            }
                            break;
                        case "q":
                            break;
                        default:
                            Console.WriteLine("Unknown switch \"{0}\"", argument);
                            break;
                    } // switch

                } // A switch
                else
                {
                    // A command argument

                    filenameList.Add(argument);

                } // A command argument

            } // for

        } // ProcessCommandLine

        private static void ShowUsage()
        {
            Console.WriteLine("FindText {switches} filenames {...}");
        }

        static int Main(string[] args)
        {
            int error = 0;
            int foundCount = 0;

            bool excludeEmbedded = false;
            string ignoreLinesBeginningWith = null;
            bool negateSearch = false;
            bool quietOperation = false;
            int firstFilenameIndex = -1;

            if ( args.Length < 2 )
            {
                ShowUsage();
                error = 1;
            }
            else
            {
                List<string> filenameList = new List<string>();
                ProcessCommandLine(args, ref excludeEmbedded, ref ignoreLinesBeginningWith, ref negateSearch, ref quietOperation, filenameList);
                if ( firstFilenameIndex < 0 )
                {
                    Console.Error.WriteLine("Search string was not supplied");
                    ShowUsage();
                }
                else
                {
                    foreach (string filename in filenameList)
                    {
                        ScanFile(filename, ref foundCount);
                    }
                }
            }

            int returnValue = 0;

            if ( error == 0 )
            {
                returnValue = foundCount;
            }
            else
            {
                returnValue = error;
            }

            if ( negateSearch )
            {
                if (returnValue != 0)
                {
                    returnValue = 0;
                }
                else
                {
                    returnValue = 1;
                }
            }

            return returnValue;
        } // Main
    }
}
