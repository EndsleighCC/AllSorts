using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace AccessDesktopWindows
{
    class Program
    {
        static List<Process> SystemProcessList()
        {
            List<Process> processList = null;

            Process[] processArray = Process.GetProcesses();

            processList = processArray.ToList<Process>();
            processList.Sort
            (
                delegate (Process processLeft, Process processRight)
                {
                    int compareValue = String.Compare(processLeft.ProcessName, processRight.ProcessName);

                    if (compareValue == 0)
                    {
                        compareValue = processLeft.Id - processRight.Id;
                    }

                    return compareValue;
                }
            );

            //int totalThreadCount = 0;
            //foreach (Process process in processList)
            //{
            //    if (!String.IsNullOrEmpty(process.MainWindowTitle))
            //    {
            //        Console.WriteLine("Process=\"{0}\", PID={1}, Window Title=\"{2}\", {3} responding, Thread Count={4}",
            //            process.ProcessName,
            //            process.Id,
            //            process.MainWindowTitle,
            //            (process.Responding ? "is" : "is not"),
            //            process.Threads.Count);
            //    }
            //    else
            //    {
            //        Console.WriteLine("Process=\"{0}\", PID={1}, Window Title=\"{2}\", {3} responding, Thread Count={4}",
            //            process.ProcessName,
            //            process.Id,
            //            "(no title)",
            //            (process.Responding ? "is" : "is not"),
            //            process.Threads.Count);
            //    }
            //    totalThreadCount += process.Threads.Count;
            //} // foreach

            // Console.WriteLine();
            // Console.WriteLine("Total threads = {0}", totalThreadCount);

            return processList;
        } // SystemProcessList

        static void Main(string[] args)
        {
            // https://stackoverflow.com/questions/7268302/get-the-titles-of-all-open-windows

            while ( ! Console.KeyAvailable)
            {
                List<Process> processList = SystemProcessList();
                Process processNotepad = null;
                try
                {
                    processNotepad = processList.First<Process>(p =>
                                String.Compare(p.ProcessName, "notepad", true /* ignore case */ ) == 0);
                }
                catch (InvalidOperationException)
                {
                    // No process of the selected name
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception : {0}", ex.ToString());
                }

                if (processNotepad == null)
                {
                    Console.WriteLine("No notepad");
                }
                else
                {
                    Console.WriteLine("Notepad \"{0}\" as \"{1}\" is running",
                        processNotepad.ProcessName, processNotepad.MainWindowTitle);
                }
                Thread.Sleep(50);
            } // while

            // Empty the input buffer
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(false);
            }
            Console.WriteLine();
        }
    }
}
