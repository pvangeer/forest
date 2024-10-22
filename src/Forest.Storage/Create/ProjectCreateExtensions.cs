using System;
using Forest.Data;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Create
{
    internal static class ProjectCreateExtensions
    {
        internal static EventTreeProjectXmlEntity Create(this EventTreeProject eventTreeProject, PersistenceRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            var entity = new EventTreeProjectXmlEntity
            {
                Name = eventTreeProject.Name.DeepClone(),
                Description = eventTreeProject.Description.DeepClone(),
                AssessmentSection = eventTreeProject.AssessmentSection.DeepClone(),
                ProjectInformation = eventTreeProject.ProjectInformation.DeepClone(),
                ProjectLeader = eventTreeProject.ProjectLeader.Create(registry),
                EventTree = eventTreeProject.EventTree.Create(registry)
            };

            AddEntitiesForExperts(eventTreeProject, entity, registry);
            AddEntitiesForHydraulicConditions(eventTreeProject, entity, registry);

            return entity;
        }

        private static void AddEntitiesForHydraulicConditions(EventTreeProject eventTreeProject, EventTreeProjectXmlEntity entity,
            PersistenceRegistry registry)
        {
            for (var index = 0; index < eventTreeProject.HydraulicConditions.Count; index++)
            {
                var hydraulicConditionElementEntity = eventTreeProject.HydraulicConditions[index].Create(registry);
                hydraulicConditionElementEntity.Order = index;
                entity.HydraulicConditions.Add(hydraulicConditionElementEntity);
            }
        }

        private static void AddEntitiesForExperts(EventTreeProject eventTreeProject, EventTreeProjectXmlEntity entity,
            PersistenceRegistry registry)
        {
            for (var index = 0; index < eventTreeProject.Experts.Count; index++)
            {
                var expertEntity = eventTreeProject.Experts[index].Create(registry);
                expertEntity.Order = index;
                entity.Experts.Add(expertEntity);
            }
        }
    }
}