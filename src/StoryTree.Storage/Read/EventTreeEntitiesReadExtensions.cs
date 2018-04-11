using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryTree.Data;
using StoryTree.Storage.Create;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Read
{
    internal static class EventTreeEntitiesReadExtensions
    {
        internal static EventTree Read(this EventTreeEntity entity, ReadConversionCollector collector)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (collector == null)
            {
                throw new ArgumentNullException(nameof(collector));
            }

            if (collector.Contains(entity))
            {
                return collector.Get(entity);
            }

            var eventTree = new EventTree
            {
                Name = entity.Name,
                Details = entity.Details,
                Summary = entity.Summary,
                MainTreeEvent = entity.TreeEventEntity.Read(collector),
                Color = entity.Color.ToColor()
            };

            collector.Collect(entity,eventTree);

            return eventTree;
        }
    }
}
