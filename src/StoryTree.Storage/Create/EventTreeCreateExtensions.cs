using System;
using StoryTree.Data;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Create
{
    internal static class EventTreeCreateExtensions
    {
        internal static EventTreeEntity Create(this EventTree model, PersistenceRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            if (registry.Contains(model))
            {
                return registry.Get(model);
            }

            var entity = new EventTreeEntity
            {
                Name = model.Name.DeepClone(),
                Details = model.Details.DeepClone(),
                Summary = model.Summary.DeepClone(),
                Color = model.Color.ToHexString(),
                TreeEventEntity = model.MainTreeEvent.Create(registry),
                NeedsSpecification = Convert.ToByte(model.NeedsSpecification)
            };

            registry.Register(model, entity);

            return entity;
        }
    }
}
