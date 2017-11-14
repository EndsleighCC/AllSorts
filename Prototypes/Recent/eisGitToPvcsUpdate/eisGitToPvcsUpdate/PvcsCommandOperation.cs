using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CommandOperations;

namespace eisGitToPvcsUpdate
{
    public static class PvcsCommandOperation
    {
        public static int GetArchivePromotionGroupNames(string pvcsArchivePathAndFilename, ref SortedSet<string> promotionGroupList)
        {
            int error = WindowsErrorDefinition.Success;

            promotionGroupList.Clear();

            List<string> stdout = new List<string>();
            List<string> stderr = new List<string>();

            string command = "vlog -q -bg \"" + pvcsArchivePathAndFilename + "\"";
            bool success = CommandOperation.RunVisibleCommand(Directory.GetCurrentDirectory(),
                                                              command,
                                                              1,
                                                              CommandOperation.DebugProgress.Enabled,
                                                              CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError,
                                                              stdout,
                                                              stderr);
            if ( ! success )
            {
                Console.WriteLine("PvcsCommandOperation.GetArchivePromotionGroupNames : Failed to get Promotions Groups from PVCS Archive \"{0}\"",
                                    pvcsArchivePathAndFilename);
                error = WindowsErrorDefinition.InvalidFunction;
            }
            else
            {
                // Look for the Promotion Groups in the output from the command

                foreach ( string line in stdout )
                {
                    Console.WriteLine("{0}", line);
                    if (line.StartsWith(pvcsArchivePathAndFilename))
                    {
                        // Output line begins with the Archive Name

                        string promotionGroup = null;
                        string[] words = line.Split(new char[] { ' ' } ,StringSplitOptions.RemoveEmptyEntries);
                        for ( int wordIndex = 0 ;
                              (promotionGroup == null)
                              && ( wordIndex < words.Count() ) ;
                              ++wordIndex
                            )
                        {
                            if (words[wordIndex] == ":")
                            {
                                // The word prior to the colon is the Promotion Group
                                promotionGroup = words[wordIndex - 1];
                                promotionGroupList.Add(promotionGroup);
                            }
                        } // for
                    } // Output line begins with the Archive Name
                } // foreach
            } // Look for the Promotion Groups in the output from the command
            return error;
        } // GetArchivePromotionGroupNames

        public static int AssignPromotionGroup( string pvcsArchivePathAndFilename, string promotionGroup )
        {
            int error = WindowsErrorDefinition.Success;

            List<string> stdout = new List<string>();
            List<string> stderr = new List<string>();

            // Assign the Promotion Group to the tip revision
            string command = "vcs -q -g" + promotionGroup + " -r \"" + pvcsArchivePathAndFilename + "\"";
            bool success = CommandOperation.RunVisibleCommand(Directory.GetCurrentDirectory(),
                                                              command,
                                                              1,
                                                              CommandOperation.DebugProgress.Enabled,
                                                              CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError,
                                                              stdout,
                                                              stderr);
            if (!success)
            {
                Console.WriteLine("PvcsCommandOperation.AssignPromotionGroup : Failed to assign Promotion Group \"{0}\" to the tip of PVCS Archive \"{1}\"",
                                    promotionGroup, pvcsArchivePathAndFilename);
                error = WindowsErrorDefinition.InvalidFunction;
            }

            return error;
        }

        public static int Promote( string pvcsArchivePathAndFilename, string fromPromotionGroup )
        {
            int error = WindowsErrorDefinition.Success;

            List<string> stdout = new List<string>();
            List<string> stderr = new List<string>();

            // Use the "-q" option to suppress questions
            string command = "vpromote -q -g" + fromPromotionGroup + " \"" + pvcsArchivePathAndFilename + "\"";
            bool success = CommandOperation.RunVisibleCommand(Directory.GetCurrentDirectory(),
                                                              command,
                                                              1,
                                                              CommandOperation.DebugProgress.Enabled,
                                                              CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError,
                                                              stdout,
                                                              stderr);
            if (!success)
            {
                Console.WriteLine("PvcsCommandOperation.Promote : Failed to Promote from Promotion Group \"{0}\" in PVCS Archive \"{1}\"",
                                    fromPromotionGroup, pvcsArchivePathAndFilename);
                error = WindowsErrorDefinition.InvalidFunction;
            }

            return error;
        } // Promote

