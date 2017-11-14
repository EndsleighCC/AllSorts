using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TmsSectorDeduplicate
{
    class Program
    {
        static void Main(string[] args)
        {
            bool deduplicate = false;
            string workingDirectory = null;

            for ( int argIndex = 0; argIndex < args.Length; ++argIndex )
            {
                string thisArg = args[argIndex];

                if ( thisArg[0] == '/')
                {
                    switch (thisArg.Substring(1).ToLower())
                    {
                        case "deduplicate":
                            deduplicate = true;
                            Console.WriteLine("De-duplication will be performed");
                            break;
                        default:
                            break;
                    } // switch
                }
                else
                {
                    // An argument

                    if ( workingDirectory == null )
                    {
                        workingDirectory = Path.GetFullPath(thisArg);
                        Console.WriteLine("Working directory is \"{0}\"",workingDirectory);
                    }
                }

            }

            if ( ! deduplicate)
            {
                Console.WriteLine("De-duplication will NOT be performed");
            }

            if ( workingDirectory == null )
            {
                workingDirectory = Directory.GetCurrentDirectory();
                Console.WriteLine("Working directory is \"{0}\"", workingDirectory);
            }

            Chassis chassis = new Chassis(workingDirectory, workingDirectory, "Household");

            SectorContainer sectorContainer = new SectorContainer(chassis);

            bool sectorInformationFileFound = false;
            try
            {
                sectorContainer.Read();
                sectorInformationFileFound = true;
            }
            catch (FileNotFoundException)
            {
                sectorInformationFileFound = false;
                Console.WriteLine();
                Console.WriteLine("The Sector Information File was not found in \"{0}\"", workingDirectory);
            }
            catch (Exception ex)
            {
                sectorInformationFileFound = false;
                Console.WriteLine();
                Console.WriteLine("An exception occurred = {0}", ex.ToString());
            }

            if ( sectorInformationFileFound )
            {
                // Sector Information File is available

                Console.WriteLine("Sector Container has {0} entries", sectorContainer.Count);

                SortedDictionary<int, SectorInformation> sectorSortedDictionary = new SortedDictionary<int, SectorInformation>();

                Console.WriteLine();
                foreach (SectorInformation sectorInformation in sectorContainer)
                {
                    try
                    {
                        sectorSortedDictionary.Add(sectorInformation.Number, sectorInformation);
                        Console.WriteLine("Added Sector Number {0}", sectorInformation.Number);
                        Console.WriteLine("    Number \"{0}\"", sectorInformation.Number);
                        Console.WriteLine("    Name \"{0}\"", sectorInformation.Name);
                        Console.WriteLine("    Notes \"{0}\"", sectorInformation.Notes);
                        Console.WriteLine("    Description \"{0}\"", sectorInformation.SectorDescription);
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine("Duplicate Sector Number {0}", sectorInformation.Number);
                        SectorInformation dictionarySectorInformation = sectorSortedDictionary[sectorInformation.Number];
                        if (String.Compare(sectorInformation.Name, dictionarySectorInformation.Name) != 0)
                        {
                            Console.WriteLine("    Sector Name \"{0}\" is different from dictionary \"{1}\"",
                                sectorInformation.Name, dictionarySectorInformation.Name);
                        }
                        if (String.Compare(sectorInformation.Notes, dictionarySectorInformation.Notes) != 0)
                        {
                            Console.WriteLine("    Sector Notes \"{0}\" is different from dictionary \"{1}\"",
                                sectorInformation.Notes, dictionarySectorInformation.Notes);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unexpected exception adding Sector Number {0} to dictionary = {1}",
                                            sectorInformation.Number, ex.ToString());
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Sector Container has {0} entries and {1} are unique",
                                    sectorContainer.Count, sectorSortedDictionary.Count);

                if (sectorContainer.Count == sectorSortedDictionary.Count)
                {
                    Console.WriteLine();
                    Console.WriteLine("Sector Container does not need de-duplication");
                }
                else
                {
                    // De-duplication was selected

                    if (!deduplicate)
                    {
                        Console.WriteLine("De-duplication is required but was not selected");
                    }
                    else
                    {
                        // De-duplicate the Sector Container

                        Console.WriteLine();
                        Console.WriteLine("De-duplication begins");

                        sectorContainer.Clear();
                        // Console.WriteLine("Sector Container now has {0} entries", sectorContainer.Count);

                        foreach (int sectorNumber in sectorSortedDictionary.Keys)
                        {
                            SectorInformation dictionarySectorInformation = sectorSortedDictionary[sectorNumber];
                            sectorContainer.Add(dictionarySectorInformation);
                        }
                        Console.WriteLine("De-duplicated Sector Container has {0} entries", sectorContainer.Count);

                        Console.WriteLine("Writing new Sector Container with {0} entries", sectorContainer.Count);
                        try
                        {
                            sectorContainer.Write();
                            Console.WriteLine("New Sector Container with {0} entries has been written", sectorContainer.Count);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception writing Sector Container = {0}", ex.ToString());
                        }

                        Console.WriteLine();
                        Console.WriteLine("De-duplication ends");

                    } // De-duplicate the Sector Container

                } // De-duplication was selected

            } // Sector Information File is available

        }
    }
}
