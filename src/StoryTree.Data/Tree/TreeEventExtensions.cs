using System;
using System.Collections.Generic;
using StoryTree.Data.Estimations;

namespace StoryTree.Data.Tree
{
    public static class TreeEventExtensions
    {
        public static FragilityCurve GetFragilityCurve(this TreeEvent treeEvent, IEnumerable<double> waterLevels)
        {
            switch (treeEvent.ProbabilitySpecificationType)
            {
                case ProbabilitySpecificationType.Classes:
                    return treeEvent.ClassesProbabilitySpecification.GetFragilityCurve(waterLevels);
                case ProbabilitySpecificationType.FixedFreqeuncy:
                    // TODO: Interpolate if necessary
                    return treeEvent.FixedFragilityCurve;
                case ProbabilitySpecificationType.FixedValue:
                    var curve = new FragilityCurve();
                    foreach (var waterLevel in waterLevels)
                    {
                        curve.Add(new FragilityCurveElement(waterLevel, treeEvent.FixedProbability));
                    }

                    return curve;
                default:
                    throw new NotImplementedException();
            }
        }

        public static FragilityCurve GetUpperFragilityCurves(this TreeEvent treeEvent, IEnumerable<double> orderedWaterLevels)
        {
            if (treeEvent.ProbabilitySpecificationType == ProbabilitySpecificationType.Classes)
            {
                return treeEvent.ClassesProbabilitySpecification.GetUpperFragilityCurve(orderedWaterLevels);
            }

            return treeEvent.GetFragilityCurve(orderedWaterLevels);
        }

        public static FragilityCurve GetLowerFragilityCurve(this TreeEvent treeEvent, IEnumerable<double> orderedWaterLevels)
        {
            if (treeEvent.ProbabilitySpecificationType == ProbabilitySpecificationType.Classes)
            {
                return treeEvent.ClassesProbabilitySpecification.GetLowerFragilityCurve(orderedWaterLevels);
            }

            return treeEvent.GetFragilityCurve(orderedWaterLevels);
        }
    }
}
