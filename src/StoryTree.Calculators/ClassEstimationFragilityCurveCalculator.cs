using System;
using System.Collections.Generic;
using System.Linq;
using StoryTree.Data;
using StoryTree.Data.Hydraulics;

namespace StoryTree.Calculators
{
    public static class ClassEstimationFragilityCurveCalculator
    {
        public static Probability CalculateProbability(HydraulicCondition[] conditions, CriticalPathElement[] treeEventCurves)
        {
            return CalculateProbability(CalculateCombinedProbabilityFragilityCurve(conditions, treeEventCurves));
        }

        public static Probability CalculateProbability(FragilityCurve partialProbabilityCurve)
        {
            return (Probability)partialProbabilityCurve.Sum(p => p.Probability);
        }

        public static FragilityCurve CalculateCombinedProbabilityFragilityCurve(HydraulicCondition[] conditions, CriticalPathElement[] treeEventCurves)
        {
            if (!CheckProbabilitiesAreEqual(conditions, treeEventCurves))
            {
                throw new ArgumentOutOfRangeException();
            }

            var curve = new FragilityCurve();
            for (int i = 0; i < conditions.Length-1; i++)
            {
                double waterLevelProbability = conditions[i].Probability;
                double nextWaterLevelProbability = conditions[i + 1].Probability;
                double estimatedCombinedProbability = CalculateConditionalProbability(conditions[i].WaterLevel, treeEventCurves);
                double nextEstimatedCombinedProbability = CalculateConditionalProbability(conditions[i+1].WaterLevel, treeEventCurves);
                var m9 = (estimatedCombinedProbability - nextEstimatedCombinedProbability) / Math.Log(waterLevelProbability/nextWaterLevelProbability);
                var n9 = estimatedCombinedProbability - m9*Math.Log(waterLevelProbability);
                var interpiolated = (Probability)((n9* waterLevelProbability + m9*(waterLevelProbability*Math.Log(waterLevelProbability)-waterLevelProbability))
                    - (n9 * nextWaterLevelProbability + m9 * (nextWaterLevelProbability * Math.Log(nextWaterLevelProbability)- nextWaterLevelProbability)));
                curve.Add(new FragilityCurveElement(conditions[i].WaterLevel, interpiolated));
            }

            var lastCondition = conditions.Last();
            curve.Add(new FragilityCurveElement(lastCondition.WaterLevel,lastCondition.Probability));
            return curve;
        }

        public static FragilityCurve CalculateCombinedFragilityCurve(HydraulicCondition[] conditions, CriticalPathElement[] criticalPathElements)
        {
            if (!CheckProbabilitiesAreEqual(criticalPathElements))
            {
                throw new ArgumentOutOfRangeException();
            }

            var curve = new FragilityCurve();
            foreach (var condition in conditions)
            {
                curve.Add(new FragilityCurveElement(condition.WaterLevel, CalculateConditionalProbability(condition.WaterLevel, criticalPathElements)));
            }

            return curve;
        }

        private static Probability CalculateConditionalProbability(double waterLevel, CriticalPathElement[] criticalPathElements)
        {
            var probability = (Probability)1.0;
            foreach (var criticalPathElement in criticalPathElements)
            {
                var fragilityCurveElement = criticalPathElement.FragilityCurve.FirstOrDefault(e => Math.Abs(e.WaterLevel - waterLevel) < 1e-8);
                if (fragilityCurveElement == null)
                {
                    throw new ArgumentException();
                }

                var estimatedProbabilityForTreeEvent = criticalPathElement.ElementFails
                    ? fragilityCurveElement.Probability
                    : 1 - fragilityCurveElement.Probability;
                probability = probability * estimatedProbabilityForTreeEvent;
            }

            return probability;
        }

        private static bool CheckProbabilitiesAreEqual(CriticalPathElement[] treeEventCurves)
        {
            //
            return true;
        }

        private static bool CheckProbabilitiesAreEqual(IEnumerable<HydraulicCondition> hydraulicConditions, IEnumerable<CriticalPathElement> treeEventCurves)
        {
            //
            return true;
        }
    }
}
