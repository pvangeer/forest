using System;
using System.ComponentModel;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Create
{
    public static class TreeEventProbabilityEstimationCreateExtensions
    {
        public static TreeEventProbabilityEstimationXmlEntity Create(this TreeEventProbabilityEstimation model, PersistenceRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            var entity = new TreeEventProbabilityEstimationXmlEntity
            {
                FixedProbability = model.FixedProbability,
                ProbabilitySpecificationType = model.ProbabilitySpecificationType.ToStorageName(),
            };

            AddFragilityCurveElements(entity, model, registry);
            AddClassProbabilitySpecifications(entity, model, registry);

            return entity;
        }


        private static void AddClassProbabilitySpecifications(TreeEventProbabilityEstimationXmlEntity entity, TreeEventProbabilityEstimation model, PersistenceRegistry registry)
        {
            for (var index = 0; index < model.ClassProbabilitySpecifications.Count; index++)
            {
                var classEstimationXmlEntity = model.ClassProbabilitySpecifications[index].Create(registry);
                classEstimationXmlEntity.Order = index;
                entity.ClassProbabilitySpecifications.Add(classEstimationXmlEntity);
            }
        }

        private static void AddFragilityCurveElements(TreeEventProbabilityEstimationXmlEntity entity, TreeEventProbabilityEstimation model, PersistenceRegistry registry)
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
