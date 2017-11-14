using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TestStartProcess
{
    class Program
    {
        class TestStartProcessMechanism
        {
        
            public TestStartProcessMechanism()
            {
            }

            public Collection<string> Start(string batchFullFilename)
            {
                Collection<string> result = new Collection<string>();

                //string batchCommand = "notepad.exe";
                //string batchFileArguments = @"c:\st04\test.bat";

                //string batchCommand = "cmd.exe";
                string batchCommand = "cmd.exe";
                string batchFileArguments = String.Format("/c \"{0}\" {1} {2} {3} {4}",
                                                          batchFullFilename,
                                                          "First",
                                                          "Second",
                                                          "Third",
                                                          "Fourth");

                ProcessStartInfo batchFileProcessStartInfo = new ProcessStartInfo(batchCommand);
                batchFileProcessStartInfo.UseShellExecute = false;
                batchFileProcessStartInfo.Arguments = batchFileArguments;
                batchFileProcessStartInfo.RedirectStandardInput = true;
                batchFileProcessStartInfo.RedirectStandardOutput = true;
                batchFileProcessStartInfo.RedirectStandardError = true;

                Process batchFileProcess = new Process();
                batchFileProcess.StartInfo = batchFileProcessStartInfo;

                try
                {
                    Console.WriteLine("Running \"{0}\" \"{1}\"", batchCommand,batchFileArguments);

                    batchFileProcess.Start();

                    batchFileProcess.StandardInput.WriteLine("\r\n");

                    batchFileProcess.WaitForExit();

                    Console.WriteLine( "Running \"{0}\" produced Exit Code {1}",batchFullFilename,batchFileProcess.ExitCode);

                    if (!batchFileProcess.StandardOutput.EndOfStream)
                    {
                        Console.WriteLine("Batch File \"{0}\" Standard Output:", batchFullFilename);
                        while (!batchFileProcess.StandardOutput.EndOfStream)
                        {
                            Console.WriteLine("    " + batchFileProcess.StandardOutput.ReadLine());
                        }
                    }
                    if (!batchFileProcess.StandardError.EndOfStream)
                    {
                        Console.WriteLine("Batch File \"{0}\" Standard Error:", batchFullFilename);
                        while (!batchFileProcess.StandardError.EndOfStream)
                        {
                            Console.WriteLine("    " + batchFileProcess.StandardError.ReadLine());
                        }
                    }

                }
                catch (Exception eek)
                {
                    Console.WriteLine("    *** {0}.{1} : Project \"{2}\" executing \"{3}\" exception \"{4}\"",
                                      MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      batchFullFilename,
                                      eek.ToString());
                }

                return result;
            }
    
        }

        static void Main(string[] args)
        {
            TestStartProcessMechanism testStartProcessMechanism = new TestStartProcessMechanism();

            testStartProcessMechanism.Start("c:\\ST04\\test.bat");
        }
    }
}
