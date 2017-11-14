using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Diagnostics;
using System.IO;
using TestMessageQueueTypes;

namespace TestMessageQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverMachineName = null;
            string queueName = null;

            if (args.Length == 0)
            {
                serverMachineName = ".";
                queueName = QueueHelper.ThisMachineQueueName;
            }
            else
            {
                serverMachineName = args[0];
                queueName = QueueHelper.OtherMachineQueueName(args[0]);
            }

            Process actionMessageAgentProcess = null;

            string actionMessageAgentExe = @"..\..\..\TestActionMessageAgent\bin\debug\TestActionMessageAgent.exe";

            if (!File.Exists(actionMessageAgentExe))
            {
                Console.WriteLine("Message Agent \"{0}\" does not exist", actionMessageAgentExe);
            }
            else
            {
                if (serverMachineName == ".")
                {
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.Arguments = null;
                    start.FileName = actionMessageAgentExe;
                    start.WindowStyle = ProcessWindowStyle.Normal;
                    start.CreateNoWindow = false;

                    actionMessageAgentProcess = Process.Start(start);
                }
                else
                {
                    Console.WriteLine("Agent is remote on \"{0}\"", serverMachineName);
                }
            }

            ActionMessage actionMessage = new ActionMessage("This is a message from "+Environment.MachineName);

            MessageQueue actionQueue = new MessageQueue(queueName,QueueAccessMode.Send);

            try
            {
                Console.WriteLine("Sending message \"{0}\" to \"{1}\"", actionMessage.Message, serverMachineName);
                actionQueue.Send(actionMessage);
                Console.WriteLine("Message \"{0}\" sent to \"{1}\"", actionMessage.Message, serverMachineName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception sending message = {0}", ex.ToString());
                if (ex.InnerException != null )
                {
                    Console.WriteLine("Inner exception = {0}", ex.InnerException.ToString());
                }
            }

            if (actionMessageAgentProcess != null )
            {
                Console.WriteLine("Waiting for local agent to end");
                actionMessageAgentProcess.WaitForExit();

                // Retrieve the app's exit code
                Console.WriteLine("Message Agent Exit Code = {0}", actionMessageAgentProcess.ExitCode);

                actionMessageAgentProcess.Close();
                actionMessageAgentProcess = null;
            }

        }
    }
}
