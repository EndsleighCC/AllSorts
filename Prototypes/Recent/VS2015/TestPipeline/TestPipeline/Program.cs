using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace TestPipeline
{
    class Program
    {
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
                Command command = new Command(commandString.ToString(), true /* is script */ );
                Console.WriteLine("Command is \"{0}\"", commandString);

                command.Parameters.Add(null, parameter1);

                pipeline.Commands.Add(command);

                // Execute PowerShell script
                Console.WriteLine("Running script");
                Collection<PSObject> results = pipeline.Invoke();

                //Close the runspace.
                runspace.Close();

                StringBuilder resultsMessage = new StringBuilder();
                foreach (PSObject result in results)
                {
                    resultsMessage.Append(result.ToString());
                }

                Console.WriteLine("Results are: \"{0}\"", resultsMessage.ToString());

                return error;
            }

            private string _scriptName = null;
        }

        static void Main(string[] args)
        {
            string scriptName = "testrun.ps1";

            string scriptFullFilename = Path.Combine(Environment.CurrentDirectory, "..\\..\\", scriptName);
            scriptFullFilename = Path.GetFullPath(scriptFullFilename);
            if (File.Exists(scriptFullFilename))
            {
                TestRunspace testRunspace = new TestRunspace(scriptFullFilename);
                testRunspace.Execute();
            }
            else
            {
                Console.WriteLine("Script file \"{0}\" does not exist", scriptFullFilename);
            }

        }
    }
}
