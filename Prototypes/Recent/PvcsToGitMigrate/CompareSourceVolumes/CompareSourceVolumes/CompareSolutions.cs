using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CompareSourceVolumes
{
    static class CompareSolutions
    {
        private static string ChangeAllForwardSlashesToBackSlashes(string filename)
        {
            return filename.Replace("/", "\\");
        } // ChangeAllForwardSlashesToBackSlashes

        private static string RemoveAllGUIDs(string str)
        {
            StringBuilder sb = new StringBuilder("", str.Length);

            bool copying = true;
            for (int charIndex = 0; charIndex < str.Length; ++charIndex)
            {
                if (copying)
                {
                    if (str[charIndex] == '{')
                    {
                        sb.Append(str[charIndex]);
                        copying = false;
                    }
                    else
                    {
                        sb.Append(str[charIndex]);
                    }
                }
                else
                {
                    if (str[charIndex] == '}')
                    {
                        sb.Append(str[charIndex]);
                        copying = true;
                    }
                }
            }

            return sb.ToString();
        } // RemoveAllGUIDs

        private const string _projectToken = "Project(";

        private static string CaseSensitivePathOfFile(string filename)
        {
            string caseSensitivePathOfFile = null;

            FileInfo fileInfo = new FileInfo(filename);
            if (!fileInfo.Exists)
            {
                // Default to returning the name supplied
                caseSensitivePathOfFile = filename;
            }
            else
            {
                // The file exists

                string[] filenamePart = filename.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                if (filenamePart.Length > 0)
                {
                    // The filename has at least one part

                    caseSensitivePathOfFile = string.Empty;

                    string caseInsensitivePathOfFile = string.Empty;
                    int firstPartIndex = 0;

                    if (filename.Substring(0, 2) == "\\\\")
                    {
                        // Skip the UNC name
                        caseInsensitivePathOfFile = "\\\\" + filenamePart[0] + "\\" + filenamePart[1];
                        firstPartIndex = 2;
                        caseSensitivePathOfFile = "\\\\" + filenamePart[0] + "\\" + filenamePart[1];
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

                        string parentDirectory = Directory.GetParent(caseInsensitivePathOfFile).FullName;

                        string[] name;

                        FileAttributes fileAttributes = File.GetAttributes(caseInsensitivePathOfFile);
                        if (fileAttributes.HasFlag(FileAttributes.Directory))
                        {
                            // Console.WriteLine("{0} is a directory", caseInsensitivePathOfFile);
                            // Match the name in the parent directory to acquire the case sensitive name
                            name = Directory.GetDirectories(parentDirectory, filenamePart[filenamePartIndex]);
                        }
                        else
                        {
                            // Console.WriteLine("{0} is a file", caseInsensitivePathOfFile);
                            // Match the name in the parent directory to acquire the case sensitive name
                            name = Directory.GetFiles(parentDirectory, filenamePart[filenamePartIndex]);
                        }

                        // Only the last part of the path actually has its case preserved
                        string[] caseSensitivePart = name[0].Split(new char[] { '\\' });
                        caseSensitivePathOfFile += "\\" + caseSensitivePart[caseSensitivePart.Length - 1];

                    } // for

                } // The filename has at least one part

            } // The file exists

            return caseSensitivePathOfFile;

        } // CaseSensitivePathOfFile

        /// <summary>
        /// Identifies the name of the Solution from the full path and filename
        /// </summary>
        /// <param name="solutionNameAndPath">The full path and filename to the Solution</param>
        /// <returns>A string containing the name of the Solution without the file extension</returns>
        private static string SolutionNameOf(string solutionNameAndPath)
        {
            string solutionName = null;

            int lastSlashIndex = solutionNameAndPath.LastIndexOf('\\');
            if (lastSlashIndex >= 0)
            {
                solutionName = solutionNameAndPath.Substring(lastSlashIndex + 1);
            }
            else
            {
                // No path supplied
                solutionName = solutionNameAndPath;
            }

            // Remove the .sln
            string solutionNameLower = solutionName.ToLower();
            int dotExtensionIndex = solutionNameLower.LastIndexOf(".sln");
            if (dotExtensionIndex >= 0)
            {
                solutionName = solutionName.Substring(0, dotExtensionIndex);
            }

            return solutionName;

        } // SolutionNameOf

        /// <summary>
        /// The GUID for the Solution is the first GUID to be encountered
        /// </summary>
        /// <param name="solutionFile">The in-memory copy of the Solution File</param>
        /// <returns>A string containing the GUID for the Solution</returns>
        private static string SolutionGUID(string solutionFile)
        {
            string projectGUID = null;
            int projectIndex = solutionFile.IndexOf(_projectToken);
            if (projectIndex >= 0)
            {
                int openCurlyBrace = solutionFile.IndexOf('{', projectIndex);
                if (openCurlyBrace >= 0)
                {
                    int closeCurlyBrace = solutionFile.IndexOf('}', openCurlyBrace);
                    if (closeCurlyBrace >= 0)
                    {
                        projectGUID = solutionFile.Substring(openCurlyBrace, closeCurlyBrace - openCurlyBrace + 1);
                    }
                }
            }
            return projectGUID;
        } // SolutionGUID

        /// <summary>
        /// Returns a Sorted Dictionary indexed by GUID of GUIDs and Solution Name pairs
        /// </summary>
        /// <param name="solutionFile">A string containing the Solution File contents</param>
        /// <returns>A Sorted Dictionary indexed by GUID of GUIDS and Solution Name string pairs</returns>
        private static SortedDictionary<string /* GUID */ , string /* Solution Name */ > FindAllProjects(string solutionFile)
        {
            SortedDictionary<string, string> projectDetails = new SortedDictionary<string, string>();
            int currentIndex = solutionFile.IndexOf(_projectToken);
            while (currentIndex != -1)
            {
                int equalSignIndex = solutionFile.IndexOf("=", currentIndex);
                if (equalSignIndex >= 0)
                {
                    int doubleQuote1Index = solutionFile.IndexOf("\"", equalSignIndex + 1);
                    if (doubleQuote1Index >= 0)
                    {
                        int doubleQuote2Index = solutionFile.IndexOf("\"", doubleQuote1Index + 1);
                        if (doubleQuote2Index >= 0)
                        {
                            string projectName = solutionFile.Substring(doubleQuote1Index + 1,
                                doubleQuote2Index - doubleQuote1Index - 1);

                            int comma1Index = solutionFile.IndexOf(',', doubleQuote2Index + 1);
                            if (comma1Index >= 0)
                            {
                                int comma2Index = solutionFile.IndexOf(",", comma1Index + 1);
                                if (comma2Index >= 0)
                                {
                                    doubleQuote1Index = solutionFile.IndexOf("\"", comma2Index);
                                    if (doubleQuote1Index >= 0)
                                    {
                                        doubleQuote2Index = solutionFile.IndexOf("\"", doubleQuote1Index + 1);
                                        if (doubleQuote2Index >= 0)
                                        {
                                            string projectGUID = solutionFile.Substring(doubleQuote1Index + 2,
                                                doubleQuote2Index - doubleQuote1Index - 3);
                                            projectDetails.Add(projectGUID, projectName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                currentIndex = solutionFile.IndexOf(_projectToken, currentIndex + 1);
            } // while

            return projectDetails;
        } // FindAllProjects

        /// <summary>
        /// Within the contents of the Solution File, replaces all the GUIDs with the corresponding Solution Name
        /// </summary>
        /// <param name="solutionFile">A string containing the contents of the Soution File</param>
        /// <param name="projectDetails">A Sorted Dictionary indexed by Solution GUID containing Solution GUID 
        /// and Solution Name string pairs</param>
        /// <returns>The Solution File contents with all the GUIDs replaced by Project Names</returns>
        private static string ReplaceEachProjectGUIDWithProjectName(string solutionFile, SortedDictionary<string, string> projectDetails)
        {
            string updatedSolutionFile = solutionFile;
            foreach (string projectGUID in projectDetails.Keys)
            {
                updatedSolutionFile = updatedSolutionFile.Replace(projectGUID, projectDetails[projectGUID]);
            }
            return updatedSolutionFile;
        } // ReplaceEachProjectGUIDWithProjectName

        /// <summary>
        /// Supplied with the full path and filename of the Solution, reads the Solution File contents from disk
        /// and replaces all the Solution GUIDs with Project Names
        /// </summary>
        /// <param name="solutionNameAndPath">The full path and filename of the Solution File</param>
        /// <param name="solutionFileContents">The contents of the Solution File exactly as read from disk</param>
        /// <param name="solutionFileContentsWithProjectNames">The Solution File contents with all the GUIDs replaced by Project Names</param>
        /// <returns>void</returns>
        private static void ReadSolutionContents(string solutionNameAndPath,
                                                 ref string solutionFileContents,
                                                 ref string solutionFileContentsWithProjectNames)
        {
            try
            {
                using (StreamReader sr = new StreamReader(solutionNameAndPath))
                {
                    solutionFileContents = sr.ReadToEnd();

                    string trunkProject1GUID = SolutionGUID(solutionFileContents);
                    SortedDictionary<string, string> projectDetails = FindAllProjects(solutionFileContents);
                    solutionFileContentsWithProjectNames =
                        ReplaceEachProjectGUIDWithProjectName(solutionFileContents, projectDetails);
                    solutionFileContentsWithProjectNames =
                        solutionFileContentsWithProjectNames.Replace(trunkProject1GUID, SolutionNameOf(solutionNameAndPath));
                }
            }
            catch (FileNotFoundException)
            {
                solutionFileContents = String.Empty;
                solutionFileContentsWithProjectNames = String.Empty;
                Console.WriteLine("File not found \"{0}\"", solutionNameAndPath);
            }
            catch (DirectoryNotFoundException)
            {
                string solutionFullNameAndPath = Path.GetFullPath(solutionNameAndPath);
                Console.WriteLine("A directory path component of \"{0}\" could not be found", solutionFullNameAndPath);
            }
            catch (Exception eek)
            {
                solutionFileContents = String.Empty;
                solutionFileContentsWithProjectNames = String.Empty;
                Console.WriteLine("Exception reading file \"{0}\" = \"{1}\"", solutionNameAndPath, eek);
            }

        } // ReadSolutionContents

        /// <summary>
        /// Output the Solution to the Console
        /// </summary>
        /// <param name="solutionPathAndName">The path and filename of the Solution</param>
        /// <param name="solutionContent">The Solution File contents</param>
        private static void DisplaySolution(string solutionPathAndName, string solutionContent)
        {
            Console.WriteLine();
            string title = String.Format("Solution \"{0}\"", solutionPathAndName);
            Console.WriteLine(title);
            Console.WriteLine(new string('~', title.Length));
            Console.WriteLine();
            Console.WriteLine(solutionContent);
        }

        public static void Compare(string solutionPathAndName,
                                     string location1,
                                     string location2,
                                     ref bool exactlyEquivalent,
                                     ref bool semanticallyEquivalent)
        {
            exactlyEquivalent = false;
            semanticallyEquivalent = false;

            solutionPathAndName = ChangeAllForwardSlashesToBackSlashes(solutionPathAndName);
            string solution1PathAndName = CaseSensitivePathOfFile(Path.Combine(ChangeAllForwardSlashesToBackSlashes(location1), solutionPathAndName));
            string solution2PathAndName = CaseSensitivePathOfFile(Path.Combine(ChangeAllForwardSlashesToBackSlashes(location1), solutionPathAndName));

            string solutionFile1Contents = "", solutionFile1ContentsWithProjectNames = "";
            ReadSolutionContents(solution1PathAndName, ref solutionFile1Contents, ref solutionFile1ContentsWithProjectNames);

            string solutionFile2Contents = "", solutionFile2ContentsWithProjectNames = "";
            ReadSolutionContents(solution2PathAndName, ref solutionFile2Contents, ref solutionFile2ContentsWithProjectNames);

            if ((solutionFile1Contents.Length == 0)
                || (solutionFile1ContentsWithProjectNames.Length == 0)
                || (solutionFile2Contents.Length == 0)
                || (solutionFile2ContentsWithProjectNames.Length == 0)
               )
            {
                Console.WriteLine();
                Console.WriteLine("The comparison could not be performed because:");
                if (solutionFile1ContentsWithProjectNames.Length == 0)
                {
                    Console.WriteLine("    Solution File \"{0}\" could not be found", solution1PathAndName);
                }
                if (solutionFile2ContentsWithProjectNames.Length == 0)
                {
                    Console.WriteLine("    Solution File \"{0}\" could not be found", solution2PathAndName);
                }
            }
            else
            {
                // Got both file contents

                if (String.Compare(solutionFile1Contents, solutionFile2Contents) == 0)
                {
                    // If exactly equivalent then must be semantically equivalent
                    exactlyEquivalent = true;
                    semanticallyEquivalent = true;
                }
                else if (String.Compare(solutionFile1ContentsWithProjectNames, solutionFile2ContentsWithProjectNames) == 0)
                {
                    // Solutions are semantically equivalent
                    semanticallyEquivalent = true;
                }

            } // Got both file contents

        } // Compare

    } // CompareSolutions
}
