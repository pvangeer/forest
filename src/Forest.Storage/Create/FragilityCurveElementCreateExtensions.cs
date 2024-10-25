using System;
using Forest.Data.Probabilities;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Create
{
    internal static class FragilityCurveElementCreateExtensions
    {
        internal static FragilityCurveElementXmlEntity Create(this FragilityCurveElement model, PersistenceRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            if (registry.Contains(model))
                return registry.Get(model);

            var entity = new FragilityCurveElementXmlEntity
            {
                Probability = model.Probability,
                WaterLevel = model.WaterLevel
            };

            registry.Register(model, entity);

            return entity;
        }
    }
}