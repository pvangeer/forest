using System;
using Forest.Data;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Create
{
    internal static class ExpertCreateExtensions
    {
        internal static ExpertXmlEntity Create(this Expert model, PersistenceRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            if (registry.Contains(model))
                return registry.Get(model);

            var entity = new ExpertXmlEntity
            {
                Name = model.Name.DeepClone(),
                Email = model.Email.DeepClone(),
                Telephone = model.Telephone.DeepClone(),
                Expertise = model.Expertise.DeepClone(),
                Organization = model.Organization.DeepClone()
            };

            registry.Register(model, entity);

            return entity;
        }
    }
}