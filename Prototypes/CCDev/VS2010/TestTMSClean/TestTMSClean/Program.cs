using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestTMSClean
{
    class Program
    {

        public class TMSClean
        {
            public TMSClean( string sourceDirectoryName, string destinationDirectoryName )
            {
                _sourceDirectoryName = sourceDirectoryName;
                _destinationDirectoryName = destinationDirectoryName;
            }

            private class FileDetailsSortedDictionary :
                SortedDictionary<string /*filename*/, DateTime /* file date/time*/>
            {
                public FileDetailsSortedDictionary() : base(StringComparer.CurrentCultureIgnoreCase)
                {
                }
            }

            private bool FilesAreDifferent( string filename1 , string filename2 )
            {
                bool different = false;

                FileInfo fileInfo1 = new FileInfo(filename1);
                FileInfo fileInfo2 = new FileInfo(filename2);

                if (fileInfo1.Length != fileInfo2.Length)
                    different = true;
                else
                {
                    // Scan the files for differences

                    using (BinaryReader binaryReader1 = new BinaryReader(File.Open(filename1, FileMode.Open)))
                    {
                        using (BinaryReader binaryReader2 = new BinaryReader(File.Open(filename2, FileMode.Open)))
                        {
                            try
                            {
                                while (!different)
                                {
                                    different = binaryReader1.ReadByte() != binaryReader2.ReadByte();
                                }
                            }
                            catch (EndOfStreamException)
                            {
                            }
                        }
                    }

                } // Scan the files for differences

                return different;
            }

            private FileDetailsSortedDictionary GetFileDetails( string directoryName )
            {
                FileDetailsSortedDictionary fileDetails = new FileDetailsSortedDictionary();
                foreach (string filename in Directory.EnumerateFiles(directoryName))
                {
                    FileInfo fileInfo = new FileInfo(filename);
                    fileDetails.Add(fileInfo.Name,fileInfo.LastWriteTime);
                }
                return fileDetails;
            }

            private enum FileDateTimeState { Unknown, SourceIsNewer , Same, SourceIsOlder }

            private void PerformCopy( string source , string destination )
            {
                try
                {
                    if (File.Exists(destination))
                    {
                        Console.WriteLine("    Copying {0} to replace {1}", source, destination);
                        File.Copy(source, destination, true);
                    }
                    else
                    {
                        Console.WriteLine("    Copying {0} to add {1}", source, destination);
                        File.Copy(source, destination, false);
                    }
                }
                catch (Exception eek)
                {
                    Console.WriteLine("Copying {0} to {1} : Exception \"{2}\"",source,destination);
                }
            }

            private void PerformDelete(string filename)
            {
                Console.WriteLine("    Deleting {0}",filename);
                File.Delete(filename);
            }

            public void Clean()
            {
                _sourceFileDetails = GetFileDetails(_sourceDirectoryName);
                _destinationFileDetails = GetFileDetails(_destinationDirectoryName);

                foreach ( string filename in _sourceFileDetails.Keys)
                {
                    string fullSourceFilenanme = Path.Combine(_sourceDirectoryName, filename);
                    string fullDestinationFilename = Path.Combine(_destinationDirectoryName, filename);

                    // Look for the same filename in the source and the destination
                    if (_destinationFileDetails.Keys.Contains(filename))
                    {
                        // File is in both source and destination
                        FileDateTimeState fileDateTimeState = FileDateTimeState.Unknown;
                        bool display = true;

                        string comparisonResult = null;

                        bool fileContentsAreDifferent = FilesAreDifferent(fullSourceFilenanme, fullDestinationFilename);

                        if (_sourceFileDetails[filename] > _destinationFileDetails[filename])
                        {
                            fileDateTimeState = FileDateTimeState.SourceIsNewer;
                            comparisonResult = "is different date/time (newer than)";
                        }
                        else if (_sourceFileDetails[filename] == _destinationFileDetails[filename])
                        {
                            fileDateTimeState = FileDateTimeState.Same;
                            comparisonResult = "is the same date/time as";
                            display = fileContentsAreDifferent;
                        }
                        else if (_sourceFileDetails[filename] < _destinationFileDetails[filename])
                        {
                            fileDateTimeState = FileDateTimeState.SourceIsOlder;
                            comparisonResult = "is different date/time (older than)";
                        }

                        if ( fileContentsAreDifferent )
                        {
                            display = true;
                            comparisonResult += " (contents different)";
                        }

                        if ( display )
                            Console.WriteLine("{0} : Source {1} {2} {3} {4} {5}",
                                filename,
                                fullSourceFilenanme,
                                _sourceFileDetails[filename],
                                comparisonResult,
                                fullDestinationFilename,
                                _destinationFileDetails[filename]);

                        if ( fileContentsAreDifferent )
                        {
                            // File contents are the same

                            switch ( fileDateTimeState )
                            {
                                case FileDateTimeState.SourceIsNewer:
                                    Console.WriteLine("    Source {0} is valid relative to obsolete {1}", fullSourceFilenanme, fullDestinationFilename);
                                    PerformCopy(fullSourceFilenanme,fullDestinationFilename);
                                    break;
                                case FileDateTimeState.Same:
                                    Console.WriteLine("    File {0} contents are different but date/times are the same",filename);
                                    // Don't delete as the contents of the source might be useful
                                    // PerformDelete(fullSourceFilenanme);
                                    break;
                                case FileDateTimeState.SourceIsOlder:
                                    Console.WriteLine("    Source {0} is obsolete relative to {1}",fullSourceFilenanme,fullDestinationFilename);
                                    PerformDelete(fullSourceFilenanme);
                                    break;
                                default :
                                    Console.WriteLine( "Unexpected date/time state for file {0}",fullSourceFilenanme);
                                    break;
                            }

                        } // File contents are the same
                        else
                        {
                            // File contents are the same

                            switch (fileDateTimeState)
                            {
                                case FileDateTimeState.SourceIsNewer:
                                    Console.WriteLine("    Source {0} is valid relative to obsolete {1}", fullSourceFilenanme, fullDestinationFilename);
                                    PerformCopy(fullSourceFilenanme, fullDestinationFilename);
                                    break;
                                case FileDateTimeState.Same:
                                    Console.WriteLine("    File {0} contents are the same and date/times are the same", filename);
                                    PerformDelete(fullSourceFilenanme);
                                    break;
                                case FileDateTimeState.SourceIsOlder:
                                    Console.WriteLine("    Source {0} is obsolete relative to {1}", fullSourceFilenanme, fullDestinationFilename);
                                    PerformDelete(fullSourceFilenanme);
                                    break;
                                default:
                                    Console.WriteLine("Unexpected date/time state for file {0}", fullSourceFilenanme);
                                    break;
                            }

                        } // File contents are the same

                    } // File is in both source and destination
                    else
                    {
                        Console.WriteLine("{0} is only present in {1} dated {2}", filename, _sourceDirectoryName, _sourceFileDetails[filename]);
                        PerformCopy(fullSourceFilenanme, fullDestinationFilename);
                    }
                }
            }

            private string _sourceDirectoryName = null;
            private FileDetailsSortedDictionary _sourceFileDetails = null;

            private string _destinationDirectoryName = null;
            private FileDetailsSortedDictionary _destinationFileDetails = null;
        }

        static void Main(string[] args)
        {
            List<string> errorStringList = new List<string>();

            string source = null;
            string destination = null;

            foreach ( string commandLineArgument in args )
            {
                if (!Directory.Exists(commandLineArgument))
                {
                    errorStringList.Add(String.Format("Directory \"{0}\" does not exist", commandLineArgument));
                }

                if (source == null)
                {
                    source = commandLineArgument;
                }
                else if (destination == null)
                {
                    destination = commandLineArgument;
                }
            }

            if ( ( source == null ) || ( destination == null ) )
                Console.WriteLine("Unsufficient arguments");
            else if (errorStringList.Count > 0)
            {
                Console.WriteLine();
                foreach (string errorString in errorStringList)
                {
                    Console.WriteLine(errorString);
                }
            }
            else
            {
                TMSClean tmsClean = new TMSClean(source, destination);
                tmsClean.Clean();
            }

        }
    }
}
