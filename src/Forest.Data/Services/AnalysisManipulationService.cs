using System;
using System.Linq;
using Forest.Data.Estimations;
using Forest.Data.Experts;
using Forest.Data.Hydrodynamics;
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

        public void AddHydraulicCondition(HydrodynamicCondition hydrodynamicCondition)
        {
            forestAnalysis.HydrodynamicConditions.Add(hydrodynamicCondition);
            foreach (var estimation in forestAnalysis.ProbabilityEstimations.OfType<ProbabilityEstimationPerTreeEvent>())
            {
                foreach (var treeEvent in forestAnalysis.EventTree.MainTreeEvent.GetAllEventsRecursive())
                {
                    var estimationsForThisTreeEvent = estimation.Estimations.FirstOrDefault(e => e.TreeEvent == treeEvent);
                    if (estimationsForThisTreeEvent == null)
                    {
                        continue;
                    }
                    foreach (var expert in forestAnalysis.Experts)
                        estimationsForThisTreeEvent.ClassProbabilitySpecification.Add(new ExpertClassEstimation
                        {
                            Expert = expert,
                            HydrodynamicCondition = hydrodynamicCondition,
                            AverageEstimation = ProbabilityClass.None,
                            MaxEstimation = ProbabilityClass.None,
                            MinEstimation = ProbabilityClass.None
                        });
                }
            }
        }

        public void RemoveHydraulicCondition(HydrodynamicCondition hydrodynamicCondition)
        {
            forestAnalysis.HydrodynamicConditions.Remove(hydrodynamicCondition);

            var estimation = forestAnalysis.ProbabilityEstimations.OfType<ProbabilityEstimationPerTreeEvent>().FirstOrDefault();
            if (estimation == null)
            {
                return;
            }
            foreach (var treeEvent in forestAnalysis.EventTree.MainTreeEvent.GetAllEventsRecursive())
            {
                var estimationsForThisTreeEvent = estimation.Estimations.FirstOrDefault(e => e.TreeEvent == treeEvent);
                if (estimationsForThisTreeEvent == null)
                {
                    continue;
                }

                var estimatesToRemove = estimationsForThisTreeEvent.ClassProbabilitySpecification.Where(e =>
                    e.HydrodynamicCondition == hydrodynamicCondition).ToArray();
                foreach (var estimationToRemove in estimatesToRemove)
                    estimationsForThisTreeEvent.ClassProbabilitySpecification.Remove(estimationToRemove);
            }
        }

        public void AddExpert(Expert expert)
        {
            forestAnalysis.Experts.Add(expert);
            foreach (var estimation in forestAnalysis.ProbabilityEstimations.OfType<ProbabilityEstimationPerTreeEvent>())
            {
                foreach (var treeEvent in forestAnalysis.EventTree.MainTreeEvent.GetAllEventsRecursive())
                {
                    var estimationsForThisTreeEvent = estimation.Estimations.FirstOrDefault(e => e.TreeEvent == treeEvent);
                    if (estimationsForThisTreeEvent == null)
                    {
                        continue;
                    }
                    foreach (var hydraulicCondition in forestAnalysis.HydrodynamicConditions)
                        estimationsForThisTreeEvent.ClassProbabilitySpecification.Add(new ExpertClassEstimation
                        {
                            Expert = expert,
                            HydrodynamicCondition = hydraulicCondition,
                            AverageEstimation = ProbabilityClass.None,
                            MaxEstimation = ProbabilityClass.None,
                            MinEstimation = ProbabilityClass.None
                        });
                }
            }
        }

        public void RemoveExpert(Expert expert)
        {
            forestAnalysis.Experts.Remove(expert);

            var estimation = forestAnalysis.ProbabilityEstimations.OfType<ProbabilityEstimationPerTreeEvent>().FirstOrDefault();
            if (estimation == null)
            {
                return;
            }
            foreach (var treeEvent in forestAnalysis.EventTree.MainTreeEvent.GetAllEventsRecursive())
            {
                var estimationsForThisTreeEvent = estimation.Estimations.FirstOrDefault(e => e.TreeEvent == treeEvent);
                if (estimationsForThisTreeEvent == null)
                {
                    continue;
                }
                var estimatesToRemove = estimationsForThisTreeEvent.ClassProbabilitySpecification.Where(e =>
                        e.Expert == expert).ToArray();
                    foreach (var estimationToRemove in estimatesToRemove)
                        estimationsForThisTreeEvent.ClassProbabilitySpecification.Remove(estimationToRemove);
            }
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

            var estimation = forestAnalysis.ProbabilityEstimations.OfType<ProbabilityEstimationPerTreeEvent>().FirstOrDefault(e => e.EventTree == eventTree);
            if (estimation != null)
            {
                var estimationToRemove = estimation.Estimations.FirstOrDefault(e => e.TreeEvent == selectedTreeEventToRemove);
                if (estimationToRemove != null)
                {
                    estimation.Estimations.Remove(estimationToRemove);
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
            var newTreeEvent = new TreeEvent("Nieuwe gebeurtenis");

            var estimation = forestAnalysis.ProbabilityEstimations.OfType<ProbabilityEstimationPerTreeEvent>()
                .FirstOrDefault(e => e.EventTree == eventTree);

            if (estimation != null)
            {
                var treeEventProbabilityEstimation = new TreeEventProbabilityEstimation(newTreeEvent);
                treeEventProbabilityEstimation.ProbabilitySpecificationType = ProbabilitySpecificationType.FixedValue;
                foreach (var expert in forestAnalysis.Experts)
                foreach (var hydraulicCondition in forestAnalysis.HydrodynamicConditions)
                    treeEventProbabilityEstimation.ClassProbabilitySpecification.Add(new ExpertClassEstimation
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
    }
}