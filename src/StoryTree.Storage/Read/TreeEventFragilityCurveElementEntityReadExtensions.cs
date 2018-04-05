using StoryTree.Data;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Read
{
    internal static class TreeEventFragilityCurveElementEntityReadExtensions
    {
        internal static FragilityCurveElement Read(this TreeEventFragilityCurveElementEntity entity,ReadConversionCollector collector)
        {
            return entity.FragilityCurveElementEntity.Read(collector);
        }
    }
}
