using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using StoryTree.Data;
using StoryTree.Data.Annotations;
using StoryTree.Gui.ViewModels;
using StoryTree.Messaging;

namespace StoryTree.Gui
{
    public class StoryTreeGui : IMessageCollection
    {
        public StoryTreeGui()
        {
            ConfigureMessaging();
            BusyIndicator = StorageState.Idle;
            Messages = new ObservableCollection<LogMessage>();
            Project = new Project();
            ProjectFilePath = "";

            LogMessageAppender.Instance.MessageCollection = this;
        }

        public StorageState BusyIndicator { get; set; }

        public Project Project { get; set; }

        public string ProjectFilePath { get; set; }

        public ObservableCollection<LogMessage> Messages { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ConfigureMessaging()
        {
            Logger rootLogger = ((Hierarchy)LogManager.GetRepository()).Root;

            if (!rootLogger.Appenders.Cast<IAppender>().Any(a => a is LogMessageAppender))
            {
                rootLogger.AddAppender(new LogMessageAppender());
                rootLogger.Repository.Configured = true;
            }
        }
    }
}