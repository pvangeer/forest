using System.Collections.Generic;
using System.Linq;
using StoryTree.Data.Tree;

namespace StoryTree.Calculators
{
    public static class CriticalPathCalculator
    {
        public static IEnumerable<TreeEvent> GetCriticalPath(TreeEvent mainTreeEvent, TreeEvent treeEventTo)
        {
            if (treeEventTo == null || mainTreeEvent == null)
            {
                return null;
            }

            if (ReferenceEquals(treeEventTo, mainTreeEvent))
            {
                return new[]{treeEventTo};
            }

            var pathToThisNode = GetCriticalPath(mainTreeEvent.FailingEvent, treeEventTo) ??
                                 GetCriticalPath(mainTreeEvent.PassingEvent, treeEventTo);
            
            return pathToThisNode == null ? null : new[] {mainTreeEvent}.Concat(pathToThisNode);
        }
    }
}
