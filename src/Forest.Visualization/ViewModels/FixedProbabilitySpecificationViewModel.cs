using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Probabilities;
using Forest.Data.Tree;

namespace Forest.Visualization.ViewModels
{
    public class FixedProbabilitySpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        public FixedProbabilitySpecificationViewModel(TreeEvent treeEvent, TreeEventProbabilityEstimate estimate) : base(treeEvent,
            estimate)
        {
        }

        public Probability FixedProbability
        {
            get => Estimate.FixedProbability;
            set => Estimate.FixedProbability = value;
        }
    }
}