using System.Collections.ObjectModel;
using Forest.Data.Tree;
using Forest.Gui;
using Forest.Visualization.TreeView.Data;
using Forest.Visualization.TreeView.ViewModels;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public class ProjectExplorerEventTreeNodeViewModel : PropertiesCollectionViewModelBase
    {
        private readonly EventTree eventTree;
        private readonly ForestGui gui;

        public ProjectExplorerEventTreeNodeViewModel(EventTree eventTree, ForestGui gui) : base(new ViewModelFactory(gui))
        {
            this.gui = gui;

            this.eventTree = eventTree;
            Items = GetItems();
            IsExpanded = false;
        }

        // TODO: Add remove command

        public override bool IsViewModelFor(object o)
        {
            return o as EventTree == eventTree;
        }

        private ObservableCollection<ITreeNodeViewModel> GetItems()
        {
            return new ObservableCollection<ITreeNodeViewModel>
            {
                new StringPropertyValueTreeNodeViewModel<EventTree>(eventTree,
                    nameof(EventTree.Name), "Naam"),
            };
        }
    }
}