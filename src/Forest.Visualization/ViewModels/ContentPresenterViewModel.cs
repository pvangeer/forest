using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Forest.Data;
using Forest.Data.Services;
using Forest.Gui.Components;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization
{
    public class ContentPresenterViewModel : INotifyPropertyChanged
    {
        private readonly ForestGui gui;
        private readonly ProjectManipulationService projectManipulationService;

        public ContentPresenterViewModel(ForestGui gui)
        {
            this.gui = gui;
            if (gui != null)
            {
                gui.PropertyChanged += GuiPropertyChanged;
                gui.SelectionManager.PropertyChanged += SelectionChanged;

                projectManipulationService = new ProjectManipulationService(gui.EventTreeProject);

                EventTree = new EventTreeViewModel(EventTreeProject.EventTree, projectManipulationService, gui.SelectionManager)
                {
                    EstimationSpecificationViewModelFactory = new EstimationSpecificationViewModelFactory(gui.EventTreeProject)
                };

                Experts = new ObservableCollection<ExpertViewModel>(EventTreeProject.Experts.Select(e => new ExpertViewModel(e)));
                Experts.CollectionChanged += ExpertViewModelsCollectionChanged;

                HydraulicConditionsList =
                    new ObservableCollection<HydraulicConditionViewModel>(
                        EventTreeProject.HydraulicConditions.Select(e => new HydraulicConditionViewModel(e)));
                HydraulicConditionsList.CollectionChanged += HydraulicsViewModelsCollectionChanged;

            }
        }

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

        public ObservableCollection<ExpertViewModel> Experts { get; }

        public ObservableCollection<HydraulicConditionViewModel> HydraulicConditionsList { get; }


        private EventTreeProject EventTreeProject => gui.EventTreeProject;

        public EventTreeViewModel EventTree { get; }

        public TreeEventViewModel SelectedTreeEvent => EventTree.SelectedTreeEvent;

        public ForestGuiState SelectedGuiState
        {
            get => gui.SelectedState;
            set
            {
                gui.SelectedState = value;
                gui.OnPropertyChanged(nameof(ForestGui.SelectedState));
            }
        }

        private void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.SelectedState):
                    OnPropertyChanged(nameof(SelectedGuiState));
                    break;
            }
        }

        private void SelectionChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SelectedTreeEvent));
        }

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


        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}