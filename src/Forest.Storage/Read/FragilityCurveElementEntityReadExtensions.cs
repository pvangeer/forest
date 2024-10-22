using System;
using Forest.Data;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Read
{
    internal static class FragilityCurveElementEntityReadExtensions
    {
        internal static FragilityCurveElement Read(this FragilityCurveElementXmlEntity entity, ReadConversionCollector collector)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (collector == null)
                throw new ArgumentNullException(nameof(collector));

            if (collector.Contains(entity))
                return collector.Get(entity);

            var element =
                new FragilityCurveElement(entity.WaterLevel, (Probability)entity.Probability);

            collector.Collect(entity, element);

            return element;
        }
    }
}