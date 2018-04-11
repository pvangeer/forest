using System;
using StoryTree.Data;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Create
{
    internal static class FragilityCurveElementCreateExtensions
    {
        internal static FragilityCurveElementEntity Create(this FragilityCurveElement model, PersistenceRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            if (registry.Contains(model))
            {
                return registry.Get(model);
            }

            var entity = new FragilityCurveElementEntity
            {
                Probability = model.Probability,
                WaterLevel = model.WaterLevel
            };

            registry.Register(model, entity);

            return entity;
        }
    }
}
