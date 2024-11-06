using System;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Create
{
    public static class ProbabilityEstimationPerTreeEventCreateExtensions
    {
        public static ProbabilityEstimationPerTreeEventXmlEntity Create(this ProbabilityEstimationPerTreeEvent model,
            PersistenceRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            var entity = new ProbabilityEstimationPerTreeEventXmlEntity
            {
                Name = model.Name.DeepClone(),
                EventTreeId = model.EventTree.Create(registry).Id
            };

            AddExpertsToEstimation(entity, model, registry);
            AddHydrodynamicConditionsToEstimation(entity, model, registry);
            AddEstimationsToEstimation(entity, model, registry);

            return entity;
        }

        private static void AddEstimationsToEstimation(ProbabilityEstimationPerTreeEventXmlEntity entity,
            ProbabilityEstimationPerTreeEvent model,
            PersistenceRegistry registry)
        {
            for (var index = 0; index < model.Estimates.Count; index++)
            {
                var estimation = model.Estimates[index].Create(registry);
                estimation.Order = index;
                entity.Estimations.Add(estimation);
            }
        }

        private static void AddHydrodynamicConditionsToEstimation(ProbabilityEstimationPerTreeEventXmlEntity entity,
            ProbabilityEstimationPerTreeEvent model,
            PersistenceRegistry registry)
        {
            for (var index = 0; index < model.HydrodynamicConditions.Count; index++)
            {
                var condition = model.HydrodynamicConditions[index].Create(registry);
                condition.Order = index;
                entity.HydrodynamicConditions.Add(condition);
            }
        }

        private static void AddExpertsToEstimation(ProbabilityEstimationPerTreeEventXmlEntity entity,
            ProbabilityEstimationPerTreeEvent model,
            PersistenceRegistry registry)
        {
            for (var index = 0; index < model.Experts.Count; index++)
            {
                var expert = model.Experts[index].Create(registry);
                expert.Order = index;
                entity.Experts.Add(expert);
            }
        }
    }
}