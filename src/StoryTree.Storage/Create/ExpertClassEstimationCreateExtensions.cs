using System;
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
                AverageEstimation = Convert.ToByte(model.AverageEstimation),
                MaxEstimation = Convert.ToByte(model.MaxEstimation),
                MinEstimation = Convert.ToByte(model.MinEstimation),
            };

            return entity;
        }
    }
}
