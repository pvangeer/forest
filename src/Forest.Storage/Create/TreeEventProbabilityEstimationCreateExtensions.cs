using System;
using System.ComponentModel;
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
                ProbabilitySpecificationType = model.ProbabilitySpecificationType.ToStorageName(),
            };

            AddFragilityCurveElements(entity, model, registry);
            AddClassProbabilitySpecifications(entity, model, registry);

            return entity;
        }


        private static void AddClassProbabilitySpecifications(TreeEventProbabilityEstimateXmlEntity entity, TreeEventProbabilityEstimate model, PersistenceRegistry registry)
        {
            for (var index = 0; index < model.ClassProbabilitySpecifications.Count; index++)
            {
                var classEstimationXmlEntity = model.ClassProbabilitySpecifications[index].Create(registry);
                classEstimationXmlEntity.Order = index;
                entity.ClassProbabilitySpecifications.Add(classEstimationXmlEntity);
            }
        }

        private static void AddFragilityCurveElements(TreeEventProbabilityEstimateXmlEntity entity, TreeEventProbabilityEstimate model, PersistenceRegistry registry)
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
