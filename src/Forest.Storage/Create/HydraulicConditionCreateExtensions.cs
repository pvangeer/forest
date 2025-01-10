using System;
using Forest.Data.Hydrodynamics;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Create
{
    internal static class HydraulicConditionCreateExtensions
    {
        internal static HydrodynamicConditionXmlEntity Create(this HydrodynamicCondition model, PersistenceRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            if (registry.Contains(model))
                return registry.Get(model);

            var entity = new HydrodynamicConditionXmlEntity
            {
                Probability = model.Probability,
                WaterLevel = model.WaterLevel,
            };

            registry.Register(model, entity);

            return entity;
        }
    }
}