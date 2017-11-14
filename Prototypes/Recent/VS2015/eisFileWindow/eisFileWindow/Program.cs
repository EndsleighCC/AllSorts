using System;
using System.IO;

namespace eisFileWindow
{

    class Program
    {
        private static Int64 _fileWindowStart = 4096;

        private static void ShowUsage()
        {

            Console.WriteLine();
            Console.WriteLine("* Display A Window Of Text From A Text File *");
            Console.WriteLine("23-Oct-2017                       C.Cornelius");
            Console.WriteLine();
            Console.WriteLine("Usage: eisFileWindow Filename { OffsetFromEnd { DisplayCount} }");
            Console.WriteLine();
            Console.WriteLine("Function:");
            Console.WriteLine();
            Console.WriteLine("    Display a window of records from a text file starting from the specified");
            Console.WriteLine("    byte offset from the end. If no offset is specified then display will");
            Console.WriteLine("    start {0} bytes from the end of the file. If the offset is negative", _fileWindowStart);
            Console.WriteLine("    or zero the display will start from the beginning of the file.");
            Console.WriteLine("    If a number of bytes to be displayed is not specified then all the file");
            Console.WriteLine("    from the specified position to the end of the file will be displayed.");
        }

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                ShowUsage();
            }
            else
            {
                string fullFilename = Path.GetFullPath(args[0]);
                if ( ! File.Exists(fullFilename) )
                {
                    Console.WriteLine("File \"{0}\" does not exist", fullFilename);
                }
                else
                {
                    Int64 fileWindowLength = _fileWindowStart;

                    Int64 initialFilePosition = 0;

                    if (args.Length > 1)
                    {
                        Int64.TryParse(args[1], out _fileWindowStart);
                    }

                    if (args.Length > 2)
                    {
                        Int64 fileWindowLengthSupplied = 0;
                        Int64.TryParse(args[2], out fileWindowLengthSupplied);
                        if (fileWindowLengthSupplied > 0)
                        {
                            fileWindowLength = fileWindowLengthSupplied;
                        }
                    }

                    // Console.WriteLine("Reading \"{0}\"", fullFilename);
                    try
                    {
                        using (FileStream fileStream = new FileStream(fullFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (StreamReader streamReader = new StreamReader(fileStream))
                            {

                                if (_fileWindowStart <= 0)
                                {
                                    // Start from the beginning of the file
                                    initialFilePosition = 0;
                                } // Start from the beginning of the file
                                else
                                {
                                    // Start from the specified distance from the end of the file
                                    initialFilePosition = streamReader.BaseStream.Length - _fileWindowStart;
                                    if (initialFilePosition < 0)
                                    {
                                        initialFilePosition = 0;
                                    }
                                }

                                streamReader.BaseStream.Position = initialFilePosition;
                                Int64 finalFilePosition = initialFilePosition + fileWindowLength;
                                if (finalFilePosition > streamReader.BaseStream.Length)
                                {
                                    finalFilePosition = streamReader.BaseStream.Length;
                                }

                                int intCharacter = 0;
                                Int64 characterCount = initialFilePosition - 1;

                                if ( initialFilePosition != 0 )
                                {
                                    // Position past the next end of line

                                    bool foundEOL = false;
                                    while (    (!foundEOL)
                                            && ((intCharacter = streamReader.Read()) != -1)
                                            && (characterCount <= finalFilePosition)
                                          )
                                    {
                                        characterCount += 1;
                                        if ( intCharacter == Environment.NewLine[0] )
                                        {
                                            if (Environment.NewLine.Length > 1)
                                            {
                                                if (streamReader.Peek() == Environment.NewLine[1])
                                                {
                                                    // Consume the next EOL character
                                                    streamReader.Read();
                                                    foundEOL = true;
                                                }

                                            }
                                            else
                                            {
                                                foundEOL = true;
                                            }
                                        }
                                    }

                                } // Position past the next end of line

                                //Console.WriteLine("File is {0} bytes long starting at {1} until {2}",
                                //                    streamReader.BaseStream.Length,
                                //                    streamReader.BaseStream.Position,
                                //                    finalFilePosition);

                                while ( ( ( intCharacter = streamReader.Read() ) != -1 ) && (characterCount <= finalFilePosition ) )
                                {
                                    characterCount += 1;
                                    Console.Write("{0}", (char)intCharacter);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception reading \"{0}\" = {1}", fullFilename, ex.ToString());
                    }
                }
            }
        }
    }
}
