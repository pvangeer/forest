using System;
using System.Linq;
using StoryTree.Data.Estimations;
using StoryTree.Data.Hydraulics;
using StoryTree.Data.Properties;
using StoryTree.Data.Tree;

namespace StoryTree.Data.Services
{
    public class ProjectManipulationService
    {
        private readonly Project project;

        public ProjectManipulationService([NotNull] Project project)
        {
            this.project = project;
        }

        public void AddHydraulicCondition(HydraulicCondition hydraulicCondition)
        {
            project.HydraulicConditions.Add(hydraulicCondition);

            foreach (var eventTree in project.EventTrees)
            {
                foreach (var treeEvent in eventTree.MainTreeEvent.GetAllEventsRecursive())
                {
                    foreach (var expert in project.Experts)
                    {
                        treeEvent.ClassesProbabilitySpecification.Add(new ExpertClassEstimation
                        {
                            Expert = expert,
                            HydraulicCondition = hydraulicCondition,
                            AverageEstimation = ProbabilityClass.None,
                            MaxEstimation = ProbabilityClass.None,
                            MinEstimation = ProbabilityClass.None
                        });
                    }
                }
            }
        }

        public void RemoveHydraulicCondition(HydraulicCondition hydraulicCondition)
        {
            project.HydraulicConditions.Remove(hydraulicCondition);

            foreach (var eventTree in project.EventTrees)
            {
                foreach (var treeEvent in eventTree.MainTreeEvent.GetAllEventsRecursive())
                {
                    foreach (var expert in project.Experts)
                    {
                        var estimatesToRemove = treeEvent.ClassesProbabilitySpecification.Where(e =>
                            e.Expert == expert && e.HydraulicCondition == hydraulicCondition).ToArray();
                        foreach (var estimation in estimatesToRemove)
                        {
                            treeEvent.ClassesProbabilitySpecification.Remove(estimation);
                        }
                    }
                }
            }
        }

        public void AddExpert(Expert expert)
        {
            project.Experts.Add(expert);

            foreach (var eventTree in project.EventTrees)
            {
                foreach (var treeEvent in eventTree.MainTreeEvent.GetAllEventsRecursive())
                {
                    foreach (var hydraulicCondition in project.HydraulicConditions)
                    {
                        treeEvent.ClassesProbabilitySpecification.Add(new ExpertClassEstimation
                        {
                            Expert = expert,
                            HydraulicCondition = hydraulicCondition,
                            AverageEstimation = ProbabilityClass.None,
                            MaxEstimation = ProbabilityClass.None,
                            MinEstimation = ProbabilityClass.None
                        });
                    }
                }
            }
        }

        public void RemoveExpert(Expert expert)
        {
            project.Experts.Remove(expert);

            foreach (var eventTree in project.EventTrees)
            {
                foreach (var treeEvent in eventTree.MainTreeEvent.GetAllEventsRecursive())
                {
                    foreach (var hydraulicCondition in project.HydraulicConditions)
                    {
                        var estimatesToRemove = treeEvent.ClassesProbabilitySpecification.Where(e =>
                            e.Expert == expert && e.HydraulicCondition == hydraulicCondition).ToArray();
                        foreach (var estimation in estimatesToRemove)
                        {
                            treeEvent.ClassesProbabilitySpecification.Remove(estimation);
                        }
                    }
                }
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

        public TreeEvent AddTreeEvent(EventTree eventTree, TreeEvent selectedTreeEventToAddTo, TreeEventType type)
        {
            var newTreeEvent = new TreeEvent
            {
                Name = "Nieuwe gebeurtenis"
            };
            foreach (var expert in project.Experts)
            {
                foreach (var hydraulicCondition in project.HydraulicConditions)
                {
                    newTreeEvent.ClassesProbabilitySpecification.Add(new ExpertClassEstimation
                    {
                        Expert = expert,
                        HydraulicCondition = hydraulicCondition,
                        AverageEstimation = ProbabilityClass.None,
                        MinEstimation = ProbabilityClass.None,
                        MaxEstimation = ProbabilityClass.None
                    });
                }
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

            return newTreeEvent;
        }
    }
}