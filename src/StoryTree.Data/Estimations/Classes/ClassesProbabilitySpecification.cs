using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using StoryTree.Data.Tree;

namespace StoryTree.Data.Estimations.Classes
{
    public class ClassesProbabilitySpecification : IProbabilitySpecification
    {
        public ClassesProbabilitySpecification()
        {
            Estimations = new ObservableCollection<ExpertClassEstimation>();
        }

        public ProbabilitySpecificationType Type => ProbabilitySpecificationType.Classes;

        public Probability GetProbability()
        {
            throw new NotImplementedException();
        }

        public Probability GetProbabilityForWaterLevel(double waterLevel)
        {
            var relevantEstimations = Estimations.Where(e => Math.Abs(e.WaterLevel - waterLevel) < 1e-8).ToArray();
            if (relevantEstimations.Length == 0)
            {
                return Probability.NaN;
            }

            return (Probability) relevantEstimations
                .Select(e => ClassToProbabilityDouble(e.AverageEstimation))
                .Average();
        }

        public FragilityCurve GetFragilityCurve(IEnumerable<double> waterLevels)
        {
            var curve = new FragilityCurve();
            foreach (var waterLevel in waterLevels)
            {
                curve.Add(new FragilityCurveElement(waterLevel,GetProbabilityForWaterLevel(waterLevel)));
            }

            return curve;
        }

        private double ClassToProbabilityDouble(ProbabilityClass probabilityClass)
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
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public ObservableCollection<ExpertClassEstimation> Estimations { get; }
    }
}
