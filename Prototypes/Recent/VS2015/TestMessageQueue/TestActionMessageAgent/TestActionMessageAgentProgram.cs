using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using TestMessageQueueTypes;

namespace TestActionMessageAgent
{
    class TestActionMessageAgentProgram
    {
        static void Main(string[] args)
        {
            bool messageQueueExists = false;

            if (MessageQueue.Exists(QueueHelper.ThisMachineQueueName))
            {
                Console.WriteLine("Message Queue \"{0}\" already exists", QueueHelper.ThisMachineQueueName);
                messageQueueExists = true;
            }
            else
            {
                try
                {
                    MessageQueue.Create(QueueHelper.ThisMachineQueueName);
                    Console.WriteLine("Created Message Queue \"{0}\"", QueueHelper.ThisMachineQueueName);
                    messageQueueExists = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception creating Message Queue \"{0}\" = {1}",
                                        QueueHelper.ThisMachineQueueName, ex.ToString());
                    messageQueueExists = false;
                }
            }

            if (messageQueueExists)
            {
                MessageQueue actionQueue = new MessageQueue(QueueHelper.ThisMachineQueueName, QueueAccessMode.Receive);

                actionQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(ActionMessage) });

                try
                {
                    Console.WriteLine("ActionMessageAgent : Reading message");
                    Message message = actionQueue.Receive();
                    ActionMessage actionMessage = (ActionMessage)message.Body;
                    Console.WriteLine("ActionMessageAgent : Queue Message received was \"{0}\"", actionMessage.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ActionMessageAgent : Exception reading queue = {0}", ex.ToString());
                }

            }
            Console.WriteLine();
            Console.WriteLine("Exiting - press enter");
            string input = Console.ReadLine();
        }
    }
}
