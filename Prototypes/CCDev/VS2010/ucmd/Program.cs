using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using udir;

namespace ucmd
{
    class Program
    {
        static void ShowUsage()
        {
            Console.WriteLine();
            Console.WriteLine("** Command With UNIX Path **");
            Console.WriteLine("C. Cornelius     14-Aug-2015");
            Console.WriteLine();
            Console.WriteLine("Usage: ucmd command {argument...}");
            Console.WriteLine();
            Console.WriteLine("Function: Converts forward slashes to backslashes and performs");
            Console.WriteLine("          the specified command. Account is taken of switches.");
        }

        static int Main(string[] args)
        {
            int error = 0;
            if (args.Count() < 1)
            {
                ShowUsage();
            }
            else
            {
                string command = args[0];
                for (int argIndex = 1; argIndex < args.Count(); ++argIndex)
                {
                    string thisParameter = args[argIndex];
                    if ((thisParameter.Length > 2) && (thisParameter[0] == '/'))
                    {
                        // A switch so just leave it alone
                        command += " " + thisParameter;
                    }
                    else
                    {
                        command += " " + thisParameter.Replace('/', '\\');
                    }

                } // for

                if (command == null)
                {
                    ShowUsage();
                }
                else
                {
                    CommandOperation.RunCommand(".\\", command, CommandOperation.DebugProgress.None,
                        CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError);
                }
                
            }
            return error;
        }
    }
}
