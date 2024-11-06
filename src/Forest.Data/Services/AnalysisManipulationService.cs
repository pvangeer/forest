using System;
using System.Linq;
using Forest.Data.Estimations;
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

            var estimation = forestAnalysis.ProbabilityEstimationsPerTreeEvent.FirstOrDefault(e => e.EventTree == eventTree);
            if (estimation != null)
            {
                var estimationToRemove = estimation.Estimations.FirstOrDefault(e => e.TreeEvent == selectedTreeEventToRemove);
                if (estimationToRemove != null)
                    estimation.Estimations.Remove(estimationToRemove);
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
            var newTreeEvent = new TreeEvent("Nieuwe gebeurtenis");

            var estimation = forestAnalysis.ProbabilityEstimationsPerTreeEvent.FirstOrDefault(e => e.EventTree == eventTree);

            if (estimation != null)
            {
                var treeEventProbabilityEstimation = new TreeEventProbabilityEstimation(newTreeEvent)
                {
                    ProbabilitySpecificationType = ProbabilitySpecificationType.FixedValue
                };
                foreach (var expert in estimation.Experts)
                foreach (var hydraulicCondition in estimation.HydrodynamicConditions)
                    treeEventProbabilityEstimation.ClassProbabilitySpecifications.Add(new ExpertClassEstimation
                    {
                        Expert = expert,
                        HydrodynamicCondition = hydraulicCondition,
                        AverageEstimation = ProbabilityClass.None,
                        MinEstimation = ProbabilityClass.None,
                        MaxEstimation = ProbabilityClass.None
                    });
                estimation.Estimations.Add(treeEventProbabilityEstimation);
            }

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

            eventTree.OnTreeEventsChanged(new TreeEventsChangedEventArgs(EventTreeModification.Add, selectedTreeEventToAddTo,
                newTreeEvent));

            return newTreeEvent;
        }

        public void AddProbabilityEstimationPerTreeEvent(EventTree eventTree)
        {
            forestAnalysis.ProbabilityEstimationsPerTreeEvent.Add(new ProbabilityEstimationPerTreeEvent
            {
                Name = "Nieuwe faalkansinschatting",
                EventTree = eventTree
            });
        }

        public void RemoveProbabilityEstimationPerTreeEvent(ProbabilityEstimationPerTreeEvent estimation)
        {
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
    }
}