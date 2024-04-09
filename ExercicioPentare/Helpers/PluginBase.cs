using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExercicioPentare.Helpers
{
    internal class PluginBase
    {
        public enum MessageName
        {
            Create
        }

        public enum Stage
        {
            PreValidation = 10,
            PreOperation = 20,
            PostOperation = 40
        }

        public enum Mode
        {
            Asynchronous = 1,
            Synchronous = 0
        }

        public static bool Validate(IPluginExecutionContext context, MessageName message, Stage stage, Mode mode)
        {
            return (MessageName)System.Enum.Parse(typeof(MessageName), context.MessageName) == message
                && (Stage)context.Stage == stage
                && (Mode)context.Mode == mode;
        }
    }
}