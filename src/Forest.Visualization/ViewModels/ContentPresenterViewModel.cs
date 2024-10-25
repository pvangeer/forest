using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Forest.Data;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Services;
using Forest.Gui.Components;

namespace Forest.Visualization.ViewModels
{
    public class ContentPresenterViewModel : INotifyPropertyChanged
    {
        private readonly AnalysisManipulationService analysisManipulationService;
        private readonly ForestGui gui;

        public ContentPresenterViewModel(ForestGui gui)
        {
            this.gui = gui;
            if (gui != null)
            {
                gui.PropertyChanged += GuiPropertyChanged;
                gui.SelectionManager.PropertyChanged += SelectionChanged;

                analysisManipulationService = new AnalysisManipulationService(gui.ForestAnalysis);

                EventTree = new EventTreeViewModel(ForestAnalysis.EventTree, analysisManipulationService, gui.SelectionManager,
                    gui.ForestAnalysis.ProbabilityEstimations)
                {
                    EstimationSpecificationViewModelFactory = new EstimationSpecificationViewModelFactory(gui.ForestAnalysis)
                };
                var probabilityEstimationPerTreeEvent =
                    gui.ForestAnalysis.ProbabilityEstimations.OfType<ProbabilityEstimationPerTreeEvent>().First();
                ExpertsViewModel =
                    new ExpertsViewModel(probabilityEstimationPerTreeEvent);
                HydrodynamicsViewModel = new HydrodynamicsViewModel(probabilityEstimationPerTreeEvent);
            }
        }

        public string ProjectName
        {
            get => ForestAnalysis.Name;
            set => ForestAnalysis.Name = value;
        }

        public string ProjectDescription
        {
            get => ForestAnalysis.Description;
            set => ForestAnalysis.Description = value;
        }

        public string AssessmentSection
        {
            get => ForestAnalysis.AssessmentSection;
            set => ForestAnalysis.AssessmentSection = value;
        }

        public string ProjectInformation
        {
            get => ForestAnalysis.ProjectInformation;
            set => ForestAnalysis.ProjectInformation = value;
        }

        public string ProjectLeaderName
        {
            get => ForestAnalysis.ProjectLeader.Name;
            set
            {
                ForestAnalysis.ProjectLeader.Name = value;
                ForestAnalysis.ProjectLeader.OnPropertyChanged(nameof(ForestAnalysis.ProjectLeader.Name));
            }
        }

        public string ProjectLeaderEmail
        {
            get => ForestAnalysis.ProjectLeader.Email;
            set
            {
                ForestAnalysis.ProjectLeader.Email = value;
                ForestAnalysis.ProjectLeader.OnPropertyChanged(nameof(ForestAnalysis.ProjectLeader.Email));
            }
        }

        public string ProjectLeaderTelephone
        {
            get => ForestAnalysis.ProjectLeader.Telephone;
            set
            {
                ForestAnalysis.ProjectLeader.Telephone = value;
                ForestAnalysis.ProjectLeader.OnPropertyChanged(nameof(ForestAnalysis.ProjectLeader.Telephone));
            }
        }

        public ObservableCollection<TreeEventProbabilityEstimation> ProbabilityEstimations { get; }

        private ForestAnalysis ForestAnalysis => gui.ForestAnalysis;

        public EventTreeViewModel EventTree { get; set; }

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

        public ExpertsViewModel ExpertsViewModel { get; }
        public HydrodynamicsViewModel HydrodynamicsViewModel { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.SelectedState):
                    OnPropertyChanged(nameof(SelectedGuiState));
                    break;
                case nameof(ForestGui.ForestAnalysis):
                    EventTree = new EventTreeViewModel(ForestAnalysis.EventTree, analysisManipulationService, gui.SelectionManager,
                        gui.ForestAnalysis.ProbabilityEstimations)
                    {
                        EstimationSpecificationViewModelFactory = new EstimationSpecificationViewModelFactory(gui.ForestAnalysis)
                    };
                    OnPropertyChanged(nameof(EventTree));
                    OnPropertyChanged(nameof(SelectedTreeEvent));
                    break;
            }
        }

        private void SelectionChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SelectedTreeEvent));
        }

        /*private void HydrodynamicsViewModelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (var item in e.NewItems.OfType<HydrodynamicConditionViewModel>())
                    analysisManipulationService.AddHydrodynamicCondition(item.HydrodynamicCondition);

            if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (var item in e.OldItems.OfType<HydrodynamicConditionViewModel>())
                    analysisManipulationService.RemoveHydraulicCondition(item.HydrodynamicCondition);
        }

        private void ExpertViewModelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (var item in e.NewItems.OfType<ExpertViewModel>())
                    analysisManipulationService.AddExpert(item.Expert);

            if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (var item in e.OldItems.OfType<ExpertViewModel>())
                    analysisManipulationService.RemoveExpert(item.Expert);
        }*/

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