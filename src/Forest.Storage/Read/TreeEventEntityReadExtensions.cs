using System;
using System.Collections.Generic;
using System.Linq;
using Forest.Data.Estimations;
using Forest.Data.Tree;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Read
{
    internal static class TreeEventEntityReadExtensions
    {
        internal static TreeEvent Read(this TreeEventXmlEntity entity, ReadConversionCollector collector)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (collector == null)
                throw new ArgumentNullException(nameof(collector));

            if (collector.Contains(entity))
                return collector.Get(entity);

            var treeEvent = new TreeEvent(entity.Name)
            {
                Summary = entity.Summary,
                Information = entity.Information,
                Discussion = entity.Discussion
                // TODO: Add PassPhrase
            };

            if (entity.FailingEvent != null)
                treeEvent.FailingEvent = entity.FailingEvent.Read(collector);

            if (entity.PassingEvent != null)
                treeEvent.PassingEvent = entity.PassingEvent.Read(collector);

            collector.Collect(entity, treeEvent);
            return treeEvent;
        }
    }
}