using System.Windows.Input;
using Forest.Data.Services;
using Forest.Gui;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public class ProjectExplorerProbabilityEstimationCollectionViewModel : ProjectExplorerProbabilityEstimationCollectionViewModelBase
    {
        public ProjectExplorerProbabilityEstimationCollectionViewModel(ForestGui gui) : base(gui)
        {
        }

        public override string DisplayName => "Faalkansinschattingen";

        public override ICommand AddItemCommand => CommandFactory.CreateCanAlwaysExecuteActionCommand(p =>
        {
            var service = new AnalysisManipulationService(Gui.ForestAnalysis);
            service.AddProbabilityEstimationPerTreeEvent(Gui.ForestAnalysis.EventTree);
        });
    }
}