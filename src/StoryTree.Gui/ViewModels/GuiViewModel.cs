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

        public GuiViewModel() : this(new StoryTreeGui()) { }

        public GuiViewModel(StoryTreeGui gui)
        {
            GuiProjectSercices = new GuiProjectServices(this);
            Gui = gui;
            if (Gui != null)
            {
                Gui.PropertyChanged += GuiPropertyChanged;
                Gui.Messages.CollectionChanged += GuiMessagesCollectionChanged;
                projectViewModel = new ProjectViewModel(Gui.Project);
                Gui.ShouldSaveOpenChanges = ShouldSaveOpenChanges;
            }
        }

        private bool ShouldSaveOpenChanges()
        {
            string messageBoxText = "U heeft aanpassingen aan uw project nog niet opgeslagen. Wilt u dat alsnog doen?";
            string caption = "Aanpassingen opslaan";

            MessageBoxButton messageBoxType = MessageBoxButton.YesNo;
            MessageBoxImage messageBoxImage = MessageBoxImage.Question;

            MessageBoxResult messageBoxResult = MessageBox.Show(messageBoxText, caption, messageBoxType, messageBoxImage);

            return messageBoxResult == MessageBoxResult.Yes;
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

        public GuiProjectServices GuiProjectSercices { get; }

        public StoryTreeGui Gui { get; }

        public StorageState BusyIndicator
        {
            get => Gui.BusyIndicator;
            set => Gui.BusyIndicator = value;
        }

        public ProjectViewModel ProjectViewModel => projectViewModel;

        public MessageListViewModel MessagesViewModel =>
            messageListViewModel ?? (messageListViewModel = new MessageListViewModel(Gui.Messages));

        public string ProjectFilePath
        {
            get => Gui.ProjectFilePath;
            set
            {
                Gui.ProjectFilePath = value;
                OnPropertyChanged(nameof(ProjectFileName));
            }
        }

        public Window Win32Window
        {
            get => GuiProjectSercices.Win32Window;
            set => GuiProjectSercices.Win32Window = value;
        }

        public ICommand FileNewCommand => new FileNewCommnd(this);

        public ICommand SaveProjectCommand => new SaveProjectCommand(this);

        public ICommand SaveProjectAsCommand => new SaveProjectAsCommand(this);

        public ICommand OpenProjectCommand => new OpenProjectCommand(this);

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
                case nameof(StoryTreeGui.Project):
                    projectViewModel = new ProjectViewModel(Gui.Project);
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

        public void OnExportElicitationForms(string fileLocation, string prefix, Expert[] expertsToExport, EventTree[] eventTreesToExport)
        {
            var exporter = new ElicitationFormsExporter(ProjectViewModel.Project);
            exporter.Export(fileLocation, prefix, expertsToExport, eventTreesToExport);
        }

        public void OnImportElicitationForms(string[] fileLocations)
        {
            var importer = new ElicitationFormImporter(ProjectViewModel.Project);
            foreach (var fileLocation in fileLocations)
            {
                importer.Import(fileLocation);
            }
        }
    }
}