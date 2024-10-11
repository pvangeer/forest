using System;
using System.Globalization;
using System.Linq;
using StoryTree.Data;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Read
{
    internal static class ProjectEntityReadExtensions
    {
        internal static Project Read(this ProjectEntity entity, ReadConversionCollector collector)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (collector == null)
            {
                throw new ArgumentNullException(nameof(collector));
            }

            var experts = entity.ExpertEntities.OrderBy(e => e.Order).Select(e => e.Read(collector));
            var hydraulicConditions = entity.HydraulicConditionElementEntities.OrderBy(e => e.Order).Select(e => e.Read(collector));
            var eventTrees = entity.EventTreeEntities.OrderBy(e => e.Order).Select(e => e.Read(collector));

            var project = new Project
            {
                Name = entity.Name,
                AssessmentSection = entity.AssessmentSection,
                Description = entity.Description,
                ProjectInformation = entity.ProjectInformation,
                ProjectLeader = entity.PersonEntity.Read(collector),
            };

            foreach (var expert in experts)
            {
                project.Experts.Add(expert);
            }

            foreach (var hydraulicCondition in hydraulicConditions)
            {
                project.HydraulicConditions.Add(hydraulicCondition);
            }

            foreach (var eventTree in eventTrees)
            {
                project.EventTrees.Add(eventTree);
            }

            return project;
        }
    }
}
