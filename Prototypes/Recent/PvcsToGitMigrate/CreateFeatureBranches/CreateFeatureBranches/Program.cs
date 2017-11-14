using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CommandOperations;

namespace CreateFeatureBranches
{
    class Program
    {

        static int CreateAllFeatureBranches( string gitRepositoryRootPath ,
                                             string mainGitBranchName ,
                                             string pvcsRevisionDetailsPathAndFilename ,
                                             string pvcsSharePath )
        {
            int error = WindowsErrorDefinition.Success;

            using (StreamReader pvcsRevisionDetailsStream = new StreamReader(pvcsRevisionDetailsPathAndFilename))
            {
                string fileLine = null;
                int lineNumber = 0;
                PvcsRevisionDetailsReadState pvcsRevisionDetailsReadState = PvcsRevisionDetailsReadState.MustBeBranchName;
                PvcsRevisionDetails pvcsRevisionDetails = null;
                while ((error == WindowsErrorDefinition.Success) && (fileLine = pvcsRevisionDetailsStream.ReadLine()) != null)
                {
                    lineNumber += 1;

                    switch (pvcsRevisionDetailsReadState)
                    {
                        case PvcsRevisionDetailsReadState.MustBeBranchName:
                            try
                            {
                                Console.WriteLine();
                                Console.WriteLine("{0:000} : {1}", lineNumber, fileLine);
                                pvcsRevisionDetails = new PvcsRevisionDetails(fileLine);
                                pvcsRevisionDetailsReadState = PvcsRevisionDetailsReadState.MustBeDescription;
                            }
                            catch (PvcsRevisionDetails.PvcsRevisionDetailsUnknownGitBranchNameDeclarationException)
                            {
                                // A blank line or a comment
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("{0:000) , Exception creating PvcsRevisionDetails \"{1}\"",
                                    lineNumber, ex.ToString());
                            }
                            break;
                        case PvcsRevisionDetailsReadState.MustBeDescription:
                            try
                            {
                                pvcsRevisionDetails.SetDescription(fileLine);
                                pvcsRevisionDetailsReadState = PvcsRevisionDetailsReadState.MustBeRevisionDetails;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("{0:000) : Exception setting Git Branch Name Description \"{1}\"",
                                        fileLine, ex.ToString());
                            }
                            break;
                        case PvcsRevisionDetailsReadState.MustBeRevisionDetails:
                            try
                            {
                                if (fileLine.StartsWith(PvcsRevisionDetails.PvcsGitBranchNameDeclare))
                                {
                                    if (pvcsRevisionDetails.GitMainBranchName == mainGitBranchName)
                                    {
                                        error = pvcsRevisionDetails.CreateFeatureBranch(gitRepositoryRootPath, pvcsSharePath);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Skipping unrelated feature branch \"{0}\"", pvcsRevisionDetails.GitFullFeatureBranchName);
                                    }
                                    if (error == WindowsErrorDefinition.Success)
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine("{0:000} : {1}", lineNumber, fileLine);
                                        pvcsRevisionDetails = new PvcsRevisionDetails(fileLine);
                                        pvcsRevisionDetailsReadState = PvcsRevisionDetailsReadState.MustBeDescription;
                                    }
                                }
                                else if (!(String.IsNullOrEmpty(fileLine) || String.IsNullOrWhiteSpace(fileLine) || fileLine.StartsWith("#")))
                                {
                                    // Not empty or a comment
                                    Console.WriteLine("{0:000} : {1}", lineNumber, fileLine);
                                    pvcsRevisionDetails.PvcsRevisionDetailsLineSortedSet.Add(fileLine);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("PvcsRevisionDetailsReadState.MustBeRevisionDetails : Exception {0}", ex.ToString());
                            }
                            break;
                        default:
                            Console.WriteLine("Unexpected Revision Details Read State \"{0}\"",
                                Enum.GetName(typeof(PvcsRevisionDetailsReadState), pvcsRevisionDetailsReadState));
                            break;
                    } // switch
                } // while

                if (error == WindowsErrorDefinition.Success)
                {
                    // Ensure that any pending commit is attempted
                    error = pvcsRevisionDetails.CreateFeatureBranch(gitRepositoryRootPath, pvcsSharePath);
                }

                if (error == WindowsErrorDefinition.Success)
                {
                    Console.WriteLine();
                    Console.WriteLine("All feature branches were created successfully");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Feature branches were created with error {0}. Last line read was {1}", error, lineNumber);
                }

            } // using

            return error;
        } // CreateAllFeatureBranches

        static int Main(string[] args)
        {
            int error = 0;

            if ( args.Length < 4 )
            {
                Console.WriteLine("CreateFeatureBranches GitRepositoryRootPath MainGitBranchName PvcsRevisionDetailsPathAndFilename PvcsSharePath");
            }
            else
            {
                string gitRepositoryRootPath = args[0].Trim( new char[] { ' ', '\\' } );
                string mainGitBranchName = args[1].Trim(new char[] { ' ', '\\' });
                string pvcsRevisionDetailsPathAndFilename = args[2].Trim(new char[] { ' ', '\\' });
                string pvcsSharePath = args[3].Trim(new char[] { ' ', '\\' });

                if (!Directory.Exists(gitRepositoryRootPath))
                {
                    Console.WriteLine("Repository Directory \"{0}\" does not exist", gitRepositoryRootPath);
                    error = WindowsErrorDefinition.BadUnit;
                }
                else
                {
                    if ( ! File.Exists(pvcsRevisionDetailsPathAndFilename) )
                    {
                        Console.WriteLine("PVCS Revision Details file \"{0}\" does not exist", pvcsRevisionDetailsPathAndFilename);
                        error = WindowsErrorDefinition.FileNotFound;
                    }
                    else
                    {
                        if (!Directory.Exists(pvcsSharePath))
                        {
                            Console.WriteLine("PVCS Share \"{0}\" does not exist", pvcsSharePath);
                            error = WindowsErrorDefinition.BadUnit;
                        }
                        else
                        {
                            Console.WriteLine("Git Repository Directory is \"{0}\"", gitRepositoryRootPath);
                            Console.WriteLine("Main Git Branch Name is \"{0}\"", mainGitBranchName);
                            Console.WriteLine("Processing PVCS Report \"{0}\"", pvcsRevisionDetailsPathAndFilename);
                            Console.WriteLine("PVCS Source files for Git Branch \"{0}\" are located in \"{1}\"",
                                                mainGitBranchName, pvcsSharePath);

                            error = CreateAllFeatureBranches(gitRepositoryRootPath, mainGitBranchName, pvcsRevisionDetailsPathAndFilename, pvcsSharePath);

                        }
                    }
                }
            }

            return error;
        }
    }
}
