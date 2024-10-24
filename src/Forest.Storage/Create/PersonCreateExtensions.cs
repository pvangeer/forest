using System;
using Forest.Data;
using Forest.Data.Experts;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Create
{
    internal static class PersonCreateExtensions
    {
        internal static PersonXmlEntity Create(this Person model, PersistenceRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            if (registry.Contains(model))
                return registry.Get(model);

            var entity = new PersonXmlEntity
            {
                Name = model.Name.DeepClone(),
                Email = model.Email.DeepClone(),
                Telephone = model.Telephone.DeepClone()
            };

            registry.Register(model, entity);

            return entity;
        }
    }
}