using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace TestFindText
{
    class Program
    {
        class FindTextInFile
        {
            public FindTextInFile( string fileName )
            {
                _filename = fileName;
            }

            public int Find( string pattern , bool negateMatch )
            {
                int totalMatchCount = 0;
                using (StreamReader streamReader = new StreamReader(_filename))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();
                        MatchCollection matches = Regex.Matches(line,
                                                                pattern,
                                                                RegexOptions.IgnoreCase);
                        if (! negateMatch)
                        {
                            // Don't negate match

                            if (matches.Count > 0)
                            {
                                Console.WriteLine(line);
                                totalMatchCount += 1;
                            }

                        } // Don't negate match
                        else
                        {
                            // Negate

                            if ( matches.Count == 0 )
                            {
                                Console.WriteLine(line);
                                totalMatchCount += 1;
                            }

                        } // Negate
                    }
                }
                return totalMatchCount;
            }

            private string _filename = null;
        }

        static void ProcessSwitches( string[] args )
        {
            foreach (string arg in args)
            {
                if ( arg[0] == '/')
                {
                    string switchCharacter = arg.Substring(1, 1).ToUpper();
                    switch (switchCharacter)
                    {
                        case "N" :
                            _negateMatch = true;
                            Console.WriteLine("Match will be negated");
                            break;
                        default :
                            Console.WriteLine("Unknown switch \"{0}\"", switchCharacter);
                            break;
                    }
                }
                else
                {
                    if ( _pattern == null )
                    {
                        _pattern = arg;
                    }
                    else if ( _filename == null )
                    {
                        _filename = arg;
                    }
                    else
                    {
                        Console.WriteLine( "Unknown argument \"{0}\"", arg );
                    }
                }
            }
        }

        static private string _pattern = null;
        static private string _filename = null;

        static private bool _negateMatch = false;

        static int Main(string[] args)
        {
            int error = 0;

            ProcessSwitches(args);

            if ((_pattern == null) || (_filename == null))
            {
                Console.WriteLine("TestFindText TextToFind Filename");
                error = 1;
            }
            else
            {
                FindTextInFile findTextInFile = new FindTextInFile(_filename);
                if (findTextInFile.Find(_pattern, _negateMatch) == 0)
                {
                    // Not found
                    error = 1;
                }
                else
                {
                    // Found
                    error = 0;
                }
            }
            return error;
        }
    }
}
