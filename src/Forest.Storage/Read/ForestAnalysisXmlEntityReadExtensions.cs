using System;
using System.Linq;
using Forest.Data;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Read
{
    internal static class ForestAnalysisXmlEntityReadExtensions
    {
        internal static ForestAnalysis Read(this ForestAnalysisXmlEntity entity, ReadConversionCollector collector)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (collector == null)
                throw new ArgumentNullException(nameof(collector));


            var analysis = new ForestAnalysis
            {
                Name = entity.Name,
                AssessmentSection = entity.AssessmentSection,
                Description = entity.Description,
                ProjectInformation = entity.ProjectInformation
            };
            var projectLeader = entity.ProjectLeader.Read(collector);
            analysis.ProjectLeader.Name = projectLeader.Name;
            analysis.ProjectLeader.Email = projectLeader.Email;
            analysis.ProjectLeader.Telephone = projectLeader.Telephone;

            var eventTrees = entity.EventTreeXmlEntities.OrderBy(e => e.Order).Select(e => e.Read(collector));
            foreach (var eventTree in eventTrees)
                analysis.EventTrees.Add(eventTree);

            var estimationsPerTreeEvent =
                entity.ProbabilityEstimationPerTreeEventXmlEntities.OrderBy(e => e.Order).Select(e => e.Read(collector));
            foreach (var estimation in estimationsPerTreeEvent)
                analysis.ProbabilityEstimationsPerTreeEvent.Add(estimation);

            return analysis;
        }
    }
}