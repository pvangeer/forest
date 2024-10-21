using System;
using StoryTree.Data;
using StoryTree.Storage.XmlEntities;

namespace StoryTree.Storage.Create
{
    internal static class EventTreeCreateExtensions
    {
        internal static EventTreeXmlEntity Create(this EventTree model, PersistenceRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            if (registry.Contains(model))
                return registry.Get(model);

            var entity = new EventTreeXmlEntity
            {
                MainTreeEvent = model.MainTreeEvent?.Create(registry)
            };

            registry.Register(model, entity);

            return entity;
        }
    }
}