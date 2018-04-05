using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryTree.Data;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Read
{
    internal static class ProjectEntityReadExtensions
    {
        internal static Project Read(this ProjectEntity entity, ReadConversionCollector collector)
        {
            var experts = entity.ExpertEntities.Select(e => e.Read(collector));
            var hydraulicConditions = entity.HydraulicConditionElementEntities.Select(e => e.Read(collector));

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

            // TODO: EventTrees

            return project;

        }
    }
}
