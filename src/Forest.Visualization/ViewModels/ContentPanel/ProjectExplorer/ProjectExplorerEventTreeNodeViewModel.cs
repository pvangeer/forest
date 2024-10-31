using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Forest.Data.Tree;
using Forest.Gui;
using Forest.Visualization.TreeView.Data;
using Forest.Visualization.TreeView.ViewModels;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public class ProjectExplorerEventTreeNodeViewModel : ProjectExplorerItemViewModelBase
    {
        private readonly EventTree eventTree;
        private readonly ForestGui gui;
        private readonly ProjectExplorerCommandFactory commandFactory;

        public ProjectExplorerEventTreeNodeViewModel(EventTree eventTree, ForestGui gui) : base(new ViewModelFactory(gui))
        {
            this.gui = gui;
            this.commandFactory = new ProjectExplorerCommandFactory(gui);
            this.eventTree = eventTree;
            this.eventTree.PropertyChanged += EventTreePropertyChanged;
            IsExpanded = false;
        }

        public override ICommand SelectItemCommand => commandFactory.CreateSelectItemCommand(this);

        public override object GetSelectableObject()
        {
            return eventTree;
        }

        public override string DisplayName => eventTree.Name;

        // TODO: Change icon
        public override string IconSourceString =>
            "pack://application:,,,/Forest.Visualization;component/Resources/forest.ico";

        // TODO: Implement once it is a list.
        public override ICommand RemoveItemCommand => null;

        public override bool IsViewModelFor(object o)
        {
            return o as EventTree == eventTree;
        }

        private void EventTreePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(EventTree.Name):
                    OnPropertyChanged(nameof(DisplayName));
                    break;
            }
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