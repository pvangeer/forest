using System;
using System.Collections.Generic;
using Forest.Data.Probabilities;

namespace Forest.Data.Estimations.PerTreeEvent
{
    public static class TreeEventProbabilityEstimationExtensions
    {
        public static FragilityCurve GetFragilityCurve(this TreeEventProbabilityEstimate estimate, IEnumerable<double> waterLevels)
        {
            switch (estimate.ProbabilitySpecificationType)
            {
                case ProbabilitySpecificationType.FixedFrequency:
                    // TODO: Interpolate if necessary
                    return estimate.FragilityCurve;
                case ProbabilitySpecificationType.FixedValue:
                    var curve = new FragilityCurve();
                    foreach (var waterLevel in waterLevels)
                        curve.Add(new FragilityCurveElement(waterLevel, estimate.FixedProbability));

                    return curve;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void ChangeProbabilityEstimationType(this TreeEventProbabilityEstimate estimate,
            ProbabilitySpecificationType type)
        {
            if (estimate.ProbabilitySpecificationType == type)
                return;

            estimate.ProbabilitySpecificationType = type;
            estimate.OnPropertyChanged(nameof(TreeEventProbabilityEstimate.ProbabilitySpecificationType));
        }
    }
}