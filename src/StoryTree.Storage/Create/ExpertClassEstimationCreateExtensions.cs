using System;
using StoryTree.Data.Tree;
using StoryTree.Storage.XmlEntities;

namespace StoryTree.Storage.Create
{
    internal static class ExpertClassEstimationCreateExtensions
    {
        internal static ExpertClassEstimationXmlEntity Create(this ExpertClassEstimation model, PersistenceRegistry registry)
        {
            var entity = new ExpertClassEstimationXmlEntity
            {
                ExpertId = model.Expert.Create(registry).Id,
                HydraulicConditionId = model.HydraulicCondition.Create(registry).Id,
                AverageEstimation = Convert.ToByte(model.AverageEstimation),
                MaxEstimation = Convert.ToByte(model.MaxEstimation),
                MinEstimation = Convert.ToByte(model.MinEstimation),
                // TODO: Add Comment to data model and map
            };

            return entity;
        }
    }
}
