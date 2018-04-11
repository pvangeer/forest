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
                ProbabilitySpecificationTypeId = (int)model.ProbabilitySpecificationType
            };

            AddExpertClassEstimations(entity, model, registry);
            AddFragilityCurveElements(entity, model, registry);

            if (model.FailingEvent != null)
            {
                entity.TreeEventEntity3 = model.FailingEvent.Create(registry);
            }

            if (model.PassingEvent != null)
            {
                entity.TreeEventEntity2 = model.PassingEvent.Create(registry);
            }
            
            registry.Register(model, entity);

            return entity;
        }

        private static void AddFragilityCurveElements(TreeEventEntity entity, TreeEvent model, PersistenceRegistry registry)
        {
            if (model.FixedFragilityCurve != null)
            {
                for (var index = 0; index < model.FixedFragilityCurve.Count; index++)
                {
                    var fragilityCurveElement = model.FixedFragilityCurve[index];
                    var curveElementEntity = new TreeEventFragilityCurveElementEntity
                    {
                        FragilityCurveElementEntity = fragilityCurveElement.Create(registry),
                        TreeEventEntity = entity,
                        Order = index
                    };
                    entity.TreeEventFragilityCurveElementEntities.Add(curveElementEntity);
                }
            }
        }

        private static void AddExpertClassEstimations(TreeEventEntity entity, TreeEvent model, PersistenceRegistry registry)
        {
            for (var index = 0; index < model.ClassesProbabilitySpecification.Count; index++)
            {
                var expertClassEstimationEntity = model.ClassesProbabilitySpecification[index].Create(registry);
                expertClassEstimationEntity.TreeEventEntity = entity;
                expertClassEstimationEntity.Order = index;
                entity.ExpertClassEstimationEntities.Add(expertClassEstimationEntity);
            }
        }
    }
}
