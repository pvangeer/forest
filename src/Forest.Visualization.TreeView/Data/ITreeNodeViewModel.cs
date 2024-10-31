using System.Collections.ObjectModel;
using System.Windows.Input;
using Forest.Visualization.TreeView.ViewModels;

namespace Forest.Visualization.TreeView.Data
{
    public interface ITreeNodeViewModel : IExpandable, ISelectable
    {
        string DisplayName { get; }

        string IconSourceString { get; }

        bool CanRemove { get; }

        ICommand RemoveItemCommand { get; }

        bool CanAdd { get; }

        ICommand AddItemCommand { get; }

        bool CanOpen { get; }

        ICommand OpenViewCommand { get; }

        ObservableCollection<ContextMenuItemViewModel> ContextMenuItems { get; }

        bool IsViewModelFor(object item);
    }
}