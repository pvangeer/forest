using System.Collections.ObjectModel;
using System.ComponentModel;
using Forest.Gui;
using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public class ProjectExplorerEventTreeCollectionViewModel : PropertiesCollectionViewModelBase
    {
        private readonly ForestGui gui;

        public ProjectExplorerEventTreeCollectionViewModel(ForestGui gui) : base(new ViewModelFactory(gui))
        {
            this.gui = gui;
            if (gui != null)
            {
                gui.PropertyChanged += GuiPropertyChanged;
            }

            RefreshChildItems();
            // TODO: React to collection changed once it is a collection.
        }

        private void RefreshChildItems()
        {
            Items = new ObservableCollection<ITreeNodeViewModel>(new[]
                { ViewModelFactory.CreateProjectExplorerEventTreeNodeViewModel(gui.ForestAnalysis.EventTree) });
            OnPropertyChanged(nameof(Items));
        }

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