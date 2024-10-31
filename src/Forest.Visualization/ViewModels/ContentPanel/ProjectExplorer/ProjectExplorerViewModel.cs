using System.Collections.ObjectModel;
using System.ComponentModel;
using Forest.Gui;
using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public class ProjectExplorerViewModel : PropertiesCollectionViewModelBase
    {
        private readonly ForestGui gui;

        public ProjectExplorerViewModel(ForestGui gui)
        {
            this.gui = gui;
            var viewModelFactory = new ViewModelFactory(gui);
            Items = new ObservableCollection<ITreeNodeViewModel>
            {
                viewModelFactory.CreateProjectExplorerEventTreeCollectionViewModel(),
                viewModelFactory.CreateProjectExplorerProbabilityAnalysesCollectionViewModel(),
            };

            gui.SelectionManager.PropertyChanged += SelectionManagerPropertyChanged;
        }

        public override ObservableCollection<ITreeNodeViewModel> Items { get; }

        public override CollectionType CollectionType => CollectionType.PropertyItemsCollection;

        public override bool IsViewModelFor(object item)
        {
            return false;
        }

        private void SelectionManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectionManager.Selection):
                    FindAndSelectObjects();
                    break;
            }
        }

        private void FindAndSelectObjects()
        {
            foreach (var item in Items)
                IsSelectObjectRecursively(item);
        }

        private void IsSelectObjectRecursively(ITreeNodeViewModel viewModel)
        {
            if (viewModel.IsSelected != viewModel.IsViewModelFor(gui.SelectionManager.Selection))
            {
                viewModel.IsSelected = !viewModel.IsSelected;
                viewModel.OnPropertyChanged(nameof(viewModel.IsSelected));
            }

            if (viewModel is ITreeNodeCollectionViewModel collection)
                foreach (var collectionItem in collection.Items)
                    IsSelectObjectRecursively(collectionItem);
        }
    }
}
