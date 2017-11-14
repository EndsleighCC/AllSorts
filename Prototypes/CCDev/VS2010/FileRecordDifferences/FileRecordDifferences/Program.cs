using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileRecordDifferences
{
    class Program
    {
        class FileRecordContainer
        {
            public FileRecordContainer(string filename)
            {
                Filename = filename ;
                if (File.Exists(filename))
                {
                    using (StreamReader fileStream = new StreamReader(filename))
                    {
                        string currentArchiveName = null;
                        while (!fileStream.EndOfStream)
                        {
                            string fileLine = fileStream.ReadLine().Trim();

                            if ( ( fileLine.Length > 0 ) && ( fileLine[0] != '#' ) )
                            {
                                // Non-blank file line

                                string archiveName;
                                string archiveDetailText;
                                if (fileLine[0] != '"')
                                {
                                    // Spaces in the filename

                                    string[] archiveDetailPart = fileLine.Split(new char[] { ' ' },
                                                                                StringSplitOptions.RemoveEmptyEntries);

                                    archiveName = archiveDetailPart[0];
                                    archiveDetailText = fileLine.Substring(archiveName.Length).TrimStart(new char[] { ' ', '-' });

                                } // Spaces in the filename
                                else
                                {
                                    // No spaces in the filename

                                    int endDoubleQuoteIndex = fileLine.IndexOf('"', 1);

                                    archiveName = fileLine.Substring(1, endDoubleQuoteIndex - 1);

                                    string[] archiveDetailPart =
                                        fileLine.Substring(endDoubleQuoteIndex + 1).Split(new char[] { ' ' },
                                                                                            StringSplitOptions.
                                                                                                RemoveEmptyEntries);

                                    // The Archive Details follow the last double quote
                                    archiveDetailText = fileLine.Substring(archiveName.Length + 2).TrimStart(new char[] { ' ', '-' });

                                } // No spaces in the filename

                                _fileRecordSet.Add(archiveName);

                            } // Non-blank file line
                        }
                    }
                }
            } // FileRecordContainer default constructor

            public string Filename { get; private set; }

            public SortedSet<string> DifferencesFrom(FileRecordContainer other)
            {
                SortedSet<string> differences = new SortedSet<string>();

                foreach (string fileRecord in other._fileRecordSet)
                {
                    if ( ! _fileRecordSet.Contains(fileRecord) )
                    {
                        differences.Add(fileRecord);
                    }
                }

                return differences ;
            }

            private SortedSet<string> _fileRecordSet = new SortedSet<string>();

        } // class FileRecordContainer

        static void ShowDifferences( FileRecordContainer fileRecordContainer1 , FileRecordContainer fileRecordContainer2 )
        {
            SortedSet<string> differences1minus2 = fileRecordContainer1.DifferencesFrom(fileRecordContainer2);
            if (differences1minus2.Count == 0)
            {
                Console.WriteLine("All of \"{0}\" is contained in \"{1}\"", fileRecordContainer2.Filename, fileRecordContainer1.Filename);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("There are {0} records in \"{1}\" that are not in \"{2}\"", differences1minus2.Count,fileRecordContainer2.Filename, fileRecordContainer1.Filename);
                Console.WriteLine();
                foreach (string fileRecord in differences1minus2)
                {
                    Console.WriteLine("    {0}",fileRecord);
                }
            }
        }

        static void Main(string[] args)
        {
            FileRecordContainer fileRecordContainer1 = null ;
            FileRecordContainer fileRecordContainer2 = null ;

            if (args.Length > 0 )
            {
                fileRecordContainer1 = new FileRecordContainer( args[0] ) ;
            }
            if ( args.Length > 1 )
            {
                fileRecordContainer2 = new FileRecordContainer( args[1] ) ;
            }

            if ((fileRecordContainer1 != null) && (fileRecordContainer2 != null))
            {
                ShowDifferences(fileRecordContainer1, fileRecordContainer2);
                ShowDifferences(fileRecordContainer2, fileRecordContainer1);
            }
        }
    }
}
