using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestAppConfigTransform
{
    class Program
    {
        static void Main(string[] args)
        {
            string machineConfigurationFileDirectory =
                System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            Console.WriteLine("Machine Config File Directory is \"{0}\"", machineConfigurationFileDirectory);

            string appConfigFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            Console.WriteLine("Application Configuration File is \"{0}\"" ,appConfigFileName);

            string localApplicationDataDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Console.WriteLine("Local Application Data Directory is \"{0}\"", localApplicationDataDirectory);

            string roamingApplicationDataDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Console.WriteLine("Roaming Application Data Directory is \"{0}\"", roamingApplicationDataDirectory);

            string localUserSettingsFilename =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            Console.WriteLine("Local User Settings Filename is \"{0}\"",localUserSettingsFilename);

            string roamingUserSettingsFilename =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming).FilePath;
            Console.WriteLine("Roaming User Settings Filename is \"{0}\"", roamingUserSettingsFilename);

        }
    }
}
