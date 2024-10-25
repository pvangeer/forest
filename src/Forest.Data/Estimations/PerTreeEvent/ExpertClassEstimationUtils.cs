using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Forest.Data.Probabilities;

namespace Forest.Data.Estimations.PerTreeEvent
{
    public static class ExpertClassEstimationUtils
    {
        public static FragilityCurve GetClassBasedUpperFragilityCurve(ExpertClassEstimation[] estimations, IEnumerable<double> waterLevels)
        {
            var curve = new FragilityCurve();
            foreach (var waterLevel in waterLevels)
                curve.Add(new FragilityCurveElement(waterLevel,
                    GetClassesBasedProbabilityForWaterLevel(estimations, waterLevel, e => e.MaxEstimation)));

            return curve;
        }

        public static FragilityCurve GetClassBasedLowerFragilityCurve(ExpertClassEstimation[] estimations, IEnumerable<double> waterLevels)
        {
            var curve = new FragilityCurve();
            foreach (var waterLevel in waterLevels)
                curve.Add(new FragilityCurveElement(waterLevel,
                    GetClassesBasedProbabilityForWaterLevel(estimations, waterLevel, e => e.MinEstimation)));

            return curve;
        }

        public static Probability GetClassesBasedProbabilityForWaterLevel(IEnumerable<ExpertClassEstimation> estimations, double waterLevel,
            Func<ExpertClassEstimation, ProbabilityClass> getProbabilityClassFunc = null)
        {
            if (getProbabilityClassFunc == null)
                getProbabilityClassFunc = e => e.AverageEstimation;

            var relevantEstimations = estimations
                .Where(e => Math.Abs(e.HydrodynamicCondition.WaterLevel - waterLevel) < 1e-8).ToArray();
            if (relevantEstimations.Length == 0)
                return Probability.NaN;

            var allRelevantEstimations = relevantEstimations.Select(e => ClassToProbabilityDouble(getProbabilityClassFunc(e))).ToArray();
            var validEstimation = allRelevantEstimations.Where(e => !double.IsNaN(e)).ToArray();
            return validEstimation.Any() ? (Probability)validEstimation.Average() : Probability.NaN;
        }

        private static double ClassToProbabilityDouble(ProbabilityClass probabilityClass)
        {
            switch (probabilityClass)
            {
                case ProbabilityClass.One:
                    return 0.999;
                case ProbabilityClass.Two:
                    return 0.99;
                case ProbabilityClass.Three:
                    return 0.9;
                case ProbabilityClass.Four:
                    return 0.5;
                case ProbabilityClass.Five:
                    return 0.1;
                case ProbabilityClass.Six:
                    return 0.01;
                case ProbabilityClass.Seven:
                    return 0.001;
                case ProbabilityClass.None:
                    return double.NaN;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}