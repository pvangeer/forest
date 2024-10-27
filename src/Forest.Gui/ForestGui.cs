using System;
using System.Dynamic;
using System.Linq;
using Forest.Data;
using Forest.Messaging;
using Forest.Storage;
using log4net;
using log4net.Repository.Hierarchy;

namespace Forest.Gui
{
    public class ForestGui : NotifyPropertyChangedObject, IMessageCollection
    {
        public ForestGui()
        {
            ConfigureMessaging();
            BusyIndicator = StorageState.Idle;
            Messages = new MessageList();
            ForestAnalysis = ForestAnalysisFactory.CreateStandardNewProject();
            ProjectFilePath = "";
            GuiProjectServices = new GuiProjectServices(this);
            SelectionManager = new SelectionManager(this);
            LogMessageAppender.Instance.MessageCollection = this;
        }

        public GuiProjectServices GuiProjectServices { get; }

        public SelectionManager SelectionManager { get; }

        public StorageState BusyIndicator { get; set; }

        public bool IsSaveToImage { get; set; }

        public ForestAnalysis ForestAnalysis { get; set; }

        public string ProjectFilePath { get; set; }

        public VersionInfo VersionInfo { get; set; }

        public Func<ShouldProceedState> ShouldSaveOpenChanges { get; set; }

        public Func<bool> ShouldMigrateProject { get; set; }

        public ForestGuiState SelectedState { get; set; }

        public MessageList Messages { get; }

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