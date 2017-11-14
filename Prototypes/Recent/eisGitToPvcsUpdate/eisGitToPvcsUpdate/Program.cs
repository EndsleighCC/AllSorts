/*************************************************************************/
/*                                                                       */
/*            COPYRIGHT ENDSLEIGH INSURANCE SERVICES LTD 2017            */
/*                                                                       */
/*                          NON-DELIVERABLE                              */
/*                                                                       */
/*************************************************************************/
/*                                                                       */
/*  PROJECT     : SDA                                                    */
/*                                                                       */
/*  LANGUAGE    : C#                                                     */
/*                                                                       */
/*  FILE NAME   : Program.cs                                             */
/*                                                                       */
/*  ENVIRONMENT : Microsoft Visual Studio                                */
/*                                                                       */
/*-----------------------------------------------------------------------*/
/*                                                                       */
/*  FILE FUNCTION   : This source file contains the C# implementation    */
/*                    for the class Program                              */
/*                                                                       */
/*  EXECUTABLE TYPE : EXE                                                */
/*                                                                       */
/*  SPECIFICATION   : None.                                              */
/*                                                                       */
/*                                                                       */
/*  RELATED DOCUMENTATION : None.                                        */
/*                                                                       */
/*                                                                       */
/*-----------------------------------------------------------------------*/
/*                                                                       */
/*  ABSTRACT : This source file contains the C# implementation for a     */
/*             utility that, provided with a "git log" in appropriate    */
/*             format, can update the necessary PVCS repositories        */
/*                                                                       */
/*  AUTHOR   : C. Cornelius         CREATION DATE: 26-Jun-2017           */
/*                                                                       */
/*-----------------------------------------------------------------------*/
/*                                                                       */
/*  BUILD INFORMATION : Microsoft Visual Studio                          */
/*                                                                       */
/*  EXECUTABLE NAME   : eisGitToPvcsUpdate                               */
/*                                                                       */
/*  ENTRY POINTS      : Main                                             */
/*                                                                       */
/*************************************************************************/
/*                                                                       */
/* PVCS SECTION :                                                        */
/* ~~~~~~~~~~~~~~
   PVCS FILENAME: $Logfile:   Z:\sda\Code\eisGitToPvcsUpdate\eisGitToPvcsUpdate\Program.cs  $
   PVCS REVISION: $Revision:   1.0  $

*/
/*************************************************************************/

using System;
using System.IO;

namespace eisGitToPvcsUpdate
{
    class Program
    {
        private static void ShowUsage()
        {
            Console.WriteLine();
            Console.WriteLine("eisGitToPvcsUpdate : PvcsUserId BuildSetSpecification GitLogReport");
        }

        static int Main(string[] args)
        {
            int error = WindowsErrorDefinition.Success;

            if (args.Length < 3)
            {
                ShowUsage();
                error = WindowsErrorDefinition.NotSupported;
            }
            else
            {
                string pvcsUserId = args[0];
                string buildSetSpecification = args[1];
                string gitLogReportFilename = args[2];

                string gitLogReportPathAndFilename = Path.GetFullPath(gitLogReportFilename);

                Console.WriteLine( "PVCS User Id is \"{0}\"",pvcsUserId);
                Console.WriteLine( "Build Set Specification is \"{0}\"",buildSetSpecification);
                Console.WriteLine( "Supplied git log filename is \"{0}\" resolving to \"{1}\"",gitLogReportFilename,gitLogReportPathAndFilename);

                BuildSetDetails buildSetDetails = new BuildSetDetails(buildSetSpecification);

                if ( ! buildSetDetails.AreValid )
                {
                    Console.WriteLine("eisGitToPvcsUpdate : Unable to identify the Build Set with specification \"{0}\"", buildSetSpecification);
                    error = WindowsErrorDefinition.BadEnvironment;
                }
                else
                {
                    buildSetDetails.Display("Selected Build Set");
                    Console.WriteLine(
                        "Managing PVCS update from Build Set \"{0}\" to PVCS Promotion Group \"{1}\"",
                        buildSetDetails.Identifier,
                        buildSetDetails.SecondaryIdentifier);
                    Console.WriteLine("Git Log report filename is \"{0}\"",gitLogReportPathAndFilename);

                    // Do not meddle with the sources if there is no associated git source change control
                    if ( String.Compare(buildSetDetails.SourceChangeControlType, "git" , true /* ignore case */ ) != 0 )
                    {
                        Console.WriteLine("Build Set \"{0}\" is not Git Source Change Control Type",buildSetDetails.Identifier);
                        error = WindowsErrorDefinition.InvalidFunction;
                    }
                    else
                    {
                        PvcsPromotionGroupDataSortedSet pvcsPromotionGroupDataSortedSet = new PvcsPromotionGroupDataSortedSet();
                        error = pvcsPromotionGroupDataSortedSet.UpdatePvcsForPromotionGroup(pvcsUserId, buildSetDetails, gitLogReportPathAndFilename);
                    }

                }
            }
            return error;
        }
    }
}
