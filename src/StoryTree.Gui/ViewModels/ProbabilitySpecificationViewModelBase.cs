using StoryTree.Data.Estimations;

namespace StoryTree.Gui.ViewModels
{
    public class ProbabilitySpecificationViewModelBase
    {
        public ProbabilitySpecificationViewModelBase(IProbabilitySpecification specification)
        {
            Specification = specification;
        }

        public IProbabilitySpecification Specification { get; }

        public ProbabilitySpecificationType Type => Specification.Type;
    }
}