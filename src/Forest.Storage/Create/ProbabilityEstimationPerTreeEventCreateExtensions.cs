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

            AddEstimationsToEstimation(entity, model, registry);

            registry.Register(entity);

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
    }
}