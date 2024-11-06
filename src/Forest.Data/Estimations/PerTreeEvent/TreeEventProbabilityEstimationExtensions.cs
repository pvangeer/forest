using System;
using System.Collections.Generic;
using System.Linq;
using Forest.Data.Probabilities;

namespace Forest.Data.Estimations.PerTreeEvent
{
    public static class TreeEventProbabilityEstimationExtensions
    {
        public static FragilityCurve GetFragilityCurve(this TreeEventProbabilityEstimate estimate, IEnumerable<double> waterLevels)
        {
            switch (estimate.ProbabilitySpecificationType)
            {
                case ProbabilitySpecificationType.Classes:
                    var classCurve = new FragilityCurve();
                    foreach (var waterLevel in waterLevels)
                        classCurve.Add(
                            new FragilityCurveElement(waterLevel,
                                ExpertClassEstimationUtils.GetClassesBasedProbabilityForWaterLevel(estimate.ClassProbabilitySpecifications,
                                    waterLevel)));

                    return classCurve;
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

        public static FragilityCurve GetUpperFragilityCurves(this TreeEventProbabilityEstimate estimate,
            IEnumerable<double> orderedWaterLevels)
        {
            if (estimate.ProbabilitySpecificationType == ProbabilitySpecificationType.Classes)
                return ExpertClassEstimationUtils.GetClassBasedUpperFragilityCurve(estimate.ClassProbabilitySpecifications.ToArray(),
                    orderedWaterLevels);

            return estimate.GetFragilityCurve(orderedWaterLevels);
        }

        public static FragilityCurve GetLowerFragilityCurve(this TreeEventProbabilityEstimate estimate,
            IEnumerable<double> orderedWaterLevels)
        {
            if (estimate.ProbabilitySpecificationType == ProbabilitySpecificationType.Classes)
                return ExpertClassEstimationUtils.GetClassBasedLowerFragilityCurve(estimate.ClassProbabilitySpecifications.ToArray(),
                    orderedWaterLevels);

            return estimate.GetFragilityCurve(orderedWaterLevels);
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