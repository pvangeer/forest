using Forest.Data;
using Forest.Data.Estimations;
using Forest.Data.Tree;

namespace Forest.Visualization.ViewModels
{
    public class FixedProbabilitySpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        public FixedProbabilitySpecificationViewModel(TreeEvent treeEvent, TreeEventProbabilityEstimation estimation) : base(treeEvent, estimation)
        {
        }

        public Probability FixedProbability
        {
            get => TreeEvent.FixedProbability;
            set => TreeEvent.FixedProbability = value;
        }
    }
}