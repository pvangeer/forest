using System.Collections.ObjectModel;
using System.Windows.Input;
using Forest.Data;
using Forest.Visualization.TreeView.Data;
using Forest.Visualization.TreeView.ViewModels;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public abstract class PropertiesCollectionViewModelBase : NotifyPropertyChangedObject, ITreeNodeCollectionViewModel
    {
        private bool isExpanded = true;

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

        public string IconSourceString => "";

        public virtual bool CanRemove => false;

        public virtual ICommand RemoveItemCommand => null;

        public virtual bool CanAdd => false;

        public virtual ICommand AddItemCommand => null;

        public bool CanOpen => false;

        public ICommand OpenViewCommand => null;

        public virtual ObservableCollection<ContextMenuItemViewModel> ContextMenuItems =>
            new ObservableCollection<ContextMenuItemViewModel>();

        public abstract bool IsViewModelFor(object item);

        public virtual ObservableCollection<ITreeNodeViewModel> Items => new ObservableCollection<ITreeNodeViewModel>();

        public abstract CollectionType CollectionType { get; }
    }
}