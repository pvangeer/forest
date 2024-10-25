using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Experts;
using Forest.Data.Properties;
using Forest.Gui.Components;
using Forest.IO.Export;
using Forest.IO.Import;
using Forest.Visualization.Dialogs;
using Forest.Visualization.ViewModels;

namespace Forest.Gui.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ForestGui gui;

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

                ContentPresenterViewModel = new ContentPresenterViewModel(gui);
                RibbonViewModel = new RibbonViewModel(gui);
                StatusBarViewModel = new StatusBarViewModel(gui);

                gui.ShouldMigrateProject = ShouldMigrateProject;
                this.gui.PropertyChanged += GuiPropertyChanged;
                this.gui.ShouldSaveOpenChanges = ShouldSaveOpenChanges;
            }
        }

        public ContentPresenterViewModel ContentPresenterViewModel { get; }

        public RibbonViewModel RibbonViewModel { get; }

        public StatusBarViewModel StatusBarViewModel { get; }

        public StorageState BusyIndicator
        {
            get => gui.BusyIndicator;
            set => gui.BusyIndicator = value;
        }

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

        private void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.BusyIndicator):
                    OnPropertyChanged(nameof(BusyIndicator));
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
    }
}