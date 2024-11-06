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
            TreeEventProbabilityEstimation estimation) : base(treeEvent, estimation)
        {
            // TODO: React on collection changes
            ClassesProbabilitySpecification = new ObservableCollection<ExpertClassEstimationViewModel>(
                estimation.ClassProbabilitySpecifications.Select(e => new ExpertClassEstimationViewModel(e)));
        }

        public ObservableCollection<ExpertClassEstimationViewModel> ClassesProbabilitySpecification { get; }
    }
}