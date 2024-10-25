using System.Windows;
using Forest.App.Properties;
using Forest.Data;
using Forest.Gui;
using Forest.Visualization;
using Forest.Visualization.Dialogs;
using Forest.Visualization.ViewModels;

namespace Forest.App.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChangedObject
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
            
            ContentPresenterViewModel = new ContentPresenterViewModel(gui);
            RibbonViewModel = new RibbonViewModel(gui);
            StatusBarViewModel = new StatusBarViewModel(gui);
            BusyOverlayViewModel = new BusyOverlayViewModel(gui);
        }

        public ContentPresenterViewModel ContentPresenterViewModel { get; }

        public RibbonViewModel RibbonViewModel { get; }

        public StatusBarViewModel StatusBarViewModel { get; }

        public BusyOverlayViewModel BusyOverlayViewModel { get; }

        public bool ForcedClosingMainWindow()
        {
            return gui.GuiProjectServices.HandleUnsavedChanges(() => { });
        }
    }
}