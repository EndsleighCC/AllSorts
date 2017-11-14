using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace TestProjectReferences
{
    class Program
    {

        public class ProjectReferenceDetails
        {
            public ProjectReferenceDetails(string projectFilename)
            {
                ProjectFilename = projectFilename;
                AnalyseProjectReferences(projectFilename);
            }

            public string ProjectFilename { get; private set; }

            public class BinaryReference : IComparable<BinaryReference>
            {
                public BinaryReference(string name)
                {
                    Name = name;
                }

                public int CompareTo(BinaryReference other)
                {
                    // return String.Compare(Name, other.Name, StringComparison.CurrentCultureIgnoreCase);
                    return String.Compare(AssemblyName, other.AssemblyName, StringComparison.CurrentCultureIgnoreCase);
                }

                public string Name { get; private set; }
                public string AssemblyName
                {
                    get
                    {
                        string[] nameParts = Name.Split(new char[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);
                        return nameParts[0];
                    }
                }
            }

            public class BinaryReferenceSortedSet : SortedSet<BinaryReference>
            {
                public BinaryReferenceSortedSet() : base()
                {
                }
            }

            public BinaryReferenceSortedSet BinaryReferenceCollection = new BinaryReferenceSortedSet();

            public class ProjectReference : IComparable<ProjectReference>
            {
                public ProjectReference( string name )
                {
                    Name = name;
                }

                public string Name { get; set; }

                public string AssemblyName
                {
                    get
                    {
                        string[] nameParts = Name.Split(new char[] {'\\'}, StringSplitOptions.RemoveEmptyEntries);
                        string assemblyFilename = nameParts[nameParts.Length - 1];
                        string assemblyName = assemblyFilename.Substring(0, assemblyFilename.LastIndexOf('.'));
                        return assemblyName;
                    }
                }

                public int CompareTo(ProjectReference other)
                {
                    // return String.Compare(Name, other.Name, StringComparison.CurrentCultureIgnoreCase );
                    return String.Compare(AssemblyName, other.AssemblyName, StringComparison.CurrentCultureIgnoreCase);
                }
            }

            public class ProjectReferenceSortedSet : SortedSet<ProjectReference>
            {
                public ProjectReferenceSortedSet()
                    : base()
                {
                }
            }

            public ProjectReferenceSortedSet ProjectReferenceCollection = new ProjectReferenceSortedSet();

            public List<string> SourceFileList = new List<string>();

            private void AnalyseProjectReferences(string projectFilename)
            {

                if (!File.Exists(projectFilename))
                {
                    Console.WriteLine();
                    Console.WriteLine("Project \"{0}\" does not exist", projectFilename);
                }
                else
                {
                    FileInfo projectFileInfo = new FileInfo(projectFilename);

                    switch (projectFileInfo.Extension)
                    {
                        case ".csproj":
                        case ".vbproj":
                            // Supported Project file

                            XDocument projectDocument = XDocument.Load(projectFilename);

                            XNamespace projectFileNamespace =
                                XNamespace.Get("http://schemas.microsoft.com/developer/msbuild/2003");

                            //References "By DLL (file)"
                            var binaryReferences =
                                from list in projectDocument.Descendants(projectFileNamespace + "ItemGroup")
                                from item in list.Elements(projectFileNamespace + "Reference")
                                /* where item.Element(projectFileNamespace + "HintPath") != null */
                                select new
                                           {
                                               CsProjFileName = projectFilename,
                                               ReferenceInclude = item.Attribute("Include").Value,
                                               RefType =
                                    (item.Element(projectFileNamespace + "HintPath") == null)
                                        ? "CompiledDLLInGac"
                                        : "CompiledDLL",
                                               HintPath =
                                    (item.Element(projectFileNamespace + "HintPath") == null)
                                        ? string.Empty
                                        : item.Element(projectFileNamespace + "HintPath").Value
                                           };

                            foreach (var v in binaryReferences)
                            {
                                // Console.WriteLine(v.ToString());
                                // Console.WriteLine("    {0}", v.ReferenceInclude);
                                BinaryReferenceCollection.Add(new BinaryReference(v.ReferenceInclude));
                            }

                            //References "By Project"
                            var projectReferences =
                                from list in projectDocument.Descendants(projectFileNamespace + "ItemGroup")
                                from item in list.Elements(projectFileNamespace + "ProjectReference")
                                where
                                    item.Element(projectFileNamespace + "Project") != null
                                select new
                                           {
                                               CsProjFileName = projectFilename,
                                               ReferenceInclude = item.Attribute("Include").Value,
                                               RefType = "ProjectReference",
                                               ProjectGuid = item.Element(projectFileNamespace + "Project").Value,
                                               ProjectName = item.Element(projectFileNamespace + "Name").Value
                                           };

                            foreach (var v in projectReferences)
                            {
                                // Console.WriteLine(v.ToString());
                                // Console.WriteLine("    {0} = \"{1}\"", v.ProjectName, v.ReferenceInclude);
                                ProjectReferenceCollection.Add(new ProjectReference(v.ReferenceInclude));
                            }

                            // List of source files
                            var sourceFileList =
                                from list in projectDocument.Descendants(projectFileNamespace + "ItemGroup")
                                from item in list.Elements(projectFileNamespace + "Compile")
                                select new
                                           {
                                               FileName = item.Attribute("Include").Value,
                                           };

                            foreach (var v in sourceFileList)
                            {
                                // Console.WriteLine("Source \"{0}\"",v.FileName);
                                SourceFileList.Add(v.FileName);
                            }

                            break; // Supported Project file
                        default :
                            Console.WriteLine();
                            Console.WriteLine("Project File \"{0}\" is an unsupported type \"{1}\"",
                                                projectFilename,projectFileInfo.Extension.Substring(1));
                            break;
                    }
                }
            }

            public void Display(int indentCount)
            {
                string indent = new string(' ',indentCount);
                Console.WriteLine();
                Console.WriteLine("Project \"{0}\"", ProjectFilename);
                Console.WriteLine("{0}Binary References:", indent);
                foreach (BinaryReference binaryReference in BinaryReferenceCollection)
                {
                    Console.WriteLine("{0}{1}{2}", indent, indent, binaryReference.Name);
                }

                Console.WriteLine("{0}Project References:", indent);
                foreach (ProjectReference projectReference in ProjectReferenceCollection)
                {
                    Console.WriteLine("{0}{1}{2}", indent, indent, projectReference.Name);
                }
            }
        }

        class ProjectDifferenceAnalysis
        {
            public ProjectDifferenceAnalysis(string projectFilename, string firstRoot, string secondRoot,
                                                int singleIndentCount)
            {
                string singleIndent = new String(' ', singleIndentCount);

                projectFilename = projectFilename.Trim('\\');

                firstRoot = (firstRoot.TrimEnd('\\') + "\\").ToUpper();
                string firstProjectFilename = Path.Combine(firstRoot, projectFilename);
                ProjectReferenceDetails projectReferenceDetails1 = new ProjectReferenceDetails(firstProjectFilename);
                projectReferenceDetails1.Display(singleIndentCount);

                if (secondRoot != null)
                {
                    // There is a second Project
                    secondRoot = (secondRoot.TrimEnd('\\') + "\\").ToUpper();
                    string secondProjectFilename = Path.Combine(secondRoot, projectFilename);
                    ProjectReferenceDetails projectReferenceDetails2 = new ProjectReferenceDetails(secondProjectFilename);
                    projectReferenceDetails2.Display(singleIndentCount);

                    Console.WriteLine();
                    Console.WriteLine("Project Differences");

                    Console.WriteLine();
                    Console.WriteLine("{0}Binary References: \"{1}\"", singleIndent, projectFilename);

                    Console.WriteLine("{0}In {1} but not in {2}", singleIndent, firstRoot, secondRoot);
                    int referenceInFirstButNotInSecondCount = 0;
                    foreach (
                        ProjectReferenceDetails.BinaryReference binaryReference in
                            projectReferenceDetails1.BinaryReferenceCollection)
                    {
                        if (!projectReferenceDetails2.BinaryReferenceCollection.Contains(binaryReference))
                        {
                            Console.WriteLine("{0}{1}{2} = \"{3}\"", singleIndent, singleIndent,
                                              binaryReference.AssemblyName, binaryReference.Name);
                            referenceInFirstButNotInSecondCount += 1;
                        }
                    }
                    if (referenceInFirstButNotInSecondCount==0)
                        Console.WriteLine("{0}{1}None",singleIndent,singleIndent);

                    // Work out the differences
                    Console.WriteLine("{0}In {1} but not in {2}", singleIndent, secondRoot, firstRoot);
                    int referenceInSecondButNotInFirstCount = 0;
                    foreach (
                        ProjectReferenceDetails.BinaryReference binaryReference in
                            projectReferenceDetails2.BinaryReferenceCollection)
                    {
                        if (!projectReferenceDetails1.BinaryReferenceCollection.Contains(binaryReference))
                        {
                            Console.WriteLine("{0}{1}{2} = \"{3}\"", singleIndent, singleIndent,
                                              binaryReference.AssemblyName, binaryReference.Name);
                            referenceInSecondButNotInFirstCount += 1;
                        }
                    }
                    if (referenceInSecondButNotInFirstCount == 0)
                        Console.WriteLine("{0}{1}None", singleIndent, singleIndent);

                    Console.WriteLine();
                    Console.WriteLine("{0}Project References: \"{1}\"", singleIndent, projectFilename);

                    Console.WriteLine("{0}In {1} but not in {2}", singleIndent, firstRoot, secondRoot);
                    referenceInFirstButNotInSecondCount = 0;
                    foreach (
                        ProjectReferenceDetails.ProjectReference projectReference in
                            projectReferenceDetails1.ProjectReferenceCollection)
                    {
                        if (!projectReferenceDetails2.ProjectReferenceCollection.Contains(projectReference))
                        {
                            Console.WriteLine("{0}{1}{2} = \"{3}\"", singleIndent, singleIndent,
                                              projectReference.AssemblyName, projectReference.Name);
                        }
                    }
                    if (referenceInFirstButNotInSecondCount == 0)
                        Console.WriteLine("{0}{1}None", singleIndent, singleIndent);

                    // Work out the reference differences
                    Console.WriteLine("{0}In {1} but not in {2}", singleIndent, secondRoot, firstRoot);
                    referenceInSecondButNotInFirstCount = 0;
                    foreach (
                        ProjectReferenceDetails.ProjectReference projectReference in
                            projectReferenceDetails2.ProjectReferenceCollection)
                    {
                        if (!projectReferenceDetails1.ProjectReferenceCollection.Contains(projectReference))
                        {
                            Console.WriteLine("{0}{1}{2} = \"{3}\"", singleIndent, singleIndent,
                                              projectReference.AssemblyName, projectReference.Name);
                        }
                    }
                    if (referenceInSecondButNotInFirstCount == 0)
                        Console.WriteLine("{0}{1}None", singleIndent, singleIndent);

                    Console.WriteLine();
                    Console.WriteLine("{0}Missing Project References: \"{1}\"", singleIndent, projectFilename);

                    // Check that all the Binary References in Second exist as either Project or Binary References in First

                    Collection<string> outputLineCollection = new Collection<string>();
                    foreach (
                        ProjectReferenceDetails.BinaryReference binaryReference in
                            projectReferenceDetails2.BinaryReferenceCollection)
                    {
                        bool found = false;
                        // Look for this Binary Reference from Second

                        // Check in the Project References of First
                        for (int index = 0;
                             (index < projectReferenceDetails1.ProjectReferenceCollection.Count) && (!found);
                             ++index)
                        {
                            string projectReferenceAssemblyName1 =
                                projectReferenceDetails1.ProjectReferenceCollection.ElementAt(index).AssemblyName;
                            found = String.Compare(projectReferenceAssemblyName1, binaryReference.AssemblyName, true) == 0;
                        }

                        if (!found)
                        {
                            // Binary Reference of Second not found in Project References of First

                            // Check that the Binary Reference for Second is in the Binary Reference for First
                            found = projectReferenceDetails1.BinaryReferenceCollection.Contains(binaryReference);

                        } // Binary Reference not found in Project References

                        if (!found)
                        {
                            outputLineCollection.Add(
                                String.Format("{0}{1}--- References in {2} do not contain {3} = \"{4}\"",
                                              singleIndent, singleIndent, firstProjectFilename,
                                              binaryReference.AssemblyName, binaryReference.Name));
                        }
                    }

                    if (outputLineCollection.Count == 0)
                    {
                        Console.WriteLine("{0}{1}The Project and Binary References in", singleIndent, singleIndent);
                        Console.WriteLine("{0}{1}{2}{3}", singleIndent, singleIndent, singleIndent, firstRoot);
                        Console.WriteLine("{0}{1}contain ALL the Binary References in", singleIndent, singleIndent);
                        Console.WriteLine("{0}{1}{2}{3}", singleIndent, singleIndent, singleIndent, secondRoot);
                    }
                    else
                    {
                        foreach (string outputLine in outputLineCollection)
                        {
                            Console.WriteLine(outputLine);
                        }
                    }

                    outputLineCollection.Clear();

                    Collection<string> fileInFirstButNotInSecond = new Collection<string>();

                    Console.WriteLine();
                    Console.WriteLine("{0}Sources: \"{1}\"", singleIndent, projectFilename);
                    Console.WriteLine("{0}In {1} but not in {2}", singleIndent, firstRoot, secondRoot);

                    foreach (string sourceName in projectReferenceDetails1.SourceFileList)
                    {
                        string thisSourceName =
                            projectReferenceDetails2.SourceFileList.Find(s => String.Compare(s, sourceName, true) == 0);
                        if (thisSourceName == null)
                        {
                            outputLineCollection.Add(
                                String.Format("{0}{1}Source File {2} in {3} is not in {4}",
                                              singleIndent, singleIndent, sourceName, firstRoot, secondRoot));
                            fileInFirstButNotInSecond.Add(sourceName);
                        }
                    }

                    if (outputLineCollection.Count == 0)
                    {
                        Console.WriteLine("{0}{1}All Source Files in {2} are in {3}",
                                          singleIndent, singleIndent, firstRoot, secondRoot);
                        Console.WriteLine("{0}{1}Sources in {2}", singleIndent, singleIndent, firstProjectFilename);
                        foreach (string sourceName in projectReferenceDetails1.SourceFileList)
                        {
                            Console.WriteLine("{0}{1}{2}{3}", singleIndent, singleIndent, singleIndent, sourceName);
                        }
                    }
                    else
                    {
                        foreach (string outputLine in outputLineCollection)
                        {
                            Console.WriteLine(outputLine);
                        }
                    }

                    outputLineCollection.Clear();

                    Collection<string> fileInSecondButNotInFirst = new Collection<string>();

                    Console.WriteLine();
                    Console.WriteLine("{0}In {1} but not in {2}", singleIndent, secondRoot, firstRoot);

                    foreach (string sourceName in projectReferenceDetails2.SourceFileList)
                    {
                        string thisSourceName =
                            projectReferenceDetails1.SourceFileList.Find(s => String.Compare(s, sourceName, true) == 0);
                        if (thisSourceName == null)
                        {
                            outputLineCollection.Add(
                                String.Format("{0}{1}Source File {2} in {3} is not in {4}",
                                              singleIndent, singleIndent, sourceName, secondRoot, firstRoot));
                            fileInSecondButNotInFirst.Add(sourceName);
                        }
                    }

                    if (outputLineCollection.Count == 0)
                    {
                        Console.WriteLine("{0}{1}All Source Files in {2} are in {3}",
                                          singleIndent, singleIndent, secondRoot, firstRoot);
                        Console.WriteLine("{0}{1}Sources in {2}", singleIndent, singleIndent, secondProjectFilename);
                        foreach (string sourceName in projectReferenceDetails2.SourceFileList)
                        {
                            Console.WriteLine("{0}{1}{2}{3}", singleIndent, singleIndent, singleIndent, sourceName);
                        }
                    }
                    else
                    {
                        foreach (string outputLine in outputLineCollection)
                        {
                            Console.WriteLine(outputLine);
                        }
                    }

                    if ((fileInFirstButNotInSecond.Count == 0) && (fileInSecondButNotInFirst.Count == 0))
                    {
                        Console.WriteLine();
                        Console.WriteLine("{0}The sources in", singleIndent);
                        Console.WriteLine("{0}{1}{2}", singleIndent, singleIndent, firstRoot);
                        Console.WriteLine("{0}match the sources in", singleIndent);
                        Console.WriteLine("{0}{1}{2}", singleIndent, singleIndent, secondRoot);
                    }
                    else
                    {
                        if (fileInFirstButNotInSecond.Count > 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine("{0}--- Sources in {1} but NOT in {2}",singleIndent,firstRoot,secondRoot);
                            foreach (string filename in fileInFirstButNotInSecond)
                            {
                                Console.WriteLine("{0}{1}\"{2}\"",singleIndent,singleIndent,filename);
                            }
                        }
                    }
                    if (fileInSecondButNotInFirst.Count > 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("{0}--- Sources in {1} but NOT in {2}", singleIndent, secondRoot, firstRoot);
                        foreach (string filename in fileInSecondButNotInFirst)
                        {
                            Console.WriteLine("{0}{1}\"{2}\"", singleIndent, singleIndent, filename);
                        }
                    }
                } // There is a second Project
            }
        }

        static void Main(string[] args)
        {
            const int singleIndentCount = 4;

            if ( args.Length>=2)
            {
                // Sufficient arguments
                string filename = args[0];
                string firstRoot = args[1].Trim('\\');
                string secondRoot = null;

                if (args.Length >= 3)
                {
                    // Second Project
                    secondRoot = args[2].Trim('\\');
                }

                string fileExtension = "unknown";
                int lastDotIndex = filename.LastIndexOf('.');
                if ( lastDotIndex >= 0 )
                    fileExtension = filename.Substring( lastDotIndex );

                switch (fileExtension.ToLower())
                {
                    case ".csproj":
                    case ".vbproj":
                        {
                            // Project filenanme supplied
                            string projectFilename = filename;

                            ProjectDifferenceAnalysis projectDifferenceAnalysis =
                                new ProjectDifferenceAnalysis(projectFilename, firstRoot, secondRoot, singleIndentCount);

                        } // Project filenanme supplied
                        break;
                    default:
                        {
                            // Assume a Contention Report was supplied

                            using (StreamReader fileStream = new StreamReader(filename))
                            {
                                while (!fileStream.EndOfStream)
                                {
                                    string fileline = fileStream.ReadLine();

                                    string projectFilename;

                                    string[] filelinePart = fileline.Split(new char[] {' '},
                                                                           StringSplitOptions.RemoveEmptyEntries);
                                    int archiveEntry;
                                    if ((filelinePart.Length >= 5) &&
                                        (int.TryParse(filelinePart[0], out archiveEntry) &&
                                         (String.Compare(filelinePart[1], ":") == 0)))
                                    {
                                        // A contention entry

                                        if (filelinePart[2] != "\"")
                                        {
                                            // No spaces in the filename
                                            projectFilename = filelinePart[2];

                                        } // Spaces in the filename
                                        else
                                        {
                                            // No spaces in the filename

                                            int beginDoubleQuoteIndex = fileline.IndexOf('"', 0);
                                            int endDoubleQuoteIndex = fileline.IndexOf('"', beginDoubleQuoteIndex + 1);

                                            projectFilename = fileline.Substring(1, endDoubleQuoteIndex - 1);

                                        } // No spaces in the filename

                                        string thisFileExtension = "unknown";
                                        int thisLastDotIndex = projectFilename.LastIndexOf('.');
                                        if (thisLastDotIndex >= 0)
                                            thisFileExtension = projectFilename.Substring(thisLastDotIndex).ToLower();

                                        switch (thisFileExtension)
                                        {
                                            case ".csproj":
                                            case ".vbproj":
                                                ProjectDifferenceAnalysis projectDifferenceAnalysis =
                                                    new ProjectDifferenceAnalysis(projectFilename, firstRoot, secondRoot,
                                                                                  singleIndentCount);
                                                break;
                                            default:
                                                Console.WriteLine();
                                                Console.WriteLine("Contention Report File \"{0}\" is not a Visual Studio Project File", projectFilename);
                                                break;
                                        }

                                    } // A contention entry
                                }
                            }

                        } // Assume a Contention Report was supplied
                        break;
                } // switch
            } // Sufficient arguments
        } // Main
    } // Program
} // namespace
