using System;
using StoryTree.Data.Tree;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Create
{
    internal static class TreeEventCreateExtensions
    {
        internal static TreeEventEntity Create(this TreeEvent model, PersistenceRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            if (registry.Contains(model))
            {
                return registry.Get(model);
            }

            var entity = new TreeEventEntity
            {
                Name = model.Name.DeepClone(),
                Details = model.Details.DeepClone(),
                Summary = model.Summary.DeepClone(),
                FixedProbability = ((double)model.FixedProbability).ToNaNAsNull(),
                ProbabilitySpecificationTypeId = (int)model.ProbabilitySpecificationType,
            };

            /*entity.ExpertClassEstimationEntities
            entity.TreeEventFragilityCurveElementEntities
            entity.ProbabilitySpecificationTypeId
            entity.TreeEventEntity1 passing?
            entity.TreeEventEntity11 failing?*/

            registry.Register(model, entity);

            return entity;
        }
    }
}
