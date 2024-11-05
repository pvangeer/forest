using System;
using Forest.Data.Estimations;
using Forest.Data.Tree;
using Forest.Gui;
using Forest.Visualization.TreeView.Data;
using Forest.Visualization.ViewModels.ContentPanel;
using Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer;
using Forest.Visualization.ViewModels.MainContentPanel;

namespace Forest.Visualization.ViewModels
{
    public class ViewModelFactory
    {
        private readonly ForestGui gui;

        public ViewModelFactory(ForestGui gui)
        {
            this.gui = gui;
        }

        private EventTreeViewModelOld CreateEventTreeMainViewModel(EventTree eventTree)
        {
            throw new NotImplementedException();
        }

        public ITreeNodeViewModel CreateProjectExplorerEventTreeCollectionViewModel()
        {
            return new ProjectExplorerEventTreeCollectionViewModel(gui);
        }

        public ITreeNodeViewModel CreateProjectExplorerProbabilityEstimationCollectionViewModel()
        {
            return new ProjectExplorerProbabilityEstimationCollectionViewModel(gui);
        }

        public ProjectExplorerViewModel CreateProjectExplorerViewModel()
        {
            return new ProjectExplorerViewModel(gui);
        }

        public BusyOverlayViewModel CreateBusyOverlayViewModel()
        {
            return new BusyOverlayViewModel(gui);
        }

        public RibbonViewModel CreateRibbonViewModel()
        {
            return new RibbonViewModel(gui);
        }

        public StatusBarViewModel CreateStatusBarViewModel()
        {
            return new StatusBarViewModel(gui);
        }

        public ITreeNodeViewModel CreateProjectExplorerEventTreeNodeViewModel(EventTree eventTree)
        {
            return new ProjectExplorerEventTreeNodeViewModel(eventTree, gui);
        }

        public ITreeNodeViewModel CreateProjectExplorerEstimationItemViewModel(ProbabilityEstimation estimation)
        {
            return new ProjectExplorerEstimationItemViewModel(estimation, gui);
        }

        public MainContentPresenterViewModel CreateMainContentPresenterViewModel()
        {
            return new MainContentPresenterViewModel(gui);
        }

        public object CreateMainContentViewModel(object selection)
        {
            if (selection is EventTree eventTree)
                return new EventTreeMainContentViewModel(eventTree, gui);

            return selection;
        }

        public TreeEventViewModel CreateTreeEventViewModel(TreeEvent treeEvent, EventTree eventTree)
        {
            return treeEvent != null ? new TreeEventViewModel(treeEvent, eventTree, gui) : null;
        }
    }
}