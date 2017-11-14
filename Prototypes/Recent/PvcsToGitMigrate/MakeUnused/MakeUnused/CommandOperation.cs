using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace CommandOperations
{
    public static class CommandOperation
    {
        public enum DebugProgress
        {
            None,
            Enabled
        }

        public enum CommandOutputDisplayType
        {
            None,
            StandardOutputOnly,
            StandardErrorOnly,
            StandardOutputAndStandardError
        }

        public static bool RunCommand(string workingDirectory,
                                        string command,
                                        DebugProgress debugProgress,
                                        CommandOutputDisplayType commandOutputDisplay)
        {
            return RunCommand(workingDirectory, command, debugProgress, commandOutputDisplay, null, null);
        } // RunCommand

        public static bool RunCommand(string workingDirectory,
                                      string command,
                                      DebugProgress debugProgress,
                                      CommandOutputDisplayType commandOutputDisplay,
                                      List<string> standardOutputBuffer,
                                      List<string> standardErrorBuffer)
        {
            bool success = false;

            string commandInterpreter = "cmd.exe";
            string commandInterpreterArguments = String.Format("/c {0}", command);

            ProcessStartInfo processStartInfo = new ProcessStartInfo(commandInterpreter);
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = commandInterpreterArguments;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.WorkingDirectory = workingDirectory;

            // Save the current Working Directory
            string currentWorkingDirectory = Directory.GetCurrentDirectory();

            Process processCommand = new Process();
            processCommand.StartInfo = processStartInfo;

            processCommand.OutputDataReceived += new DataReceivedEventHandler(AsyncStandardOutputHandler);
            _standardOutputLineCollection.Clear();

            processCommand.ErrorDataReceived += new DataReceivedEventHandler(AsyncStandardErrorHandler);
            _standardErrorLineCollection.Clear();

            try
            {
                if (debugProgress == DebugProgress.Enabled)
                {
                    Console.WriteLine("Running \"{0}\" \"{1}\"", commandInterpreter, commandInterpreterArguments);
                }

                processCommand.Start();

                processCommand.BeginOutputReadLine();
                processCommand.BeginErrorReadLine();

                processCommand.StandardInput.WriteLine("\r\n");

                processCommand.WaitForExit();

                success = processCommand.ExitCode == 0;

                if ((debugProgress == DebugProgress.Enabled) || (!success))
                {
                    Console.WriteLine("--- Running \"{0}\" produced Exit Code {1}", command, processCommand.ExitCode);
                }

                if ((_standardOutputLineCollection.Count > 0)
                    && ((commandOutputDisplay == CommandOutputDisplayType.StandardOutputOnly)
                            || (commandOutputDisplay == CommandOutputDisplayType.StandardOutputAndStandardError)
                            || (standardOutputBuffer != null)
                        )
                    )
                {
                    if (debugProgress == DebugProgress.Enabled)
                    {
                        Console.WriteLine("Executable {0} : \"{1}\" Standard Output:",
                            (success ? "Succeeded" : "Failed"), command);
                    }
                    foreach (string outputLine in _standardOutputLineCollection)
                    {
                        if (outputLine != null)
                        {
                            if (standardOutputBuffer != null)
                            {
                                standardOutputBuffer.Add(outputLine);
                            }
                            if ((commandOutputDisplay == CommandOutputDisplayType.StandardOutputOnly)
                                    || (commandOutputDisplay == CommandOutputDisplayType.StandardOutputAndStandardError)
                                )
                            {
                                Console.WriteLine("    " + outputLine);
                            }
                        }
                    }
                }

                if ((_standardErrorLineCollection.Count > 0)
                    && ((commandOutputDisplay == CommandOutputDisplayType.StandardErrorOnly)
                            || (commandOutputDisplay == CommandOutputDisplayType.StandardOutputAndStandardError)
                            || standardErrorBuffer != null
                        )
                    )
                {
                    if (debugProgress == DebugProgress.Enabled)
                    {
                        Console.WriteLine("Executable {0} : \"{1}\" Standard Error:",
                            (success ? "Succeeded" : "Failed"), command);
                    }
                    foreach (string errorLine in _standardErrorLineCollection)
                    {
                        if (standardErrorBuffer != null)
                        {
                            standardErrorBuffer.Add(errorLine);
                        }
                        if ((commandOutputDisplay == CommandOutputDisplayType.StandardErrorOnly)
                            || (commandOutputDisplay == CommandOutputDisplayType.StandardOutputAndStandardError)
                            )
                        {
                            Console.WriteLine("    " + errorLine);
                        }
                    }
                }

            }
            catch (Exception eek)
            {
                Console.WriteLine("    *** {0}.{1} : Executing \"{2}\" exception \"{3}\"",
                                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    MethodBase.GetCurrentMethod().Name,
                                    command,
                                    eek);
            }

            _standardOutputLineCollection.Clear();
            _standardErrorLineCollection.Clear();

            return success;

        } // RunCommand

        private static List<string> _standardOutputLineCollection = new List<string>();

        private static void AsyncStandardOutputHandler(object sendingProcess,
                                                        DataReceivedEventArgs outLine)
        {
            if (outLine.Data != null)
            {
                _standardOutputLineCollection.Add(outLine.Data);
            }
        }

        private static List<string> _standardErrorLineCollection = new List<string>();

        private static void AsyncStandardErrorHandler(object sendingProcess,
                                                      DataReceivedEventArgs errorLine)
        {
            if (errorLine.Data != null)
            {
                _standardOutputLineCollection.Add(errorLine.Data);
            }
        }

        public static bool RunMonitoredCommand(string workingDirectory,
                                               string command,
                                               int indent,
                                               DebugProgress debugProgress,
                                               CommandOutputDisplayType commandOutputDisplay)
        {
            return RunMonitoredCommand(workingDirectory,
                                       command,
                                       indent,
                                       debugProgress,
                                       commandOutputDisplay,
                                       null,
                                       null);
        } // RunMonitoredCommand

        public static bool RunMonitoredCommand(string workingDirectory,
                                               string command,
                                               int indent,
                                               DebugProgress debugProgress,
                                               CommandOutputDisplayType commandOutputDisplay,
                                               List<string> standardOutputBuffer,
                                               List<string> standardErrorBuffer)
        {
            bool success = false;

            if (RunCommand(workingDirectory, command, debugProgress, commandOutputDisplay, standardOutputBuffer, standardErrorBuffer))
            {
                success = true;
                Console.WriteLine("{0}Succeeded \"{1}\"",
                        Indent(indent + 1),
                        command);
            }
            else
            {
                success = false;
                Console.WriteLine("{0}*** Failed \"{1}\"",
                        Indent(indent + 1),
                        command);
            }

            // Switch to the branch

            return success;

        } // RunMonitoredCommand

        private static string Indent(int indent)
        {
            return new String(' ', indent * 4);
        }

    } // CommandOperation
}
