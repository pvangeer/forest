using System;
using Forest.Data;
using Forest.Data.Estimations;
using Forest.Data.Hydrodynamics;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Read
{
    internal static class HydraulicConditionEntityReadExtensions
    {
        internal static HydrodynamicCondition Read(this HydrodynamicConditionXmlEntity entity, ReadConversionCollector collector)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (collector == null)
                throw new ArgumentNullException(nameof(collector));

            if (collector.Contains(entity))
                return collector.Get(entity);

            var condition = new HydrodynamicCondition
            {
                Probability = double.IsNaN(entity.Probability) ? Probability.NaN : (Probability)entity.Probability,
                WaterLevel = entity.WaterLevel,
                WavePeriod = entity.WavePeriod,
                WaveHeight = entity.WaveHeight
            };

            collector.Collect(entity, condition);
            return condition;
        }
    }
}