using System.Collections.ObjectModel;
using System.ComponentModel;
using Forest.Gui;
using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public class ProjectExplorerViewModel : PropertiesCollectionViewModelBase
    {
        private readonly ForestGui gui;

        public ProjectExplorerViewModel(ForestGui gui) : base(new ViewModelFactory(gui))
        {
            this.gui = gui;
            Items = new ObservableCollection<ITreeNodeViewModel>
            {
                ViewModelFactory.CreateProjectExplorerEventTreeCollectionViewModel(),
                ViewModelFactory.CreateProjectExplorerProbabilityEstimationCollectionViewModel(),
            };

            gui.SelectionManager.PropertyChanged += SelectionManagerPropertyChanged;
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
