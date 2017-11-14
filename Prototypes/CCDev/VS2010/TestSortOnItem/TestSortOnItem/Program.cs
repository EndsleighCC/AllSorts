using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestSortOnItem
{
    class Program
    {
        public class FileDetail : IComparable<FileDetail>, IComparer<FileDetail>
        {
            public FileDetail( string filename , string fileDetailText)
            {
                Name = filename;
                DetailText = fileDetailText;

                string[] filenamePart = filename.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if ( filenamePart.Length <= 1)
                    Extension = null;
                else
                {
                    Extension = filenamePart[filenamePart.Length - 1];
                }
            }

            public string Name { get; private set; }
            public string DetailText { get; private set; }
            public string Extension { get; private set; }

            // IComparable
            public int CompareTo( FileDetail otherFileDetail )
            {
                return Compare(this, otherFileDetail);
            }

            // IComparer
            public int Compare( FileDetail fileDetailLeft , FileDetail fileDetailRight )
            {
                int compare = 0;

                if ((fileDetailLeft.Extension == null) || (fileDetailRight.Extension == null))
                    compare = String.Compare(fileDetailLeft.Name, fileDetailRight.Name, /* ignore case */ true);
                else
                {
                    compare = String.Compare(fileDetailLeft.Extension, fileDetailRight.Extension, /* ignore case */ true);
                    if (compare == 0)
                        compare = String.Compare(fileDetailLeft.Name, fileDetailRight.Name, /* ignore case */ true);
                }

                return compare;
            }

            public override string ToString()
            {
                return Name + ":" + Extension + ":" + DetailText;
            }
        }

        public class FileDetailSortedDictionary : SortedDictionary<FileDetail,FileDetail>
        {
            public FileDetailSortedDictionary()
                : base()
            {
            }
        }

        static void Main(string[] args)
        {
            if ( args.Length >= 1 )
            {
                string filename = args[0];

                if ( !File.Exists(filename))
                {
                    Console.WriteLine( "File \"{0}\" does not exist",filename);
                }
                else
                {
                    FileDetailSortedDictionary fileDetailSortedDictionary = new FileDetailSortedDictionary();

                    using (StreamReader filestream = new StreamReader(filename))
                    {
                        while (!filestream.EndOfStream)
                        {
                            string fileLine = filestream.ReadLine();
                            string filenameEntry;

                            if (fileLine[0] != '"')
                            {
                                // Spaces in the filename

                                string[] fileDetailPart = fileLine.Split(new char[] { ' ' },
                                                                            StringSplitOptions.RemoveEmptyEntries);

                                filenameEntry = fileDetailPart[0];

                            } // Spaces in the filename
                            else
                            {
                                // No spaces in the filename

                                int endDoubleQuoteIndex = fileLine.IndexOf('"', 1);

                                filenameEntry = fileLine.Substring(1, endDoubleQuoteIndex - 1);

                            } // No spaces in the filename

                            FileDetail fileDetail = new FileDetail(filenameEntry,fileLine);

                            fileDetailSortedDictionary.Add(fileDetail, fileDetail);

                        }
                    }

                    if ( fileDetailSortedDictionary.Count == 0 )
                        Console.WriteLine("No files to analyse");
                    else
                    {
                        string currentExtension = "";
                        foreach ( KeyValuePair<FileDetail,FileDetail> entry in fileDetailSortedDictionary)
                        {
                            if (String.Compare(entry.Key.Extension, currentExtension, true) != 0)
                            {
                                Console.WriteLine();
                                Console.WriteLine("{0}",entry.Key.Extension);
                                currentExtension = entry.Key.Extension;
                            }
                            Console.WriteLine("    {0}",entry.Key.Name);
                        }
                    }
                }
            }
        }
    }
}
