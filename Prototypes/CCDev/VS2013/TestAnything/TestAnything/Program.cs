using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAnything
{
    class Program
    {
        static string CaseSensitivePathOfFile(string filename)
        {
            string caseSensitivePathOfFile = null;

            string[] filenamePart = filename.Split(new char[] {'\\'},StringSplitOptions.RemoveEmptyEntries);

            if (filenamePart.Length > 0)
            {
                // The filename has at least one part

                caseSensitivePathOfFile = string.Empty;

                string caseInsensitivePathOfFile = string.Empty;
                int firstPartIndex = 0;

                if (filename.Substring(0, 2) == "\\\\")
                {
                    // Skip the UNC name
                    caseInsensitivePathOfFile = "\\\\" + filenamePart[0];
                    firstPartIndex = 1;
                    caseSensitivePathOfFile = "\\\\" + filenamePart[0];
                } // Skip the UNC name
                else if (filename[1] == ':')
                {
                    // Skip the drive specifier
                    firstPartIndex = 1;
                    caseInsensitivePathOfFile = filenamePart[0];
                    caseSensitivePathOfFile = filenamePart[0];
                } // Skip the drive specifier
                else
                {
                    // Must be a relative path
                    firstPartIndex = 0;
                }

                // Progress down the tree acquiring the case sensitive name of each directory and eventually the file
                for (int filenamePartIndex = firstPartIndex;
                    filenamePartIndex < filenamePart.Length;
                    ++filenamePartIndex)
                {

                    if (String.IsNullOrEmpty(caseInsensitivePathOfFile))
                    {
                        caseInsensitivePathOfFile = filenamePart[filenamePartIndex];
                    }
                    else
                    {
                        caseInsensitivePathOfFile += "\\" + filenamePart[filenamePartIndex];
                    }

                    // Get the name of the parent directory for the directory or file at this level
                    string parentDirectory = Directory.GetParent(caseInsensitivePathOfFile).FullName;

                    string[] name;

                    FileAttributes fileAttributes = File.GetAttributes(caseInsensitivePathOfFile);
                    if (fileAttributes.HasFlag(FileAttributes.Directory))
                    {
                        Console.WriteLine("{0} is a directory", caseInsensitivePathOfFile);
                        // Match the name in the parent directory to acquire the case sensitive name
                        name = Directory.GetDirectories(parentDirectory, filenamePart[filenamePartIndex]);
                    }
                    else
                    {
                        Console.WriteLine("{0} is a file", caseInsensitivePathOfFile);
                        // Match the name in the parent directory to acquire the case sensitive name
                        name = Directory.GetFiles(parentDirectory, filenamePart[filenamePartIndex]);
                    }

                    // Only the last part of the path actually has its case preserved
                    string[] caseSensitivePart = name[0].Split(new char[] {'\\'});
                    caseSensitivePathOfFile += "\\" + caseSensitivePart[caseSensitivePart.Length-1];

                } // for

            } // The filename has at least one part

            return caseSensitivePathOfFile;

        } // CaseSensitivePathOfFile

        static void Main(string[] args)
        {
            FileInfo fileInfo = new FileInfo("c:\\st00\\EIS\\include\\eis.h");

            Console.WriteLine("FileInfo Path is \"{0}\"",fileInfo.FullName);

            const string directoryName = @"D:\Repos\HT2";
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
            if (directoryInfo.Exists)
            {
                Console.WriteLine("Directory {0} exists", directoryName);
            }
            else
            {
                Console.WriteLine("Directory {0} does not exist",directoryName);
            }

            string[] filenames = Directory.GetFiles("c:\\st00\\EIS\\include", "EIS.h");

            Console.WriteLine("GetFiles path is \"{0}\"",filenames[0]);

            string rootWorkingDirectory = "c:\\st00";
            string filename = rootWorkingDirectory + "\\EIS\\include\\EIS.H";
            string caseSensitiveFilename = CaseSensitivePathOfFile(filename);
            Console.WriteLine("Case sensitive path is \"{0}\"", filenames[0]);

            string workfileFullPath = CaseSensitivePathOfFile(filename);
            string workfileRelativePath = workfileFullPath.Substring(rootWorkingDirectory.Length + 1);
            Console.WriteLine("Workfile relative path is \"{0}\"",workfileRelativePath);

        }
    }
}
