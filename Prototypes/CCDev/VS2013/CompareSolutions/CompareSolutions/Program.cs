using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Xml.Serialization;

namespace CompareSolutions
{
    class Program
    {
        static string ChangeAllForwardSlashesToBackSlashes(string filename)
        {
            return filename.Replace("/", "\\");
        } // ChangeAllForwardSlashesToBackSlashes

        static string RemoveAllGUIDs(string str)
        {
            StringBuilder sb = new StringBuilder("",str.Length);

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

        private static SortedDictionary<string /* GUID */ , string /* Solution Name */ > FindAllProjects(string solutionFile)
        {
            SortedDictionary<string,string> projectDetails = new SortedDictionary<string, string>();
            int currentIndex = solutionFile.IndexOf(_projectToken);
            while (currentIndex != -1)
            {
                int equalSignIndex = solutionFile.IndexOf("=", currentIndex);
                if (equalSignIndex >= 0)
                {
                    int doubleQuote1Index = solutionFile.IndexOf("\"", equalSignIndex+1);
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
                                int comma2Index = solutionFile.IndexOf(",", comma1Index+1);
                                if (comma2Index >= 0)
                                {
                                    doubleQuote1Index = solutionFile.IndexOf("\"", comma2Index);
                                    if (doubleQuote1Index >= 0)
                                    {
                                        doubleQuote2Index = solutionFile.IndexOf("\"", doubleQuote1Index+1);
                                        if (doubleQuote2Index >= 0)
                                        {
                                            string projectGUID = solutionFile.Substring(doubleQuote1Index+2,
                                                doubleQuote2Index - doubleQuote1Index - 3);
                                            projectDetails.Add(projectGUID,projectName);
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

        private static string ReplaceEachProjectGUIDWithProjectName(string solutionFile,SortedDictionary<string,string> projectDetails )
        {
            string updatedSolutionFile = solutionFile;
            foreach (string projectGUID in projectDetails.Keys)
            {
                updatedSolutionFile = updatedSolutionFile.Replace(projectGUID, projectDetails[projectGUID]);
            }
            return updatedSolutionFile;
        } // ReplaceEachProjectGUIDWithProjectName

        private static string GenerateSolutionContentsWithProjectNames(string solutionNameAndPath)
        {
            string solutionFileContents = null;
            string solutionFileContentsWithProjectNames = null;
            try
            {
                using (StreamReader sr = new StreamReader(solutionNameAndPath))
                {
                    solutionFileContents = sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to read file \"{0}\"", solutionNameAndPath);
            }

            if (solutionFileContents != null)
            {
                // Got both file contents

                string trunkProject1GUID = SolutionGUID(solutionFileContents);
                SortedDictionary<string, string> project1Details = FindAllProjects(solutionFileContents);
                solutionFileContentsWithProjectNames =
                    ReplaceEachProjectGUIDWithProjectName(solutionFileContents, project1Details);
                solutionFileContentsWithProjectNames =
                    solutionFileContentsWithProjectNames.Replace(trunkProject1GUID, SolutionNameOf(solutionNameAndPath));
            }

            return solutionFileContentsWithProjectNames;
        } // GenerateSolutionContentsWithProjectNames

        private static void DisplaySolution(string solutionName, string solutionContent)
        {
            Console.WriteLine();
            string title = String.Format("Solution \"{0}\"", solutionName);
            Console.WriteLine(title);
            Console.WriteLine(new string('~',title.Length));
            Console.WriteLine();
            Console.WriteLine(solutionContent);
        }

        static void ShowUsage()
        {
            const string applicationTitle = "*** Compare Visual Studio Solutions Utility ***";

            const string pvcsSourceRevision = "$Revision:   0.0  $";
            const string pvcsCheckInDate = "$Date:   Jul 31 2015 11:01:06  $";

            string[] pvcsSourceRevisionPart = pvcsSourceRevision.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] pvcsCheckInDatePart = pvcsCheckInDate.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string version = String.Format("{0,-2}", pvcsSourceRevisionPart[1]);
            string date = String.Format("{0,2}-{1,3}-{2,4}",
                                    pvcsCheckInDatePart[2], pvcsCheckInDatePart[1], pvcsCheckInDatePart[3]);

            int spaceFileCount = applicationTitle.Length - version.Length - date.Length;

            string versiondatetime = String.Format("{0}{1}{2}", version, new string(' ', spaceFileCount), date);

            Console.WriteLine();
            Console.WriteLine(applicationTitle);
            Console.WriteLine(versiondatetime);
            Console.WriteLine();
            Console.WriteLine("Usage: slncomp {/c} RelativeSolutionPath RootPath1 RootPath2");
            Console.WriteLine();
            Console.WriteLine("Where:");
            Console.WriteLine("    c = Display the contents of the Solution Files once Project GUIDs have been replaced by Project Names");
            Console.WriteLine();
            Console.WriteLine("Description:");
            Console.WriteLine("    Read the supplied solution files and, once all the Project GUIDs have been replaced with Project Names,");
            Console.WriteLine("    perform a character by character difference.");
            Console.WriteLine("    If necessary, forward slashes present in the RelativeSolutionPath will be replaced with backslashes.");
            Console.WriteLine("    If the contents of the Solutions once they have the Project GUIDs replaced by Project Names are the same,");
            Console.WriteLine("    this application will return zero otherwise it will return non-zero i.e. 1");

        } // ShowUsage

        static int Main(string[] args)
        {
            int error = 1;

            string solutionNameAndPath = null;
            string solution1NameAndPath = null;
            string solution2NameAndPath = null;

            bool displaySolutionContents = false;

            for (int argId = 0; argId < args.Count(); ++argId)
            {
                if (args[argId][0] == '/')
                {
                    // A switch

                    switch (args[argId].ToLower())
                    {
                        case "c" :
                            displaySolutionContents = true;
                            Console.WriteLine("Solution contents will be displayed");
                            break;
                        default :
                            Console.WriteLine("Unknown switch \"{0}\"",args[argId]);
                            break;
                    } // switch

                } // A switch
                else
                {
                    if (solutionNameAndPath == null)
                    {
                        solutionNameAndPath = ChangeAllForwardSlashesToBackSlashes(args[0]);
                    }
                    else if (solution1NameAndPath == null)
                    {
                        solution1NameAndPath = Path.Combine(args[argId], solutionNameAndPath);
                    }
                    else if (solution2NameAndPath == null)
                    {
                        solution2NameAndPath = Path.Combine(args[argId], solutionNameAndPath);
                    }
                        
                }
            }

            if ((solutionNameAndPath == null) || (solution1NameAndPath == null) || (solution2NameAndPath == null))
            {
                ShowUsage();
            }
            else
            {
                // Sufficient arguments

                string solutionFile1ContentsWithProjectNames = GenerateSolutionContentsWithProjectNames(solution1NameAndPath);
                string solutionFile2ContentsWithProjectNames = GenerateSolutionContentsWithProjectNames(solution2NameAndPath);

                if ((solutionFile1ContentsWithProjectNames != null) && (solutionFile2ContentsWithProjectNames != null))
                {
                    // Got both file contents

                    if (String.Compare(solutionFile1ContentsWithProjectNames, solutionFile2ContentsWithProjectNames) ==0)
                    {
                        error = 0;
                        Console.WriteLine("Solution \"{0}\" and Solution \"{1}\" are equivalent",
                            solution1NameAndPath,solution2NameAndPath);
                        if (displaySolutionContents)
                        {
                            DisplaySolution(solution1NameAndPath, solutionFile1ContentsWithProjectNames);
                            DisplaySolution(solution2NameAndPath, solutionFile2ContentsWithProjectNames);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Solution \"{0}\" and Solution \"{1}\" are NOT equivalent", 
                            solution1NameAndPath,solution2NameAndPath);
                        DisplaySolution(solution1NameAndPath, solutionFile1ContentsWithProjectNames);
                        DisplaySolution(solution2NameAndPath, solutionFile2ContentsWithProjectNames);
                    }

                } // Got both file contents

            } // Sufficient arguments

            return error;
        }
    }
}
