using System;
using Forest.Data;
using Forest.Data.Experts;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Read
{
    internal static class PersonEntityReadExtensions
    {
        internal static Person Read(this PersonXmlEntity entity, ReadConversionCollector collector)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (collector == null)
                throw new ArgumentNullException(nameof(collector));

            if (collector.Contains(entity))
                return collector.Get(entity);

            var person = new Person
            {
                Name = entity.Name,
                Email = entity.Email,
                Telephone = entity.Telephone
            };

            collector.Collect(entity, person);

            return person;
        }
    }
}