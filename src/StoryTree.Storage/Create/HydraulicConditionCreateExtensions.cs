using System;
using StoryTree.Data.Hydraulics;
using StoryTree.Storage.XmlEntities;

namespace StoryTree.Storage.Create
{
    internal static class HydraulicConditionCreateExtensions
    {
        internal static HydraulicConditionXmlEntity Create(this HydraulicCondition model, PersistenceRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            if (registry.Contains(model))
            {
                return registry.Get(model);
            }

            var entity = new HydraulicConditionXmlEntity
            {
                Probability = ((double)model.Probability),
                WaterLevel = model.WaterLevel,
                WaveHeight = model.WaveHeight,
                WavePeriod = model.WavePeriod
            };

            registry.Register(model, entity);

            return entity;
        }
    }
}
