using System;
using Forest.Data.Hydraulics;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Create
{
    internal static class HydraulicConditionCreateExtensions
    {
        internal static HydraulicConditionXmlEntity Create(this HydraulicCondition model, PersistenceRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            if (registry.Contains(model))
                return registry.Get(model);

            var entity = new HydraulicConditionXmlEntity
            {
                Probability = model.Probability,
                WaterLevel = model.WaterLevel,
                WaveHeight = model.WaveHeight,
                WavePeriod = model.WavePeriod
            };

            registry.Register(model, entity);

            return entity;
        }
    }
}