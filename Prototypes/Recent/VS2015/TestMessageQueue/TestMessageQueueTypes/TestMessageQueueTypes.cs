using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestMessageQueueTypes
{
    public static class Constants
    {
        public const string PartialActionQueueName = @"\ActionQueue";
    }

    public static class QueueHelper
    {
        public static string ThisMachineQueueName { get { return "." + Constants.PartialActionQueueName; } }
        public static string OtherMachineQueueName( string machineName )
        {
            return machineName + Constants.PartialActionQueueName;
        }
    }

    [Serializable]
    public class ActionMessage
    {
        public ActionMessage()
        {
        }

        public ActionMessage(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
