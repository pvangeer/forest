
namespace StoryTree.Messaging
{
    public class LogMessage
    {
        public bool HasPriority { get; set; }

        public MessageSeverity Severity { get; set; }

        public string Message { get; set; }
    }
}