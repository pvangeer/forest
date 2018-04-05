using System;
using System.Collections.Generic;
using System.Linq;
using StoryTree.Data;
using StoryTree.Data.Estimations;
using StoryTree.Data.Tree;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Read
{
    internal static class TreeEventEntityReadExtensions
    {
        internal static TreeEvent Read(this TreeEventEntity entity, ReadConversionCollector collector)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (collector == null)
            {
                throw new ArgumentNullException(nameof(collector));
            }

            if (collector.Contains(entity))
            {
                return collector.Get(entity);
            }

            var fixedFragilityCurve = ReadFragilityCurve(entity.TreeEventFragilityCurveElementEntities, collector);
            var treeEvent = new TreeEvent
            {
                Name = entity.Name,
                Details = entity.Details,
                FailingEvent = entity.TreeEventEntity2.Read(collector),
                PassingEvent = entity.TreeEventEntity3.Read(collector),
                FixedProbability = entity.FixedProbability == null
                    ? Probability.NaN
                    : (Probability) (double) entity.FixedProbability,
                FixedFragilityCurve = fixedFragilityCurve,
                ProbabilitySpecificationType = (ProbabilitySpecificationType) entity.ProbabilitySpecificationTypeId,
                Summary = entity.Summary
            };

            ReadExpertClassSpecifications(entity, collector, treeEvent);
            collector.Collect(entity,treeEvent);
            return treeEvent;
        }

        private static void ReadExpertClassSpecifications(TreeEventEntity entity, ReadConversionCollector collector, TreeEvent treeEvent)
        {
            var specifications = entity.ExpertClassEstimationEntities.Select(e => e.Read(collector));
            foreach (var specification in specifications)
            {
                treeEvent.ClassesProbabilitySpecification.Add(specification);
            }
        }

        private static FragilityCurve ReadFragilityCurve(ICollection<TreeEventFragilityCurveElementEntity> entities, ReadConversionCollector collector)
        {
            var curve = new FragilityCurve();
            foreach (var treeEventFragilityCurveElementEntity in entities)
            {
                curve.Add(treeEventFragilityCurveElementEntity.Read(collector));
            }

            return curve;
        }
    }
}
