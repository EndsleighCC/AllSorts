using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TestAppConfigTransform
{
    class Program
    {
        static string ReadAppSetting( string key )
        {
            string value = null;

            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                value = appSettings[key] ?? "";
                // var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                // var appSettings = configFile.AppSettings.Settings;
                // value = appSettings[key].Value ?? "";
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app configuration for \"{0}\"",key);
            }

            return value;
        }

        static void ShowAppSettingValue( string key )
        {
            string value = ReadAppSetting(key);
            Console.WriteLine("AppSettings Value of \"{0}\" is \"{1}\"", key, value);
        }

        static void TestAppSettings()
        {
            Console.WriteLine();
            Console.WriteLine("appSettings");
            Console.WriteLine();
            ShowAppSettingValue("AppSettingsKey1");
            ShowAppSettingValue("ApplicationSettingsKey1");
        }

        static void TestApplicationSettings()
        {
            Console.WriteLine();
            Console.WriteLine("Settings Designer Settings");
            Console.WriteLine();
            string applicationSettingsKey1Name = "ApplicationSettingsKey1";
            string applicationSettingsKey1Value = Properties.Settings.Default.ApplicationSettingsKey1;
            Console.WriteLine("String ApplicationSettings Value of \"{0}\" is \"{1}\"", applicationSettingsKey1Name, applicationSettingsKey1Value);

            var applicationSettingsPropertyCollection = Properties.Settings.Default.Properties;

            System.Configuration.SettingsProperty applicationSettingsProperty = applicationSettingsPropertyCollection[applicationSettingsKey1Name];
            string applicationSettingsPropertyName1 = applicationSettingsProperty.Name;
            string applicationSettingsPropertyValue1 = (string)applicationSettingsProperty.DefaultValue;
            Console.WriteLine("Object ApplicationSettings Value of \"{0}\" is \"{1}\"", applicationSettingsKey1Name, applicationSettingsKey1Value);
        }

        static void Main(string[] args)
        {
            TestAppSettings();
            TestApplicationSettings();
        }
    }
}
