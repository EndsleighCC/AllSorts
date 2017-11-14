using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestMove
{
    class Program
    {
        const string _UnusedDirectory = "Unused";

        static bool RenameFileOrDirectory( string fileOrDirectoryName )
        {
            bool success = false;

            // Find the extension
            int lastSlashPos = fileOrDirectoryName.LastIndexOf('\\');

            const int invalidPos = -1;

            int lastDotPos = invalidPos;

            int versionInsertPos = invalidPos;

            if (lastSlashPos < 0)
            {
                // No directory specifier
                lastDotPos = fileOrDirectoryName.LastIndexOf('.');
                if (lastDotPos < 0)
                {
                    // No extension specifier
                    // Add at the end
                    versionInsertPos = invalidPos;
                }
                else
                {
                    // There is an extension
                    // Insert before the last dot
                    versionInsertPos = lastDotPos;
                }

            } // No directory specifier
            else
            {
                // There is at least one directory specifier
                lastDotPos = fileOrDirectoryName.LastIndexOf('.');
                if (lastDotPos < 0)
                {
                    // No extension specifier
                    // Add at the end
                    versionInsertPos = invalidPos;
                }
                else
                {
                    // There is an extension
                    // Check that it is after the last slash
                    if (lastDotPos < lastSlashPos)
                    {
                        // Dot is before the slash to it's in a directory name
                        // Add the version number on the end
                        versionInsertPos = invalidPos;
                    }
                    else
                    {
                        // Insert before the last dot
                        versionInsertPos = lastDotPos;
                    }
                }
            } // There is at least one directory specifier

            bool failed = false;
            for (int fileOrDirectoryId = 0; (!success) && (!failed) && (fileOrDirectoryId < 1000); ++fileOrDirectoryId)
            {
                string newName = null;
                if (versionInsertPos < 0)
                {
                    // Add the "version" to the end
                    newName = String.Format("{0}.{1}", fileOrDirectoryName , fileOrDirectoryId.ToString("D3"));
                }
                else
                {
                    // Insert the version before the extension
                    newName = fileOrDirectoryName.Insert(
                                    versionInsertPos,
                                    String.Format(".{0}", fileOrDirectoryId.ToString("D3")));
                }
                if (!File.Exists(newName))
                {
                    try
                    {
                        Console.WriteLine("    Renaming \"{0}\"", fileOrDirectoryName);
                        Console.WriteLine("        to \"{0}\"", newName);
                        File.Move(fileOrDirectoryName, newName);
                        success = true;
                    }
                    catch
                    {
                        failed = true;
                    }
                }
            }

            return success;
        }

        static string ParentDirectoryOf( string pathname )
        {
            string parentDirectory = null;
            int lastSlashPos = pathname.LastIndexOf('\\');
            if (lastSlashPos > 0 )
            {
                parentDirectory = pathname.Substring(0, lastSlashPos);
            }
            return parentDirectory;
        }

        static bool MoveFileOrDirectory(string pathname)
        {
            bool success = false;

            pathname = Path.GetFullPath(pathname);

            if (File.Exists(pathname) || Directory.Exists(pathname))
            {
                // Remove any leading and trailing slashes
                pathname = pathname.Trim('\\');

                string sourcePath = null;
                string destinationPath = null;

                int firstSlashPos = pathname.IndexOf('\\');
                if (firstSlashPos < 0)
                {
                    // No extension
                    sourcePath = "\\" + pathname;
                    destinationPath = "\\" + _UnusedDirectory + "\\" + pathname;
                }
                else
                {
                    sourcePath = pathname;
                    destinationPath = pathname.Insert(firstSlashPos,
                                                      "\\"+_UnusedDirectory);
                }

                // It may be necessary to rename a file or directory that already exists
                if (File.Exists(sourcePath))
                {
                    // It's a file
                    // Ensure that the destination does not already exist
                    if (File.Exists(destinationPath))
                    {
                        // The destination already exists so rename it
                        success = RenameFileOrDirectory(destinationPath);
                        if (!success)
                        {
                            Console.WriteLine("Unable to rename the existing destination file \"{0}\"",
                                                destinationPath);
                        }
                    }
                }
                else
                {
                    // It's a directory
                    // Ensure that the destination does not already exist
                    if ( Directory.Exists(destinationPath))
                    {
                        success = RenameFileOrDirectory(destinationPath);
                        if ( ! success )
                        {
                            Console.WriteLine("Unable to rename the existing destination directory \"{0}\"",
                                                destinationPath);
                        }
                    }
                }

                if (success)
                {
                    try
                    {
                        string parentDirectory = ParentDirectoryOf(destinationPath);
                        if (parentDirectory == null)
                        {
                            Console.WriteLine("    {0} \"{1}\" not moved. No parent directory",
                                                ( File.Exists(sourcePath) ? "File" : "Directory" ) ,
                                                sourcePath);
                        }
                        else
                        {
                            if (!Directory.Exists(parentDirectory))
                            {
                                // Ensure that the parent directory exists to which to move the file
                                Directory.CreateDirectory(parentDirectory);
                            }
                            if (File.Exists(sourcePath))
                            {
                                // Move the file
                                Console.WriteLine("    Move file \"{0}\"", sourcePath);
                                Console.WriteLine("        to \"{0}\"", destinationPath);
                                File.Move(sourcePath, destinationPath);
                            }
                            else
                            {
                                // Move the directory
                                Console.WriteLine("    Move directory \"{0}\"", sourcePath);
                                Console.WriteLine("        to \"{0}\"", destinationPath);
                                Directory.Move(sourcePath, destinationPath);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unable to move \"{0}\" to \"{1}\". Exception = \"{2}\"",
                            sourcePath, destinationPath, ex.ToString());
                    }
                }
            }

            return success;
        }

        static void Main(string[] args)
        {
            MoveFileOrDirectory(@"u:\Endsleigh\Configuration\Permissions\Deployment\Endsleigh.Configuration.Controls.msi");
            // MoveFileOrDirectory(@"u:\Endsleigh\Configuration\Permissions\Deployment\Crap");
        }
    }
}
