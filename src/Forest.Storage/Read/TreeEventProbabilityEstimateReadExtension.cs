﻿using System.Linq;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Probabilities;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Read
{
    internal static class TreeEventProbabilityEstimateReadExtension
    {
        internal static TreeEventProbabilityEstimate Read(this TreeEventProbabilityEstimateXmlEntity entity,
            ReadConversionCollector collector)
        {
            var estimate = new TreeEventProbabilityEstimate(collector.GetReferencedTreeEvent(entity.TreeEventId))
            {
                FixedProbability = double.IsNaN(entity.FixedProbability) ? Probability.NaN : new Probability(entity.FixedProbability),
                ProbabilitySpecificationType = ProbabilitySpecificationTypeUtils.FromStorageName(entity.ProbabilitySpecificationType)
            };

            var fragilityCurveElements = entity.FragilityCurve.OrderBy(e => e.Order).Select(e => e.Read(collector));
            foreach (var element in fragilityCurveElements)
                estimate.FragilityCurve.Add(element);

            return estimate;
        }
    }
}