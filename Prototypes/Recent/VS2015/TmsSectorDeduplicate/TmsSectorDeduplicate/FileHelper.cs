#region Using Directives

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

#endregion

namespace TmsSectorDeduplicate
{
    public class FileHelper
    {
        /// <summary>
        /// Returns a stream reader for the specified file
        /// </summary>
        /// <param name="directory">the directory location of the file</param>
        /// <param name="sourceFilename">The source file name</param>
        /// <returns>StreamReader for the specified file</returns>
        public static StreamReader GetStreamReader(string directory, string sourceFilename)
        {
            return GetStreamReader(Path.Combine(directory, sourceFilename));
        }

        /// <summary>
        /// Returns a stream reader for the specified file
        /// </summary>
        /// <param name="sourceFile">The source file name including directory</param>
        /// <returns>StreamReader for the specified file</returns>
        public static StreamReader GetStreamReader(string sourceFile)
        {
            // Create a Stream Reader using the File Stream and with the default Encoding
            return new StreamReader(GetReadFileStream(sourceFile), Encoding.Default);
        }

        /// <summary>
        /// Returns a stream writer for the specified file creating a backup of the original
        /// if it exists to the specified backup file
        /// </summary>
        /// <param name="directory">The directory location of the destination file</param>
        /// <param name="destinationFilename">The name of the destination file</param>
        /// <param name="backupFilename">The name of the backup file</param>
        /// <returns>StreamReader for the specified file</returns>
        public static StreamWriter GetStreamWriter(string directory, string destinationFilename, string backupFilename)
        {
            // Create a Stream Writer using the File Stream and with the default Encoding
            return new StreamWriter(GetWriteFileStream(directory, destinationFilename, backupFilename), Encoding.Default);
        }

        /// <summary>
        /// Returns a binary reader for the specified file
        /// </summary>
        /// <param name="directory">the directory location of the file</param>
        /// <param name="destinationFilename">the file name</param>
        /// <param name="encoding">The encoding type</param>
        /// <returns>BinaryReader for the specified file</returns>
        public static BinaryReader GetBinaryReader(string directory, string destinationFilename, Encoding encoding)
        {
            return GetBinaryReader(Path.Combine(directory, destinationFilename), encoding);
        }

        /// <summary>
        /// Returns a binary reader for the specified file 
        /// </summary>
        /// <param name="fileLocation">The file path and name</param>
        /// <param name="encoding">The encoding type</param>
        /// <returns>BinaryReader for the specified file</returns>
        public static BinaryReader GetBinaryReader(string fileLocation, Encoding encoding)
        {
            // Create a binary reader using the file stream
            return new BinaryReader(GetReadFileStream(fileLocation), encoding);
        }

        /// <summary>
        /// Returns a binary writer for the specified file 
        /// </summary>
        /// <param name="directory">The directory location of the file</param>
        /// <param name="fileName">The file name</param>
        /// <param name="backupFilename">The name of the backup file</param>
        /// <param name="encoding">The encoding type</param>
        /// <returns>BinaryWriter for the specified file</returns>
        public static BinaryWriter GetBinaryWriter(string directory, string fileName, string backupFilename, Encoding encoding)
        {
            // Create a binary writer using a write file stream
            return new BinaryWriter(GetWriteFileStream(directory, fileName, backupFilename), encoding);
        }

