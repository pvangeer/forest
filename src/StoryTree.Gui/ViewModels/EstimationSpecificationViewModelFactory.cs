using System;
using System.ComponentModel;
using StoryTree.Data.Estimations;
using StoryTree.Data.Estimations.Classes;

namespace StoryTree.Gui.ViewModels
{
    public static class EstimationSpecificationViewModelFactory
    {
        public static ProbabilitySpecificationViewModelBase CreateViewModel(IProbabilitySpecification probabilitySpecification)
        {
            switch (probabilitySpecification.Type)
            {
                case ProbabilitySpecificationType.Classes:
                    return new ClassesProbabilitySpecificationViewModel((ClassesProbabilitySpecification) probabilitySpecification);
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