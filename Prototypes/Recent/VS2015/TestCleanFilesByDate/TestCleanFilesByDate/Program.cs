using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestCleanFilesByDate
{
    class Program
    {
        static bool RenameFile(string filename)
        {
            bool success = false;

            if ( File.Exists(filename))
            {
                string filenameStem = null;
                string filenameExtension = null;

                int dotPos = filename.LastIndexOf('.');
                if ( dotPos < 0 )
                {
                    // No extension
                    filenameStem = filename;
                    filenameExtension = "";
                }
                else
                {
                    filenameStem = filename.Substring(0, dotPos);
                    filenameExtension = filename.Substring(dotPos + 1);
                }

                bool failed = false;
                for ( int fileId = 0; (!success) && (!failed) && ( fileId < 1000 ) ; ++ fileId)
                {
                    string newFilename = null;
                    if (filenameExtension.Length == 0)
                    {
                        newFilename = String.Format("{0}.{1}", filenameStem, fileId.ToString("D3"));
                    }
                    else
                    {
                        newFilename = String.Format("{0}.{1}.{2}", filenameStem, fileId.ToString("D3"),filenameExtension);
                    }
                    if ( ! File.Exists(newFilename))
                    {
                        try
                        {
                            Console.WriteLine("    Renaming \"{0}\"", filename);
                            Console.WriteLine("        to \"{0}\"", newFilename);
                            // File.Move(filename, newFilename);
                            success = true;
                        }
                        catch
                        {
                            failed = true;
                        }
                    }
                }
            }

            return success;
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("TestCleanFilesByDate filename MaxDate");
            }
            else
            {
                string fileSpec = args[0];
                DateTime fileMaxDate;

                try
                {
                    fileMaxDate = DateTime.Parse(args[1]);

                    string path = null;
                    string pattern = null;

                    int lastBackSlashPos = fileSpec.LastIndexOf('\\');
                    if (lastBackSlashPos < 0)
                    {
                        // No path specified so assume the current drive
                        path = "\\";
                        pattern = fileSpec;
                    }
                    else
                    {
                        path = fileSpec.Substring(0, lastBackSlashPos + 1);
                        pattern = fileSpec.Substring(lastBackSlashPos + 1);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Searching in \"{0}\" for \"{1}\" with a Last Write Date earlier than {2}",
                                            path, pattern, fileMaxDate);

                    string[] filenameList = Directory.GetFiles(path, pattern , SearchOption.AllDirectories);

                    Console.WriteLine();
                    Console.WriteLine("Number of files detected was {0}", filenameList.Length);

                    int displayCount = 0;
                    foreach ( string filename in filenameList)
                    {
                        // Don't do files in the "Package" or "Unused" directories
                        if (    ( ! filename.Contains("\\Package\\") )
                             && ( ! filename.Contains("\\Unused\\") )
                           )
                        {
                            FileInfo fileinfo = new FileInfo(filename);
                            if (fileinfo.LastWriteTime < fileMaxDate)
                            {
                                displayCount += 1;
                                if (displayCount == 1)
                                {
                                    Console.WriteLine();
                                }
                                Console.WriteLine("\"{0}\" {1} is too old", filename, fileinfo.LastWriteTime.ToString("yyyy-MMM-dd HH:mm:ss"));
                                RenameFile(filename);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid date \"{0}\"", ex.ToString());
                }
            }
        }
    }
}
