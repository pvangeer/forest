using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Forest.Data.Tree;
using Forest.Gui;
using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public class ProjectExplorerEventTreeCollectionViewModel : ItemsCollectionViewModelBase
    {
        private readonly ProjectExplorerCommandFactory projectExplorerCommandFactory;
        private readonly ForestGui gui;

        public ProjectExplorerEventTreeCollectionViewModel(ForestGui gui) : base(new ViewModelFactory(gui))
        {
            this.gui = gui;
            if (gui != null)
            {
                gui.PropertyChanged += GuiPropertyChanged;
                gui.ForestAnalysis.EventTrees.CollectionChanged += EventTreeCollectionChanged;
            }
            projectExplorerCommandFactory = new ProjectExplorerCommandFactory(gui);
            RefreshChildItems();
        }

        public override string DisplayName => "Faalpaden";

        public override bool IsExpandable => true;

        public override ICommand ToggleIsExpandedCommand => projectExplorerCommandFactory.CreateToggleIsExpandedCommand(this);

        public override bool CanAdd => true;

        public override ICommand AddItemCommand => projectExplorerCommandFactory.CreateAddEventTreeCommand();

        private void RefreshChildItems()
        {
            Items = new ObservableCollection<ITreeNodeViewModel>(
                gui.ForestAnalysis.EventTrees.Select(et => ViewModelFactory.CreateProjectExplorerEventTreeNodeViewModel(et))
            );
            OnPropertyChanged(nameof(Items));
        }

        private void EventTreeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var eventTree in e.NewItems.OfType<EventTree>())
                    {
                        Items.Add(ViewModelFactory.CreateProjectExplorerEventTreeNodeViewModel(eventTree));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var eventTree in e.OldItems.OfType<EventTree>())
                    {
                        var itemToRemove = Items.FirstOrDefault(i => i.IsViewModelFor(eventTree));
                        if (itemToRemove != null)
                        {
                            Items.Remove(itemToRemove);
                        }
                    }
                    break;
            }
        }

        protected void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.ForestAnalysis):
                    RefreshChildItems();
                    gui.ForestAnalysis.EventTrees.CollectionChanged += EventTreeCollectionChanged;
                    break;
            }
        }
    }
}