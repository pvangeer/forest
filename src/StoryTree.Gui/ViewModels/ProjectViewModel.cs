using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using StoryTree.Data;
using StoryTree.Data.Properties;
using StoryTree.Data.Services;
using StoryTree.Gui.Command;

namespace StoryTree.Gui.ViewModels
{
    public class ProjectViewModel : INotifyPropertyChanged
    {
        private readonly AddTreeEventCommand addTreeEventCommand;
        private readonly RemoveTreeEventCommand removeTreeEventCommand;

        public ProjectViewModel() : this(new Project()) { }

        public ProjectViewModel([NotNull]Project project)
        {
            BusyIndicator = StorageState.Idle;

            Project = project;
            projectManipulationService = new ProjectManipulationService(project);

            var eventTreeViewModels = new ObservableCollection<EventTreeViewModel>(project.EventTrees.Select(te =>
            {
                var eventTreeViewModel = new EventTreeViewModel(te, projectManipulationService)
                {
                    EstimationSpecificationViewModelFactory = new EstimationSpecificationViewModelFactory(project)
                };
                eventTreeViewModel.PropertyChanged += EventTreeViewModelPropertyChanged;
                return eventTreeViewModel;
            }));

            EventTrees = eventTreeViewModels;
            addTreeEventCommand = new AddTreeEventCommand(this);
            removeTreeEventCommand = new RemoveTreeEventCommand(this);

            expertViewModels = new ObservableCollection<ExpertViewModel>(Project.Experts.Select(e => new ExpertViewModel(e)));
            expertViewModels.CollectionChanged += ExpertViewModelsCollectionChanged;

            hydraulicsViewModels = new ObservableCollection<HydraulicConditionViewModel>(Project.HydraulicConditions.Select(e => new HydraulicConditionViewModel(e)));
            hydraulicsViewModels.CollectionChanged += HydraulicsViewModelsCollectionChanged;

            project.EventTrees.CollectionChanged += EventTreesCollectionChanged;

            SelectedEventTree = EventTrees.FirstOrDefault();
            foreach (var eventTreeViewModel in EventTrees)
            {
                eventTreeViewModel.SelectedTreeEvent = eventTreeViewModel.MainTreeEventViewModel;
            }
        }

        public Project Project { get; }

        public ObservableCollection<EventTreeViewModel> EventTrees { get; }

        public string ProjectName
        {
            get => Project.Name;
            set => Project.Name = value;
        }

        public string ProjectDescription
        {
            get => Project.Description;
            set => Project.Description = value;
        }

        public string AssessmentSection
        {
            get => Project.AssessmentSection;
            set => Project.AssessmentSection = value;
        }

        public string ProjectInformation
        {
            get => Project.ProjectInformation;
            set => Project.ProjectInformation = value;
        }

        public ICommand RemoveTreeEventCommand => removeTreeEventCommand;

        public ICommand AddTreeEventCommand => addTreeEventCommand;

        private EventTreeViewModel selectedEventTree;
        private readonly ObservableCollection<ExpertViewModel> expertViewModels;
        private readonly ObservableCollection<HydraulicConditionViewModel> hydraulicsViewModels;
        private readonly ProjectManipulationService projectManipulationService;

        public EventTreeViewModel SelectedEventTree
        {
            get => selectedEventTree;
            set
            {
                selectedEventTree = value;
                OnPropertyChanged(nameof(SelectedTreeEvent));
                addTreeEventCommand.FireCanExecuteChanged();
                removeTreeEventCommand.FireCanExecuteChanged();
            }
        }

        public TreeEventViewModel SelectedTreeEvent => SelectedEventTree?.SelectedTreeEvent;

        public ObservableCollection<ExpertViewModel> Experts => expertViewModels;

        public string ProjectLeaderName
        {
            get => Project.ProjectLeader.Name;
            set
            {
                Project.ProjectLeader.Name = value;
                Project.ProjectLeader.OnPropertyChanged(nameof(Project.ProjectLeader.Name));
            }
        }

        public string ProjectLeaderEmail
        {
            get => Project.ProjectLeader.Email;
            set
            {
                Project.ProjectLeader.Email = value;
                Project.ProjectLeader.OnPropertyChanged(nameof(Project.ProjectLeader.Email));
            }
        }

        public string ProjectLeaderTelephone
        {
            get => Project.ProjectLeader.Telephone;
            set
            {
                Project.ProjectLeader.Telephone = value;
                Project.ProjectLeader.OnPropertyChanged(nameof(Project.ProjectLeader.Telephone));
            }
        }

        public ObservableCollection<HydraulicConditionViewModel> HydraulicConditionsList => hydraulicsViewModels;

        public StorageState BusyIndicator { get; set; }

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
                    var eventTreeViewModel = new EventTreeViewModel(eventTree, projectManipulationService)
                    {
                        EstimationSpecificationViewModelFactory = new EstimationSpecificationViewModelFactory(Project)
                    };
                    eventTreeViewModel.PropertyChanged += EventTreeViewModelPropertyChanged;
                    EventTrees.Add(eventTreeViewModel);
                }

            }
        }

        private void HydraulicsViewModelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems.OfType<HydraulicConditionViewModel>())
                {
                    projectManipulationService.AddHydraulicCondition(item.HydraulicCondition);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems.OfType<HydraulicConditionViewModel>())
                {
                    projectManipulationService.RemoveHydraulicCondition(item.HydraulicCondition);
                }
            }
        }

        private void ExpertViewModelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems.OfType<ExpertViewModel>())
                {
                    projectManipulationService.AddExpert(item.Expert);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems.OfType<ExpertViewModel>())
                {
                    projectManipulationService.RemoveExpert(item.Expert);
                }
            }
        }


        private void EventTreeViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is EventTreeViewModel))
            {
                return;
            }

            switch (e.PropertyName)
            {
                case nameof(EventTreeViewModel.MainTreeEventViewModel):
                    OnPropertyChanged(nameof(SelectedTreeEvent));
                    addTreeEventCommand.FireCanExecuteChanged();
                    removeTreeEventCommand.FireCanExecuteChanged();
                    break;
                case nameof(EventTreeViewModel.SelectedTreeEvent):
                    OnPropertyChanged(nameof(SelectedTreeEvent));
                    addTreeEventCommand.FireCanExecuteChanged();
                    removeTreeEventCommand.FireCanExecuteChanged();
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnProcessChanged()
        {
            addTreeEventCommand.FireCanExecuteChanged();
            removeTreeEventCommand.FireCanExecuteChanged();
        }
    }
}