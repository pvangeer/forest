using Forest.Data;
using Forest.Gui;
using Forest.Visualization;
using Forest.Visualization.Dialogs;
using Forest.Visualization.ViewModels;
using Forest.Visualization.ViewModels.ContentPanel;
using Forest.Visualization.ViewModels.Ribbon;
using Forest.Visualization.ViewModels.StatusBar;

namespace Forest.App
{
    public class MainWindowViewModel : Entity
    {
        private readonly ForestGui gui;

        public MainWindowViewModel()
        {
            gui = new ForestGui
            {
                GuiProjectServices =
                {
                    SaveProjectFileNameFunc = FileDialogFactory.AskUserForFileNameToSaveToFunc(),
                    OpenProjectFileNameFunc = FileDialogFactory.AskUserForFileNameToOpenFunc()
                },
                ShouldMigrateProject = FileDialogFactory.ShouldMigrateProject,
                ShouldSaveOpenChanges = FileDialogFactory.ShouldSaveOpenChanges
            };

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