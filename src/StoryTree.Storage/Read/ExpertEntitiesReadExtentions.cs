using System;
using StoryTree.Data;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Read
{
    internal static class ExpertEntitiesReadExtentions
    {
        internal static Expert Read(this ExpertEntity entity, ReadConversionCollector collector)
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

            var person = entity.PersonEntity.Read(collector);
            var expert = new Expert
            {
                Name = person.Name,
                Email = person.Email,
                Telephone = person.Telephone,
                Expertise = entity.Expertise,
                Organization = entity.Organisation
            };

            collector.Collect(entity,expert);
            return expert;
        }
    }
}
