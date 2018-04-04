using System;
using System.Collections.Generic;
using StoryTree.Data.Estimations.Classes;

namespace StoryTree.Data.Estimations
{
    public class FixedValueProbabilitySpecification : IProbabilitySpecification
    {
        public FixedValueProbabilitySpecification(double probability = 1)
        {
            FixedValueProbability = (Probability)probability;
        }

        public ProbabilitySpecificationType Type => ProbabilitySpecificationType.FixedValue;

        public FragilityCurve GetFragilityCurve(IEnumerable<double> waterLevels)
        {
            var curve = new FragilityCurve();
            foreach (var waterLevel in waterLevels)
            {
                curve.Add(new FragilityCurveElement(waterLevel,FixedValueProbability));
            }

            return curve;
        }

        public Probability FixedValueProbability { get; set; }
    }
}
