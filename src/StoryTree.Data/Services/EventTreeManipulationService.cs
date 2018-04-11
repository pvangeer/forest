using System;
using StoryTree.Data.Tree;

namespace StoryTree.Data.Services
{
    public static class EventTreeManipulationService
    {
        public static TreeEvent RemoveTreeEvent(EventTree eventTree, TreeEvent selectedTreeEventToRemove)
        {
            if (Equals(eventTree.MainTreeEvent, selectedTreeEventToRemove))
            {
                eventTree.MainTreeEvent = null;
                eventTree.OnPropertyChanged(nameof(eventTree.MainTreeEvent));
                return null;
            }

            var parent = FindTreeEvent(eventTree.MainTreeEvent, treeEvent => treeEvent.FailingEvent == selectedTreeEventToRemove || treeEvent.PassingEvent == selectedTreeEventToRemove);
            if (parent == null)
            {
                throw new ArgumentNullException();
            }

            if (parent.FailingEvent == selectedTreeEventToRemove)
            {
                parent.FailingEvent = null;
                parent.OnPropertyChanged(nameof(parent.FailingEvent));
            }
            else
            {
                parent.PassingEvent = null;
                parent.OnPropertyChanged(nameof(parent.PassingEvent));
            }

            return parent;
        }

        public static TreeEvent AddTreeEvent(EventTree eventTree, TreeEvent selectedTreeEventToAddTo, TreeEventType type)
        {
            var newTreeEvent = new TreeEvent
            {
                Name = "Nieuwe gebeurtenis"
            };

            if (eventTree.MainTreeEvent == null)
            {
                eventTree.MainTreeEvent = newTreeEvent;
                eventTree.OnPropertyChanged(nameof(eventTree.MainTreeEvent));
                return newTreeEvent;
            }

            switch (type)
            {
                case TreeEventType.Failing:
                    selectedTreeEventToAddTo.FailingEvent = newTreeEvent;
                    selectedTreeEventToAddTo.OnPropertyChanged(nameof(selectedTreeEventToAddTo.FailingEvent));
                    break;
                case TreeEventType.Passing:
                    selectedTreeEventToAddTo.PassingEvent = newTreeEvent;
                    selectedTreeEventToAddTo.OnPropertyChanged(nameof(selectedTreeEventToAddTo.PassingEvent));
                    break;
            }

            return newTreeEvent;
        }

        private static TreeEvent FindTreeEvent(TreeEvent treeEvent, Func<TreeEvent, bool> findAction)
        {
            return findAction(treeEvent) ? treeEvent : FindTreeEvent(treeEvent.FailingEvent, findAction);
        }
    }
}
