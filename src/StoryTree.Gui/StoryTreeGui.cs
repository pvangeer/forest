using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using StoryTree.Data;
using StoryTree.Data.Properties;
using StoryTree.Gui.ViewModels;
using StoryTree.Messaging;
using StoryTree.Storage;

namespace StoryTree.Gui
{
    public class StoryTreeGui : IMessageCollection
    {
        public StoryTreeGui()
        {
            ConfigureMessaging();
            BusyIndicator = StorageState.Idle;
            Messages = new MessageList();
            EventTreeProject = new EventTreeProject();
            ProjectFilePath = "";
            GuiProjectServices = new GuiProjectServices(this);
            LogMessageAppender.Instance.MessageCollection = this;
        }

        public GuiProjectServices GuiProjectServices { get; }

        public StorageState BusyIndicator { get; set; }

        public EventTreeProject EventTreeProject { get; set; }

        public string ProjectFilePath { get; set; }

        public Func<ShouldProceedState> ShouldSaveOpenChanges { get; set; }

        public VersionInfo VersionInfo { get; set; }

        public Func<bool> ShouldMigrateProject { get; set; }

        public MessageList Messages { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ConfigureMessaging()
        {
            var rootLogger = ((Hierarchy)LogManager.GetRepository()).Root;

            if (!rootLogger.Appenders.Cast<IAppender>().Any(a => a is LogMessageAppender))
            {
                rootLogger.AddAppender(new LogMessageAppender());
                rootLogger.Repository.Configured = true;
            }
        }
    }
}