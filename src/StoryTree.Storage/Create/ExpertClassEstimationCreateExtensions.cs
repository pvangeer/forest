using StoryTree.Data.Tree;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Create
{
    internal static class ExpertClassEstimationCreateExtensions
    {
        internal static ExpertClassEstimationEntity Create(this ExpertClassEstimation model, PersistenceRegistry registry)
        {
            var entity = new ExpertClassEstimationEntity
            {
                ExpertEntity = model.Expert.Create(registry),
                WaterLevel = model.WaterLevel.ToNaNAsNull(),
                AverageEstimationId = (int)model.AverageEstimation,
                MaxEstimationId = (int)model.MaxEstimation,
                MinEstimationId = (int)model.MinEstimation,
            };

            return entity;
        }
    }
}
