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
            if (treeEvent.ProbabilitySpecificationType == type)
            {
                return;
            }

            treeEvent.ProbabilitySpecificationType = type;
            treeEvent.OnPropertyChanged(nameof(TreeEvent.ProbabilitySpecificationType));
        }
    }
}
