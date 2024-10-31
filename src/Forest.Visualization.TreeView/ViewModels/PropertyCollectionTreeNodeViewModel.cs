using System.Collections.ObjectModel;
using System.Windows.Input;
using Forest.Data;
using Forest.Visualization.TreeView.Commands;
using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.TreeView.ViewModels
{
    public class PropertyCollectionTreeNodeViewModel : NotifyPropertyChangedObject, ITreeNodeCollectionViewModel
    {
        private string iconSourceString;
        private bool isExpanded;

        public PropertyCollectionTreeNodeViewModel(string displayName,
            ObservableCollection<ITreeNodeViewModel> items, CollectionType collectionType)
        {
            DisplayName = displayName;
            Items = items;
            ContextMenuItems = new ObservableCollection<ContextMenuItemViewModel>();
            CollectionType = collectionType;
            IsExpanded = true;
        }

        public bool IsExpandable => true;

        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                isExpanded = value;
                OnPropertyChanged();
            }
        }

        public CollectionType CollectionType { get; }

        public ICommand ToggleIsExpandedCommand => new ToggleIsExpandedCommand(this);

        public string DisplayName { get; set; }

        public string IconSourceString
        {
            get => iconSourceString;
            set
            {
                iconSourceString = value;
                OnPropertyChanged(IconSourceString);
            }
        }

        public bool CanRemove => false;

        public ICommand RemoveItemCommand => null;

        public bool CanAdd => false;

        public ICommand AddItemCommand => null;

        public bool CanOpen => false;

        public ICommand OpenViewCommand => null;

        public bool CanSelect => false;

        public bool IsSelected { get; set; }

        public ICommand SelectItemCommand => null;

        public object GetSelectableObject()
        {
            return null;
        }

        public ObservableCollection<ContextMenuItemViewModel> ContextMenuItems { get; }

        public bool IsViewModelFor(object item)
        {
            return false;
        }

        public ObservableCollection<ITreeNodeViewModel> Items { get; }
    }
}