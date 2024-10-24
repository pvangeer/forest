using System;
using Forest.Data.Tree;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Create
{
    internal static class ExpertClassEstimationCreateExtensions
    {
        internal static ExpertClassEstimationXmlEntity Create(this ExpertClassEstimation model, PersistenceRegistry registry)
        {
            var entity = new ExpertClassEstimationXmlEntity
            {
                ExpertId = model.Expert.Create(registry).Id,
                HydraulicConditionId = model.HydrodynamicCondition.Create(registry).Id,
                AverageEstimation = Convert.ToByte(model.AverageEstimation),
                MaxEstimation = Convert.ToByte(model.MaxEstimation),
                MinEstimation = Convert.ToByte(model.MinEstimation)
                // TODO: Add Comment to data model and map
            };

            return entity;
        }
    }
}