using Forest.Data.Probabilities;

namespace Forest.Data.Estimations.PerTreeEvent
{
    public static class ProbabilityEstimationPerTreeEventExtensions
    {
        public static void AddHydrodynamicCondition(this ProbabilityEstimationPerTreeEvent probabilityEstimation,
            FragilityCurveElement hydrodynamicCondition)
        {
            probabilityEstimation.HydrodynamicConditions.Add(hydrodynamicCondition);
        }

        public static void RemoveHydraulicCondition(this ProbabilityEstimationPerTreeEvent probabilityEstimation,
            FragilityCurveElement hydrodynamicCondition)
        {
            probabilityEstimation.HydrodynamicConditions.Remove(hydrodynamicCondition);
        }
    }
}