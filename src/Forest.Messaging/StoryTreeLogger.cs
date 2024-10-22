using System;
using log4net;

namespace Forest.Messaging
{
    public class StoryTreeLog
    {
        private readonly ILog log;

        public StoryTreeLog(Type loggerType)
        {
            log = LogManager.GetLogger(loggerType);
        }

        public void Error(string message, bool hasPriority = true)
        {
            log.Error(new LogMessage
            {
                Severity = MessageSeverity.Error,
                Message = message,
                HasPriority = hasPriority
            });
        }

        public void Warning(string message, bool hasPriority = false)
        {
            log.Warn(new LogMessage
            {
                Severity = MessageSeverity.Warning,
                Message = message,
                HasPriority = hasPriority
            });
        }

        public void Info(string message, bool hasPriority = false)
        {
            log.Info(new LogMessage
            {
                Severity = MessageSeverity.Information,
                Message = message,
                HasPriority = hasPriority
            });
        }
    }
}