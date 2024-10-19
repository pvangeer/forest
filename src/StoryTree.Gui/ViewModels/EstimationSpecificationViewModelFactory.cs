using System;
using System.ComponentModel;
using StoryTree.Data;
using StoryTree.Data.Estimations;
using StoryTree.Data.Tree;

namespace StoryTree.Gui.ViewModels
{
    public class EstimationSpecificationViewModelFactory
    {
        public EstimationSpecificationViewModelFactory(Project project)
        {
            Project = project;
        }

        public Project Project { get; }

        public ProbabilitySpecificationViewModelBase CreateViewModel(TreeEvent treeEvent)
        {
            switch (treeEvent.ProbabilitySpecificationType)
            {
                case ProbabilitySpecificationType.Classes:
                    return new ClassesProbabilitySpecificationViewModel(treeEvent, Project);
                case ProbabilitySpecificationType.FixedValue:
                    return new FixedProbabilitySpecificationViewModel(treeEvent);
                case ProbabilitySpecificationType.FixedFrequency:
                    return new FixedFragilityCurveSpecificationViewModel(treeEvent, Project);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}