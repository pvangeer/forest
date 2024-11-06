using System;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Read
{
    internal static class ProbabilityEstimationPerTreeEventReadExtensions
    {
        internal static ProbabilityEstimationPerTreeEvent Read(this ProbabilityEstimationPerTreeEventXmlEntity entity, ReadConversionCollector collector)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (collector == null)
                throw new ArgumentNullException(nameof(collector));

            var estimation = new ProbabilityEstimationPerTreeEvent
            {
                EventTree = collector.GetReferencedEventTree(entity.EventTreeId),
                Name = entity.Name,
            };

            // TODO: Add estimations, HC and Experts
            return estimation;
        }
    }
}
