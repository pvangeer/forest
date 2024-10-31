using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Forest.Data;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Services;
using Forest.Gui;

namespace Forest.Visualization.ViewModels
{
    public class ContentPresenterViewModel : GuiViewModelBase
    {
        private readonly AnalysisManipulationService analysisManipulationService;

        public ContentPresenterViewModel(ViewModelFactory factory,ForestGui gui) : base(factory, gui)
        {
            if (Gui != null)
            {
                Gui.SelectionManager.PropertyChanged += SelectionChanged;

                analysisManipulationService = new AnalysisManipulationService(Gui.ForestAnalysis);

                EventTreeViewModel = new EventTreeViewModel(ForestAnalysis.EventTree, analysisManipulationService, Gui.SelectionManager,
                    gui.ForestAnalysis.ProbabilityEstimations)
                {
                    EstimationSpecificationViewModelFactory = new EstimationSpecificationViewModelFactory(Gui.ForestAnalysis)
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

        private ForestAnalysis ForestAnalysis => Gui.ForestAnalysis;

        public EventTreeViewModel EventTreeViewModel { get; set; }

        public TreeEventViewModel SelectedTreeEvent => EventTreeViewModel.SelectedTreeEvent;

        public ForestGuiState SelectedGuiState
        {
            get => Gui.SelectedState;
            set
            {
                Gui.SelectedState = value;
                Gui.OnPropertyChanged(nameof(ForestGui.SelectedState));
            }
        }

        public ExpertsViewModel ExpertsViewModel { get; }

        public HydrodynamicsViewModel HydrodynamicsViewModel { get; }

        protected override void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.SelectedState):
                    OnPropertyChanged(nameof(SelectedGuiState));
                    break;
                case nameof(ForestGui.ForestAnalysis):
                    EventTreeViewModel = new EventTreeViewModel(ForestAnalysis.EventTree, analysisManipulationService, Gui.SelectionManager,
                        Gui.ForestAnalysis.ProbabilityEstimations)
                    {
                        EstimationSpecificationViewModelFactory = new EstimationSpecificationViewModelFactory(Gui.ForestAnalysis)
                    };
                    OnPropertyChanged(nameof(EventTreeViewModel));
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
    }
}