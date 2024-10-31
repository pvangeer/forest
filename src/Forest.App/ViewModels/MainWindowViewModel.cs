using Forest.App.Properties;
using Forest.Data;
using Forest.Gui;
using Forest.Visualization;
using Forest.Visualization.Dialogs;
using Forest.Visualization.ViewModels;
using Forest.Visualization.ViewModels.ContentPanel;

namespace Forest.App.ViewModels
{
    public class MainWindowViewModel : Entity
    {
        private readonly ForestGui gui;
        
        public MainWindowViewModel() : this(new ForestGui())
        {
        }

        public MainWindowViewModel([NotNull]ForestGui gui)
        {
            this.gui = gui;

            gui.GuiProjectServices.SaveProjectFileNameFunc = FileDialogFactory.AskUserForFileNameToSaveToFunc();
            gui.GuiProjectServices.OpenProjectFileNameFunc = FileDialogFactory.AskUserForFileNameToOpenFunc();
            gui.ShouldMigrateProject = FileDialogFactory.ShouldMigrateProject;
            gui.ShouldSaveOpenChanges = FileDialogFactory.ShouldSaveOpenChanges; 
            var viewModelFactory = new ViewModelFactory(gui);

            MainContentPresenterViewModel = viewModelFactory.CreateMainContentPresenterViewModel();
            RibbonViewModel = viewModelFactory.CreateRibbonViewModel();
            StatusBarViewModel = viewModelFactory.CreateStatusBarViewModel();
            BusyOverlayViewModel = viewModelFactory.CreateBusyOverlayViewModel();
        }

        public MainContentPresenterViewModel MainContentPresenterViewModel { get; }

        public RibbonViewModel RibbonViewModel { get; }

        public StatusBarViewModel StatusBarViewModel { get; }

        public BusyOverlayViewModel BusyOverlayViewModel { get; }

        public bool ForcedClosingMainWindow()
        {
            return gui.GuiProjectServices.HandleUnsavedChanges(() => { });
        }
    }
}