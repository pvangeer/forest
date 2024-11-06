using System.Collections.ObjectModel;
using System.Linq;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Properties;
using Forest.Data.Tree;

namespace Forest.Visualization.ViewModels
{
    public class ClassesProbabilitySpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        public ClassesProbabilitySpecificationViewModel([NotNull] TreeEvent treeEvent,
            TreeEventProbabilityEstimate estimate) : base(treeEvent, estimate)
        {
            // TODO: React on collection changes
            ClassesProbabilitySpecification = new ObservableCollection<ExpertClassEstimationViewModel>(
                estimate.ClassProbabilitySpecifications.Select(e => new ExpertClassEstimationViewModel(e)));
        }

        public ObservableCollection<ExpertClassEstimationViewModel> ClassesProbabilitySpecification { get; }
    }
}