using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Forest.Data;
using Forest.Data.Services;
using Forest.Gui.Command;

namespace Forest.Gui.ViewModels
{
    public class ProjectViewModel : INotifyPropertyChanged
    {
        private readonly AddTreeEventCommand addTreeEventCommand;

        private readonly ProjectManipulationService projectManipulationService;
        private readonly RemoveTreeEventCommand removeTreeEventCommand;

        public ProjectViewModel() : this(new EventTreeProject())
        {
        }

        public ProjectViewModel(EventTreeProject eventTreeProject)
        {
            BusyIndicator = StorageState.Idle;

            EventTreeProject = eventTreeProject;
            projectManipulationService = new ProjectManipulationService(eventTreeProject);

            var eventTreeViewModel = new EventTreeViewModel(EventTreeProject.EventTree, projectManipulationService)
            {
                EstimationSpecificationViewModelFactory = new EstimationSpecificationViewModelFactory(eventTreeProject)
            };
            eventTreeViewModel.PropertyChanged += EventTreeViewModelPropertyChanged;
            EventTree = eventTreeViewModel;

            addTreeEventCommand = new AddTreeEventCommand(this);
            removeTreeEventCommand = new RemoveTreeEventCommand(this);

            Experts = new ObservableCollection<ExpertViewModel>(EventTreeProject.Experts.Select(e => new ExpertViewModel(e)));
            Experts.CollectionChanged += ExpertViewModelsCollectionChanged;

            HydraulicConditionsList =
                new ObservableCollection<HydraulicConditionViewModel>(
                    EventTreeProject.HydraulicConditions.Select(e => new HydraulicConditionViewModel(e)));
            HydraulicConditionsList.CollectionChanged += HydraulicsViewModelsCollectionChanged;
        }

        public EventTreeProject EventTreeProject { get; }

        public string ProjectName
        {
            get => EventTreeProject.Name;
            set => EventTreeProject.Name = value;
        }

        public string ProjectDescription
        {
            get => EventTreeProject.Description;
            set => EventTreeProject.Description = value;
        }

        public string AssessmentSection
        {
            get => EventTreeProject.AssessmentSection;
            set => EventTreeProject.AssessmentSection = value;
        }

        public string ProjectInformation
        {
            get => EventTreeProject.ProjectInformation;
            set => EventTreeProject.ProjectInformation = value;
        }

        public ICommand RemoveTreeEventCommand => removeTreeEventCommand;

        public ICommand AddTreeEventCommand => addTreeEventCommand;

        public EventTreeViewModel EventTree { get; }

        public TreeEventViewModel SelectedTreeEvent => EventTree.SelectedTreeEvent;

        public ObservableCollection<ExpertViewModel> Experts { get; }

        public string ProjectLeaderName
        {
            get => EventTreeProject.ProjectLeader.Name;
            set
            {
                EventTreeProject.ProjectLeader.Name = value;
                EventTreeProject.ProjectLeader.OnPropertyChanged(nameof(EventTreeProject.ProjectLeader.Name));
            }
        }

        public string ProjectLeaderEmail
        {
            get => EventTreeProject.ProjectLeader.Email;
            set
            {
                EventTreeProject.ProjectLeader.Email = value;
                EventTreeProject.ProjectLeader.OnPropertyChanged(nameof(EventTreeProject.ProjectLeader.Email));
            }
        }

        public string ProjectLeaderTelephone
        {
            get => EventTreeProject.ProjectLeader.Telephone;
            set
            {
                EventTreeProject.ProjectLeader.Telephone = value;
                EventTreeProject.ProjectLeader.OnPropertyChanged(nameof(EventTreeProject.ProjectLeader.Telephone));
            }
        }

        public ObservableCollection<HydraulicConditionViewModel> HydraulicConditionsList { get; }

        public StorageState BusyIndicator { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void HydraulicsViewModelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (var item in e.NewItems.OfType<HydraulicConditionViewModel>())
                    projectManipulationService.AddHydraulicCondition(item.HydraulicCondition);

            if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (var item in e.OldItems.OfType<HydraulicConditionViewModel>())
                    projectManipulationService.RemoveHydraulicCondition(item.HydraulicCondition);
        }

        private void ExpertViewModelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (var item in e.NewItems.OfType<ExpertViewModel>())
                    projectManipulationService.AddExpert(item.Expert);

            if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (var item in e.OldItems.OfType<ExpertViewModel>())
                    projectManipulationService.RemoveExpert(item.Expert);
        }


        private void EventTreeViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is EventTreeViewModel))
                return;

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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
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