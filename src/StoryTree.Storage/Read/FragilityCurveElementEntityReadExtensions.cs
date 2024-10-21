using System;
using StoryTree.Data;
using StoryTree.Storage.XmlEntities;

namespace StoryTree.Storage.Read
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