using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Forest.Gui;
using Forest.Visualization.Commands;
using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public class ProjectExplorerEventTreeCollectionViewModel : ItemsCollectionViewModelBase
    {
        protected readonly ProjectExplorerCommandFactory ProjectExplorerCommandFactory;
        private readonly ForestGui gui;

        public ProjectExplorerEventTreeCollectionViewModel(ForestGui gui) : base(new ViewModelFactory(gui))
        {
            this.gui = gui;
            if (gui != null)
            {
                gui.PropertyChanged += GuiPropertyChanged;
            }
            ProjectExplorerCommandFactory = new ProjectExplorerCommandFactory(gui);
            RefreshChildItems();
            // TODO: React to collection changed once it is a collection.
        }

        private void RefreshChildItems()
        {
            Items = new ObservableCollection<ITreeNodeViewModel>(new[]
                { ViewModelFactory.CreateProjectExplorerEventTreeNodeViewModel(gui.ForestAnalysis.EventTree) });
            OnPropertyChanged(nameof(Items));
        }

        public override string DisplayName => "Faalpaden";

        public override bool IsExpandable => true;

        public override ICommand ToggleIsExpandedCommand => ProjectExplorerCommandFactory.CreateToggleIsExpandedCommand(this);

        protected void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.ForestAnalysis):
                    RefreshChildItems();
                    // TODO: subscribe to collection changed of new collection (and unsubscribe?).
                    break;
            }
        }
    }
}