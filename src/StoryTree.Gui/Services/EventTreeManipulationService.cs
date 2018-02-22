using System;
using StoryTree.Data;
using StoryTree.Data.Tree;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Services
{
    public static class EventTreeManipulationService
    {
        public static TreeEvent RemoveTreeEvent(EventTree eventTree, TreeEventViewModel selectedTreeEventToRemove)
        {
            // TODO: Dangerous. Also the possibility that the selected tree event does not belong to this eventtree. This should also be solved.
            var parent = FindTreeEvent(eventTree.MainTreeEvent, treeEvent => treeEvent.FailingEvent == selectedTreeEventToRemove.TreeEvent || treeEvent.PassingEvent == selectedTreeEventToRemove.TreeEvent);
            if (parent.FailingEvent == selectedTreeEventToRemove.TreeEvent)
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

        public static TreeEvent AddTreeEvent(EventTree eventTree, TreeEventViewModel selectedTreeEventToAddTo, TreeEventType type)
        {
            var newTreeEvent = new TreeEvent { Name = "Nieuwe gebeurtenis" };
            if (eventTree.MainTreeEvent == null)
            {
                eventTree.MainTreeEvent = newTreeEvent;
                return newTreeEvent;
            }

            switch (type)
            {
                case TreeEventType.Failing:
                    selectedTreeEventToAddTo.TreeEvent.FailingEvent = newTreeEvent;
                    selectedTreeEventToAddTo.TreeEvent.OnPropertyChanged(nameof(selectedTreeEventToAddTo.TreeEvent.FailingEvent));
                    break;
                case TreeEventType.Passing:
                    selectedTreeEventToAddTo.TreeEvent.PassingEvent = newTreeEvent;
                    selectedTreeEventToAddTo.TreeEvent.OnPropertyChanged(nameof(selectedTreeEventToAddTo.TreeEvent.PassingEvent));
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
