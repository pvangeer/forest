using StoryTree.Data.Estimations;
using StoryTree.Data.Tree;

namespace StoryTree.Gui.ViewModels
{
    public class ProbabilitySpecificationViewModelBase
    {
        public ProbabilitySpecificationViewModelBase(TreeEvent treeEvent)
        {
            TreeEvent = treeEvent;
        }

        public TreeEvent TreeEvent { get; }

        public ProbabilitySpecificationType Type => TreeEvent.ProbabilitySpecificationType;
    }
}