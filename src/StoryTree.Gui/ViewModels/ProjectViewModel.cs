using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using StoryTree.Data;
using StoryTree.Gui.Command;

namespace StoryTree.Gui.ViewModels
{
    public class ProjectViewModel
    {
        private readonly AddTreeEventCommand addTreeEventCommand;
        private readonly RemoveTreeEventCommand removeTreeEventCommand;
        
        public ProjectViewModel()
        {
            addTreeEventCommand = new AddTreeEventCommand(SelectedEventTree);
            removeTreeEventCommand = new RemoveTreeEventCommand(SelectedEventTree);
        }

        public ProjectViewModel(Project project)
        {
            if (project != null)
            {
                project.EventTrees.CollectionChanged -= EventTreesCollectionChanged;
            }
            Project = project;
            var eventTreeViewModels = new ObservableCollection<EventTreeViewModel>();
            if (project != null)
            {
                project.EventTrees.CollectionChanged += EventTreesCollectionChanged;
                eventTreeViewModels = new ObservableCollection<EventTreeViewModel>(project.EventTrees.Select(te => new EventTreeViewModel(te)));
            }
            EventTrees = eventTreeViewModels;
            addTreeEventCommand = new AddTreeEventCommand(SelectedEventTree);
            removeTreeEventCommand = new RemoveTreeEventCommand(SelectedEventTree);
        }

        private Project Project { get; }

        public ObservableCollection<EventTreeViewModel> EventTrees { get; }

        public string ProjectName => Project.Name;

        public ICommand AddEventTreeCommand => new Command.AddEventTreeCommand(this);

        public ICommand RemoveEventTreeCommand => new RemoveEventTreeCommand(this);

        public ICommand RemoveTreeEventCommand => removeTreeEventCommand;

        public ICommand AddTreeEventCommand => addTreeEventCommand;

        
        private EventTreeViewModel selectedEventTree;
        
        public EventTreeViewModel SelectedEventTree
        {
            get => selectedEventTree;
            set
            {
                selectedEventTree = value;
                addTreeEventCommand.SelectedEventTreeViewModel = value;
                removeTreeEventCommand.SelectedEventTreeViewModel = value;
            }
        }

        public void AddNewEventTree()
        {
            Project.EventTrees.Add(new EventTree {Description = "Nieuwe gebeurtenis"});
        }

        public void RemoveSelectedEventTree()
        {
            EventTrees.Remove(SelectedEventTree);
        }

        private void EventTreesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var eventTree in e.OldItems.OfType<EventTree>())
                {
                    EventTrees.Remove(EventTrees.First(et => et.IsViewModelFor(eventTree)));
                }
                
            }
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var eventTree in e.NewItems.OfType<EventTree>())
                {
                    EventTrees.Add(new EventTreeViewModel(eventTree));
                }

            }
        }
    }
}