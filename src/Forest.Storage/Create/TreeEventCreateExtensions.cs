using System;
using Forest.Data.Tree;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Create
{
    internal static class TreeEventCreateExtensions
    {
        internal static TreeEventXmlEntity Create(this TreeEvent model, PersistenceRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            if (registry.Contains(model))
                return registry.Get(model);

            var entity = new TreeEventXmlEntity
            {
                Name = model.Name.DeepClone(),
                Summary = model.Summary.DeepClone(),
                Information = model.Information.DeepClone(),
                TreeEventType = model.Type.ToStorageName()
            };

            if (model.FailingEvent != null)
                entity.FailingEvent = model.FailingEvent.Create(registry);

            if (model.PassingEvent != null)
                entity.PassingEvent = model.PassingEvent.Create(registry);

            registry.Register(model, entity);

            return entity;
        }
    }
}