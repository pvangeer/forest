using System;
using Forest.Data;
using Forest.Data.Experts;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Read
{
    internal static class ExpertEntitiesReadExtensions
    {
        internal static Expert Read(this ExpertXmlEntity entity, ReadConversionCollector collector)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (collector == null)
                throw new ArgumentNullException(nameof(collector));

            if (collector.Contains(entity))
                return collector.Get(entity);

            var expert = new Expert
            {
                Name = entity.Name,
                Email = entity.Email,
                Telephone = entity.Telephone,
                Expertise = entity.Expertise,
                Organization = entity.Organization
            };

            collector.Collect(entity, expert);
            return expert;
        }
    }
}