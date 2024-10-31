using System.Collections.ObjectModel;
using System.Windows.Input;
using Forest.Gui;
using Forest.Visualization.TreeView.Data;
using Forest.Visualization.TreeView.ViewModels;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public class PropertiesCollectionViewModelBase : ViewModelBase, ITreeNodeCollectionViewModel
    {
        private bool isExpanded = true;

        protected PropertiesCollectionViewModelBase(ViewModelFactory factory) : base(factory)
        {
            Items = new ObservableCollection<ITreeNodeViewModel>();
            ContextMenuItems = new ObservableCollection<ContextMenuItemViewModel>();
        }

        public virtual bool IsExpandable => false;

        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                isExpanded = value;
                OnPropertyChanged();
            }
        }

        public virtual ICommand ToggleIsExpandedCommand => null;

        public bool CanSelect => false;

        public bool IsSelected { get; set; }

        public ICommand SelectItemCommand => null;

        public object GetSelectableObject()
        {
            return null;
        }

        public virtual string DisplayName => "";

        public virtual string IconSourceString => "";

        public virtual bool CanRemove => false;

        public virtual ICommand RemoveItemCommand => null;

        public virtual bool CanAdd => false;

        public virtual ICommand AddItemCommand => null;

        public bool CanOpen => false;

        public ICommand OpenViewCommand => null;

        public ObservableCollection<ContextMenuItemViewModel> ContextMenuItems { get; protected set; }

        public virtual bool IsViewModelFor(object item)
        {
            return false;
        }

        public ObservableCollection<ITreeNodeViewModel> Items { get; protected set; }

        public CollectionType CollectionType => CollectionType.PropertyItemsCollection;

    }
}