using System;
using Forest.Data.Tree;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Create
{
    internal static class TreeEventCreateExtensions
    {
        internal static TreeEventXmlEntity Create(this TreeEvent model, PersistenceRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            if (registry.Contains(model))
                return registry.Get(model);

            var entity = new TreeEventXmlEntity
            {
                Name = model.Name.DeepClone(),
                Summary = model.Summary.DeepClone(),
                FixedProbability = model.FixedProbability,
                ProbabilitySpecificationType = Convert.ToByte(model.ProbabilitySpecificationType),
                Information = model.Information.DeepClone(),
                Discussion = model.Discussion.DeepClone()
                // Add passphrase
            };

            AddExpertClassEstimations(entity, model, registry);
            AddFragilityCurveElements(entity, model, registry);

            if (model.FailingEvent != null)
                entity.FailingEvent = model.FailingEvent.Create(registry);

            if (model.PassingEvent != null)
                entity.PassingEvent = model.PassingEvent.Create(registry);

            registry.Register(model, entity);

            return entity;
        }

        private static void AddFragilityCurveElements(TreeEventXmlEntity entity, TreeEvent model, PersistenceRegistry registry)
        {
            if (model.FixedFragilityCurve != null)
                for (var index = 0; index < model.FixedFragilityCurve.Count; index++)
                {
                    var fragilityCurveElement = model.FixedFragilityCurve[index];
                    entity.FixedFragilityCurveElements.Add(fragilityCurveElement.Create(registry));
                }
        }

        private static void AddExpertClassEstimations(TreeEventXmlEntity entity, TreeEvent model, PersistenceRegistry registry)
        {
            for (var index = 0; index < model.ClassesProbabilitySpecification.Count; index++)
            {
                var expertClassEstimationEntity = model.ClassesProbabilitySpecification[index].Create(registry);
                expertClassEstimationEntity.Order = index;
                entity.ClassesProbabilitySpecifications.Add(expertClassEstimationEntity);
            }
        }
    }
}