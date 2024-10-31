using System.Collections.ObjectModel;
using System.Windows.Input;
using Forest.Data;
using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.TreeView.ViewModels
{
    public abstract class PropertyValueTreeNodeViewModelBase : NotifyPropertyChangedObject, ITreeNodeViewModel
    {
        public PropertyValueTreeNodeViewModelBase(string displayName)
        {
            DisplayName = displayName;
            ContextMenuItems = new ObservableCollection<ContextMenuItemViewModel>();
        }

        public string DisplayName { get; }

        public bool IsExpandable => false;

        public bool IsExpanded
        {
            get => false;
            set { }
        }

        public ICommand ToggleIsExpandedCommand => null;

        public string IconSourceString => null;

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

        public abstract bool IsViewModelFor(object o);
    }
}