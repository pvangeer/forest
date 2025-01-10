using Forest.Data.Hydrodynamics;
using Forest.Data.Probabilities;

namespace Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent
{
    public class HydrodynamicConditionViewModel : FragilityCurveElementViewModel
    {
        public HydrodynamicConditionViewModel() : this(new FragilityCurveElement(0, (Probability)(1 / 1000.0)))
        {
        }

        public HydrodynamicConditionViewModel(FragilityCurveElement condition) : base(condition)
        {
            HydrodynamicCondition = condition;
        }

        public FragilityCurveElement HydrodynamicCondition { get; }
    }
}