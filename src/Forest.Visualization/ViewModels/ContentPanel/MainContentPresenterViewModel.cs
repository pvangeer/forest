using System.ComponentModel;
using Forest.Gui;
using Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer;

namespace Forest.Visualization.ViewModels.ContentPanel
{
    public class MainContentPresenterViewModel : GuiViewModelBase
    {
        public MainContentPresenterViewModel(ForestGui gui) : base(gui)
        {
            if (gui != null)
            {
                ProjectExplorerViewModel = ViewModelFactory.CreateProjectExplorerViewModel();
                gui.SelectionManager.PropertyChanged += SelectionManagerPropertyChanged;
            }
        }

        public ProjectExplorerViewModel ProjectExplorerViewModel { get; private set; }

        public object SelectedContentItem => ViewModelFactory.CreateMainContentViewModel(Gui?.SelectionManager.Selection);

        protected override void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.ForestAnalysis):
                    ProjectExplorerViewModel = ViewModelFactory.CreateProjectExplorerViewModel();
                    OnPropertyChanged(nameof(ProjectExplorerViewModel));
                    break;
            }
        }

        private void SelectionManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectionManager.Selection):
                    OnPropertyChanged(nameof(SelectedContentItem));
                    break;
            }
        }
    }
}