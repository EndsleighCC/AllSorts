using System;

namespace eisGitToPvcsUpdate
{
    public static class PvcsConfiguration
    {
        public const string PvcsArchiveDriveSpec = "Z:";

        public static int SetPvcsEnvironment(string pvcsUserId)
        {
            int error = WindowsErrorDefinition.Success;

            Environment.SetEnvironmentVariable(_pvcsUserIdEnvironmentVariableName,pvcsUserId);
            Environment.SetEnvironmentVariable(_pvcsProjectConfigurationFileEnvironmentVariableName,
                                               _pvcsProjectConfigurationFilePathAndName);
            try
            {
                string windir = Environment.GetEnvironmentVariable(_pvcsConfigurationFilePathEnvironmentVariableName);
                if (windir == null)
                {
                    Console.WriteLine("PvcsConfiguration.SetPvcsEnvironment : Unable to identify Windows Environment Variable \"{0}\"",
                                        _pvcsConfigurationFilePathEnvironmentVariableName);
                    error = WindowsErrorDefinition.BadEnvironment;
                }
                else
                {
                    // Got contents of WINDIR Environment Variable

                    Environment.SetEnvironmentVariable(_pvcsConfigurationEnvironmentVariableName, windir);

                } // Got contents of WINDIR Environment Variable
            }
            catch (Exception ex)
            {
                Console.WriteLine("PvcsConfiguration.SetPvcsEnvironment : Exception = {0}",ex.ToString());
                error = WindowsErrorDefinition.BadEnvironment;
            }

            return error;

        } // SetPvcsEnvironment

        private const string _pvcsUserIdEnvironmentVariableName = "VCSID";
        private const string _pvcsProjectConfigurationFileEnvironmentVariableName = "VCSCFG";
        private const string _pvcsProjectConfigurationFilePathAndName = @"v:\pvcs\eis\projcfg\EISProjectDB.cfg";

        private const string _pvcsConfigurationEnvironmentVariableName = "ISLVINI";
        private const string _pvcsConfigurationFilePathEnvironmentVariableName = "WINDIR";

    } // PvcsConfiguration
}
