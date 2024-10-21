using System;
using System.ComponentModel;
using StoryTree.Data;
using StoryTree.Data.Estimations;
using StoryTree.Data.Tree;

namespace StoryTree.Gui.ViewModels
{
    public class EstimationSpecificationViewModelFactory
    {
        public EstimationSpecificationViewModelFactory(EventTreeProject eventTreeProject)
        {
            EventTreeProject = eventTreeProject;
        }

        public EventTreeProject EventTreeProject { get; }

        public ProbabilitySpecificationViewModelBase CreateViewModel(TreeEvent treeEvent)
        {
            switch (treeEvent.ProbabilitySpecificationType)
            {
                case ProbabilitySpecificationType.Classes:
                    return new ClassesProbabilitySpecificationViewModel(treeEvent, EventTreeProject);
                case ProbabilitySpecificationType.FixedValue:
                    return new FixedProbabilitySpecificationViewModel(treeEvent);
                case ProbabilitySpecificationType.FixedFrequency:
                    return new FixedFragilityCurveSpecificationViewModel(treeEvent, EventTreeProject);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}