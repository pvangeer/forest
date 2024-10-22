using System;
using Forest.Data;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Read
{
    internal static class EventTreeEntitiesReadExtensions
    {
        internal static EventTree Read(this EventTreeXmlEntity entity, ReadConversionCollector collector)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (collector == null)
                throw new ArgumentNullException(nameof(collector));

            if (collector.Contains(entity))
                return collector.Get(entity);

            var eventTree = new EventTree
            {
                MainTreeEvent = entity.MainTreeEvent?.Read(collector)
            };

            collector.Collect(entity, eventTree);

            return eventTree;
        }
    }
}