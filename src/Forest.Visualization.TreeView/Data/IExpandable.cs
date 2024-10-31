using System.ComponentModel;
using System.Windows.Input;

namespace Forest.Visualization.TreeView.Data
{
    public interface IExpandable : INotifyPropertyChanged
    {
        bool IsExpandable { get; }

        bool IsExpanded { get; set; }

        ICommand ToggleIsExpandedCommand { get; }
    }
}