using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NonEolHash
{
    class Program
    {
        private static long NonEolHash(string filename)
        {
            long hashValue = 0;

            try
            {
                using (BinaryReader binaryReader = new BinaryReader(File.Open(filename, FileMode.Open)))
                {
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        char ch = binaryReader.ReadChar();
                        switch (ch)
                        {
                            case '\r':
                            case '\n':
                                // Don't count EOL characters
                                break;
                            default:
                                hashValue += (long)ch;
                                break;
                        } // switch
                    } // while
                }

                Console.WriteLine("\"{0}\" non-EOL hash {1}", filename, hashValue);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("\"{0}\" does not exist", filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception reading \"{0}\" = \"{1}\"", filename, ex.ToString());
            }

            return hashValue;
        }

        private class FilenameAndHashValue
        {
            public FilenameAndHashValue(string filename, long hashValue)
            {
                Filename = filename;
                Hash = hashValue;
            }

            public string Filename { get; private set; }
            public long Hash { get; private set;  }
        }

        static int Main(string[] args)
        {
            int error = 0;

            List<FilenameAndHashValue> filenameAndHashValueList = new List<FilenameAndHashValue>();
            foreach ( string filename in args)
            {
                string fullFilename = Path.GetFullPath(filename);
                filenameAndHashValueList.Add(new FilenameAndHashValue(fullFilename, NonEolHash(fullFilename)));
            }

            for ( int indexOuter = 0; indexOuter < filenameAndHashValueList.Count; ++indexOuter)
            {
                for ( int indexInner = indexOuter+1; indexInner < filenameAndHashValueList.Count; ++indexInner)
                {
                    if (filenameAndHashValueList[indexInner].Hash != filenameAndHashValueList[indexOuter].Hash)
                    {
                        Console.WriteLine("Files \"{0}\" and \"{1}\" are more than EOL different",
                                        filenameAndHashValueList[indexOuter].Filename, filenameAndHashValueList[indexInner].Filename);
                        error = 1;
                    }
                    else
                    {
                        Console.WriteLine("Files \"{0}\" and \"{1}\" are EOL the same",
                                        filenameAndHashValueList[indexOuter].Filename, filenameAndHashValueList[indexInner].Filename);
                    }
                }
            }

            return error;
        }
    }
}
