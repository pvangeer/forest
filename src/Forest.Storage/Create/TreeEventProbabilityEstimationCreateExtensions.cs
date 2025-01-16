using System;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Create
{
    public static class TreeEventProbabilityEstimationCreateExtensions
    {
        public static TreeEventProbabilityEstimateXmlEntity Create(this TreeEventProbabilityEstimate model, PersistenceRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            var entity = new TreeEventProbabilityEstimateXmlEntity
            {
                FixedProbability = model.FixedProbability,
                ProbabilitySpecificationType = model.ProbabilitySpecificationType.ToStorageName()
            };

            AddFragilityCurveElements(entity, model, registry);

            registry.Register(entity);

            return entity;
        }


        private static void AddFragilityCurveElements(TreeEventProbabilityEstimateXmlEntity entity,
            TreeEventProbabilityEstimate model,
            PersistenceRegistry registry)
        {
            for (var index = 0; index < model.FragilityCurve.Count; index++)
            {
                var elementXmlEntity = model.FragilityCurve[index].Create(registry);
                elementXmlEntity.Order = index;
                entity.FragilityCurve.Add(elementXmlEntity);
            }
        }
    }
}