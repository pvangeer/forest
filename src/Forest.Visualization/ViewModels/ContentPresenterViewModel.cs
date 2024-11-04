using System.ComponentModel;
using System.Linq;
using Forest.Data;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Services;
using Forest.Gui;

namespace Forest.Visualization.ViewModels
{
    // TODO: Remove this viewmodel
    public class ContentPresenterViewModel : GuiViewModelBase
    {
        private readonly AnalysisManipulationService analysisManipulationService;

        public ContentPresenterViewModel(ForestGui gui) : base(gui)
        {
            if (Gui != null)
            {
                Gui.SelectionManager.PropertyChanged += SelectionChanged;

                analysisManipulationService = new AnalysisManipulationService(Gui.ForestAnalysis);

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

        private ForestAnalysis ForestAnalysis => Gui.ForestAnalysis;

        public EventTreeViewModelOld EventTreeViewModel { get; set; }

        public TreeEventViewModelOld SelectedTreeEvent => EventTreeViewModel.SelectedTreeEvent;

        public ExpertsViewModel ExpertsViewModel { get; }

        public HydrodynamicsViewModel HydrodynamicsViewModel { get; }

        protected override void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.ForestAnalysis):
                    OnPropertyChanged(nameof(EventTreeViewModel));
                    OnPropertyChanged(nameof(SelectedTreeEvent));
                    break;
            }
        }

        private void SelectionChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SelectedTreeEvent));
        }
    }
}