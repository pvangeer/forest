using System;
using System.Linq;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Read
{
    internal static class ProbabilityEstimationPerTreeEventReadExtensions
    {
        internal static ProbabilityEstimationPerTreeEvent Read(this ProbabilityEstimationPerTreeEventXmlEntity entity,
            ReadConversionCollector collector)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (collector == null)
                throw new ArgumentNullException(nameof(collector));

            var estimation = new ProbabilityEstimationPerTreeEvent
            {
                EventTree = collector.GetReferencedEventTree(entity.EventTreeId),
                Name = entity.Name
            };

            var hydrodynamicCondition =
                entity.HydrodynamicConditions.OrderBy(e => e.Order).Select(e => e.Read(collector));
            foreach (var condition in hydrodynamicCondition)
                estimation.HydrodynamicConditions.Add(condition);

            var estimationsPerTreeEvent =
                entity.Estimations.OrderBy(e => e.Order).Select(e => e.Read(collector));
            foreach (var estimate in estimationsPerTreeEvent)
                estimation.Estimates.Add(estimate);

            return estimation;
        }
    }
}