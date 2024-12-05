using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Tree;
using Forest.Gui;
using Forest.Visualization.TreeView.Data;
using Forest.Visualization.ViewModels.ContentPanel;
using Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.EventTreeEditing;
using Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent;
using Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer;
using Forest.Visualization.ViewModels.Ribbon;
using Forest.Visualization.ViewModels.StatusBar;

namespace Forest.Visualization.ViewModels
{
    public class ViewModelFactory
    {
        private readonly ForestGui gui;

        public ViewModelFactory(ForestGui gui)
        {
            this.gui = gui;
        }

        public ITreeNodeViewModel CreateProjectExplorerEventTreeCollectionViewModel()
        {
            return new ProjectExplorerEventTreeCollectionViewModel(gui);
        }

        public ITreeNodeViewModel CreateProjectExplorerProbabilityEstimationCollectionViewModel()
        {
            return new ProjectExplorerProbabilityEstimationCollectionViewModel(gui);
        }

        public ProjectExplorerViewModel CreateProjectExplorerViewModel()
        {
            return new ProjectExplorerViewModel(gui);
        }

        public BusyOverlayViewModel CreateBusyOverlayViewModel()
        {
            return new BusyOverlayViewModel(gui);
        }

        public RibbonViewModel CreateRibbonViewModel()
        {
            return new RibbonViewModel(gui);
        }

        public StatusBarViewModel CreateStatusBarViewModel()
        {
            return new StatusBarViewModel(gui);
        }

        public ITreeNodeViewModel CreateProjectExplorerEventTreeNodeViewModel(EventTree eventTree)
        {
            return new ProjectExplorerEventTreeNodeViewModel(eventTree, gui);
        }

        public ITreeNodeViewModel CreateProjectExplorerEstimationItemViewModel(ProbabilityEstimationPerTreeEvent estimation)
        {
            return new ProjectExplorerEstimationItemViewModel(estimation, gui);
        }

        public MainContentPresenterViewModel CreateMainContentPresenterViewModel()
        {
            return new MainContentPresenterViewModel(gui);
        }

        public object CreateMainContentViewModel(object selection)
        {
            switch (selection)
            {
                case EventTree eventTree:
                    return new EventTreeMainContentViewModel(eventTree, gui);
                case ProbabilityEstimationPerTreeEvent estimation:
                    return new ProbabilityPerTreeEventMainContentViewModel(estimation, gui);
                default:
                    return selection;
            }
        }

        public TreeEventViewModel CreateTreeEventViewModel(TreeEvent treeEvent, EventTree eventTree)
        {
            return treeEvent != null ? new TreeEventViewModel(treeEvent, eventTree, gui) : null;
        }

        public ExpertsViewModel CreateExpertsViewModel(ProbabilityEstimationPerTreeEvent estimation)
        {
            return new ExpertsViewModel(estimation);
        }

        public HydrodynamicsViewModel CreateHydrodynamicsViewModel(ProbabilityEstimationPerTreeEvent estimation)
        {
            return new HydrodynamicsViewModel(estimation);
        }

        public ClassesProbabilitySpecificationViewModel CreateClassesProbabilitySpecificationViewModel(TreeEventProbabilityEstimate estimation)
        {
            return new ClassesProbabilitySpecificationViewModel(estimation, this);
        }

        public FixedFragilityCurveSpecificationViewModel CreateFragilityCurveSpecificationTypeViewModel(TreeEventProbabilityEstimate estimation,
            ProbabilityEstimationPerTreeEvent parentEstimation)
        {
            return new FixedFragilityCurveSpecificationViewModel(estimation, parentEstimation, this);
        }

        public ExpertClassEstimationViewModel CreateExpertClassEstimationViewModel(ExpertClassEstimation expertClassEstimation)
        {
            return new ExpertClassEstimationViewModel(expertClassEstimation);
        }

        public FixedProbabilitySpecificationViewModel CreateFixedProbabilitySpecificationTypeViewModel(TreeEventProbabilityEstimate estimation)
        {
            return new FixedProbabilitySpecificationViewModel(estimation, this);
        }

        public EstimationPerTreeEventSpecificationViewModel CreateEstimationPerTreeEventSpecificationViewModel(ProbabilityEstimationPerTreeEvent estimation)
        {
            return new EstimationPerTreeEventSpecificationViewModel(estimation, gui, this);
        }

        public TreeEventEstimationViewModel CreateTreeEventEstimationViewModel(TreeEventProbabilityEstimate estimation,
            ProbabilityEstimationPerTreeEvent parentEstimation)
        {
            return new TreeEventEstimationViewModel(estimation, parentEstimation, gui, this);
        }

        public AboutBoxViewModel CreateAboutBoxViewModel()
        {
            return new AboutBoxViewModel(this);
        }
    }
}