        public static int ArchiveIsEmpty( string pvcsArchivePathAndFilename, out bool isEmpty )
        {
            int error = WindowsErrorDefinition.Success;

            // Initialise to the Archive being empty
            isEmpty = true;

            List<string> stdout = new List<string>();
            List<string> stderr = new List<string>();

            // Generate just a brief report of the newest revision
            string command = "vlog -q -bn \"" + pvcsArchivePathAndFilename + "\"";
            bool success = CommandOperation.RunVisibleCommand(Directory.GetCurrentDirectory(),
                                                              command, 1,
                                                              CommandOperation.DebugProgress.Enabled,
                                                              CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError,
                                                              stdout,
                                                              stderr);
            if (!success)
            {
                Console.WriteLine("PvcsCommandOperation.ArchiveIsEmpty : Failed to determine empty status of PVCS Archive \"{0}\"",
                                    pvcsArchivePathAndFilename);
                error = WindowsErrorDefinition.GeneralFailure;
            }
            else
            {
                // Look for details about any revision

                for (int lineIndex = 0; ( isEmpty ) && ( lineIndex < stdout.Count); ++lineIndex )
                {
                    string line = stdout[lineIndex];
                    Console.WriteLine("{0}", line);
                    if (line.StartsWith(pvcsArchivePathAndFilename))
                    {
                        // Found a revision so the Archive is not empty
                        isEmpty = false;
                    }
                }

            } // Look for details about any revision

            return error;
        } // ArchiveIsEmpty

        public static int Lock( string pvcsArchivePathAndFilename, string promotionGroup )
        {
            int error = WindowsErrorDefinition.Success;

            List<string> stdout = new List<string>();
            List<string> stderr = new List<string>();

            // Lock the tip revision at the specified Promotion Group
            string command = "vcs -q -l -r -g" + promotionGroup + " \"" + pvcsArchivePathAndFilename + "\"";
            bool success = CommandOperation.RunVisibleCommand(Directory.GetCurrentDirectory(),
                                                              command,
                                                              1,
                                                              CommandOperation.DebugProgress.Enabled,
                                                              CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError,
                                                              stdout,
                                                              stderr);
            if (!success)
            {
                Console.WriteLine("PvcsCommandOperation.Lock : Failed to Lock a revision at Promotion Group \"{0}\" in PVCS Archive \"{1}\"",
                                    promotionGroup, pvcsArchivePathAndFilename);
                error = WindowsErrorDefinition.InvalidFunction;
            }

            return error;
        } // Lock

        private static int WriteChangeDescriptionFile( string changeDescription , out string filename )
        {
            int error = WindowsErrorDefinition.Success;

            filename = Path.GetTempFileName();

            File.WriteAllText(filename,changeDescription);

            return error;
        } // WriteChangeDescriptionFile

        public static int CheckIntoNonEmptyArchive( string pvcsUserId,
                                                    string changeDescription,
                                                    string pvcsArchivePathAndFilename,
                                                    string workfilePathAndName)
        {
            int error = CheckIn(pvcsUserId,
                                changeDescription,
                                null,
                                pvcsArchivePathAndFilename,
                                workfilePathAndName);
            return error;
        }

        public static int CheckIntoEmptyArchive(string pvcsUserId,
                                                string changeDescription,
                                                string promotionGroup ,
                                                string pvcsArchivePathAndFilename,
                                                string workfilePathAndName)
        {
            int error = CheckIn(pvcsUserId,
                                changeDescription,
                                promotionGroup,
                                pvcsArchivePathAndFilename,
                                workfilePathAndName);
            return error;
        }

        private static int CheckIn( string pvcsUserId,
                                    string changeDescription ,
                                    string promotionGroup ,
                                    string pvcsArchivePathAndFilename ,
                                    string workfilePathAndName )
        {
            int error = WindowsErrorDefinition.Success;

            string changeDescriptionPathAndFilename = null;
            error = WriteChangeDescriptionFile(changeDescription, out changeDescriptionPathAndFilename);
            if ( error == WindowsErrorDefinition.Success )
            {
                // Change description filename exists

                List<string> stdout = new List<string>();
                List<string> stderr = new List<string>();

                string command = "put -y \"-m@"
                                 + changeDescriptionPathAndFilename + "\"";
                if (promotionGroup!=null)
                {
                    // Assign a Promotion Group upon CheckIn which will only work for an empty Archive
                    command += " -g" + promotionGroup;
                }
                command += " \""
                        + pvcsArchivePathAndFilename
                        + "(" + workfilePathAndName + ")"
                        + "\"";
                bool success = CommandOperation.RunVisibleCommand(Directory.GetCurrentDirectory(),
                                                                  command,
                                                                  1,
                                                                  CommandOperation.DebugProgress.Enabled,
                                                                  CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError,
                                                                  stdout,
                                                                  stderr);
                if (!success)
                {
                    Console.WriteLine("PvcsCommandOperation.CheckIn : Failed to CheckIn revision from \"{0}\" to PVCS Archive \"{1}\"",
                                        workfilePathAndName, pvcsArchivePathAndFilename);
                    error = WindowsErrorDefinition.InvalidFunction;
                }

                // Tidy up the change description file
                File.Delete(changeDescriptionPathAndFilename);

            } // Change description filename exists

            return error;
        }

    } // PvcsCommandOperation
}
