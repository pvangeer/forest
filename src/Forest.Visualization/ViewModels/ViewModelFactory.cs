using Forest.Gui;
using Forest.Visualization.TreeView.Data;
using Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer;

namespace Forest.Visualization.ViewModels
{
    public class ViewModelFactory
    {
        private readonly ForestGui gui;

        public ViewModelFactory(ForestGui gui)
        {
            this.gui = gui;
        }

        public ITreeNodeViewModel CreateProjectExplorerEventTreeCollectionViewModel()
        {
            throw new System.NotImplementedException();
        }

        public ITreeNodeViewModel CreateProjectExplorerProbabilityAnalysisCollectionViewModel()
        {
            throw new System.NotImplementedException();
        }

        public ProjectExplorerViewModel CreateProjectExplorerViewModel()
        {
            return new ProjectExplorerViewModel(gui);
        }

        public ContentPresenterViewModel CreateContentPresenterViewModel()
        {
            return new ContentPresenterViewModel(this, gui);
        }

        public BusyOverlayViewModel CreateBusyOverlayViewModel()
        {
            return new BusyOverlayViewModel(this, gui);
        }

        public RibbonViewModel CreateRibbonViewModel()
        {
            return new RibbonViewModel(this, gui);
        }

        public StatusBarViewModel CreateStatusBarViewModel()
        {
            return new StatusBarViewModel(this, gui);
        }
    }
}
