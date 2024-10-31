using System;
using Forest.Data;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Create
{
    internal static class ProjectCreateExtensions
    {
        internal static ForestAnalysisXmlEntity Create(this ForestAnalysis forestAnalysis, PersistenceRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            var entity = new ForestAnalysisXmlEntity
            {
                Name = forestAnalysis.Name.DeepClone(),
                Description = forestAnalysis.Description.DeepClone(),
                AssessmentSection = forestAnalysis.AssessmentSection.DeepClone(),
                ProjectInformation = forestAnalysis.ProjectInformation.DeepClone(),
                ProjectLeader = forestAnalysis.ProjectLeader.Create(registry)
            };

            AddEntriesForEventTrees(forestAnalysis, entity, registry);

            // TODO: Store also probability analysis

            return entity;
        }

        private static void AddEntriesForEventTrees(ForestAnalysis analysis, ForestAnalysisXmlEntity entity, PersistenceRegistry registry)
        {
            for (var index = 0; index < analysis.EventTrees.Count; index++)
            {
                var eventTreeEntity = analysis.EventTrees[index].Create(registry);
                eventTreeEntity.Order = index;
                entity.EventTreeXmlEntities.Add(eventTreeEntity);
            }
        }
    }
}