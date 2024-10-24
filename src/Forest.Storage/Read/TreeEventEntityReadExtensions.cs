using System;
using System.Collections.Generic;
using System.Linq;
using Forest.Data.Estimations;
using Forest.Data.Tree;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Read
{
    internal static class TreeEventEntityReadExtensions
    {
        internal static TreeEvent Read(this TreeEventXmlEntity entity, ReadConversionCollector collector)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (collector == null)
                throw new ArgumentNullException(nameof(collector));

            if (collector.Contains(entity))
                return collector.Get(entity);

            var treeEvent = new TreeEvent(entity.Name)
            {
                FixedProbability = double.IsNaN(entity.FixedProbability)
                    ? Probability.NaN
                    : (Probability)entity.FixedProbability,
                Summary = entity.Summary,
                Information = entity.Information,
                Discussion = entity.Discussion
                // TODO: Add PassPhrase
            };

            ReadFragilityCurve(treeEvent, entity.FixedFragilityCurveElements, collector);

            if (entity.FailingEvent != null)
                treeEvent.FailingEvent = entity.FailingEvent.Read(collector);

            if (entity.PassingEvent != null)
                treeEvent.PassingEvent = entity.PassingEvent.Read(collector);

            ReadExpertClassSpecifications(entity, collector, treeEvent);
            collector.Collect(entity, treeEvent);
            return treeEvent;
        }

        private static void ReadExpertClassSpecifications(TreeEventXmlEntity entity, ReadConversionCollector collector, TreeEvent treeEvent)
        {
            var specifications = entity.ClassesProbabilitySpecifications.OrderBy(e => e.Order).Select(e => e.Read(collector));
            foreach (var specification in specifications)
                treeEvent.ClassesProbabilitySpecification.Add(specification);
        }

        private static void ReadFragilityCurve(TreeEvent treeEvent, IEnumerable<FragilityCurveElementXmlEntity> entities,
            ReadConversionCollector collector)
        {
            foreach (var fragilityCurveElementEntity in entities.OrderBy(e => e.Order))
                treeEvent.FixedFragilityCurve.Add(fragilityCurveElementEntity.Read(collector));
        }
    }
}