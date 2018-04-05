using StoryTree.Data.Estimations.Classes;
using StoryTree.Data.Tree;

namespace StoryTree.Gui.ViewModels
{
    public class FixedProbabilitySpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        public FixedProbabilitySpecificationViewModel(TreeEvent treeEvent) : base(treeEvent) { }

        public Probability FixedProbability
        {
            get => TreeEvent.FixedProbability;
            set => TreeEvent.FixedProbability = value;
        }
    }
}