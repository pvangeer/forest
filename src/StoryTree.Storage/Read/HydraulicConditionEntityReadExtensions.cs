using System;
using StoryTree.Data;
using StoryTree.Data.Hydraulics;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Read
{
    internal static class HydraulicConditionEntityReadExtensions
    {
        internal static HydraulicCondition Read(this HydraulicConditionElementEntity entity,ReadConversionCollector collector)
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

            FragilityCurveElement curveElement = entity.FragilityCurveElementEntity.Read(collector);
            var condition = new HydraulicCondition
            {
                Probability = curveElement.Probability,
                WaterLevel = curveElement.WaterLevel,
                WavePeriod = entity.WavePeriod == null ? double.NaN : (double)entity.WavePeriod,
                WaveHeight = entity.WaveHeight == null ? double.NaN : (double)entity.WaveHeight
            };

            collector.Collect(entity,condition);
            return condition;
        }
    }
}
