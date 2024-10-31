using System.Collections.ObjectModel;

namespace Forest.Visualization.TreeView.Data
{
    public interface ITreeNodeCollectionViewModel : ITreeNodeViewModel
    {
        ObservableCollection<ITreeNodeViewModel> Items { get; }

        CollectionType CollectionType { get; }
    }
}