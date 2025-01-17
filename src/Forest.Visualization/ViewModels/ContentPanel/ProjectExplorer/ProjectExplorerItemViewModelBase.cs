using System.Collections.ObjectModel;
using System.Windows.Input;
using Forest.Visualization.TreeView.Data;
using Forest.Visualization.TreeView.ViewModels;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public abstract class ProjectExplorerItemViewModelBase : ViewModelBase, ITreeNodeCollectionViewModel
    {
        protected ProjectExplorerItemViewModelBase(ViewModelFactory viewModelFactory) : base(viewModelFactory)
        {
            ContextMenuItems = new ObservableCollection<ContextMenuItemViewModel>();
        }

        public bool IsExpandable => false;

        public bool IsExpanded { get; set; }

        public ICommand ToggleIsExpandedCommand => null;

        public bool CanSelect => true;

        public bool IsSelected { get; set; }

        public abstract ICommand SelectItemCommand { get; }

        public abstract object GetSelectableObject();

        public abstract string DisplayName { get; }

        public abstract string IconSourceString { get; }

        public bool CanRemove => true;

        public abstract ICommand RemoveItemCommand { get; }

        public bool CanAdd => false;

        public ICommand AddItemCommand => null;

        public bool CanOpen => false;

        public ICommand OpenViewCommand => null;

        public ObservableCollection<ContextMenuItemViewModel> ContextMenuItems { get; protected set; }

        public abstract bool IsViewModelFor(object item);

        public ObservableCollection<ITreeNodeViewModel> Items => new ObservableCollection<ITreeNodeViewModel>();

        public CollectionType CollectionType => CollectionType.PropertyItemsCollection;
    }
}