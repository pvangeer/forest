using System;
using System.Linq;
using StoryTree.Storage.XmlEntities;

namespace StoryTree.Storage.Read
{
    internal static class ProjectEntityReadExtensions
    {
        internal static Data.EventTreeProject Read(this EventTreeProjectXmlEntity entity, ReadConversionCollector collector)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (collector == null)
            {
                throw new ArgumentNullException(nameof(collector));
            }

            var experts = entity.Experts.OrderBy(e => e.Order).Select(e => e.Read(collector));
            var hydraulicConditions = entity.HydraulicConditions.OrderBy(e => e.Order).Select(e => e.Read(collector));

            var project = new Data.EventTreeProject
            {
                Name = entity.Name,
                AssessmentSection = entity.AssessmentSection,
                Description = entity.Description,
                ProjectInformation = entity.ProjectInformation,
                ProjectLeader = entity.ProjectLeader.Read(collector),
                EventTree = entity.EventTree.Read(collector)
        };

            foreach (var expert in experts)
            {
                project.Experts.Add(expert);
            }

            foreach (var hydraulicCondition in hydraulicConditions)
            {
                project.HydraulicConditions.Add(hydraulicCondition);
            }

            return project;
        }
    }
}
