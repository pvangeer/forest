using System;
using System.Globalization;
using StoryTree.Data;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Create
{
    internal static class ProjectCreateExtensions
    {
        internal static ProjectEntity Create(this Project project, PersistenceRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            var entity = new ProjectEntity
            {
                Name = project.Name.DeepClone(),
                Description = project.Description.DeepClone(),
                AssessmentSection = project.AssessmentSection.DeepClone(),
                ProjectInformation = project.ProjectInformation.DeepClone(),
                PersonEntity = project.ProjectLeader.Create(registry),
            };

            AddEntitiesForExperts(project, entity, registry);
            AddEntitiesForHydraulicConditions(project, entity, registry);
            AddEntitiesForEventTrees(project, entity, registry);

            return entity;
        }

        private static void AddEntitiesForEventTrees(Project project, ProjectEntity entity, PersistenceRegistry registry)
        {
            for (var index = 0; index < project.EventTrees.Count; index++)
            {
                var eventTreeEntity = project.EventTrees[index].Create(registry);
                eventTreeEntity.Order = index;
                entity.EventTreeEntities.Add(eventTreeEntity);
            }
        }

        private static void AddEntitiesForHydraulicConditions(Project project, ProjectEntity entity, PersistenceRegistry registry)
        {
            for (var index = 0; index < project.HydraulicConditions.Count; index++)
            {
                var hydraulicConditionElementEntity = project.HydraulicConditions[index].Create(registry);
                hydraulicConditionElementEntity.Order = index;
                entity.HydraulicConditionElementEntities.Add(hydraulicConditionElementEntity);
            }
        }

        private static void AddEntitiesForExperts(Project project, ProjectEntity entity, PersistenceRegistry registry)
        {
            for (var index = 0; index < project.Experts.Count; index++)
            {
                var expertEntity = project.Experts[index].Create(registry);
                expertEntity.Order = index;
                entity.ExpertEntities.Add(expertEntity);
            }
        }
    }
}
