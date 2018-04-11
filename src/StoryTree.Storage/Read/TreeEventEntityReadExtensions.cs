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

            var treeEvent = new TreeEvent
            {
                Name = entity.Name,
                Details = entity.Details,
                FixedProbability = entity.FixedProbability == null
                    ? Probability.NaN
                    : (Probability) (double) entity.FixedProbability,
                ProbabilitySpecificationType = (ProbabilitySpecificationType) entity.ProbabilitySpecificationTypeId,
                Summary = entity.Summary
            };

            ReadFragilityCurve(treeEvent, entity.TreeEventFragilityCurveElementEntities, collector);

            if (entity.TreeEventEntity3 != null)
            {
                treeEvent.FailingEvent = entity.TreeEventEntity3.Read(collector);
            }

            if (entity.TreeEventEntity2 != null)
            {
                treeEvent.PassingEvent = entity.TreeEventEntity2.Read(collector);
            }

            ReadExpertClassSpecifications(entity, collector, treeEvent);
            collector.Collect(entity,treeEvent);
            return treeEvent;
        }

        private static void ReadExpertClassSpecifications(TreeEventEntity entity, ReadConversionCollector collector, TreeEvent treeEvent)
        {
            var specifications = entity.ExpertClassEstimationEntities.OrderBy(e => e.Order).Select(e => e.Read(collector));
            foreach (var specification in specifications)
            {
                treeEvent.ClassesProbabilitySpecification.Add(specification);
            }
        }

        private static void ReadFragilityCurve(TreeEvent treeEvent, IEnumerable<TreeEventFragilityCurveElementEntity> entities, ReadConversionCollector collector)
        {
            foreach (var treeEventFragilityCurveElementEntity in entities.OrderBy(e => e.Order))
            {
                treeEvent.FixedFragilityCurve.Add(treeEventFragilityCurveElementEntity.Read(collector));
            }
        }
    }
}
