using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Probabilities;

namespace Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent
{
    public class FixedProbabilitySpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        public FixedProbabilitySpecificationViewModel(TreeEventProbabilityEstimate estimate, ViewModelFactory factory) : base(estimate, factory)
        {
        }

        public Probability FixedProbability
        {
            get => Estimate.FixedProbability;
            set => Estimate.FixedProbability = value;
        }
    }
}