        /// <summary>
        /// Returns a file stream from a specified file
        /// </summary>
        /// <param name="fileLocation">the file location</param>
        /// <returns>FileStream of the specified file</returns>
        private static FileStream GetReadFileStream(string fileLocation)
        {
            CheckDirectory(Path.GetDirectoryName(fileLocation), false);
            CheckFile(fileLocation, false);

            return new FileStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// Creates a new file and stream whilst backing up the original file
        /// </summary>
        /// <param name="directory">the directory name</param>
        /// <param name="fileName">the file name</param>
        /// <param name="backupFilename">The backup filename</param>
        /// <returns>FileStream of the specified file</returns>
        private static FileStream GetWriteFileStream(string directory, string fileName, string backupFilename)
        {
            FileStream fileStream;

            string destinationFile = Path.Combine(directory, fileName);
            string backupFile = string.Empty;

            bool backup = false;

            if (!String.IsNullOrEmpty(backupFilename))
            {
                backupFile = Path.Combine(directory, backupFilename);
                backup = true;
            }

            CheckDirectory(directory, true);
            CheckFile(destinationFile, true);

            try
            {
                // Makes a backup copy of the specified file, deletes the original and creates a new file and filestream
                if (backup)
                    BackupFile(destinationFile, backupFile);

                fileStream = new FileStream(destinationFile, FileMode.Create);
            }
            catch (Exception)
            {
                // Restore the backup copy as the new file cannot be created
                if (backup)
                    RestoreFile(destinationFile, backupFile);

                throw;
            }

            return fileStream;
        }

        /// <summary>
        /// Replaces a file with a the specified backup file.
        /// </summary>
        /// <param name="fileLocation">The complete path and filename of the file to restore</param>
        /// <param name="backupFile">The backup filename and location</param>
        private static void RestoreFile(string fileLocation, string backupFile)
        {
            // Delete an existing new file and replace with the old
            File.Delete(fileLocation);
            File.Copy(backupFile, fileLocation);

        }

        /// <summary>
        /// Makes a backup copy of the specified file to the specified file.
        /// </summary>
        /// <param name="fileLocation">the complete path and filename of the file to backup</param>
        /// <param name="backupFile">The backup filename and location</param>
        private static void BackupFile(string fileLocation, string backupFile)
        {
            // Delete an existing old file and move the original file
            if (!String.IsNullOrEmpty(backupFile))
            {
                try
                {
                    CheckFile(backupFile, false);
                    File.Delete(backupFile);
                }
                catch
                {
                }

                File.Move(fileLocation, backupFile);

            }

        }

        /// <summary>
        /// Checks that a string name of a directory is not null, empty and that the directory exists
        /// </summary>
        /// <param name="directory">the file path</param>
        /// <param name="createDirectory">should the directory be created if not found</param>
        /// <exception cref="ArgumentException">The directory is either null or empty</exception>
        /// <exception cref="DirectoryNotFoundException">Cannot find the specified directory</exception>
        public static void CheckDirectory(string directory, bool createDirectory)
        {
            if (String.IsNullOrEmpty(directory))
            {
                throw new ArgumentException("directory: The directory parameter is empty or null.",
                                             MethodBase.GetCurrentMethod().GetParameters()[0].Name);
            }

            if (!Directory.Exists(directory))
            {
                if (createDirectory)
                {
                    try
                    {
                        Directory.CreateDirectory(directory);
                    }
                    catch (Exception eek)
                    {
                        throw new Exception(String.Format("Directory {0} could not be created : {1}", directory,
                                                            eek.Message));

                    }
                }
                else
                    throw new DirectoryNotFoundException(String.Format("Directory {0} could not be found", directory));
            }
        }

        /// <summary>
        /// Checks that a string name of a file path is not null, empty and that the file exists
        /// </summary>
        /// <param name="filePath">the file path</param>
        /// <param name="createFile">should the file be created if not found</param>
        public static void CheckFile(string filePath, bool createFile)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("filePath: The filePath parameter is empty or null.",
                                             MethodBase.GetCurrentMethod().GetParameters()[0].Name);
            }

            if (!File.Exists(filePath))
            {
                if (createFile)
                {
                    try
                    {
                        CheckDirectory(Path.GetDirectoryName(filePath), true);
                        FileStream tempStream = File.Create(filePath);
                        tempStream.Close();
                    }
                    catch (Exception eek)
                    {
                        throw new Exception(
                            String.Format(
                                "Either the directory {0} does not exist or the file {1} could not be created : {2}",
                                Path.GetDirectoryName(filePath), Path.GetFileName(filePath), eek));
                    }
                }
                else
                {
                    throw new FileNotFoundException(String.Format("Cannot find the file: {0}", filePath));
                }
            }
        }
    }
}
