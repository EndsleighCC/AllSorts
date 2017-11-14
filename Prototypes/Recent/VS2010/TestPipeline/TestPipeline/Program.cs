using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using System.Management.Automation ;
using System.Management.Automation.Runspaces ;
using System.Threading;

namespace TestPipeline
{
    internal class Program
    {
        public class TestPowershell
        {
            public TestPowershell(string commandFilePathAndName)
            {
                _powershell = PowerShell.Create();
                _powershell.AddCommand(commandFilePathAndName);
            }

            public void AddParameter(string parameter)
            {
                _powershell.AddParameter(null,parameter);
            }

            public Collection<PSObject> Execute()
            {
                Console.WriteLine("Running script from Powershell Object");
                Collection<PSObject> powershellResults = _powershell.Invoke();
                StringBuilder resultsMessage = new StringBuilder();
                foreach (PSObject powershellResult in powershellResults)
                {
                    resultsMessage.Append(powershellResult.ToString());
                }
                Console.WriteLine("Results are:");
                Console.WriteLine(resultsMessage);
                return powershellResults;
            }

            public Collection<string> ExecuteAsynchronously()
            {
                Console.WriteLine("Running script asynchronously from Powershell Object");

                PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
                outputCollection.DataAdded += Output_DataAdded;
                _powershell.Streams.Error.DataAdded += Error_DataAdded;

                IAsyncResult result = _powershell.BeginInvoke<PSObject, PSObject>(null, outputCollection);

                while (!result.IsCompleted)
                {
                    Console.WriteLine("Waiting for asynchonous result");
                    Thread.Sleep(2000);
                }

                Console.WriteLine("Asynchronous Execution ends");

                Collection<string> outputResults = new Collection<string>();

                Console.WriteLine("Results are:");
                foreach (PSObject outputItem in outputCollection)
                {
                    string outputLine = outputItem.BaseObject.ToString();
                    
                    outputResults.Add(outputLine);
                    // Line should already contain an End-Of-Line character
                    Console.Write(outputLine);
                }
                return outputResults;
            }

            public void Output_DataAdded(object sender, DataAddedEventArgs e)
            {
                var outputCollection = (PSDataCollection<PSObject>)sender;
                PSObject psObject = outputCollection[e.Index];
                // Line should already contain an End-Of-Line character
                Console.Write("Output: {0}",psObject.BaseObject.ToString());
            }

            public void Error_DataAdded(object sender, DataAddedEventArgs e)
            {
                var errorCollection = (PSDataCollection<ErrorRecord>)sender;
                ErrorRecord errorRecord = errorCollection[e.Index];
                Console.WriteLine("Error: {0}", errorRecord.ToString());
            }

            private PowerShell _powershell = null;
        }

    public class TestRunspace
        {
            public TestRunspace(string scriptName)
            {
                _scriptName = scriptName;
            }

            public int Execute()
            {
                int error = 0;

                Runspace runspace = null;
                runspace = RunspaceFactory.CreateRunspace();
                runspace.Open();

                Pipeline pipeline = runspace.CreatePipeline();
            
                //Construct the powershell command and add it to the pipeline.
                String parameter1 = "From the hosting application";
                StringBuilder commandString = new StringBuilder();
                commandString.Append(_scriptName);
                Command command = new System.Management.Automation.Runspaces.Command(commandString.ToString());
                Console.WriteLine( "Command is \"{0}\"",commandString);

                // CommandParameter commandParameter = new CommandParameter("$FromHostApp",parameter1);
                CommandParameter commandParameter = new CommandParameter(null,parameter1);
                command.Parameters.Add(commandParameter);

                pipeline.Commands.Add(command);

                // Execute PowerShell script
                Console.WriteLine("Running script");
                Collection<PSObject> results = null;
                try
                {
                    results = pipeline.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception from Invoke = {0}",ex.ToString());
                }

                if (results != null)
                {
                    StringBuilder resultsMessage = new StringBuilder();
                    foreach (PSObject result in results)
                    {
                        resultsMessage.Append(result.ToString());
                    }

                    Console.WriteLine("Results are:");
                    Console.WriteLine(resultsMessage);
                }

                //Close the runspace.
                runspace.Close();

                return error;
            }

            private string _scriptName = null;
        }

        static void Main(string[] args)
        {
            string scriptName = "testrun.ps1";

            string scriptFullFilename = Path.Combine(Environment.CurrentDirectory, "..\\..\\" , scriptName);
            scriptFullFilename = Path.GetFullPath(scriptFullFilename);
            if (File.Exists(scriptFullFilename))
            {
                TestRunspace testRunspace = new TestRunspace(scriptFullFilename);
                testRunspace.Execute();

                TestPowershell testPowershell = new TestPowershell(scriptFullFilename);
                testPowershell.AddParameter("From the powershell object");
                testPowershell.Execute();

                testPowershell = new TestPowershell(scriptFullFilename);
                testPowershell.AddParameter("From the asynchronous powershell object");
                testPowershell.ExecuteAsynchronously();
            }
            else
            {
                Console.WriteLine("Script file \"{0}\" does not exist", scriptFullFilename);
            }

        }
    }
}
