using System;
using log4net.Appender;
using log4net.Core;

namespace StoryTree.Messaging
{
    public class LogMessageAppender : AppenderSkeleton
    {
        public IMessageCollection MessageCollection { get; set; }

        public LogMessageAppender()
        {
            Instance = this;
        }

        public static LogMessageAppender Instance { get; set; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (!(loggingEvent.MessageObject is LogMessage message))
            {
                var severity = GetMessageSeverityFromNativeLevel(loggingEvent.Level);
                message = new LogMessage
                {
                    Severity = severity,
                    Message = loggingEvent.RenderedMessage,
                    HasPriority = severity == MessageSeverity.Error
                };
            }

            MessageCollection.Messages.Insert(0,message);

            /*if (message.HasPriority)
            {
                MessageCollection.PriorityMessage = message;
                MessageCollection.OnPropertyChanged(nameof(IMessageCollection.PriorityMessage));
            }*/
        }

        private MessageSeverity GetMessageSeverityFromNativeLevel(Level loggingEventLevel)
        {
            if (loggingEventLevel == Level.Critical ||
                loggingEventLevel == Level.Emergency ||
                loggingEventLevel == Level.Error ||
                loggingEventLevel == Level.Fatal ||
                loggingEventLevel == Level.Severe)
            {
                return MessageSeverity.Error;
            }

            if (loggingEventLevel == Level.Warn)
            {
                return MessageSeverity.Warning;
            }

            return MessageSeverity.Information;
        }
    }
}
