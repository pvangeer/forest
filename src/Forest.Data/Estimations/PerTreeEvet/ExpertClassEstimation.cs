using Forest.Data.Experts;
using Forest.Data.Hydrodynamics;

namespace Forest.Data.Estimations.PerTreeEvet
{
    public class ExpertClassEstimation
    {
        public Expert Expert { get; set; }

        public HydrodynamicCondition HydrodynamicCondition { get; set; }

        public ProbabilityClass MinEstimation { get; set; }

        public ProbabilityClass AverageEstimation { get; set; }

        public ProbabilityClass MaxEstimation { get; set; }
    }
}