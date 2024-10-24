using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Forest.Data.Estimations;

namespace Forest.Data.Tree
{
    public static class TreeEventExtensions
    {
        public static FragilityCurve GetFragilityCurve(this TreeEventProbabilityEstimation estimation, IEnumerable<double> waterLevels)
        {
            switch (estimation.ProbabilitySpecificationType)
            {
                case ProbabilitySpecificationType.Classes:
                    var classCurve = new FragilityCurve();
                    foreach (var waterLevel in waterLevels)
                        classCurve.Add(
                            new FragilityCurveElement(waterLevel, GetClassesBasedProbabilityForWaterLevel(estimation.ClassProbabilitySpecification, waterLevel)));

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

        public static FragilityCurve GetUpperFragilityCurves(this TreeEventProbabilityEstimation estimation, IEnumerable<double> orderedWaterLevels)
        {
            if (estimation.ProbabilitySpecificationType == ProbabilitySpecificationType.Classes)
                return GetClassBasedUpperFragilityCurve(estimation.ClassProbabilitySpecification.ToArray(),orderedWaterLevels);

            return estimation.GetFragilityCurve(orderedWaterLevels);
        }

        public static FragilityCurve GetLowerFragilityCurve(this TreeEventProbabilityEstimation estimation, IEnumerable<double> orderedWaterLevels)
        {
            if (estimation.ProbabilitySpecificationType == ProbabilitySpecificationType.Classes)
                return GetClassBasedLowerFragilityCurve(estimation.ClassProbabilitySpecification.ToArray(), orderedWaterLevels);

            return estimation.GetFragilityCurve(orderedWaterLevels);
        }

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

        public static IEnumerable<TreeEvent> GetAllEventsRecursive(this TreeEvent treeEvent)
        {
            var list = new[] { treeEvent };

            if (treeEvent == null)
                return list;

            if (treeEvent.FailingEvent != null)
                list = list.Concat(GetAllEventsRecursive(treeEvent.FailingEvent)).ToArray();

            if (treeEvent.PassingEvent != null)
                list = list.Concat(GetAllEventsRecursive(treeEvent.PassingEvent)).ToArray();

            return list;
        }

        public static TreeEvent FindTreeEvent(this TreeEvent treeEvent, Func<TreeEvent, bool> findAction)
        {
            if (findAction(treeEvent))
                return treeEvent;

            if (treeEvent.FailingEvent != null)
            {
                var possibleEvent = FindTreeEvent(treeEvent.FailingEvent, findAction);
                if (possibleEvent != null)
                    return possibleEvent;
            }

            if (treeEvent.PassingEvent != null)
            {
                var possibleEvent = FindTreeEvent(treeEvent.PassingEvent, findAction);
                if (possibleEvent != null)
                    return possibleEvent;
            }

            return null;
        }
    }
}