using System;
using System.Linq;
using Forest.Data.Probabilities;

namespace Forest.Calculators
{
    public static class EstimationFragilityCurveCalculator
    {
        public static Probability CalculateProbability(FragilityCurveElement[] conditions, CriticalPathElement[] treeEventCurves)
        {
            return CalculateProbability(CalculateCombinedProbabilityFragilityCurve(conditions, treeEventCurves));
        }

        public static FragilityCurve CalculateCombinedProbabilityFragilityCurve(FragilityCurveElement[] conditions,
            CriticalPathElement[] treeEventCurves)
        {
            var curve = new FragilityCurve();
            for (var i = 0; i < conditions.Length - 1; i++)
            {
                double waterLevelProbability = conditions[i].Probability;
                double nextWaterLevelProbability = conditions[i + 1].Probability;
                double estimatedCombinedProbability = CalculateConditionalProbability(conditions[i].WaterLevel, treeEventCurves);
                double nextEstimatedCombinedProbability = CalculateConditionalProbability(conditions[i + 1].WaterLevel, treeEventCurves);
                var m9 = (estimatedCombinedProbability - nextEstimatedCombinedProbability) /
                         Math.Log(waterLevelProbability / nextWaterLevelProbability);
                var n9 = estimatedCombinedProbability - m9 * Math.Log(waterLevelProbability);
                var interpolated = (Probability)(n9 * waterLevelProbability +
                                                  m9 * (waterLevelProbability * Math.Log(waterLevelProbability) - waterLevelProbability)
                                                  - (n9 * nextWaterLevelProbability +
                                                     m9 * (nextWaterLevelProbability * Math.Log(nextWaterLevelProbability) -
                                                           nextWaterLevelProbability)));
                curve.Add(new FragilityCurveElement(conditions[i].WaterLevel, interpolated));
            }

            var lastCondition = conditions.Last();
            curve.Add(new FragilityCurveElement(lastCondition.WaterLevel, lastCondition.Probability));
            return curve;
        }

        public static FragilityCurve CalculateCombinedFragilityCurve(FragilityCurveElement[] conditions,
            CriticalPathElement[] criticalPathElements)
        {
            var curve = new FragilityCurve();
            foreach (var condition in conditions)
            {
                curve.Add(new FragilityCurveElement(condition.WaterLevel,
                    CalculateConditionalProbability(condition.WaterLevel, criticalPathElements)));
            }

            return curve;
        }

        private static Probability CalculateProbability(FragilityCurve partialProbabilityCurve)
        {
            return (Probability)partialProbabilityCurve.Sum(p => p.Probability);
        }

        private static Probability CalculateConditionalProbability(double waterLevel, CriticalPathElement[] criticalPathElements)
        {
            var probability = (Probability)1.0;
            foreach (var criticalPathElement in criticalPathElements)
            {
                var fragilityCurveElement =
                    criticalPathElement.FragilityCurve.FirstOrDefault(e => Math.Abs(e.WaterLevel - waterLevel) < 1e-8);
                if (fragilityCurveElement == null)
                    throw new ArgumentException();

                var estimatedProbabilityForTreeEvent = criticalPathElement.ElementFails
                    ? fragilityCurveElement.Probability
                    : 1 - fragilityCurveElement.Probability;
                probability *= estimatedProbabilityForTreeEvent;
            }

            return probability;
        }
    }
}