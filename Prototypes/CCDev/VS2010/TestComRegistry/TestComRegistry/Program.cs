using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace TestComRegistry
{
    class Program
    {
        static bool ProcessCommandLineArguments(string[] args, out bool debugEnabled, out bool deleteKeys, out string registrationPathElement)
        {
            bool success = false;

            debugEnabled = false;
            deleteKeys = false;
            registrationPathElement = null;

            foreach (string arg in args)
            {
                if (arg[0] == '/')
                {
                    // A switch

                    switch (arg[1].ToString().ToLower())
                    {
                        case "d":
                            Console.WriteLine("Debug enabled");
                            debugEnabled = true;
                            break;
                        case "x":
                            Console.WriteLine("Keys will be deleted");
                            deleteKeys = true;
                            break;
                        default:
                            Console.WriteLine("Unknown switch \"{0}\"", arg);
                            success = false;
                            break;
                    }

                } // A switch
                else
                {
                    // An argument

                    if (registrationPathElement == null)
                    {
                        registrationPathElement = arg;
                        Console.WriteLine("COM Registration Path Element is \"{0}\"", registrationPathElement);
                        success = true;
                    }

                } // An argument
            }
            return success;
        }

        static void Main(string[] args)
        {
            bool manageRegistry = true;

            bool debugOutput = false;
            bool deleteKeys = false;

            string registrationPathElement = null;

            manageRegistry = ProcessCommandLineArguments(args, out debugOutput, out deleteKeys, out registrationPathElement);

            if (!manageRegistry)
            {
                Console.WriteLine();
                Console.WriteLine("Nothing to do");
            }
            else if (registrationPathElement.Equals("*") && deleteKeys)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid operation selected: Delete All");
            }
            else
            {
                // Sufficient arguments

                int endsleighCOMComponentCount = 0;

                using (RegistryKey rkClassesRoot = Registry.ClassesRoot)
                {
                    if (debugOutput)
                    {
                        Console.WriteLine();
                        Console.WriteLine("    Opened HKEY_CLASSES_ROOT");
                    }
                    using (RegistryKey rkCLSID = rkClassesRoot.OpenSubKey("CLSID", true))
                    {
                        String[] classGUIDs = rkCLSID.GetSubKeyNames();

                        if (debugOutput)
                        {
                            Console.WriteLine();
                            Console.WriteLine("    Opened HKEY_CLASSES_ROOT\\CLSID containing {0} entries",
                                              classGUIDs.Length);
                            Console.WriteLine();
                        }

                        Console.WriteLine();
                        Console.WriteLine("Total COM Class Count is {0}", classGUIDs.Length);

                        Console.WriteLine();
                        Console.WriteLine("Endsleigh COM Component List");
                        Console.WriteLine();

                        bool finished = false;
                        for (int classIndex = 0; (!finished) && (classIndex < classGUIDs.Length); ++classIndex)
                        {
                            string classGUID = classGUIDs[classIndex];

                            bool relevantSubKey = false;
                            using (RegistryKey rkClass = rkCLSID.OpenSubKey(classGUID))
                            {
                                if (debugOutput)
                                {
                                    Console.WriteLine("        Opened {0}", classGUID);
                                }
                                try
                                {
                                    using (RegistryKey rkInprocServer32 = rkClass.OpenSubKey("InprocServer32"))
                                    {
                                        if (debugOutput)
                                        {
                                            Console.WriteLine("            Opened {0}\\{1}", classGUID, "InprocServer32");
                                        }

                                        try
                                        {
                                            // Get the default value
                                            Object inprocServerFilePathObject = rkInprocServer32.GetValue(null);
                                            if (inprocServerFilePathObject != null)
                                            {
                                                if (inprocServerFilePathObject.GetType() == typeof(string))
                                                {
                                                    string inprocServerFilePath = (string)inprocServerFilePathObject;

                                                    string longFilename = Path.GetFullPath(inprocServerFilePath);
                                                    if ((registrationPathElement.Equals("*"))
                                                         || (longFilename.IndexOf(registrationPathElement, 0,
                                                                             StringComparison.CurrentCultureIgnoreCase) >= 0)
                                                       )
                                                    {
                                                        Console.WriteLine("    {0} = \"{1}\"", classGUID, longFilename);
                                                        endsleighCOMComponentCount += 1;
                                                        relevantSubKey = true;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (debugOutput)
                                                {
                                                    Console.WriteLine(
                                                        "                Read {0}\\{1}\\(default) is NULL",
                                                        classGUID, "InprocServer32");
                                                }
                                            }
                                        }
                                        catch (Exception eek)
                                        {
                                            if (debugOutput)
                                            {
                                                Console.WriteLine(
                                                    "                Exception Reading {0}\\{1}\\(default) = {2}",
                                                    classGUID,
                                                    "InprocServer32", eek.ToString());
                                            }
                                        }
                                    }
                                }
                                catch (Exception eek)
                                {
                                }
                            }

                            if (relevantSubKey)
                            {
                                if (deleteKeys)
                                {
                                    Console.WriteLine("    Deleting HKEY_CLASSES_ROOT\\CLSID\\{0}", classGUID);
                                    try
                                    {
                                        rkCLSID.DeleteSubKeyTree(classGUID);
                                    }
                                    catch (Exception eek)
                                    {

                                        Console.WriteLine("    Exception deleting HKEY_CLASSES_ROOT\\CLSID\\{0} = \"{1}\"", classGUID, eek.ToString());
                                        finished = true;
                                    }
                                }
                            }

                        } // foreach

                        if (finished)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Exiting prematurely");
                        }

                        Console.WriteLine();
                        Console.WriteLine("Endsleigh \"{0}\" COM Component Count is {1}", registrationPathElement, endsleighCOMComponentCount);
                    }
                }

            } // Sufficient arguments
        }
    }
}
