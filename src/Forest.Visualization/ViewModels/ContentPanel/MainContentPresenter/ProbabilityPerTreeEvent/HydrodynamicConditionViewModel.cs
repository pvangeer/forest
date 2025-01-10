using Forest.Data.Hydrodynamics;

namespace Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent
{
    public class HydrodynamicConditionViewModel : FragilityCurveElementViewModel
    {
        public HydrodynamicConditionViewModel() : this(new HydrodynamicCondition())
        {
        }

        public HydrodynamicConditionViewModel(HydrodynamicCondition condition) : base(condition)
        {
            HydrodynamicCondition = condition;
        }

        public HydrodynamicCondition HydrodynamicCondition { get; }
    }
}