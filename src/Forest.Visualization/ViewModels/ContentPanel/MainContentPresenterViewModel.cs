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
                gui.SelectionManager.PropertyChanged += SelectionManagerPropertyChanged;
            }
        }

        public ProjectExplorerViewModel ProjectExplorerViewModel => ViewModelFactory.CreateProjectExplorerViewModel();

        public object SelectedContentItem => ViewModelFactory.CreateMainContentViewModel(Gui?.SelectionManager.Selection);

        protected override void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.ForestAnalysis):
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