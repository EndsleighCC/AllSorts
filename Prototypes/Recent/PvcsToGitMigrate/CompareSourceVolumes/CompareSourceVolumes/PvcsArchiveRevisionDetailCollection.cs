using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CompareSourceVolumes
{
    public class PvcsArchiveRevisionDetailCollection : SortedSet<PvcsArchiveRevisionDetail>
    {
        public PvcsArchiveRevisionDetailCollection( string reportPath ) : base()
        {
            string allGroupPathAndFilename = Path.Combine(reportPath, _allGroupFilename);
            if (!File.Exists(allGroupPathAndFilename))
            {
                Console.WriteLine();
                Console.WriteLine("PvcsArchiveRevisionDetailCollection : All Group Report file \"{0}\" was not found", allGroupPathAndFilename);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("{0} : Reading PVCS Promotion Group Report File \"{1}\"",
                                        DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"), allGroupPathAndFilename);
                try
                {
                    using (StreamReader fileStream = new StreamReader(allGroupPathAndFilename))
                    {
                        int lineCount = 0;
                        while (!fileStream.EndOfStream)
                        {
                            lineCount += 1;
                            string fileLine = fileStream.ReadLine().Trim();

                            string archiveName = null;
                            int archiveDetailIndex = -1;

                            if (fileLine.StartsWith("\""))
                            {
                                // Filename delimited by double quotes
                                int rightDoubleQuoteIndex = fileLine.IndexOf("\"", 1);
                                if (rightDoubleQuoteIndex != -1)
                                {
                                    archiveName = fileLine.Substring(1, rightDoubleQuoteIndex - 1);
                                    archiveDetailIndex = rightDoubleQuoteIndex + 1;
                                }
                            } // Filename delimited by double quotes
                            else
                            {
                                string[] fileLinePart = fileLine.Split(' ');
                                archiveName = fileLinePart[0];
                                archiveDetailIndex = archiveName.Length;
                            }
                            if ( archiveDetailIndex >= 0 )
                            {
                                string[] archiveDetailPart = fileLine.Substring(archiveDetailIndex).Split(new char[] { ' ', '-', ':', '=' }, StringSplitOptions.RemoveEmptyEntries);
                                PvcsArchiveRevisionDetail pvcsArchiveRevisionDetail = new PvcsArchiveRevisionDetail(archiveName, archiveDetailPart[0]);
                                this.Add(pvcsArchiveRevisionDetail);
                            }
                        } // while
                    } // using
                } // try
                catch (FileNotFoundException)
                {
                    throw new UnableToOpenFileException(allGroupPathAndFilename);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("PvcsArchiveRevisionDetailCollection exception {0}",ex.ToString());
                }

                Console.WriteLine();
                Console.WriteLine("{0} : Completed reading PVCS Promotion Group Report File \"{1}\" with {2} entries",
                                        DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"), allGroupPathAndFilename,this.Count);

            }
        }

        const string _allGroupFilename = "AllGroup.txt";

        PvcsPromotionGroupHierarchy _pvcsPromotionGroupHierarchy = new PvcsPromotionGroupHierarchy();

        public class UnableToOpenFileException : ApplicationException
        {
            public UnableToOpenFileException(string filename) : base(filename)
            {
            }
        }

    } // PvcsArchiveRevisionDetailCollection
}
