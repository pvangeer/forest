using System.ComponentModel;
using StoryTree.Data.Estimations;
using StoryTree.Data.Estimations.Classes;
using StoryTree.Data.Tree;

namespace StoryTree.Data.Services
{
    public static class TreeEventManipulations
    {
        public static void ChangeProbabilityEstimationType(TreeEvent treeEvent, ProbabilitySpecificationType type)
        {
            if (treeEvent.ProbabilityInformation == null || treeEvent.ProbabilityInformation.Type == type)
            {
                return;
            }

            switch (type)
            {
                case ProbabilitySpecificationType.Classes:
                    treeEvent.ProbabilityInformation = new ClassesProbabilitySpecification();
                    break;
                case ProbabilitySpecificationType.FixedValue:
                    treeEvent.ProbabilityInformation = new FixedValueProbabilitySpecification();
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }

            treeEvent.OnPropertyChanged(nameof(TreeEvent.ProbabilityInformation));

        }
    }
}
