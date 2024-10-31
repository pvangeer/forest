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
            }
        }

        public ProjectExplorerViewModel ProjectExplorerViewModel { get; private set; }

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
    }
}
