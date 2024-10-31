using System.ComponentModel;

namespace Forest.Visualization.TreeView.Data
{
    public interface INotifyPropertyChangedImplementation : INotifyPropertyChanged
    {
        void OnPropertyChanged(string propertyName);
    }
}