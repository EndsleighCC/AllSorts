using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestCOMAnything
{
    class Program
    {
        static int Main(string[] args)
        {
            int error = 0;

            if (args.Count() < 1)
            {
                Console.WriteLine("TestCOMAnything TypeLibraryFilename");
            }
            else
            {
                string filename = args[0];

                try
                {
                    FileInfo fileInfo = new FileInfo(filename);
                    if (! fileInfo.Exists)
                    {
                        throw new FileNotFoundException(filename);
                    }

                    string typeLibraryFilename = fileInfo.FullName;

                    Type typeTypeLibraryApplication = Type.GetTypeFromProgID("TLI.TLIApplication");
                    System.Object typeLibraryObject = Activator.CreateInstance(typeTypeLibraryApplication); // CoCreateInstance
                    TLI.TLIApplication typeLibraryApplication = (TLI.TLIApplication) typeLibraryObject; // QueryInterface

                    Console.WriteLine("Attempting to Register Type Library \"{0}\"", typeLibraryFilename);
                    typeLibraryApplication.TypeLibInfoFromFile(typeLibraryFilename).Register();
                    Console.WriteLine("Successfully Registered Type Library \"{0}\"", typeLibraryFilename);
                }
                catch (FileNotFoundException eek)
                {
                    Console.WriteLine("File \"{0}\" was not found", eek.Message);
                    error = 1;
                }
                catch (System.Runtime.InteropServices.COMException eek)
                {
                    switch ((uint)eek.ErrorCode)
                    {
                        case 0x8002801C : // TYPE_E_REGISTRYACCESS
                            Console.WriteLine("The application does not have sufficient access to the Registry in order to Register the Type Library");
                            break;
                        default :
                            Console.WriteLine("Unexpected COM Exception \"{0}\"", eek.Message);
                            break;
                    }
                    error = 2;
                }
                catch (Exception eekException)
                {
                    Console.WriteLine( "Exception trying to Register \"{0}\" = {1}",
                                            filename,eekException.ToString());
                    error = 3;
                }

            }
            return error;
        } // Main
    }
}
