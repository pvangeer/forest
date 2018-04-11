using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryTree.Data;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Create
{
    internal static class ExpertCreateExtensions
    {
        internal static ExpertEntity Create(this Expert model, PersistenceRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            if (registry.Contains(model))
            {
                return registry.Get(model);
            }

            var entity = new ExpertEntity
            {
                PersonEntity = ((Person) model).Create(registry),
                Expertise = model.Expertise.DeepClone(),
                Organisation = model.Organisation.DeepClone()
            };

            registry.Register(model, entity);

            return entity;
        }
    }
}
