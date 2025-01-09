using Forest.Data.Estimations.PerTreeEvent.Experts;
using Forest.Data.Hydrodynamics;

namespace Forest.Data.Estimations.PerTreeEvent
{
    public static class ProbabilityEstimationPerTreeEventExtensions
    {
        public static void AddHydrodynamicCondition(this ProbabilityEstimationPerTreeEvent probabilityEstimation,
            HydrodynamicCondition hydrodynamicCondition)
        {
            probabilityEstimation.HydrodynamicConditions.Add(hydrodynamicCondition);
        }

        public static void RemoveHydraulicCondition(this ProbabilityEstimationPerTreeEvent probabilityEstimation,
            HydrodynamicCondition hydrodynamicCondition)
        {
            probabilityEstimation.HydrodynamicConditions.Remove(hydrodynamicCondition);
        }

        public static void AddExpert(this ProbabilityEstimationPerTreeEvent probabilityEstimation, Expert expert)
        {
            probabilityEstimation.Experts.Add(expert);
        }

        public static void RemoveExpert(this ProbabilityEstimationPerTreeEvent probabilityEstimation, Expert expert)
        {
            probabilityEstimation.Experts.Remove(expert);
        }
    }
}