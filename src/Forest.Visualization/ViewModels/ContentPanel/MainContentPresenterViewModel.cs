using System.ComponentModel;
using Forest.Data;
using Forest.Gui;
using Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer;

namespace Forest.Visualization.ViewModels.ContentPanel
{
    public class MainContentPresenterViewModel : Entity
    {
        private readonly ForestGui gui;
        private readonly ViewModelFactory viewModelFactory;

        public MainContentPresenterViewModel(ForestGui gui)
        {
            this.gui = gui;
            if (gui != null)
            {
                gui.PropertyChanged += GuiPropertyChanged;
                viewModelFactory = new ViewModelFactory(gui);
                ProjectExplorerViewModel = viewModelFactory.CreateProjectExplorerViewModel();
            }
        }

        public ProjectExplorerViewModel ProjectExplorerViewModel { get; private set; }

        private void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.ForestAnalysis):
                    ProjectExplorerViewModel = viewModelFactory.CreateProjectExplorerViewModel();
                    OnPropertyChanged(nameof(ProjectExplorerViewModel));
                    break;
            }
        }
    }
}
