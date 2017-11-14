using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace TestRegistry
{
    class Program
    {
        private static void ShowKey( string keyName )
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName);

            if (key == null)
            {
                Console.WriteLine("User Key \"{0}\" does not exist", keyName);
            }
            else
            {
                string[] subkeyNames = key.GetSubKeyNames();
                Console.WriteLine("User Key \"{0}\" exists as \"{1}\"", keyName,key.Name);

                foreach (string subKeyName in subkeyNames)
                {
                    Console.WriteLine("    Sub-key Name = \"{0}\"",subKeyName);
                }
            }
       
        }

        private static void Main(string[] args)
        {
            ShowKey("Software\\Endsleigh");
            ShowKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            ShowKey("Software\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
        }
    }
}
