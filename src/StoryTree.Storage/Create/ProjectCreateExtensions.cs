using System;
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
                //PersonEntity = project.ProjectLeader.Create(registry),
            };

            /*AddEntitiesForExperts(project, entity, registry);*/
            /*AddEntitiesForHydraulicConditions(project, entity, registry);
            AddEntitiesForEventTrees(project, entity, registry);*/

            return entity;

            // TODO: Implement
        }

        private static void AddEntitiesForExperts(Project project, ProjectEntity entity, PersistenceRegistry registry)
        {
            foreach (var expert in project.Experts)
            {
                entity.ExpertEntities.Add(expert.Create(registry));
            }
        }
    }
}
