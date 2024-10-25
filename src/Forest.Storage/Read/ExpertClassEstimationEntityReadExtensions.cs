using System;
using Forest.Data.Estimations;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Tree;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Read
{
    internal static class ExpertClassEstimationEntityReadExtensions
    {
        internal static ExpertClassEstimation Read(this ExpertClassEstimationXmlEntity entity,
            ReadConversionCollector collector)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (collector == null)
                throw new ArgumentNullException(nameof(collector));

            if (collector.Contains(entity))
                return collector.Get(entity);

            var estimation = new ExpertClassEstimation
            {
                Expert = collector.GetReferencedExpert(entity.ExpertId),
                HydrodynamicCondition = collector.GetReferencedHydraulicCondition(entity.HydraulicConditionId),
                AverageEstimation = (ProbabilityClass)entity.AverageEstimation,
                MinEstimation = (ProbabilityClass)entity.MinEstimation,
                MaxEstimation = (ProbabilityClass)entity.MaxEstimation
                // TODO: Add Comment
            };

            collector.Collect(entity, estimation);

            return estimation;
        }
    }
}