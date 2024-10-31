using System.ComponentModel;
using System.Windows.Input;

namespace Forest.Visualization.TreeView.Data
{
    public interface ISelectable : INotifyPropertyChanged
    {
        bool CanSelect { get; }

        bool IsSelected { get; set; }

        ICommand SelectItemCommand { get; }

        object GetSelectableObject();

        void OnPropertyChanged(string propertyName);
    }
}