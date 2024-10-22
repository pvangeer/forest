using System;
using System.Linq;
using Forest.Data.Estimations;
using Forest.Data.Hydraulics;
using Forest.Data.Properties;
using Forest.Data.Tree;

namespace Forest.Data.Services
{
    public class ProjectManipulationService
    {
        private readonly EventTreeProject eventTreeProject;

        public ProjectManipulationService([NotNull] EventTreeProject eventTreeProject)
        {
            this.eventTreeProject = eventTreeProject;
        }

        public void AddHydraulicCondition(HydraulicCondition hydraulicCondition)
        {
            eventTreeProject.HydraulicConditions.Add(hydraulicCondition);

            foreach (var treeEvent in eventTreeProject.EventTree.MainTreeEvent.GetAllEventsRecursive())
            foreach (var expert in eventTreeProject.Experts)
                treeEvent.ClassesProbabilitySpecification.Add(new ExpertClassEstimation
                {
                    Expert = expert,
                    HydraulicCondition = hydraulicCondition,
                    AverageEstimation = ProbabilityClass.None,
                    MaxEstimation = ProbabilityClass.None,
                    MinEstimation = ProbabilityClass.None
                });
        }

        public void RemoveHydraulicCondition(HydraulicCondition hydraulicCondition)
        {
            eventTreeProject.HydraulicConditions.Remove(hydraulicCondition);

            foreach (var treeEvent in eventTreeProject.EventTree.MainTreeEvent.GetAllEventsRecursive())
            foreach (var expert in eventTreeProject.Experts)
            {
                var estimatesToRemove = treeEvent.ClassesProbabilitySpecification.Where(e =>
                    e.Expert == expert && e.HydraulicCondition == hydraulicCondition).ToArray();
                foreach (var estimation in estimatesToRemove)
                    treeEvent.ClassesProbabilitySpecification.Remove(estimation);
            }
        }

        public void AddExpert(Expert expert)
        {
            eventTreeProject.Experts.Add(expert);

            foreach (var treeEvent in eventTreeProject.EventTree.MainTreeEvent.GetAllEventsRecursive())
            foreach (var hydraulicCondition in eventTreeProject.HydraulicConditions)
                treeEvent.ClassesProbabilitySpecification.Add(new ExpertClassEstimation
                {
                    Expert = expert,
                    HydraulicCondition = hydraulicCondition,
                    AverageEstimation = ProbabilityClass.None,
                    MaxEstimation = ProbabilityClass.None,
                    MinEstimation = ProbabilityClass.None
                });
        }

        public void RemoveExpert(Expert expert)
        {
            eventTreeProject.Experts.Remove(expert);

            foreach (var treeEvent in eventTreeProject.EventTree.MainTreeEvent.GetAllEventsRecursive())
            foreach (var hydraulicCondition in eventTreeProject.HydraulicConditions)
            {
                var estimatesToRemove = treeEvent.ClassesProbabilitySpecification.Where(e =>
                    e.Expert == expert && e.HydraulicCondition == hydraulicCondition).ToArray();
                foreach (var estimation in estimatesToRemove)
                    treeEvent.ClassesProbabilitySpecification.Remove(estimation);
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
            foreach (var expert in eventTreeProject.Experts)
            foreach (var hydraulicCondition in eventTreeProject.HydraulicConditions)
                newTreeEvent.ClassesProbabilitySpecification.Add(new ExpertClassEstimation
                {
                    Expert = expert,
                    HydraulicCondition = hydraulicCondition,
                    AverageEstimation = ProbabilityClass.None,
                    MinEstimation = ProbabilityClass.None,
                    MaxEstimation = ProbabilityClass.None
                });

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