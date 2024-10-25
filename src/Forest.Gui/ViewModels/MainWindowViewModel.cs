﻿using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Forest.Data.Estimations;
using Forest.Data.Estimations.PerTreeEvet;
using Forest.Data.Experts;
using Forest.Data.Properties;
using Forest.Gui.Command;
using Forest.Gui.Components;
using Forest.IO.Export;
using Forest.IO.Import;
using Forest.Messaging;
using Forest.Visualization.Dialogs;
using Forest.Visualization.ViewModels;

namespace Forest.Gui.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ForestGui gui;
        private MessageListViewModel messageListViewModel;

        public MainWindowViewModel() : this(new ForestGui())
        {
        }

        public MainWindowViewModel(ForestGui gui)
        {
            this.gui = gui;
            if (this.gui != null)
            {
                gui.GuiProjectServices.SaveProjectFileNameFunc = FileDialogFactory.AskUserForFileNameToSaveToFunc();
                gui.GuiProjectServices.OpenProjectFileNameFunc = FileDialogFactory.AskUserForFileNameToOpenFunc();
                gui.ShouldMigrateProject = ShouldMigrateProject;
                this.gui.PropertyChanged += GuiPropertyChanged;
                this.gui.Messages.CollectionChanged += GuiMessagesCollectionChanged;
                ContentPresenterViewModel = new ContentPresenterViewModel(gui);
                RibbonViewModel = new RibbonViewModel(gui);
                this.gui.ShouldSaveOpenChanges = ShouldSaveOpenChanges;
            }
        }


        public ICommand RemoveLastMessageCommand => new RemovePriorityMessageCommand(this);

        public ICommand ShowMessageListCommand => new ShowMessageListCommand(this);

        public ContentPresenterViewModel ContentPresenterViewModel { get; }

        public RibbonViewModel RibbonViewModel { get; }

        public event PropertyChangedEventHandler PropertyChanged;

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
                    if (PriorityMessage == logMessage)
                    {
                        PriorityMessage = null;
                        OnPropertyChanged(nameof(PriorityMessage));
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

        private void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.BusyIndicator):
                    OnPropertyChanged(nameof(BusyIndicator));
                    break;
                case nameof(ForestGui.Messages):
                    messageListViewModel = null;
                    OnPropertyChanged(nameof(MessagesViewModel));
                    break;
                case nameof(ForestGui.ProjectFilePath):
                    OnPropertyChanged(nameof(ProjectFileName));
                    break;
            }
        }

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnExportElicitationForms(string fileLocation, string prefix, Expert[] expertsToExport,
            ProbabilityEstimationPerTreeEvent estimationToExport)
        {
            var exporter = new ElicitationFormsExporter(estimationToExport);
            exporter.Export(fileLocation, prefix, expertsToExport, estimationToExport);
        }

        public void OnImportElicitationForms(string[] fileLocations)
        {
            var estimationToImportTo = gui.SelectionManager.Selection as ProbabilityEstimationPerTreeEvent;
            if (estimationToImportTo == null)
                return;
            var importer = new ElicitationFormImporter(estimationToImportTo);
            foreach (var fileLocation in fileLocations)
                importer.Import(fileLocation);
        }

        public bool ForcedClosingMainWindow()
        {
            return gui.GuiProjectServices.HandleUnsavedChanges(() => { });
        }

        public bool SelectedEstimationHasExperts()
        {
            var selectedEstimation = gui.SelectionManager.Selection as ProbabilityEstimationPerTreeEvent;
            return selectedEstimation != null && selectedEstimation.Experts.Any();
        }

        public ProbabilityEstimationPerTreeEvent GetSelectedEstimationPerTreeEvent()
        {
            return gui.SelectionManager.Selection as ProbabilityEstimationPerTreeEvent;
        }

        #region Statusbar related

        public bool ShowMessages { get; set; }

        public LogMessage PriorityMessage { get; set; }

        public StorageState BusyIndicator
        {
            get => gui.BusyIndicator;
            set => gui.BusyIndicator = value;
        }

        public MessageListViewModel MessagesViewModel =>
            messageListViewModel ?? (messageListViewModel = new MessageListViewModel(gui.Messages));

        public string ProjectFileName => string.IsNullOrEmpty(gui.ProjectFilePath)
            ? "Nieuw bestand*"
            : Path.GetFileNameWithoutExtension(gui.ProjectFilePath);

        #endregion
    }
}