using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using StoryTree.Data;
using StoryTree.Data.Estimations;
using StoryTree.Data.Estimations.Classes;

namespace StoryTree.Gui.ViewModels
{
    public class EstimationSpecificationViewModelFactory
    {
        public EstimationSpecificationViewModelFactory(Project project)
        {
            Project = project;
        }

        public Project Project { get; }

        public ProbabilitySpecificationViewModelBase CreateViewModel(IProbabilitySpecification probabilitySpecification)
        {
            switch (probabilitySpecification.Type)
            {
                case ProbabilitySpecificationType.Classes:
                    return new ClassesProbabilitySpecificationViewModel((ClassesProbabilitySpecification) probabilitySpecification, Project);
                case ProbabilitySpecificationType.FixedValue:
                    return new FixedProbabilitySpecificationViewModel((FixedValueProbabilitySpecification) probabilitySpecification);
                case ProbabilitySpecificationType.FixedFreqeuncy:
                    throw new NotImplementedException();
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}