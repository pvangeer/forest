using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using StoryTree.Data;
using StoryTree.Data.Properties;
using StoryTree.Gui.Command;
using StoryTree.IO.Export;
using StoryTree.IO.Import;
using StoryTree.Messaging;

namespace StoryTree.Gui.ViewModels
{
    public class GuiViewModel : INotifyPropertyChanged
    {
        private ProjectViewModel projectViewModel;
        private MessageListViewModel messageListViewModel;
        private StoryTreeProcess selectedProcess;
        private readonly StoryTreeGui gui;

        public GuiViewModel() : this(new StoryTreeGui()) { }

        public GuiViewModel(StoryTreeGui gui)
        {
            this.gui = gui;
            if (this.gui != null)
            {
                gui.ShouldMigrateProject = ShouldMigrateProject;
                this.gui.PropertyChanged += GuiPropertyChanged;
                this.gui.Messages.CollectionChanged += GuiMessagesCollectionChanged;
                projectViewModel = new ProjectViewModel(this.gui.EventTreeProject);
                this.gui.ShouldSaveOpenChanges = ShouldSaveOpenChanges;
            }
        }

        private bool ShouldMigrateProject()
        {
            var messageBoxText =
                "U wilt een verouderd bestand openen. Wilt u dit bestand migreren naar het nieuwe format om het te kunnen openen?";
            var caption = "Bestand migreren naar nieuwste versie";
            var messageBoxResult =
                MessageBox.Show(messageBoxText, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);

            return messageBoxResult == MessageBoxResult.Yes;
        }

        private ShouldProceedState ShouldSaveOpenChanges()
        {
            var messageBoxText = "U heeft aanpassingen aan uw project nog niet opgeslagen. Wilt u dat alsnog doen?";
            var caption = "Aanpassingen opslaan";
            var messageBoxResult =
                MessageBox.Show(messageBoxText, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            return messageBoxResult == MessageBoxResult.Yes ? ShouldProceedState.Yes :
                messageBoxResult == MessageBoxResult.No ? ShouldProceedState.No :
                ShouldProceedState.Cancel;
        }

        private void GuiMessagesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newItem = e.NewItems.OfType<LogMessage>().First();
                if (newItem.HasPriority)
                {
                    PriorityMessage = newItem;
                    OnPropertyChanged(nameof(PriorityMessage));
                }
                OnPropertyChanged(nameof(MessagesViewModel));
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var items = e.OldItems.OfType<LogMessage>();
                foreach (var logMessage in items)
                {
                    if (PriorityMessage == logMessage)
                    {
                        PriorityMessage = null;
                        OnPropertyChanged(nameof(PriorityMessage));
                    }
                }

                if (!MessagesViewModel.MessageList.Any())
                {
                    ShowMessages = false;
                    OnPropertyChanged(nameof(ShowMessages));
                }
                OnPropertyChanged(nameof(MessagesViewModel));
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                PriorityMessage = null;
                OnPropertyChanged(nameof(PriorityMessage));
                OnPropertyChanged(nameof(MessagesViewModel));
                ShowMessages = false;
                OnPropertyChanged(nameof(ShowMessages));
            }
        }

        public bool ShowMessages { get; set; }

        public LogMessage PriorityMessage { get; set; }

        public StorageState BusyIndicator
        {
            get => gui.BusyIndicator;
            set => gui.BusyIndicator = value;
        }

        public ProjectViewModel ProjectViewModel => projectViewModel;

        public MessageListViewModel MessagesViewModel =>
            messageListViewModel ?? (messageListViewModel = new MessageListViewModel(gui.Messages));

        public string ProjectFilePath
        {
            get => gui.ProjectFilePath;
            set
            {
                gui.ProjectFilePath = value;
                OnPropertyChanged(nameof(ProjectFileName));
            }
        }

        public Window Win32Window { get; set; }

        public ICommand FileNewCommand => new FileNewCommnd(this);

        public ICommand SaveProjectCommand => new SaveProjectCommand(this);

        public ICommand SaveProjectAsCommand => new SaveProjectAsCommand(this);

        public ICommand OpenProjectCommand => new OpenProjectCommand(this.gui.GuiProjectServices);

        public ICommand RemoveLastMessageCommand => new RemovePriorityMessageCommand(this);

        public ICommand ShowMessageListCommand => new ShowMessageListCommand(this);

        public StoryTreeProcess SelectedProcess
        {
            get => selectedProcess;
            set
            {
                selectedProcess = value;
                OnPropertyChanged();
                ProjectViewModel.OnProcessChanged();
            }
        }

        public ICommand ChangeProcessStepCommand => new ChangeProcessStepCommand(this);

        public string ProjectFileName => string.IsNullOrEmpty(ProjectFilePath)
            ? "Nieuw bestand*"
            : Path.GetFileNameWithoutExtension(ProjectFilePath);

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler OnInvalidateVisual;

        private void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(StoryTreeGui.BusyIndicator):
                    OnPropertyChanged(nameof(BusyIndicator));
                    break;
                case nameof(StoryTreeGui.EventTreeProject):
                    projectViewModel = new ProjectViewModel(gui.EventTreeProject);
                    OnPropertyChanged(nameof(ProjectViewModel));
                    break;
                case nameof(StoryTreeGui.Messages):
                    messageListViewModel = null;
                    OnPropertyChanged(nameof(MessagesViewModel));
                    break;
                case nameof(StoryTreeGui.ProjectFilePath):
                    OnPropertyChanged(nameof(ProjectFilePath));
                    OnPropertyChanged(nameof(ProjectFileName));
                    break;
            }
        }

        public void InvokeInvalidateVisual()
        {
            OnInvalidateVisual?.Invoke(this, null);
        }

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnExportElicitationForms(string fileLocation, string prefix, Expert[] expertsToExport, EventTree eventTreeToExport)
        {
            var exporter = new ElicitationFormsExporter(ProjectViewModel.EventTreeProject);
            exporter.Export(fileLocation, prefix, expertsToExport, eventTreeToExport);
        }

        public void OnImportElicitationForms(string[] fileLocations)
        {
            var importer = new ElicitationFormImporter(ProjectViewModel.EventTreeProject);
            foreach (var fileLocation in fileLocations)
            {
                importer.Import(fileLocation);
            }
        }

        public void NewProject()
        {
            gui.GuiProjectServices.NewProject();
        }

        public void OpenProject()
        {
            gui.GuiProjectServices.OpenProject();
        }

        public void SaveProjectAs()
        {
            gui.GuiProjectServices.SaveProjectAs();
        }

        public bool CanSaveProject()
        {
            return gui.EventTreeProject != null;
        }

        public void SaveProject()
        {
            gui.GuiProjectServices.SaveProject();
        }

        public bool ForcedClosingMainWindow()
        {
            return gui.GuiProjectServices.HandleUnsavedChanges(() => { });
        }
    }
}