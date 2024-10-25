using System;
using System.Collections.Generic;
using System.Linq;

namespace Forest.Data.Tree
{
    public static class TreeEventExtensions
    {
        public static IEnumerable<TreeEvent> GetAllEventsRecursive(this TreeEvent treeEvent)
        {
            var list = new[] { treeEvent };

            if (treeEvent == null)
                return list;

            if (treeEvent.FailingEvent != null)
                list = list.Concat(GetAllEventsRecursive(treeEvent.FailingEvent)).ToArray();

            if (treeEvent.PassingEvent != null)
                list = list.Concat(GetAllEventsRecursive(treeEvent.PassingEvent)).ToArray();

            return list;
        }

        public static TreeEvent FindTreeEvent(this TreeEvent treeEvent, Func<TreeEvent, bool> findAction)
        {
            if (findAction(treeEvent))
                return treeEvent;

            if (treeEvent.FailingEvent != null)
            {
                var possibleEvent = FindTreeEvent(treeEvent.FailingEvent, findAction);
                if (possibleEvent != null)
                    return possibleEvent;
            }

            if (treeEvent.PassingEvent != null)
            {
                var possibleEvent = FindTreeEvent(treeEvent.PassingEvent, findAction);
                if (possibleEvent != null)
                    return possibleEvent;
            }

            return null;
        }
    }
}