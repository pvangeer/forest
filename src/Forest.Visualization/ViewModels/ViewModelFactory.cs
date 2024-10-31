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

        public ITreeNodeViewModel CreateProjectExplorerProbabilityAnalysesCollectionViewModel()
        {
            throw new System.NotImplementedException();
        }

        public ProjectExplorerViewModel CreateProjectExplorerViewModel()
        {
            return new ProjectExplorerViewModel(gui);
        }
    }
}
