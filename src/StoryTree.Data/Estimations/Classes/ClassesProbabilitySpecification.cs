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
        public ProbabilitySpecificationType Type => ProbabilitySpecificationType.Classes;

        public Dictionary<double,Probability> Probabilities
        {
            get
            {
                var allEstimations = Estimations.SelectMany(e => e.Estimations).ToArray();
                var allWaterlevels = allEstimations.Select(e => e.WaterLevel).Distinct().ToArray();

                var dict = new Dictionary<double,Probability>();

                foreach (var waterlevel in allWaterlevels)
                {
                    dict[waterlevel] = (Probability) allEstimations
                        .Where(e => Math.Abs(e.WaterLevel - waterlevel) < 1e-8)
                        .Select(e => ClassToProbabilityDouble(e.AverageEstimation))
                        .Average();
                }

                return dict;
            }
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
