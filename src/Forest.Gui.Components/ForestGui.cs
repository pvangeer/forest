using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Forest.Data;
using Forest.Data.Properties;
using Forest.Messaging;
using Forest.Storage;
using log4net;
using log4net.Repository.Hierarchy;

namespace Forest.Gui.Components
{
    public class ForestGui : IMessageCollection
    {
        public ForestGui()
        {
            ConfigureMessaging();
            BusyIndicator = StorageState.Idle;
            Messages = new MessageList();
            EventTreeProject = EventTreeProjectFactory.CreateStandardNewProject();
            ProjectFilePath = "";
            GuiProjectServices = new GuiProjectServices(this);
            SelectionManager = new SelectionManager(this);
            LogMessageAppender.Instance.MessageCollection = this;
        }

        public GuiProjectServices GuiProjectServices { get; }

        public SelectionManager SelectionManager { get; }

        public StorageState BusyIndicator { get; set; }

        public EventTreeProject EventTreeProject { get; set; }

        public string ProjectFilePath { get; set; }

        public VersionInfo VersionInfo { get; set; }

        // TODO: Implement
        public Func<ShouldProceedState> ShouldSaveOpenChanges { get; set; }

        // TODO: Implement
        public Func<bool> ShouldMigrateProject { get; set; }

        public MessageList Messages { get; }

        public ForestGuiState SelectedState { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ConfigureMessaging()
        {
            var rootLogger = ((Hierarchy)LogManager.GetRepository()).Root;

            if (!rootLogger.Appenders.Any(a => a is LogMessageAppender))
            {
                rootLogger.AddAppender(new LogMessageAppender());
                rootLogger.Repository.Configured = true;
            }
        }
    }
}