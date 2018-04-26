using System;
using StoryTree.Data.Estimations;
using StoryTree.Data.Tree;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Read
{
    internal static class ExpertClassEstimationEntityReadExtensions
    {
        internal static ExpertClassEstimation Read(this ExpertClassEstimationEntity entity,
            ReadConversionCollector collector)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (collector == null)
            {
                throw new ArgumentNullException(nameof(collector));
            }

            if (collector.Contains(entity))
            {
                return collector.Get(entity);
            }

            var estimation = new ExpertClassEstimation
            {
                Expert = entity.ExpertEntity.Read(collector),
                WaterLevel = (double)entity.WaterLevel,
                AverageEstimation = (ProbabilityClass)entity.AverageEstimation,
                MinEstimation = (ProbabilityClass)entity.MinEstimation,
                MaxEstimation = (ProbabilityClass)entity.MaxEstimation
            };

            collector.Collect(entity,estimation);

            return estimation;
        }
    }
}
