using System.Windows.Input;
using Forest.Data.Estimations;
using Forest.Data.Services;
using Forest.Gui;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public class ProjectExplorerEstimationItemViewModel : ProjectExplorerItemViewModelBase
    {
        private readonly ProbabilityEstimation estimation;
        private readonly ProjectExplorerCommandFactory commandFactory;
        private readonly ForestGui gui;

        public ProjectExplorerEstimationItemViewModel(ProbabilityEstimation estimation, ForestGui gui) : base(new ViewModelFactory(gui))
        {
            this.estimation = estimation;
            this.gui = gui;
            commandFactory = new ProjectExplorerCommandFactory(gui);
        }

        public override ICommand SelectItemCommand => commandFactory.CreateSelectItemCommand(this);

        public override object GetSelectableObject()
        {
            return estimation;
        }

        public override string DisplayName => estimation.Name;

        public override string IconSourceString =>
            "pack://application:,,,/Forest.Visualization;component/Resources/ProjectExplorer/probability_estimation.ico";

        public override bool IsViewModelFor(object item)
        {
            return item is ProbabilityEstimation probability && probability == estimation;
        }

        public override ICommand RemoveItemCommand => commandFactory.CreateCanAlwaysExecuteActionCommand(o =>
        {
            var service = new AnalysisManipulationService(gui.ForestAnalysis);
            service.RemoveProbabilityEstimation(estimation);
        });
    }
}