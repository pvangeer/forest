using Forest.Data;
using Forest.Data.Tree;

namespace Forest.Gui.ViewModels
{
    public class FixedProbabilitySpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        public FixedProbabilitySpecificationViewModel(TreeEvent treeEvent) : base(treeEvent)
        {
        }

        public Probability FixedProbability
        {
            get => TreeEvent.FixedProbability;
            set => TreeEvent.FixedProbability = value;
        }
    }
}