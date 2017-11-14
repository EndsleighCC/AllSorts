using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestSynchronousProcessExecutor
{
    class Program
    {
        static void Main(string[] args)
        {
            SynchronousProcessExecutor synchronousProcessExecutor = new SynchronousProcessExecutor();

            if ( synchronousProcessExecutor.Execute(
                    @"C:\Users\corc1\Documents\CCDev\VS2010\TestSynchronousProcessExecutor\TestSynchronousProcessExecutor",
                    "eisNumBackup.exe",
                    SynchronousProcessExecutor.DebugProgress.None,
                    SynchronousProcessExecutor.CommandOutputDisplayType.StandardOutputAndStandardError)
               )
            {
                Console.WriteLine();
                Console.WriteLine("SynchronousProcessExecutor Succeeded");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("SynchronousProcessExecutor Failed");
            }

            Console.WriteLine();
            Console.WriteLine("Standard Output:");
            foreach (string line in synchronousProcessExecutor.StandardOutputBuffer)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine();
            Console.WriteLine("Standard Error:");
            foreach (string line in synchronousProcessExecutor.StandardErrorBuffer)
            {
                Console.WriteLine(line);
            }

            synchronousProcessExecutor.Execute(
                @"C:\Users\corc1\Documents\CCDev\VS2010\TestSynchronousProcessExecutor\TestSynchronousProcessExecutor",
                "notepad.exe",
                SynchronousProcessExecutor.DebugProgress.Enabled,
                SynchronousProcessExecutor.CommandOutputDisplayType.StandardOutputAndStandardError);

            if ( synchronousProcessExecutor.Success)
            {
                Console.WriteLine();
                Console.WriteLine("SynchronousProcessExecutor Succeeded");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("SynchronousProcessExecutor Failed");
            }

            Console.WriteLine();
            Console.WriteLine("Standard Output:");
            foreach (string line in synchronousProcessExecutor.StandardOutputBuffer)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine();
            Console.WriteLine("Standard Error:");
            foreach (string line in synchronousProcessExecutor.StandardErrorBuffer)
            {
                Console.WriteLine(line);
            }

        }
    }
}
