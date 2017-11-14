/*************************************************************************/
/*                                                                       */
/*            COPYRIGHT ENDSLEIGH INSURANCE SERVICES LTD 2017            */
/*                                                                       */
/*                          NON-DELIVERABLE                              */
/*                                                                       */
/*************************************************************************/
/*                                                                       */
/*  PROJECT     : Endsleigh Utility                                      */
/*                                                                       */
/*  LANGUAGE    : C#                                                     */
/*                                                                       */
/*  FILE NAME   : BuildSetDetails.cs                                     */
/*                                                                       */
/*  ENVIRONMENT : Microsoft Visual Studio                                */
/*                                                                       */
/*-----------------------------------------------------------------------*/
/*                                                                       */
/*  FILE FUNCTION   : This source file contains the C# implementation    */
/*                    for the utility class BuildSetDetails              */
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
/*  ABSTRACT : This source file contains the C# implementation for the   */
/*             class BuildSetDetails which is a utility class to obtain  */
/*             the details of a specified Build Set from a central       */
/*             location                                                  */
/*                                                                       */
/*  AUTHOR   : C. Cornelius         CREATION DATE: 19-Jun-2017           */
/*                                                                       */
/*-----------------------------------------------------------------------*/
/*                                                                       */
/*  BUILD INFORMATION : Microsoft Visual Studio                          */
/*                                                                       */
/*  EXECUTABLE NAME   : eisGitToPvcsUpdate.exe                           */
/*                                                                       */
/*  ENTRY POINTS      : main                                             */
/*                                                                       */
/*************************************************************************/
/*                                                                       */
/* PVCS SECTION :                                                        */
/* ~~~~~~~~~~~~~~
   PVCS FILENAME: $Logfile:   Z:\sda\Code\eisGitToPvcsUpdate\eisGitToPvcsUpdate\BuildSetDetails.cs  $
   PVCS REVISION: $Revision:   1.1  $

   $Log:   Z:\sda\Code\eisGitToPvcsUpdate\eisGitToPvcsUpdate\BuildSetDetails.cs  $
// 
//    Rev 1.1   Jun 29 2017 09:34:08   corc1
// 
//  inet OS Transition
// 
//  Added processing for Build Set Deployment Server Package Location
//  being read from the Build Set List Configuration File
// 
//    Rev 1.0   Jun 28 2017 11:39:28   corc1
// 
//  inet OS Transition
// 
//  Initial revision

*/
/*************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace eisGitToPvcsUpdate
{
    public class BuildSetDetails
    {
        // Constructor
        public BuildSetDetails( String buildSetSpecification )
        {
            try
            {
                _originalBuildSetSpecification = buildSetSpecification;
                _buildSetListPathAndFilename = ConfigurationManager.AppSettings[_buildSetListFullFilePathConfigKey];

                if ( ! File.Exists( _buildSetListPathAndFilename ) )
                {
                    Console.WriteLine( "BuildSetDetails constructor : Build Set List File \"{0}\" does not exist" ,
                                            _buildSetListPathAndFilename ) ;
                }
                else
                {
                    // Configuration file exists

                    using (StreamReader buildSetListFileStream = new StreamReader(_buildSetListPathAndFilename))
                    {
                        string buildSetLine = null;
                        int lineNumber = 0;
                        while ((buildSetLine = buildSetListFileStream.ReadLine()) != null)
                        {
                            lineNumber += 1;
                            if (buildSetLine.Substring(0, 1) != "#")
                            {
                                // Not a comment line

                                //Console.WriteLine("{0} : {1} : \"{2}\"",
                                //                            _buildSetListPathAndFilename, lineNumber, buildSetLine);

                                string[] buildSetDetail = buildSetLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                string buildSetIdentifier = buildSetDetail[0];
                                string buildSetNickname = buildSetDetail[1];
                                string buildSetSecondaryIdentifier = buildSetDetail[2];
                                string buildSetCode = buildSetDetail[3];
                                string buildSetSourceChangeControlType = buildSetDetail[4];
                                string buildSetServerName = buildSetDetail[5];
                                string buildSetServerDriveSpecifier = buildSetDetail[6];
                                string buildSetServerDirectoryName = buildSetDetail[7];
                                string buildSetDeploymentServerPackageLocation = buildSetDetail[8];
                                string buildSetOedEnvName = null;
                                if (buildSetDetail.Length >= 10)
                                {
                                    buildSetOedEnvName = buildSetDetail[9];
                                }

                                if ((buildSetSpecification == buildSetIdentifier)
                                     || (String.Compare(buildSetSpecification, buildSetNickname, true /* ignore case */ ) == 0)
                                   )
                                {
                                    //Console.WriteLine("Found \"{0}\" = \"{1}\" \"{2}\"",
                                    //                       buildSetSpecification, buildSetDetail[0], buildSetDetail[1]);
                                    if (buildSetServerName.ToLower() != CurrentBuildServerName.ToLower())
                                    {
                                        Console.WriteLine("Build Set \"{0}\" is resident on Build Server \"{1}\""
                                                            + " and not on this Build Server \"{2}\"",
                                                                buildSetIdentifier,
                                                                buildSetServerName.ToUpper(),
                                                                CurrentBuildServerName.ToUpper());
                                    }
                                    else
                                    {
                                        Identifier = buildSetIdentifier;
                                        Nickname = buildSetNickname;
                                        SecondaryIdentifier = buildSetSecondaryIdentifier;
                                        Code = buildSetCode;
                                        SourceChangeControlType = buildSetSourceChangeControlType;
                                        BuildSetServerName = buildSetServerName;
                                        BuildSetServerDriveSpecifier = buildSetServerDriveSpecifier;
                                        BuildSetServerDirectoryName = buildSetServerDirectoryName;
                                        BuildSetDeploymentServerPackageLocation = buildSetDeploymentServerPackageLocation;
                                        // The OEDIPUS Environment might not be specified and hence can be Nothing
                                        OedipusEnvironmentName = buildSetOedEnvName;
                                        AreValid = true;
                                        // Display();
                                    }
                                }

                            } // Not a comment line
                        } // while
                    } // using
                } // Configuration file exists
            } // try
            catch (Exception ex)
            {
                Console.WriteLine("BuildSetDetails : Exception = \"{0}\"",ex.ToString());
            }
        } // Constructor

        public void Display()
        {
            Display( "BuildSetDetails" ) ;
        }

        public void Display( string description )
        {
            if ( ! AreValid )
            {
                Console.WriteLine( "{0} : Original Build Set specification \"{1}\" is not known" ,
                                        description , _originalBuildSetSpecification ) ;
            }
            else
            {
                Console.WriteLine( "{0} : \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\" \"{7}\" \"{8}\" \"{9}\" \"{10}\" \"{11}\"" ,
                                    description ,
                                    Identifier ,
                                    Nickname ,
                                    SecondaryIdentifier ,
                                    Code ,
                                    SourceChangeControlType ,
                                    BuildSetServerName ,
                                    BuildSetServerDriveSpecifier ,
                                    BuildSetServerDirectoryName ,
                                    BuildSetDeploymentServerPackageLocation ,
                                    ( OedipusEnvironmentName == null ) ? "(unspecified)" : OedipusEnvironmentName ,
                                    BuildSetRootPath ) ;
            }
        }

        private string CurrentBuildServerName
        {
            get
            {
                return System.Net.Dns.GetHostName();
            }
        }

        public string OriginalBuildSetSpecification { get { return _originalBuildSetSpecification; } }

        public string BuildSetListPathAndFilename { get { return _buildSetListPathAndFilename;  } }

        public string BuildSetRootPath
        {
            get
            {
                if ( String.IsNullOrEmpty( _buildSetRootPath ) )
                {
                    // Generate the Build Set Root Path from its elements

                    if (    ( String.IsNullOrEmpty( BuildSetServerDriveSpecifier ) )
                         || ( String.IsNullOrEmpty( BuildSetServerDirectoryName ) )
                       )
                    {
                        _buildSetRootPath = null ;
                    }
                    else
                    {
                        char [] trimChars = new [] { Path.DirectorySeparatorChar } ;
                        _buildSetRootPath
                            = Path.Combine( BuildSetServerDriveSpecifier.Trim( trimChars ) + Path.DirectorySeparatorChar ,
                                             BuildSetServerDirectoryName.Trim( trimChars ) + Path.DirectorySeparatorChar ) ;
                    }
                } // Generate the Build Set Root Path from its elements

                return _buildSetRootPath;
            }
        }

        public bool AreValid { get; private set; }

        public string Identifier { get; private set;  }
        public string Nickname { get; private set; }
        public string SecondaryIdentifier { get; private set; }
        public string Code { get; private set; }
        public string SourceChangeControlType { get; private set; }
        public string BuildSetServerName { get; private set; }
        public string BuildSetServerDriveSpecifier { get; private set; }
        public string BuildSetServerDirectoryName { get; private set; }
        public string BuildSetDeploymentServerPackageLocation { get; private set; }
        public string OedipusEnvironmentName { get; private set; }

        private string _buildSetListPathAndFilename = "";
        private string _buildSetListFullFilePathConfigKey = "BuildSetListFullFilePath" ;

        private string _originalBuildSetSpecification = "";

        private string _buildSetRootPath { get; set; }

    }
}
