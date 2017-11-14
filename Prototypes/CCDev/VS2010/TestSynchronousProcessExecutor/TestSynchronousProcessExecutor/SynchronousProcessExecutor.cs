using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace TestSynchronousProcessExecutor
{
    public class SynchronousProcessExecutor
    {

        #region Public Member Functions

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

        public bool Execute(string workingDirectory,
                            string command,
                            DebugProgress debugProgress,
                            CommandOutputDisplayType commandOutputDisplay)
        {
            return Execute(workingDirectory, command, debugProgress, commandOutputDisplay, null, null);
        } // Execute

        public bool Execute(string workingDirectory,
                            string command,
                            DebugProgress debugProgress,
                            CommandOutputDisplayType commandOutputDisplay,
                            List<string> standardOutputBuffer,
                            List<string> standardErrorBuffer)
        {
            _success = false;

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

                _success = processCommand.ExitCode == 0;

                // If the command failed generate a message unconditionally
                if ((debugProgress == DebugProgress.Enabled) || (!_success))
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
                            (_success ? "Succeeded" : "Failed"), command);
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
                            (_success ? "Succeeded" : "Failed"), command);
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
                Console.WriteLine("*** {0}.{1} : Executing \"{2}\" exception \"{3}\"",
                                  MethodBase.GetCurrentMethod().DeclaringType.Name,
                                  MethodBase.GetCurrentMethod().Name,
                                  command,
                                  eek);
            }

            return _success;

        } // Execute

        #endregion

        #region Public Properties

        public bool Success
        {
            get
            {
                return _success;
            }
        }

        public ReadOnlyCollection<string> StandardOutputBuffer
        {
            get { return new ReadOnlyCollection<string>(_standardOutputLineCollection); }
        }

        public ReadOnlyCollection<string> StandardErrorBuffer
        {
            get { return new ReadOnlyCollection<string>(_standardErrorLineCollection); }
        }

        public string Indent(int indent)
        {
            return new String(' ', indent * 4);
        }

        #endregion

        #region Private Member Functions

        private void AsyncStandardOutputHandler(object sendingProcess,
                                                       DataReceivedEventArgs outLine)
        {
            if (outLine.Data != null)
            {
                _standardOutputLineCollection.Add(outLine.Data);
            }
        }

        private void AsyncStandardErrorHandler(object sendingProcess,
                                               DataReceivedEventArgs errorLine)
        {
            if (errorLine.Data != null)
            {
                _standardOutputLineCollection.Add(errorLine.Data);
            }
        }

        #endregion

        #region Private Member Variables

        private bool _success = false;
        private List<string> _standardOutputLineCollection = new List<string>();
        private List<string> _standardErrorLineCollection = new List<string>();

        #endregion

    } // SynchronousProcessExecutor
}
