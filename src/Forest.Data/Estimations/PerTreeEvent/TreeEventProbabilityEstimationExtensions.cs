using System;
using System.Collections.Generic;
using System.Linq;
using Forest.Data.Probabilities;

namespace Forest.Data.Estimations.PerTreeEvent
{
    public static class TreeEventProbabilityEstimationExtensions
    {
        public static FragilityCurve GetFragilityCurve(this TreeEventProbabilityEstimation estimation, IEnumerable<double> waterLevels)
        {
            switch (estimation.ProbabilitySpecificationType)
            {
                case ProbabilitySpecificationType.Classes:
                    var classCurve = new FragilityCurve();
                    foreach (var waterLevel in waterLevels)
                        classCurve.Add(
                            new FragilityCurveElement(waterLevel,
                                ExpertClassEstimationUtils.GetClassesBasedProbabilityForWaterLevel(estimation.ClassProbabilitySpecifications,
                                    waterLevel)));

                    return classCurve;
                case ProbabilitySpecificationType.FixedFrequency:
                    // TODO: Interpolate if necessary
                    return estimation.FragilityCurve;
                case ProbabilitySpecificationType.FixedValue:
                    var curve = new FragilityCurve();
                    foreach (var waterLevel in waterLevels)
                        curve.Add(new FragilityCurveElement(waterLevel, estimation.FixedProbability));

                    return curve;
                default:
                    throw new NotImplementedException();
            }
        }

        public static FragilityCurve GetUpperFragilityCurves(this TreeEventProbabilityEstimation estimation,
            IEnumerable<double> orderedWaterLevels)
        {
            if (estimation.ProbabilitySpecificationType == ProbabilitySpecificationType.Classes)
                return ExpertClassEstimationUtils.GetClassBasedUpperFragilityCurve(estimation.ClassProbabilitySpecifications.ToArray(),
                    orderedWaterLevels);

            return estimation.GetFragilityCurve(orderedWaterLevels);
        }

        public static FragilityCurve GetLowerFragilityCurve(this TreeEventProbabilityEstimation estimation,
            IEnumerable<double> orderedWaterLevels)
        {
            if (estimation.ProbabilitySpecificationType == ProbabilitySpecificationType.Classes)
                return ExpertClassEstimationUtils.GetClassBasedLowerFragilityCurve(estimation.ClassProbabilitySpecifications.ToArray(),
                    orderedWaterLevels);

            return estimation.GetFragilityCurve(orderedWaterLevels);
        }

        public static void ChangeProbabilityEstimationType(this TreeEventProbabilityEstimation estimation,
            ProbabilitySpecificationType type)
        {
            if (estimation.ProbabilitySpecificationType == type)
                return;

            estimation.ProbabilitySpecificationType = type;
            estimation.OnPropertyChanged(nameof(TreeEventProbabilityEstimation.ProbabilitySpecificationType));
        }
    }
}