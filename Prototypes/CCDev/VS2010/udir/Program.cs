using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace udir
{
    class Program
    {
        static void Main(string[] args)
        {
            string command = null;
            for ( int argIndex = 0 ; argIndex < args.Count() ; ++argIndex )
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
                Console.WriteLine();
                Console.WriteLine("** DIR With UNIX Path **");
                Console.WriteLine("C. Cornelius 13-Aug-2015");
                Console.WriteLine();
                Console.WriteLine("Usage: udir files {files...}");
                Console.WriteLine();
                Console.WriteLine("Function: Converts forward slashes to backslashes and performs");
                Console.WriteLine("          a \"dir\" followed by an \"attrib\" with the result.");
                Console.WriteLine("          Account is taken of providing DIR switches.");
            }
            else
            {
                string dirCommand = "dir " + command;
                CommandOperation.RunCommand(".\\", dirCommand, CommandOperation.DebugProgress.None,
                    CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError);
                Console.WriteLine();
                string attribCommand = "attrib " + command;
                CommandOperation.RunCommand(".\\", attribCommand, CommandOperation.DebugProgress.None,
                    CommandOperation.CommandOutputDisplayType.StandardOutputAndStandardError);
            }
        }
    }
}
