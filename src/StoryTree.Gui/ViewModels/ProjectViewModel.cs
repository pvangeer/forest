using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using StoryTree.Data;
using StoryTree.Data.Properties;
using StoryTree.Gui.Command;

namespace StoryTree.Gui.ViewModels
{
    public class ProjectViewModel : INotifyPropertyChanged
    {
        private readonly AddTreeEventCommand addTreeEventCommand;
        private readonly RemoveTreeEventCommand removeTreeEventCommand;
        
        public ProjectViewModel()
        {
            addTreeEventCommand = new AddTreeEventCommand(this);
            removeTreeEventCommand = new RemoveTreeEventCommand(this);
        }

        public ProjectViewModel([NotNull]Project project)
        {
            Project = project;

            var eventTreeViewModels = new ObservableCollection<EventTreeViewModel>(project.EventTrees.Select(te =>
            {
                var eventTreeViewModel = new EventTreeViewModel(te);
                eventTreeViewModel.PropertyChanged += EventTreeViewModelPropertyChanged;
                return eventTreeViewModel;
            }));

            EventTrees = eventTreeViewModels;
            addTreeEventCommand = new AddTreeEventCommand(this);
            removeTreeEventCommand = new RemoveTreeEventCommand(this);

            expertViewModels = new ObservableCollection<ExpertViewModel>(Project.Experts.Select(e => new ExpertViewModel(e)));
            expertViewModels.CollectionChanged += ExpertViewModelsCollectionChanged;

            project.Experts.CollectionChanged += ExpertsCollectionChanged;
            project.EventTrees.CollectionChanged += EventTreesCollectionChanged;
        }

        private Project Project { get; }

        public ObservableCollection<EventTreeViewModel> EventTrees { get; }

        public string ProjectName => Project.Name;

        public ICommand AddEventTreeCommand => new AddEventTreeCommand(this);

        public ICommand RemoveEventTreeCommand => new RemoveEventTreeCommand(this);

        public ICommand RemoveTreeEventCommand => removeTreeEventCommand;

        public ICommand AddTreeEventCommand => addTreeEventCommand;

        private EventTreeViewModel selectedEventTree;
        private ObservableCollection<ExpertViewModel> expertViewModels;

        public EventTreeViewModel SelectedEventTree
        {
            get => selectedEventTree;
            set
            {
                selectedEventTree = value;
                OnPropertyChanged(nameof(SelectedEventTree));
                OnPropertyChanged(nameof(SelectedTreeEvent));
                foreach (var eventTreeViewModel in EventTrees)
                {
                    eventTreeViewModel.Selected = Equals(SelectedEventTree, eventTreeViewModel);
                }
                addTreeEventCommand.FireCanExecuteChanged();
                removeTreeEventCommand.FireCanExecuteChanged();
            }
        }

        public TreeEventViewModel SelectedTreeEvent => SelectedEventTree?.SelectedTreeEvent;

        public ObservableCollection<ExpertViewModel> Experts
        {
            get
            {
                if (Project == null)
                {
                    return null;
                }

                if (expertViewModels == null)
                {
                    expertViewModels = new ObservableCollection<ExpertViewModel>(Project?.Experts.Select(e => new ExpertViewModel(e)));
                }

                return expertViewModels;
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
                    var eventTreeViewModel = EventTrees.First(et => et.IsViewModelFor(eventTree));
                    eventTreeViewModel.PropertyChanged -= EventTreeViewModelPropertyChanged;
                    EventTrees.Remove(eventTreeViewModel);
                }
                
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var eventTree in e.NewItems.OfType<EventTree>())
                {
                    var eventTreeViewModel = new EventTreeViewModel(eventTree);
                    eventTreeViewModel.PropertyChanged += EventTreeViewModelPropertyChanged;
                    EventTrees.Add(eventTreeViewModel);
                }

            }
        }

        private void ExpertsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private void ExpertViewModelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems.OfType<ExpertViewModel>())
                {
                    Project.Experts.Add(item.Expert);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems.OfType<ExpertViewModel>())
                {
                    Project.Experts.Remove(item.Expert);
                }
            }
        }


        private void EventTreeViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is EventTreeViewModel eventTreeViewModel))
            {
                return;
            }

            switch (e.PropertyName)
            {
                case "MainTreeEventViewModel":
                    OnPropertyChanged(nameof(SelectedTreeEvent));
                    addTreeEventCommand.FireCanExecuteChanged();
                    removeTreeEventCommand.FireCanExecuteChanged();
                    break;
                case "SelectedTreeEvent":
                    OnPropertyChanged(nameof(SelectedTreeEvent));
                    addTreeEventCommand.FireCanExecuteChanged();
                    removeTreeEventCommand.FireCanExecuteChanged();
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}