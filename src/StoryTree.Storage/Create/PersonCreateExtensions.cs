using System;
using StoryTree.Data;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Create
{
    internal static class PersonCreateExtensions
    {
        internal static PersonEntity Create(this Person model, PersistenceRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            if (registry.Contains(model))
            {
                return registry.Get(model);
            }

            var entity = new PersonEntity
            {
                Name = model.Name.DeepClone(),
                Email = model.Email.DeepClone(),
                Telephone = model.Telephone.DeepClone()
            };

            registry.Register(entity,model);

            return entity;
        }
    }
}
