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

            // TODO: Move this to probabilityspecifications
            var experts = entity.Experts.OrderBy(e => e.Order).Select(e => e.Read(collector));
            var hydraulicConditions = entity.HydraulicConditions.OrderBy(e => e.Order).Select(e => e.Read(collector));

            var eventTrees = entity.EventTreeXmlEntities.OrderBy(e => e.Order).Select(e => e.Read(collector));

            var project = new ForestAnalysis
            {
                Name = entity.Name,
                AssessmentSection = entity.AssessmentSection,
                Description = entity.Description,
                ProjectInformation = entity.ProjectInformation
            };
            var projectLeader = entity.ProjectLeader.Read(collector);
            project.ProjectLeader.Name = projectLeader.Name;
            project.ProjectLeader.Email = projectLeader.Email;
            project.ProjectLeader.Telephone = projectLeader.Telephone;

            foreach (var eventTree in eventTrees)
                project.EventTrees.Add(eventTree);

            return project;
        }
    }
}