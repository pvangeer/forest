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
    }
}