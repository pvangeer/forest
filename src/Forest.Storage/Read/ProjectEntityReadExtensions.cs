using System;
using System.Linq;
using Forest.Data;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Read
{
    internal static class ProjectEntityReadExtensions
    {
        internal static ForestAnalysis Read(this ForestAnalysisXmlEntity entity, ReadConversionCollector collector)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (collector == null)
                throw new ArgumentNullException(nameof(collector));

            var experts = entity.Experts.OrderBy(e => e.Order).Select(e => e.Read(collector));
            var hydraulicConditions = entity.HydraulicConditions.OrderBy(e => e.Order).Select(e => e.Read(collector));

            var project = new ForestAnalysis
            {
                Name = entity.Name,
                AssessmentSection = entity.AssessmentSection,
                Description = entity.Description,
                ProjectInformation = entity.ProjectInformation,
                ProjectLeader = entity.ProjectLeader.Read(collector),
                EventTree = entity.EventTree.Read(collector)
            };

            foreach (var hydraulicCondition in hydraulicConditions)
                project.HydrodynamicConditions.Add(hydraulicCondition);

            return project;
        }
    }
}