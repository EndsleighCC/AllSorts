using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TestCompleteSolutions
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
                        string[] nameParts = Name.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        return nameParts[0];
                    }
                }
            }

            public class BinaryReferenceSortedSet : SortedSet<BinaryReference>
            {
                public BinaryReferenceSortedSet()
                    : base()
                {
                }
            }

            public BinaryReferenceSortedSet BinaryReferenceCollection = new BinaryReferenceSortedSet();

            public class ProjectReference : IComparable<ProjectReference>
            {
                public ProjectReference(string name)
                {
                    Name = name;
                }

                public string Name { get; set; }

                public string AssemblyName
                {
                    get
                    {
                        string[] nameParts = Name.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
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
                    XDocument projectDocument = XDocument.Load(projectFilename);

                    XNamespace projectFileNamespace = XNamespace.Get("http://schemas.microsoft.com/developer/msbuild/2003");

                    //References "By DLL (file)"
                    var binaryReferences = from list in projectDocument.Descendants(projectFileNamespace + "ItemGroup")
                                           from item in list.Elements(projectFileNamespace + "Reference")
                                           /* where item.Element(projectFileNamespace + "HintPath") != null */
                                           select new
                                           {
                                               CsProjFileName = projectFilename,
                                               ReferenceInclude = item.Attribute("Include").Value,
                                               RefType = (item.Element(projectFileNamespace + "HintPath") == null) ? "CompiledDLLInGac" : "CompiledDLL",
                                               HintPath = (item.Element(projectFileNamespace + "HintPath") == null) ? string.Empty : item.Element(projectFileNamespace + "HintPath").Value
                                           };

                    foreach (var v in binaryReferences)
                    {
                        // Console.WriteLine(v.ToString());
                        // Console.WriteLine("    {0}", v.ReferenceInclude);
                        BinaryReferenceCollection.Add(new BinaryReference(v.ReferenceInclude));
                    }

                    //References "By Project"
                    var projectReferences = from list in projectDocument.Descendants(projectFileNamespace + "ItemGroup")
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
                    var sourceFileList = from list in projectDocument.Descendants(projectFileNamespace + "ItemGroup")
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

                }
            }

            public void Display(int indentCount)
            {
                string indent = new string(' ', indentCount);
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

        public class ProjectReferenceCollection : Collection<ProjectReferenceDetails>
        {
            public ProjectReferenceCollection( string filename) : base(String.Comparer)
            {
                
            }
        }

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string filename = args[0];


            }
        }
    }
}
