using StoryTree.Data.Estimations;
using StoryTree.Data.Estimations.Classes;

namespace StoryTree.Gui.ViewModels
{
    public class FixedProbabilitySpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        public FixedProbabilitySpecificationViewModel(FixedValueProbabilitySpecification probabilitySpecification) : base(probabilitySpecification) { }

        public FixedValueProbabilitySpecification FixedValueProbabilitySpecification => (FixedValueProbabilitySpecification)Specification;

        public Probability FixedProbability
        {
            get => FixedValueProbabilitySpecification.FixedValueProbability;
            set => FixedValueProbabilitySpecification.FixedValueProbability = value;
        }
    }
}