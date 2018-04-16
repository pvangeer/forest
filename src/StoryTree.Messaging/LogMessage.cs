
using System;

namespace StoryTree.Messaging
{
    public class LogMessage
    {
        public LogMessage()
        {
            Time = DateTime.Now;
        }

        public DateTime Time { get; }

        public bool HasPriority { get; set; }

        public MessageSeverity Severity { get; set; }

        public string Message { get; set; }
    }
}