using System;
using StoryTree.Data;
using StoryTree.Data.Hydraulics;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Create
{
    internal static class HydraulicConditionCreateExtensions
    {
        internal static HydraulicConditionElementEntity Create(this HydraulicCondition model, PersistenceRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            if (registry.Contains(model))
            {
                return registry.Get(model);
            }

            var entity = new HydraulicConditionElementEntity
            {
                FragilityCurveElementEntity = ((FragilityCurveElement)model).Create(registry),
                WaveHeight = model.WaveHeight.ToNaNAsNull(),
                WavePeriod = model.WavePeriod.ToNaNAsNull()
            };

            registry.Register(model, entity);

            return entity;
        }
    }
}
