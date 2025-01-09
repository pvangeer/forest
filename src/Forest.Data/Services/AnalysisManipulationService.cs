using System;
using System.Collections.Generic;
using System.Linq;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Properties;
using Forest.Data.Tree;

namespace Forest.Data.Services
{
    public class AnalysisManipulationService
    {
        private readonly ForestAnalysis forestAnalysis;

        public AnalysisManipulationService([NotNull] ForestAnalysis forestAnalysis)
        {
            this.forestAnalysis = forestAnalysis;
        }

        public TreeEvent RemoveTreeEvent(EventTree eventTree, TreeEvent selectedTreeEventToRemove)
        {
            if (Equals(eventTree.MainTreeEvent, selectedTreeEventToRemove))
            {
                eventTree.MainTreeEvent = null;
                eventTree.OnPropertyChanged(nameof(eventTree.MainTreeEvent));
                return null;
            }

            var parent = eventTree.MainTreeEvent.FindTreeEvent(treeEvent => treeEvent.FailingEvent == selectedTreeEventToRemove ||
                                                                            treeEvent.PassingEvent == selectedTreeEventToRemove);
            if (parent == null)
                throw new ArgumentNullException();

            var removedTreeEvents = GetAllTreeNodes(selectedTreeEventToRemove);
            foreach (var estimation in forestAnalysis.ProbabilityEstimationsPerTreeEvent.Where(e => e.EventTree == eventTree))
            {
                foreach (var eventProbabilityEstimation in estimation.Estimates
                             .Where(e => removedTreeEvents.Any(te => te == e.TreeEvent))
                             .ToList())
                {
                    estimation.Estimates.Remove(eventProbabilityEstimation);
                }
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

            eventTree.OnTreeEventsChanged(new TreeEventsChangedEventArgs(EventTreeModification.Remove, parent));

            return parent;
        }

        public TreeEvent AddTreeEvent(EventTree eventTree, TreeEvent selectedTreeEventToAddTo, TreeEventType type)
        {
            var newTreeEvent = new TreeEvent("Nieuwe gebeurtenis", type);

            foreach (var estimation in forestAnalysis.ProbabilityEstimationsPerTreeEvent.Where(e => e.EventTree == eventTree))
            {
                estimation.Estimates.Add(new TreeEventProbabilityEstimate(newTreeEvent)
                {
                    ProbabilitySpecificationType = ProbabilitySpecificationType.FixedValue
                });
            }

            if (eventTree.MainTreeEvent == null)
            {
                newTreeEvent.Type = TreeEventType.MainEvent;
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

            eventTree.OnTreeEventsChanged(new TreeEventsChangedEventArgs(EventTreeModification.Add,
                selectedTreeEventToAddTo,
                newTreeEvent));

            return newTreeEvent;
        }

        public void AddProbabilityEstimationPerTreeEvent(EventTree eventTree)
        {
            var probabilityEstimation = new ProbabilityEstimationPerTreeEvent
            {
                Name = "Nieuwe faalkansinschatting",
                EventTree = eventTree
            };
            if (eventTree.MainTreeEvent != null)
            {
                foreach (var treeEvent in GetAllTreeNodes(eventTree.MainTreeEvent))
                {
                    probabilityEstimation.Estimates.Add(new TreeEventProbabilityEstimate(treeEvent)
                    {
                        ProbabilitySpecificationType = ProbabilitySpecificationType.FixedValue
                    });
                }
            }
            
            forestAnalysis.ProbabilityEstimationsPerTreeEvent.Add(probabilityEstimation);
        }

        public void RemoveProbabilityEstimationPerTreeEvent(ProbabilityEstimationPerTreeEvent estimation)
        {
            if (estimation == null || !forestAnalysis.ProbabilityEstimationsPerTreeEvent.Contains(estimation))
                return;

            forestAnalysis.ProbabilityEstimationsPerTreeEvent.Remove(estimation);
        }

        public EventTree AddEventTree()
        {
            var eventTree = new EventTree
            {
                Name = "Nieuw faalpad"
            };
            forestAnalysis.EventTrees.Add(eventTree);
            return eventTree;
        }

        public void RemoveEventTree(EventTree eventTree)
        {
            if (eventTree == null || !forestAnalysis.EventTrees.Contains(eventTree))
                return;

            var estimationsToRemove = forestAnalysis.ProbabilityEstimationsPerTreeEvent
                .Where(e => e.EventTree == eventTree)
                .ToArray();

            foreach (var estimation in estimationsToRemove)
                RemoveProbabilityEstimationPerTreeEvent(estimation);
            forestAnalysis.EventTrees.Remove(eventTree);
        }

        private static IEnumerable<TreeEvent> GetAllTreeNodes(TreeEvent treeEvent)
        {
            yield return treeEvent;
            if (treeEvent.FailingEvent != null)
            {
                foreach (var node in GetAllTreeNodes(treeEvent.FailingEvent))
                {
                    yield return node;
                }
            }

            if (treeEvent.PassingEvent != null)
            {
                foreach (var node in GetAllTreeNodes(treeEvent.PassingEvent))
                {
                    yield return node;
                }
            }
        }
    }
}